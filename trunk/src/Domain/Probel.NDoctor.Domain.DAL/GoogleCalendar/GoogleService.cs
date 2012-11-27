#region Header

/*
    This file is part of NDoctor.

    NDoctor is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    NDoctor is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with NDoctor.  If not, see <http://www.gnu.org/licenses/>.
*/

#endregion Header

namespace Probel.NDoctor.Domain.DAL.GoogleCalendar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Google.GData.Calendar;
    using Google.GData.Extensions;

    using log4net;

    using Probel.Helpers.Benchmarking;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.GoogleCalendar;
    using Probel.NDoctor.Domain.DTO.Exceptions;

    public class GoogleService
    {
        #region Fields

        private const string SynchronisationProperty = "nDoctorSynchronisationId";

        private readonly ILog Logger = LogManager.GetLogger(typeof(GoogleService));

        #endregion Fields

        #region Constructors

        public GoogleService(GoogleConfiguration config)
        {
            this.UserName = config.UserName;
            this.Password = config.Password;
            this.CalendarUri = config.CalendarUri;
        }

        #endregion Constructors

        #region Properties

        public string CalendarUri
        {
            get;
            private set;
        }

        public string Password
        {
            get;
            private set;
        }

        public string UserName
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds the specified appointment into the configured Google Calendar.
        /// This action is done into a thread and the action will be done when it's done
        /// </summary>
        /// <param name="appointment">The appointment to insert.</param>
        public void AddAppointment(Appointment appointment)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                try
                {
                    var service = this.GetService();

                    var entry = new EventEntry();
                    entry.Title.Text = appointment.Subject;
                    entry.Times.Add(new When()
                    {
                        StartTime = appointment.StartTime,
                        EndTime = appointment.EndTime,
                    });

                    var postUri = new Uri(this.CalendarUri);
                    entry = service.Insert(postUri, entry) as EventEntry;

                    entry.ExtensionElements.Add(new ExtendedProperty()
                    {
                        Name = SynchronisationProperty,
                        Value = appointment.GoogleSynchronisationId.ToString(),
                    });
                    entry.Update();
                    this.Logger.Info("Appointment correctly added into Google Calendar.");
                }
                catch (Exception ex) { throw new GoogleCalendarException(ex.Message, ex); }
            });
        }

        /// <summary>
        /// Removes the specified appointment into the configured Google Calendar.
        /// This action is done into a thread and the action will be done when it's done.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        public void RemoveAppointment(Appointment appointment)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                try
                {
                    //If the appointment to remove is binded to a Google Calendar appointment then do nothing
                    if (appointment.GoogleSynchronisationId == null || appointment.GoogleSynchronisationId == new Guid()) { return; }

                    var service = GetService();
                    var query = new EventQuery()
                    {
                        Uri = new Uri(this.CalendarUri),
                        ExtraParameters = string.Format("extq=[{0}:{1}]", SynchronisationProperty, appointment.GoogleSynchronisationId),
                    };

                    var feed = service.Query(query) as EventFeed;

                    var appointments = new List<EventEntry>();
                    while (feed != null && feed.Entries.Count > 0)
                    {
                        foreach (EventEntry entry in feed.Entries)
                        {
                            appointments.Add(entry);
                        }

                        if (feed.NextChunk != null)
                        {
                            query.Uri = new Uri(feed.NextChunk);
                            feed = service.Query(query) as EventFeed;
                        }
                        else { feed = null; }

                        if (appointments.Count == 1)
                        {
                            foreach (var a in appointments)
                            {
                                a.Delete();
                                this.Logger.Info("Appointment correctly removed from Google Calendar.");
                            }
                        }
                        else if (appointments.Count == 0) { this.Logger.Debug("This appointment doesn't exist in Google Calendar"); }
                        else { throw new Exception("This id exist more than once"); }
                    }
                }
                catch (Exception ex) { throw new GoogleCalendarException(ex.Message, ex); }
            });
        }

        /// <summary>
        /// Process a dummy query to avoid the spin-up time of the first query. 
        /// All this work is done in a thread, it is then invisible from user.
        /// </summary>
        public void SpinUp()
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                this.Logger.Debug("Spinning up Google Calendar");
                try
                {
                    using (new Benchmark(e => this.Logger.DebugFormat("Google calendar loaded in {0,2},{1,3} sec", e.Seconds, e.Milliseconds)))
                    {
                        this.GetAppointments(DateTime.Today, new Tag());
                    }
                }
                catch (Exception ex) { this.Logger.Warn("Silent Exception: an error occured when spinning-up access Google Calendar.", ex); }
            });
        }

        internal IList<Appointment> GetAppointments(DateTime day, Tag googleTag)
        {
            try
            {
                var service = this.GetService();

                var appointments = new List<Appointment>();
                var query = new EventQuery()
                {
                    Uri = new Uri(this.CalendarUri),
                    StartTime = day.Date,
                    EndTime = day.Date.AddDays(1),
                };

                var feed = service.Query(query) as EventFeed;

                while (feed != null && feed.Entries.Count > 0)
                {
                    foreach (EventEntry entry in feed.Entries)
                    {
                        appointments.Add(new Appointment()
                        {
                            Subject = entry.Title.Text,
                            StartTime = entry.Times[0].StartTime,
                            EndTime = entry.Times[0].EndTime,
                            GoogleSynchronisationId = GetExtentedPropertyValue(entry),
                            Tag = googleTag,
                        });
                    }

                    if (feed.NextChunk != null)
                    {
                        query.Uri = new Uri(feed.NextChunk);
                        feed = service.Query(query) as EventFeed;
                    }
                    else { feed = null; }
                }

                var temp = (from a in appointments
                            where a.GoogleSynchronisationId == null
                               || a.GoogleSynchronisationId == new Guid()
                            select a).ToList();
                return temp;
            }
            catch (Exception ex) { throw new GoogleCalendarException(ex.Message, ex); }
        }

        private Guid GetExtentedPropertyValue(EventEntry entry)
        {
            var result = new Guid();

            foreach (var extension in entry.ExtensionElements)
            {
                if (extension is ExtendedProperty)
                {
                    var property = extension as ExtendedProperty;
                    if (property.Name == SynchronisationProperty)
                    {
                        result = new Guid(property.Value);
                    }
                    break;
                }
            }
            return result;
        }

        private CalendarService GetService()
        {
            var service = new CalendarService("nDoctor");
            service.setUserCredentials(this.UserName, this.Password);
            return service;
        }

        #endregion Methods
    }
}