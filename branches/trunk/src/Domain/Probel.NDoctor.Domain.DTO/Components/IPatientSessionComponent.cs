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
    using System.Collections.Generic;

    using Probel.NDoctor.Domain.DTO.Objects;

    #region Enumerations

    /// <summary>
    /// Indicates on what criterium the search should be done
    /// </summary>
    public enum SearchOn
    {
        /// <summary>
        /// Search on the first name
        /// </summary>
        FirstName,
        /// <summary>
        /// Search on the last name
        /// </summary>
        LastName,
        /// <summary>
        /// Search on both the first and the last name
        /// </summary>
        FirstAndLastName
    }

    #endregion Enumerations

    /// <summary>
    /// Get the features of the patient session
    /// </summary>
    public interface IPatientSessionComponent : IBaseComponent
    {
        #region Methods

        /// <summary>
        /// Gets the top X patient. Where X is specified as an argument.
        /// Everytime a user is loaded in memory, a counter is incremented. This
        /// value is used to select the most 'famous' patients.
        /// </summary>
        /// <param name="x">The number of patient this method returns.</param>
        /// <returns>An array of patients</returns>
        IList<LightPatientDto> GetTopXPatient(uint x);

        /// <summary>
        /// Increments the patient counter.
        /// </summary>
        /// <param name="patient">The patient.</param>
        void IncrementPatientCounter(LightPatientDto patient);

        #endregion Methods
    }
}