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
    /// 
    /// </summary>
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
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        /// <returns>The id of the just created item</returns>
        long Create(TagDto item);

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        long Create(DrugDto item);

        /// <summary>
        /// Gets the drugs which has in their name the specified criteria.
        /// </summary>
        /// <param name="name">The criteria.</param>
        /// <returns>A list of drugs</returns>
        IList<DrugDto> GetDrugsByName(string name);

        /// <summary>
        /// Gets the drugs by tags.
        /// </summary>
        /// <param name="tag">The tag name.</param>
        /// <returns>A list of drugs</returns>
        IList<DrugDto> GetDrugsByTags(string tagName);

        /// <summary>
        /// Gets the prescriptions between the specified dates for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>A list of prescriptions</returns>
        IList<PrescriptionDocumentDto> GetPrescriptionsByDates(LightPatientDto patient, DateTime start, DateTime end);

        /// <summary>
        /// Gets all the tags with the specified catagory.
        /// </summary>
        /// <returns></returns>
        IList<TagDto> GetTags(TagCategory category);

        /// <summary>
        /// Removes the specified item but doesn't touch the drugs liked to it.
        /// </summary>
        /// <param name="item">The item.</param>
        void Remove(PrescriptionDocumentDto item);

        /// <summary>
        /// Removes the specified prescription.
        /// </summary>
        /// <param name="item">The item.</param>
        void Remove(PrescriptionDto item);

        /// <summary>
        /// Updates the specified prescription.
        /// </summary>
        /// <param name="item">The item.</param>
        void Update(PrescriptionDto item);

        #endregion Methods
    }
}