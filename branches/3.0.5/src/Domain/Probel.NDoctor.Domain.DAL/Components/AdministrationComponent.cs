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
namespace Probel.NDoctor.Domain.DAL.Components
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.NDoctor.Domain.DAL.AopConfiguration;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Subcomponents;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Memory;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Provides management for administration
    /// </summary>
    public class AdministrationComponent : BaseComponent, IAdministrationComponent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdministrationComponent"/> class.
        /// </summary>
        public AdministrationComponent()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdministrationComponent"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public AdministrationComponent(ISession session)
            : base(session)
        {
        }

        #endregion Constructors

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
            return new Remover(this.Session).CanRemove(item);
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
            return new Remover(this.Session).CanRemove(item);
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
            return new Remover(this.Session).CanRemove(item);
        }

        /// <summary>
        /// Determines whether this instance can remove the specified drug dto.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified drug dto; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRemove(DrugDto item)
        {
            return new Remover(this.Session).CanRemove(item);
        }

        /// <summary>
        /// Determines whether this instance can remove the specified reputation dto.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified reputation dto; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRemove(ReputationDto item)
        {
            return new Remover(this.Session).CanRemove(item);
        }

        /// <summary>
        /// Determines whether this instance can remove the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRemove(TagDto item)
        {
            return new Remover(this.Session).CanRemove(item);
        }

        public bool CanRemove(ProfessionDto item)
        {
            return new Remover(this.Session).CanRemove(item);
        }

        /// <summary>
        /// Determines whether the specified doctor can be removed.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRemove(DoctorDto item)
        {
            return new Remover(this.Session).CanRemove(item);
        }

        /// <summary>
        /// Creates the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        [Granted(To.Everyone)]
        public long Create(TagDto tag)
        {
            return new Creator(this.Session).Create(tag);
        }

        /// <summary>
        /// Creates the specified profession.
        /// </summary>
        /// <param name="profession">The tag.</param>
        public long Create(ProfessionDto profession)
        {
            return new Creator(this.Session).Create(profession);
        }

        /// <summary>
        /// Creates the specified reputation.
        /// </summary>
        /// <param name="reputation">The tag.</param>
        public long Create(ReputationDto reputation)
        {
            return new Creator(this.Session).Create(reputation);
        }

        /// <summary>
        /// Creates the specified pathology.
        /// </summary>
        /// <param name="pathology">The drug.</param>
        public long Create(PathologyDto pathology)
        {
            return new Creator(this.Session).Create(pathology);
        }

        /// <summary>
        /// Creates the specified practice.
        /// </summary>
        /// <param name="practice">The drug.</param>
        public long Create(PracticeDto practice)
        {
            return new Creator(this.Session).Create(practice);
        }

        /// <summary>
        /// Creates the specified insurance.
        /// </summary>
        /// <param name="insurance">The drug.</param>
        public long Create(InsuranceDto insurance)
        {
            return new Creator(this.Session).Create(insurance);
        }

        /// <summary>
        /// Creates the specified doctor.
        /// </summary>
        /// <param name="doctor">The doctor.</param>
        /// <returns></returns>
        public long Create(DoctorDto doctor)
        {
            return new Creator(this.Session).Create(doctor);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        /// <returns></returns>
        public long Create(DrugDto item)
        {
            return new Creator(this.Session).Create(item);
        }

        /// <summary>
        /// Gets all doctors.
        /// </summary>
        /// <returns></returns>
        public IList<DoctorDto> GetAllDoctors()
        {
            var entities = (from d in this.Session.Query<Doctor>()
                            select d).ToList();
            return Mapper.Map<IList<Doctor>, IList<DoctorDto>>(entities);
        }

        /// <summary>
        /// Gets all drugs from the database.
        /// </summary>
        /// <returns></returns>
        public IList<DrugDto> GetAllDrugs()
        {
            return new Selector(this.Session).GetAllDrugs();
        }

        /// <summary>
        /// Gets all insurances stored in the database.
        /// </summary>
        /// <returns></returns>
        public IList<InsuranceDto> GetAllInsurances()
        {
            return new Selector(this.Session).GetAllInsurances();
        }

        /// <summary>
        /// Gets all pathologies stored in the database.
        /// </summary>
        /// <returns></returns>
        public IList<PathologyDto> GetAllPathologies()
        {
            return new Selector(this.Session).GetAllPathologies();
        }

        /// <summary>
        /// Gets all practices stored in the database.
        /// </summary>
        /// <returns></returns>
        public IList<PracticeDto> GetAllPractices()
        {
            return new Selector(this.Session).GetAllPractices();
        }

        /// <summary>
        /// Gets all professions stored in the database.
        /// </summary>
        /// <returns></returns>
        public IList<ProfessionDto> GetAllProfessions()
        {
            return new Selector(this.Session).GetAllProfessions();
        }

        /// <summary>
        /// Gets all reputations stored in the database.
        /// </summary>
        /// <returns></returns>
        public IList<ReputationDto> GetAllReputations()
        {
            return new Selector(this.Session).GetAllReputations();
        }

        /// <summary>
        /// Gets all the tags
        /// </summary>
        /// <returns></returns>
        public IList<TagDto> GetAllTags()
        {
            return new Selector(this.Session).GetAllTags();
        }

        /// <summary>
        /// Gets a doctor searcher filled with all the doctors.
        /// </summary>
        /// <returns>
        /// A memory searcher
        /// </returns>
        public DoctorRefiner GetDoctorRefiner()
        {
            var entities = this.Session.Query<Doctor>().AsEnumerable();
            var dto = Mapper.Map<IEnumerable<Doctor>, IEnumerable<DoctorDto>>(entities);
            return new DoctorRefiner(dto);
        }

        /// <summary>
        /// Gets a memory searcher filled with all the drugs.
        /// </summary>
        /// <returns>
        /// A memory searcher
        /// </returns>
        public DrugRefiner GetDrugRefiner()
        {
            var entities = this.Session.Query<Drug>().AsEnumerable();
            var dto = Mapper.Map<IEnumerable<Drug>, IEnumerable<DrugDto>>(entities);
            return new DrugRefiner(dto);
        }

        /// <summary>
        /// Gets a memory searcher filled with all the insurances.
        /// </summary>
        /// <returns>
        /// A memory searcher
        /// </returns>
        public InsuranceRefiner GetInsurancesRefiner()
        {
            var entities = this.Session.Query<Insurance>().AsEnumerable();
            var dto = Mapper.Map<IEnumerable<Insurance>, IEnumerable<InsuranceDto>>(entities);
            return new InsuranceRefiner(dto);
        }

        /// <summary>
        /// Gets a memory searcher filled with all the pathologies.
        /// </summary>
        /// <returns>
        /// A memory searcher
        /// </returns>
        public PathologyRefiner GetPathologyRefiner()
        {
            var entities = this.Session.Query<Pathology>().AsEnumerable();
            var dto = Mapper.Map<IEnumerable<Pathology>, IEnumerable<PathologyDto>>(entities);
            return new PathologyRefiner(dto);
        }

        /// <summary>
        /// Gets a memory searcher filled with all the practices.
        /// </summary>
        /// <returns>
        /// A memory searcher
        /// </returns>
        public PracticeRefiner GetPracticeRefiner()
        {
            var entities = this.Session.Query<Practice>().AsEnumerable();
            var dto = Mapper.Map<IEnumerable<Practice>, IEnumerable<PracticeDto>>(entities);
            return new PracticeRefiner(dto);
        }

        /// <summary>
        /// Gets a memory searcher filled with all the professions.
        /// </summary>
        /// <returns>
        /// A memory searcher
        /// </returns>
        public ProfessionRefiner GetProfessionRefiner()
        {
            var entities = this.Session.Query<Profession>().AsEnumerable();
            var dto = Mapper.Map<IEnumerable<Profession>, IEnumerable<ProfessionDto>>(entities);
            return new ProfessionRefiner(dto);
        }

        /// <summary>
        /// Gets a memory searcher filled with all the reputations.
        /// </summary>
        /// <returns>
        /// A memory searcher
        /// </returns>
        public ReputationRefiner GetReputationRefiner()
        {
            var entities = this.Session.Query<Reputation>().AsEnumerable();
            var dto = Mapper.Map<IEnumerable<Reputation>, IEnumerable<ReputationDto>>(entities);
            return new ReputationRefiner(dto);
        }

        /// <summary>
        /// Gets a memory searcher filled with all the tags.
        /// </summary>
        /// <returns>
        /// A memory searcher
        /// </returns>
        public TagRefiner GetTagRefiner()
        {
            var entities = this.Session.Query<Tag>().AsEnumerable();
            var dto = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagDto>>(entities);
            return new TagRefiner(dto);
        }

        /// <summary>
        /// Gets all the tags with the specified catagory.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public IList<TagDto> GetTags(TagCategory category)
        {
            return new Selector(this.Session).GetTags(category);
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <param name="item"></param>
        public void Remove(TagDto item)
        {
            new Remover(this.Session).Remove(item);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove</param>
        public void Remove(PathologyDto item)
        {
            new Remover(this.Session).Remove(item);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove</param>
        public void Remove(DrugDto item)
        {
            new Remover(this.Session).Remove(item);
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove</typeparam>
        /// <param name="id">The id of the item to remove.</param>
        public void Remove(InsuranceDto item)
        {
            new Remover(this.Session).Remove(item);
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove</typeparam>
        /// <param name="id">The id of the item to remove.</param>
        public void Remove(PracticeDto item)
        {
            new Remover(this.Session).Remove(item);
        }

        /// <summary>
        /// Removes the specified doctor.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(DoctorDto item)
        {
            new Remover(this.Session).Remove(item);
        }

        public void Remove(ProfessionDto item)
        {
            var remover = new Remover(this.Session);

            if (remover.CanRemove(item)) { remover.Remove<Profession>(item); }
        }

        public void Remove(ReputationDto item)
        {
            var remover = new Remover(this.Session);

            if (remover.CanRemove(item)) { remover.Remove<Reputation>(item); }
        }

        /// <summary>
        /// Updates the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public void Update(TagDto tag)
        {
            new Updator(this.Session).Update(tag);
        }

        /// <summary>
        /// Updates the specified profession.
        /// </summary>
        /// <param name="profession">The tag.</param>
        public void Update(ProfessionDto profession)
        {
            new Updator(this.Session).Update(profession);
        }

        /// <summary>
        /// Updates the specified reputation.
        /// </summary>
        /// <param name="reputation">The tag.</param>
        public void Update(ReputationDto reputation)
        {
            new Updator(this.Session).Update(reputation);
        }

        /// <summary>
        /// Updates the specified drug.
        /// </summary>
        /// <param name="drug">The drug.</param>
        public void Update(DrugDto drug)
        {
            new Updator(this.Session).Update(drug);
        }

        /// <summary>
        /// Updates the specified pathology.
        /// </summary>
        /// <param name="pathology">The drug.</param>
        public void Update(PathologyDto pathology)
        {
            new Updator(this.Session).Update(pathology);
        }

        /// <summary>
        /// Updates the specified practice.
        /// </summary>
        /// <param name="practice">The drug.</param>
        public void Update(PracticeDto practice)
        {
            new Updator(this.Session).Update(practice);
        }

        /// <summary>
        /// Updates the specified insurance.
        /// </summary>
        /// <param name="insurance">The drug.</param>
        public void Update(InsuranceDto insurance)
        {
            new Updator(this.Session).Update(insurance);
        }

        /// <summary>
        /// Updates the specified doctor.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Update(DoctorDto item)
        {
            new Updator(this.Session).Update(item);
        }

        #endregion Methods
    }
}