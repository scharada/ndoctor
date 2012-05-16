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

    using NHibernate.Linq;

    using Probel.Helpers.Data;
    using Probel.NDoctor.Domain.DAL.Entities;
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
        public void Create(AppointmentDto meeting, LightPatientDto patient)
        {
            var patientEntity = this.Session.Get<Patient>(patient.Id);
            var meetingEntity = Mapper.Map<AppointmentDto, Appointment>(meeting);

            patientEntity.Appointments.Add(meetingEntity);
            this.Session.SaveOrUpdate(patientEntity);
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
        /// Finds the slots.
        /// </summary>
        /// <param name="startDate">From.</param>
        /// <param name="endDate">To.</param>
        /// <param name="timeSpan">The time span.</param>
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
        public void Remove(AppointmentDto meeting, LightPatientDto patient)
        {
            var patientEntity = this.Session.Get<Patient>(patient.Id);

            var toRemove = (from a in patientEntity.Appointments
                            where a.Id == meeting.Id
                            select a).FirstOrDefault();
            if (toRemove == null) throw new EntityNotFoundException(typeof(Appointment));

            patientEntity.Appointments.Remove(toRemove);

            this.Session.Update(patientEntity);
            this.Session.Delete(toRemove);
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