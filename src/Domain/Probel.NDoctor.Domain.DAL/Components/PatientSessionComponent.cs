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
    using System;
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
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.DTO.Specification;

    /// <summary>
    /// Get the features of the patient session
    /// </summary>
    public class PatientSessionComponent : BaseComponent, IPatientSessionComponent
    {
        #region Constructors

        public PatientSessionComponent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientSessionComponent"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public PatientSessionComponent(ISession session)
            : base(session)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        /// <returns>
        /// The id of the just created item
        /// </returns>
        public long Create(LightPatientDto item)
        {
            return new Creator(this.Session).Create(item);
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
        /// Execute a search on the name of the patient and refines the result with the predicates.
        /// If the criteria is an "*" (asterisk), all the patient will be loaded in memory and afterward
        /// the refiner will be executed.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="specification"></param>
        /// <returns>
        /// All the patient that fullfill the criteria
        /// </returns>
        public IList<LightPatientDto> GetPatientsByNameLight(string criteria, SpecificationExpression<LightPatientDto> specification)
        {
            List<Patient> patients = new List<Patient>();
            var searchAll = false;

            if (criteria != "*" && !string.IsNullOrEmpty(criteria))
            {
                patients = (from p in this.Session.Query<Patient>()
                            where p.FirstName.ToLower().Contains(criteria.ToLower())
                            || p.LastName.ToLower().Contains(criteria.ToLower())
                            select new Patient
                            {
                                Id = p.Id,
                                FirstName = p.FirstName,
                                LastName = p.LastName,
                                Gender = p.Gender,
                                BirthDate = p.BirthDate,
                                Profession = p.Profession,
                                Height = p.Height,
                            }).ToList();
            }
            else
            {
                patients = (from p in this.Session.Query<Patient>()
                            select new Patient
                            {
                                Id = p.Id,
                                FirstName = p.FirstName,
                                LastName = p.LastName,
                                Gender = p.Gender,
                                BirthDate = p.BirthDate,
                                Profession = p.Profession,
                                Height = p.Height,
                            }).ToList();
                searchAll = true;
            }

            var preSearchResult = Mapper.Map<List<Patient>, List<LightPatientDto>>(patients);
            this.CheckIfSearchAllReturnsZeroResult(preSearchResult, searchAll);

            var result = preSearchResult.FindAll(specification.IsSatisfiedBy);
            return result;
        }

        /// <summary>
        /// Gets the patients that fullfill the specified criterium.
        /// </summary>
        /// <param name="criterium">The criterium.</param>
        /// <param name="search">The search should be done on the specified property.</param>
        /// <returns></returns>
        public IList<LightPatientDto> GetPatientsByNameLight(string criterium, SearchOn search)
        {
            return new Selector(this.Session).GetPatientByNameLight(criterium, search);
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
        /// Gets the top X patient. Where X is specified as an argument.
        /// Everytime a user is loaded in memory, a counter is incremented. This
        /// value is used to select the most 'famous' patients.
        /// </summary>
        /// <param name="x">The number of patient this method returns.</param>
        /// <returns>
        /// An array of patients
        /// </returns>
        public IList<LightPatientDto> GetTopXPatient(uint x)
        {
            var result = (from patient in this.Session.Query<Patient>()
                          orderby patient.Counter descending
                          select patient)
                            .Take((int)x)
                            .ToList();

            return Mapper.Map<IList<Patient>, IList<LightPatientDto>>(result)
                         .ToList();
        }

        [Granted(To.Everyone)]
        public void IncrementCounter(LightPatientDto patient)
        {
            var entity = (from p in this.Session.Query<Patient>()
                          where p.Id == patient.Id
                          select p).FirstOrDefault();
            if (entity == null) { throw new EntityNotFoundException(typeof(Patient)); }

            entity.Counter++;
            this.Session.Update(entity);
        }

        /// <summary>
        /// Increments the patient counter.
        /// </summary>
        /// <param name="patient">The patient.</param>
        public void IncrementPatientCounter(LightPatientDto patient)
        {
            var entity = this.Session.Get<Patient>(patient.Id);
            entity.Counter++;
            this.Session.Update(entity);
        }

        private void CheckIfSearchAllReturnsZeroResult(IList<LightPatientDto> patients, bool searchAll)
        {
            if (patients.Count == 0 && searchAll)
            {
                var msg = "A query that asks all the patients returned zero results after the execution of the specification pattern. Maybe you have some null properties (Checl the SELECT clause)";
                this.Logger.Warn(msg);
#if DEBUG
                throw new NotImplementedException(msg);
#endif
            }
        }

        #endregion Methods


        /// <summary>
        /// Gets the count of the specified patient. That's the number of time he/she was selected
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>
        /// The number of times the patient was selected
        /// </returns>
        public long GetCountOf(LightPatientDto patient)
        {
            var entity = (from p in this.Session.Query<Patient>()
                          where p.Id == patient.Id
                          select p).FirstOrDefault();
            if (entity == null) { throw new EntityNotFoundException(typeof(Patient)); }

            return entity.Counter;
        }
    }
}