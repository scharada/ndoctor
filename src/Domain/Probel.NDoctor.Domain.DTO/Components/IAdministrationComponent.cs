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

    public interface IAdministrationComponent : IBaseComponent
    {
        #region Methods

        /// <summary>
        /// Determines whether the specified item can be removed.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified item; otherwise, <c>false</c>.
        /// </returns>
        bool CanRemove(InsuranceDto item);

        /// <summary>
        /// Determines whether the specified item can be removed.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified item; otherwise, <c>false</c>.
        /// </returns>
        bool CanRemove(PracticeDto item);

        /// <summary>
        /// Determines whether the specified item can be removed.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified item; otherwise, <c>false</c>.
        /// </returns>
        bool CanRemove(PathologyDto item);

        /// <summary>
        /// Determines whether this instance can remove the specified drug dto.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified drug dto; otherwise, <c>false</c>.
        /// </returns>
        bool CanRemove(DrugDto item);

        /// <summary>
        /// Determines whether this instance can remove the specified reputation dto.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified reputation dto; otherwise, <c>false</c>.
        /// </returns>
        bool CanRemove(ReputationDto item);

        /// <summary>
        /// Determines whether this instance can remove the specified tag dto.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified tag dto; otherwise, <c>false</c>.
        /// </returns>
        bool CanRemove(TagDto item);

        /// <summary>
        /// Determines whether this instance can remove the specified profession dto.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified profession dto; otherwise, <c>false</c>.
        /// </returns>
        bool CanRemove(ProfessionDto item);

        /// <summary>
        /// Determines whether the specified doctor can be removed.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified item; otherwise, <c>false</c>.
        /// </returns>
        bool CanRemove(DoctorDto item);

        /// <summary>
        /// Creates the specified profession.
        /// </summary>
        /// <param name="profession">The tag.</param>
        long Create(ProfessionDto profession);

        /// <summary>
        /// Creates the specified reputation.
        /// </summary>
        /// <param name="reputation">The tag.</param>
        long Create(ReputationDto reputation);

        /// <summary>
        /// Creates the specified pathology.
        /// </summary>
        /// <param name="pathology">The drug.</param>
        long Create(PathologyDto pathology);

        /// <summary>
        /// Creates the specified practice.
        /// </summary>
        /// <param name="practice">The drug.</param>
        long Create(PracticeDto practice);

        /// <summary>
        /// Creates the specified insurance.
        /// </summary>
        /// <param name="insurance">The drug.</param>
        long Create(InsuranceDto insurance);

        /// <summary>
        /// Creates the specified doctor.
        /// </summary>
        /// <param name="doctor">The doctor.</param>
        /// <returns></returns>
        long Create(DoctorDto doctor);

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        long Create(DrugDto item);

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        /// <returns>The id of the just created item</returns>
        long Create(TagDto item);

        /// <summary>
        /// Gets all doctors.
        /// </summary>
        /// <returns></returns>
        IList<DoctorDto> GetAllDoctors();

        /// <summary>
        /// Gets all drugs from the database.
        /// </summary>
        /// <returns></returns>
        IList<DrugDto> GetAllDrugs();

        /// <summary>
        /// Gets all insurances stored in the database.
        /// </summary>
        /// <returns></returns>
        IList<InsuranceDto> GetAllInsurances();

        /// <summary>
        /// Gets all pathologies stored in the database.
        /// </summary>
        /// <returns></returns>
        IList<PathologyDto> GetAllPathologies();

        /// <summary>
        /// Gets all practices stored in the database.
        /// </summary>
        /// <returns></returns>
        IList<PracticeDto> GetAllPractices();

        /// <summary>
        /// Gets all professions stored in the database.
        /// </summary>
        /// <returns></returns>
        IList<ProfessionDto> GetAllProfessions();

        /// <summary>
        /// Gets all reputations stored in the database.
        /// </summary>
        /// <returns></returns>
        IList<ReputationDto> GetAllReputations();

        /// <summary>
        /// Gets all the tags
        /// </summary>
        /// <returns></returns>
        IList<TagDto> GetAllTags();

        /// <summary>
        /// Gets all the tags with the specified catagory.
        /// </summary>
        /// <returns></returns>
        IList<TagDto> GetTags(TagCategory category);

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove</param>
        void Remove(InsuranceDto item);

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove</param>
        void Remove(PracticeDto item);

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove</param>
        void Remove(PathologyDto item);

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove</param>
        void Remove(DrugDto item);

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove</param>
        void Remove(ProfessionDto item);

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove</param>
        void Remove(ReputationDto item);

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove</param>
        void Remove(TagDto item);

        /// <summary>
        /// Removes the specified doctor.
        /// </summary>
        /// <param name="item">The item.</param>
        void Remove(DoctorDto item);

        /// <summary>
        /// Updates the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        void Update(TagDto tag);

        /// <summary>
        /// Updates the specified profession.
        /// </summary>
        /// <param name="tag">The tag.</param>
        void Update(ProfessionDto tag);

        /// <summary>
        /// Updates the specified reputation.
        /// </summary>
        /// <param name="reputation">The tag.</param>
        void Update(ReputationDto reputation);

        /// <summary>
        /// Updates the specified drug.
        /// </summary>
        /// <param name="drug">The drug.</param>
        void Update(DrugDto drug);

        /// <summary>
        /// Updates the specified pathology.
        /// </summary>
        /// <param name="pathology">The drug.</param>
        void Update(PathologyDto pathology);

        /// <summary>
        /// Updates the specified practice.
        /// </summary>
        /// <param name="practice">The drug.</param>
        void Update(PracticeDto practice);

        /// <summary>
        /// Updates the specified insurance.
        /// </summary>
        /// <param name="insurance">The drug.</param>
        void Update(InsuranceDto insurance);

        /// <summary>
        /// Updates the specified doctor.
        /// </summary>
        /// <param name="item">The item.</param>
        void Update(DoctorDto item);

        #endregion Methods
    }
}