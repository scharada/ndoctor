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
    using Probel.NDoctor.Domain.DTO.Objects;

    public interface IAdministrationComponent : IBaseComponent
    {
        #region Methods

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

        #endregion Methods
    }
}