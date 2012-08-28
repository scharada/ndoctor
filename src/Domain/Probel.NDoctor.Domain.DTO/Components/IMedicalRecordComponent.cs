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

    /// <summary>
    /// Provides business logic for the medical record management
    /// </summary>
    public interface IMedicalRecordComponent : IBaseComponent
    {
        #region Methods

        /// <summary>
        /// Creates the specified record and link it to the specidied patient.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="forPatient">For patient.</param>
        void Create(MedicalRecordDto record, LightPatientDto forPatient);

        /// <summary>
        /// Creates the specified macro.
        /// </summary>
        /// <param name="macro">The macro.</param>
        /// <returns>The id of the created macro</returns>
        long Create(MacroDto macro);

        /// <summary>
        /// Finds the medical record by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The medical record or <c>Null</c> if not found </returns>
        MedicalRecordDto FindMedicalRecordById(long id);

        /// <summary>
        /// Gets all the medical records of the specified patient. The records are packed into a 
        /// medical record cabinet which contains medical records folders. Each folder contains a list 
        /// of medical records.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        MedicalRecordCabinetDto FindMedicalRecordCabinet(LightPatientDto patient);

        /// <summary>
        /// Gets all the macros.
        /// </summary>
        /// <returns></returns>
        MacroDto[] GetAllMacros();

        /// <summary>
        /// Gets the history of the specified medical record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns>The items contained in the history</returns>
        IEnumerable<MedicalRecordStateDto> GetHistory(MedicalRecordDto record);

        /// <summary>
        /// Determines whether the specified macro is valid.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        ///   <c>true</c> if macro is valid; otherwise, <c>false</c>.
        /// </returns>
        bool IsValid(MacroDto macro);

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Remove(MacroDto item);

        /// <summary>
        /// Resolves the specified macro with the data of the specified patient.
        /// </summary>
        /// <param name="macro">The macro.</param>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        string Resolve(MacroDto macro, LightPatientDto patient);

        /// <summary>
        /// Revert the specified medical record into the specified state
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="toState">The state.</param>
        void Revert(MedicalRecordDto record, MedicalRecordStateDto toState);

        /// <summary>
        /// Updates the specified macro.
        /// </summary>
        /// <param name="macro">The macro.</param>
        void Update(MacroDto macro);

        /// <summary>
        /// Commits the changes on medical record cabinet.
        /// </summary>
        /// <param name="cabinet">The cabinet.</param>
        void Update(MedicalRecordCabinetDto cabinet);

        /// <summary>
        /// Updates the specified macros.
        /// </summary>
        /// <param name="macros">The macros.</param>
        void Update(IEnumerable<MacroDto> macros);

        #endregion Methods
    }
}