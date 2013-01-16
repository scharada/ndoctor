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
namespace Probel.NDoctor.Domain.DAL.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Data;
    using Probel.NDoctor.Domain.DAL.AopConfiguration;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.GoogleCalendar;
    using Probel.NDoctor.Domain.DAL.Helpers;
    using Probel.NDoctor.Domain.DAL.Subcomponents;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.GoogleCalendar;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Provides features to manage the meetings
    /// </summary>
    public class CalendarComponent : BaseComponent, ICalendarComponent
    {
        #region Constructors

        public CalendarComponent(ISession session)
            : base(session)
        {
        }

        public CalendarComponent()
            : base()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates the specified meeting.
        /// </summary>
        /// <param name="meeting">The meeting.</param>
        /// <param name="patient">The patient.</param>
        [Granted(To.EditCalendar)]
        public void Create(AppointmentDto meeting, LightPatientDto patient)
        {
            new Creator(this.Session).Create(meeting, patient);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        /// <returns>
        /// The id of the just created item
        /// </returns>
        public long Create(TagDto item)
        {
            return new Creator(this.Session).Create(item);
        }

        /// <summary>
        /// Creates the specified appointment and uses the Google Calendar configuration
        /// to create (or not) this appointment into the Google Calendar.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <param name="patient">The patient.</param>
        /// <param name="config">The config.</param>
        [Granted(To.EditCalendar)]
        public void Create(AppointmentDto appointment, LightPatientDto patient, GoogleConfiguration config)
        {
            new Creator(this.Session).Create(appointment, patient, config);
        }

        /// <summary>
        /// Gets all the appointments of the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public IList<AppointmentDto> GetAppointments(LightPatientDto patient)
        {
            var entity = this.Session.Get<Patient>(patient.Id);
            return Mapper.Map<IList<Appointment>, IList<AppointmentDto>>(entity.Appointments);
        }

        /// <summary>
        /// Gets all the appointments between the specified start and end threshold for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="startThreshold">The start threshold.</param>
        /// <param name="endThreshold">The end threshold.</param>
        /// <returns></returns>
        public IList<AppointmentDto> GetAppointments(LightPatientDto patient, DateTime startThreshold, DateTime endThreshold)
        {
            var result = this.GetAppointments(patient);

            return (from r in result
                    where r.StartTime >= startThreshold
                       && r.EndTime <= endThreshold.AddDays(1)
                    select r).ToList();
        }

        /// <summary>
        /// Gets the appointments for the specified day.
        /// </summary>
        /// <param name="day">The day.</param>
        /// <returns>A list of appointments</returns>
        public IList<AppointmentDto> GetAppointments(DateTime day)
        {
            return this.GetAppointments(day, GoogleConfiguration.NotBinded);
        }

        /// <summary>
        /// Gets the appointments for the specified day and add all the appointments from Google Calendar.
        /// </summary>
        /// <param name="day">The day.</param>
        /// <returns>A list of appointments</returns>
        public IList<AppointmentDto> GetAppointments(DateTime day, GoogleConfiguration config)
        {
            var result = (from a in this.Session.Query<Appointment>()
                          where a.StartTime >= day.Date
                             && a.EndTime <= day.Date.AddDays(1)
                          select a).ToList();

            if (config.IsActive)
            {
                result.AddRange(new GoogleService(config).GetAppointments(day, this.GetGoogleTag()));
            }

            return Mapper.Map<IList<Appointment>, IList<AppointmentDto>>(result);
        }

        /// <summary>
        /// Gets the patients that fullfill the specified criterium.
        /// </summary>
        /// <param name="criterium">The criterium.</param>
        /// <param name="search">The search should be done on the specified property.</param>
        /// <returns></returns>
        public IList<LightPatientDto> GetPatientsByNameLight(string criterium, SearchOn search)
        {
            return new Selector(this.Session).GetPatientByNameLight(criterium, search);
        }

        /// <summary>
        /// The doctor/secretary uses this method to Get free allowable time for a appointment with a patient.
        /// </summary>
        /// <param name="startDate">The starting point for the search. That's, the search won't try to Get free slots before this date (included)</param>
        /// <param name="endDate">The end point for the search. That's, the search won't try to Get free slots after this date (included)</param>
        /// <param name="workday">The workday is defined by a start and an end time. A classic workday starts at 8:00 and finishes at 17:00. In other
        /// words, the method will search free slots between 8:00 and 17:00</param>
        /// <returns>
        /// A list of free allowable slots
        /// </returns>
        [Granted(To.EditCalendar)]
        public TimeSlotCollection GetSlots(DateTime startDate, DateTime endDate, Workday workday)
        {
            startDate = startDate.Date;
            endDate = endDate.Date;

            var slots = TimeSlotCollection.Create(startDate, endDate, workday);

            //Get appointments between start and end date.
            var appointments = (from a in this.Session.Query<Appointment>()
                                where a.StartTime >= startDate
                                   && a.EndTime <= endDate.AddDays(1)
                                select a).ToList();

            //Remove slots that are overlapping with aleady taken meetings
            var result = (from slot in slots
                          where IsNotOverlapping(appointments, slot)
                          select slot);

            return TimeSlotCollection.Create(result);
        }

        /// <summary>
        /// Gets all the tags with the specified catagory.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public IList<TagDto> GetTags(TagCategory category)
        {
            return new Selector(this.Session).GetTags(category);
        }

        /// <summary>
        /// Determines whether the google calendar tag is in the database.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the google calendar tag is in the database; otherwise, <c>false</c>.
        /// </returns>
        internal bool HasGoogleCalendarTag()
        {
            return (from t in this.Session.Query<Tag>()
                    where t.Category == TagCategory.Appointment
                    && t.Name == Default.GoogleCalendarTagName
                    select t).Count() > 0;
        }

        /// <summary>
        /// Removes the specified meeting.
        /// </summary>
        /// <param name="meeting">The meeting.</param>
        /// <param name="patient">The patient.</param>
        [Granted(To.EditCalendar)]
        public void Remove(AppointmentDto meeting, LightPatientDto patient)
        {
            new Remover(this.Session).Remove(meeting, patient);
        }

        /// <summary>
        /// Removes the specified meeting.
        /// </summary>
        /// <param name="meeting">The meeting.</param>
        /// <param name="patient">The patient.</param>
        /// <param name="config">The configuration that will be used to remove the appointment from Google Calendar if exist.</param>
        [Granted(To.EditCalendar)]
        public void Remove(AppointmentDto meeting, LightPatientDto patient, GoogleConfiguration config)
        {
            var entity = this.Session.Get<Appointment>(meeting.Id);
            if (config.IsActive)
            {
                new GoogleService(config).RemoveAppointment(entity);
            }
            this.Remove(meeting, patient);
        }

        /// <summary>
        /// Execute a dummy query to Google Calendar to spin it up.
        /// If an error occured, it is gracefully swallowed and logged.
        /// </summary>
        public void SpinUpGoogle(GoogleConfiguration config)
        {
            new GoogleService(config).SpinUp();
        }

        /// <summary>
        /// Gets the google tag.
        /// </summary>
        /// <returns></returns>
        private Tag GetGoogleTag()
        {
            try
            {
                return (from tag in this.Session.Query<Tag>()
                        where tag.Category == TagCategory.Appointment
                           && tag.Name == Default.GoogleCalendarTagName
                        select tag).First();
            }
            catch (Exception ex) { throw new EntityNotFoundException(typeof(TagDto), ex); }
        }

        private bool IsNotOverlapping(List<Appointment> appointments, DateRange slot)
        {
            return (from appointment in appointments
                    where appointment.DateRange.Overlaps(slot)
                    select appointment).Count() == 0;
        }

        #endregion Methods
    }
}