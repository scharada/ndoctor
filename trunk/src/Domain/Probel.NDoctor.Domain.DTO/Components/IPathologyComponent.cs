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
namespace Probel.NDoctor.Domain.DTO.Components
{
    using System;
    using System.Collections.Generic;

    using Probel.Helpers.Data;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Provides all the features to query the database about the 
    /// pathologies
    /// </summary>
    public interface IPathologyComponent : IBaseComponent
    {
        #region Methods

        /// <summary>
        /// Creates the specified pathology.
        /// </summary>
        /// <param name="item">The item.</param>
        void Create(PathologyDto item);

        /// <summary>
        /// Creates the specified illness period for the specified patient.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <param name="patient">The patient.</param>
        void Create(IllnessPeriodDto period, LightPatientDto patient);

        /// <summary>
        /// Gets the illness history for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>The history of illness periods</returns>
        IllnessHistoryDto GetIllnessHistory(LightPatientDto patient);

        /// <summary>
        /// Gets the ilness as a chart. That's a X/Y axes chart when X axes is
        /// </summary>
        /// <param name="patient">The patient used to create the chart.</param>
        /// <returns>
        /// A <see cref="Chart"/> with X axes as pathology name and Y axes
        /// as <see cref="TimeSpan"/> for the duration of the illness.
        /// </returns>
        Chart<string, double> GetIlnessAsChart(LightPatientDto patient);

        /// <summary>
        /// Removes the specified illness period list from the specified patient's
        /// illness history.
        /// </summary>
        /// <param name="illnessPeriods">The illness periods.</param>
        /// <param name="patient">The patient.</param>
        void Remove(IList<IllnessPeriodDto> illnessPeriods, LightPatientDto patient);

        /// <summary>
        /// Removes the specified illness period from the specified patient's
        /// illness history.
        /// </summary>
        /// <param name="illnessPeriod">The illness period.</param>
        /// <param name="patient">The patient.</param>
        void Remove(IllnessPeriodDto illnessPeriod, LightPatientDto patient);

        #endregion Methods
    }
}