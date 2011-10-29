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

namespace Probel.NDoctor.Domain.DAL.Components
{
    using System;
    using System.Collections.Generic;

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;

    public interface ICalendarComponent : IBaseComponent
    {
        #region Methods

        /// <summary>
        /// Creates the specified meeting.
        /// </summary>
        /// <param name="meeting">The meeting.</param>
        /// <param name="patient">The patient.</param>
        void Create(AppointmentDto meeting, LightPatientDto patient);

        /// <summary>
        /// Finds all the appointments of the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        IList<AppointmentDto> FindAppointments(LightPatientDto patient);

        /// <summary>
        /// Finds all the appointments between the specified start and end threshold for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="startThreshold">The start threshold.</param>
        /// <param name="endThreshold">The end threshold.</param>
        /// <returns></returns>
        IList<AppointmentDto> FindAppointments(LightPatientDto patient, DateTime startThreshold, DateTime endThreshold);

        /// <summary>
        /// Finds the appointments for the specified day.
        /// </summary>
        /// <param name="day">The day.</param>
        /// <returns>A list of appointments</returns>
        IList<AppointmentDto> FindAppointments(DateTime day);

        /// <summary>
        /// Finds the slots.
        /// </summary>
        /// <param name="startDate">From.</param>
        /// <param name="endDate">To.</param>
        /// <param name="timeSpan">The time span.</param>
        TimeSlotCollection FindSlots(DateTime startDate, DateTime endDate, Workday workday);

        /// <summary>
        /// Removes the specified meeting.
        /// </summary>
        /// <param name="meeting">The meeting.</param>
        /// <param name="patient">The patient.</param>
        void Remove(AppointmentDto meeting, LightPatientDto patient);

        #endregion Methods
    }
}