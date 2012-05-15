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

    public interface IPrescriptionComponent : IBaseComponent
    {
        #region Methods

        /// <summary>
        /// Creates the specified prescription document for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="document">The document.</param>
        void Create(PrescriptionDocumentDto document, LightPatientDto patient);

        /// <summary>
        /// Finds the prescriptions between the specified dates for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>A list of prescriptions</returns>
        IList<PrescriptionDocumentDto> FindPrescriptionsByDates(LightPatientDto patient, DateTime start, DateTime end);

        #endregion Methods
    }
}