﻿/*
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

    using NHibernate.Linq;

    using Probel.Helpers.Data;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Subcomponents;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Provides features to manage the meetings
    /// </summary>
    public class CalendarComponent : BaseComponent, ICalendarComponent
    {
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
        /// Finds all the appointments of the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public IList<AppointmentDto> FindAppointments(LightPatientDto patient)
        {
            var entity = this.Session.Get<Patient>(patient.Id);
            return Mapper.Map<IList<Appointment>, IList<AppointmentDto>>(entity.Appointments);
        }

        /// <summary>
        /// Finds all the appointments between the specified start and end threshold for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="startThreshold">The start threshold.</param>
        /// <param name="endThreshold">The end threshold.</param>
        /// <returns></returns>
        public IList<AppointmentDto> FindAppointments(LightPatientDto patient, DateTime startThreshold, DateTime endThreshold)
        {
            var result = this.FindAppointments(patient);

            return (from r in result
                    where r.StartTime >= startThreshold
                       && r.EndTime <= endThreshold.AddDays(1)
                    select r).ToList();
        }

        /// <summary>
        /// Finds the appointments for the specified day.
        /// </summary>
        /// <param name="day">The day.</param>
        /// <returns>A list of appointments</returns>
        public IList<AppointmentDto> FindAppointments(DateTime day)
        {
            var result = (from a in this.Session.Query<Appointment>()
                          where a.StartTime >= day.Date
                             && a.EndTime <= day.Date.AddDays(1)
                          select a).ToList();
            return Mapper.Map<IList<Appointment>, IList<AppointmentDto>>(result);
        }

        /// <summary>
        /// The doctor/secretary uses this method to find free allowable time for a appointment with a patient.
        /// </summary>
        /// <param name="startDate">The starting point for the search. That's, the search won't try to find free slots before this date (included)</param>
        /// <param name="endDate">The end point for the search. That's, the search won't try to find free slots after this date (included)</param>
        /// <param name="workday">The workday is defined by a start and an end time. A classic workday starts at 8:00 and finishes at 17:00. In other
        /// words, the method will search free slots between 8:00 and 17:00</param>
        /// <returns>
        /// A list of free allowable slots
        /// </returns>
        [Granted(To.EditCalendar)]
        public TimeSlotCollection FindSlots(DateTime startDate, DateTime endDate, Workday workday)
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
        /// Removes the specified meeting.
        /// </summary>
        /// <param name="meeting">The meeting.</param>
        /// <param name="patient">The patient.</param>
        [Granted(To.EditCalendar)]
        public void Remove(AppointmentDto meeting, LightPatientDto patient)
        {
            new Remover(this.Session).Remove(meeting, patient);
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