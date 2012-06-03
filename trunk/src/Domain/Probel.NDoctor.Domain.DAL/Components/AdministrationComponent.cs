﻿/*
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
namespace Probel.NDoctor.Domain.DAL.Components
{
    using System.Linq;

    using AutoMapper;

    using NHibernate.Linq;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Properties;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;

    public class AdministrationComponent : BaseComponent, IAdministrationComponent
    {
        #region Methods

        /// <summary>
        /// Determines whether the specified item can be removed.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRemove(PathologyDto item)
        {
            return (from t in this.Session.Query<Patient>()
                    where t.IllnessHistory.Where(e => e.Pathology.Id == item.Id).Count() > 0
                    select t).Count() == 0;
        }

        /// <summary>
        /// Determines whether the specified item can be removed.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRemove(InsuranceDto item)
        {
            return (from t in this.Session.Query<Patient>()
                    where t.Insurance.Id == item.Id
                    select t).Count() == 0;
        }

        /// <summary>
        /// Determines whether the specified item can be removed.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRemove(PracticeDto item)
        {
            return (from t in this.Session.Query<Patient>()
                    where t.Practice.Id == item.Id
                    select t).Count() == 0;
        }

        /// <summary>
        /// Creates the specified profession.
        /// </summary>
        /// <param name="profession">The tag.</param>
        public long Create(ProfessionDto profession)
        {
            var entity = Mapper.Map<ProfessionDto, Profession>(profession);
            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Creates the specified reputation.
        /// </summary>
        /// <param name="reputation">The tag.</param>
        public long Create(ReputationDto reputation)
        {
            var entity = Mapper.Map<ReputationDto, Reputation>(reputation);
            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Creates the specified pathology.
        /// </summary>
        /// <param name="pathology">The drug.</param>
        public long Create(PathologyDto pathology)
        {
            var entity = Mapper.Map<PathologyDto, Pathology>(pathology);
            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Creates the specified practice.
        /// </summary>
        /// <param name="practice">The drug.</param>
        public long Create(PracticeDto practice)
        {
            var entity = Mapper.Map<PracticeDto, Practice>(practice);
            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Creates the specified insurance.
        /// </summary>
        /// <param name="insurance">The drug.</param>
        public long Create(InsuranceDto insurance)
        {
            var entity = Mapper.Map<InsuranceDto, Insurance>(insurance);
            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove</param>
        public void Remove(PathologyDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");
            if (!this.CanRemove(item)) throw new ReferencialIntegrityException(Messages.Ex_ReferencialIntegrityException_Deletion);
            this.Remove<Pathology>(item);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove</param>
        public void Remove(DrugDto item)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove</typeparam>
        /// <param name="id">The id of the item to remove.</param>
        public void Remove(InsuranceDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");
            if (!this.CanRemove(item)) throw new ReferencialIntegrityException(Messages.Ex_ReferencialIntegrityException_Deletion);
            this.Remove<Insurance>(item);
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove</typeparam>
        /// <param name="id">The id of the item to remove.</param>
        public void Remove(PracticeDto item)
        {
            Assert.IsNotNull(item, "item");
            if (!this.CanRemove(item)) throw new ReferencialIntegrityException(Messages.Ex_ReferencialIntegrityException_Deletion);
            this.Remove<Practice>(item);
        }

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