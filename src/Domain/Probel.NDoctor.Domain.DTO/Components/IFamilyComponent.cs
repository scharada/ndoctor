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

    public interface IFamilyComponent : IBaseComponent
    {
        #region Methods

        /// <summary>
        /// Adds the new child to the specified patient.
        /// Under the hood, the child will receive the specifed
        /// patient as a father or a mother depending on the gender
        /// of the patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="child">The child.</param>
        void AddNewChild(LightPatientDto patient, LightPatientDto child);

        /// <summary>
        /// Adds the new parent to the specified patient. A check
        /// on the gende of the parent will infer whether the parent
        /// is the father or the mother
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="parent">The parent.</param>
        void AddNewParent(LightPatientDto patient, LightPatientDto parent);

        /// <summary>
        /// Get all family members for the specified Patient
        /// </summary>
        /// <param name="patient">The connected patient</param>
        /// <returns>The list of the family members</returns>
        IList<LightPatientDto> GetAllFamilyMembers(LightPatientDto patient);

        /// <summary>
        /// Gets the family of the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        FamilyDto GetFamily(LightPatientDto patient);

        /// <summary>
        /// Get all the patient respecting the criteria and the search mode which
        /// aren't in the family of the specified patient
        /// </summary>
        /// <param name="patient">The patient connected into the session</param>
        /// <param name="criteria">The search criteria</param>
        /// <param name="search">The search mode</param>
        /// <returns>A list of patient</returns>
        IList<LightPatientDto> GetPatientNotFamilyMembers(LightPatientDto patient, string criteria, SearchOn search);

        /// <summary>
        /// Gets all the tags with the specified catagory.
        /// </summary>
        /// <returns></returns>
        IList<TagDto> GetTags(TagCategory category);

        /// <summary>
        /// Remove the specified member from the specified patient
        /// </summary>
        /// <param name="member">The family member to remove</param>
        /// <param name="patient">The family of this patient will be updated</param>
        void RemoveFamilyMember(LightPatientDto member, LightPatientDto patient);

        #endregion Methods
    }
}