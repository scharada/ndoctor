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
    using AutoMapper;

    using NHibernate;

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

    public class AdministrationComponent : BaseComponent, IAdministrationComponent
    {
        #region Methods

        /// <summary>
        /// Updates the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public void Update(TagDto tag)
        {
            var entity = Mapper.Map<TagDto, Tag>(tag);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified profession.
        /// </summary>
        /// <param name="profession">The tag.</param>
        public void Update(ProfessionDto profession)
        {
            var entity = Mapper.Map<ProfessionDto, Profession>(profession);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified reputation.
        /// </summary>
        /// <param name="reputation">The tag.</param>
        public void Update(ReputationDto reputation)
        {
            var entity = Mapper.Map<ReputationDto, Reputation>(reputation);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified drug.
        /// </summary>
        /// <param name="drug">The drug.</param>
        public void Update(DrugDto drug)
        {
            var entity = Mapper.Map<DrugDto, Drug>(drug);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified pathology.
        /// </summary>
        /// <param name="pathology">The drug.</param>
        public void Update(PathologyDto pathology)
        {
            var entity = Mapper.Map<PathologyDto, Pathology>(pathology);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified practice.
        /// </summary>
        /// <param name="practice">The drug.</param>
        public void Update(PracticeDto practice)
        {
            var entity = Mapper.Map<PracticeDto, Practice>(practice);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified insurance.
        /// </summary>
        /// <param name="insurance">The drug.</param>
        public void Update(InsuranceDto insurance)
        {
            var entity = Mapper.Map<InsuranceDto, Insurance>(insurance);
            this.Session.Update(entity);
        }

        #endregion Methods
    }
}