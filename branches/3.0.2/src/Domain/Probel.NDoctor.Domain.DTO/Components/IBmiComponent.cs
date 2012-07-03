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

    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Represents all the features a Bmi record should have
    /// </summary>
    public interface IBmiComponent : IBaseComponent
    {
        #region Methods

        /// <summary>
        /// Adds a bmi entry to the specified patient.
        /// </summary>
        /// <param name="bmi">The bmi.</param>
        /// <param name="patient">The patient.</param>
        void CreateBmi(BmiDto bmi, LightPatientDto patient);

        /// <summary>
        /// Finds the bmi history for the specified patient and that are between the start and end date.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns>A list of BMI</returns>
        IList<BmiDto> FindBmiHistory(LightPatientDto patient, DateTime from, DateTime to);

        /// <summary>
        /// Gets all bmi history for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>A list of BMI</returns>
        IList<BmiDto> GetAllBmiHistory(LightPatientDto patient);

        /// <summary>
        /// Gets the bmi history of the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        PatientBmiDto GetPatientWithBmiHistory(LightPatientDto patient);

        /// <summary>
        /// Removes the specified BMI entry from the specified patient.
        /// </summary>
        /// <param name="bmi">The bmi to remove.</param>
        /// <param name="from">The patient.</param>
        void Remove(BmiDto bmi, LightPatientDto from);

        /// <summary>
        /// Deletes the bmi with the specified date.
        /// </summary>
        /// <param name="patient">The patient to be processed.</param>
        /// <param name="date">The date to remove.</param>
        void RemoveBmiWithDate(LightPatientDto patient, DateTime date);

        #endregion Methods
    }
}