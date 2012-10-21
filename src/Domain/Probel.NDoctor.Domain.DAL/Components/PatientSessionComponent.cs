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

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Components;
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
        /// Execute a search on the name of the patient and refines the result with the predicates.
        /// If the criteria is an "*" (asterisk), all the patient will be loaded in memory and afterward
        /// the refiner will be executed.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="specification"></param>
        /// <returns>
        /// All the patient that fullfill the criteria
        /// </returns>
        public IList<LightPatientDto> GetPatientsByNameLight(string criteria, SpecificationExpression<PatientDto> specification)
        {
            List<Patient> patients = new List<Patient>();
            var searchAll = false;

            if (criteria != "*")
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
                            }).ToList();
                searchAll = true;
            }

            var entities = Mapper.Map<List<Patient>, List<PatientDto>>(patients);

            this.CheckIfSearchAllReturnsZeroResult(entities, searchAll);

            return Mapper.Map<List<PatientDto>, IList<LightPatientDto>>(entities.FindAll(specification.IsSatisfiedBy));
        }

        private void CheckIfSearchAllReturnsZeroResult(IList<PatientDto> patients, bool searchAll)
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

        #endregion Methods
    }
}