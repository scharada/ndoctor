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
    using System.Collections.ObjectModel;
    using System.Linq;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Conversions;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Exceptions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

    public class FamilyComponent : BaseComponent, IFamilyComponent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BmiComponent"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public FamilyComponent(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BmiComponent"/> class.
        /// </summary>
        public FamilyComponent()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Finds the family of the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public FamilyDto FindFamily(LightPatientDto patient)
        {
            this.CheckSession();
            var current = (from p in this.Session.Query<Patient>()
                           where patient.Id == p.Id
                           select p).FirstOrDefault();
            var father = (current.Father == null)
                ? null
                : Mapper.Map<Patient, LightPatientDto>(current.Father);

            var mother = (current.Mother == null)
                ? null
                : Mapper.Map<Patient, LightPatientDto>(current.Mother);

            var children = this.GetChildrenAsObservable(current);

            return new FamilyDto()
            {
                Current = patient,
                Father = father,
                Mother = mother,
                Children = children,
            };
        }

        /// <summary>
        /// Find all the patient respecting the criteria and the search mode which
        /// aren't in the family of the specified patient
        /// </summary>
        /// <param name="patient">The patient connected into the session</param>
        /// <param name="criteria">The search criteria</param>
        /// <param name="search">The search mode</param>
        /// <returns>A list of patient</returns>
        public IList<LightPatientDto> FindPatientNotFamilyMembers(LightPatientDto patient, string criteria, SearchOn search)
        {
            this.CheckSession();

            var result = this.FindPatientsByNameLight(criteria, search);
            return this.RemoveFamilyMembers(result, patient);
        }

        /// <summary>
        /// Find all family members for the specified Patient
        /// </summary>
        /// <param name="patient">The connected patient</param>
        /// <returns>The list of the family members</returns>
        public IList<LightPatientDto> GetAllFamilyMembers(LightPatientDto patient)
        {
            this.CheckSession();
            var p = this.Session.Get<Patient>(patient.Id);
            return this.GetAllFamilyMembers(p);
        }

        /// <summary>
        /// Returns all the family members of the current patient
        /// </summary>
        /// <param name="patient">The patient </param>
        /// <returns>The list of the familly members</returns>
        public IList<LightPatientDto> GetFamilyMembers(LightPatientDto patient)
        {
            this.CheckSession();
            var entity = this.Session.Get<Patient>(patient.Id);
            return this.GetAllFamilyMembers(entity);
        }

        /// <summary>
        /// Remove the specified member from the specified patient
        /// </summary>
        /// <param name="member">The family member to remove</param>
        /// <param name="patient">The family of this patient will be updated</param>
        public void RemoveFamilyMember(LightPatientDto member, LightPatientDto patient)
        {
            Assert.IsNotNull(member, "Member");
            Assert.IsNotNull(patient, "Patient");

            this.CheckSession();
            var entity = this.Session.Get<Patient>(patient.Id);

            var isUpdated = this.UpdateParent(member, entity);
            if (!isUpdated)
            {
                foreach (var child in this.GetChildren(entity))
                {
                    if (child.Id == member.Id)
                    {
                        var memberEntity = this.Session.Get<Patient>(child.Id);
                        this.UpdateParent(patient, memberEntity);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the specified family.
        /// When the members' state is set to new, it'll create a new relation for the patient.
        /// All other members' state will throw a <see cref="BusinessLogicException"/>.
        /// </summary>
        /// <param name="family">The family.</param>
        public void Update(FamilyDto family)
        {
            this.CheckSession();
            var patient = (from p in this.Session.Query<Patient>()
                           where p.Id == family.Current.Id
                           select p).FirstOrDefault();

            if (patient == null) throw new EntityNotFoundException(typeof(Patient));

            if (family.Father != null && family.Father.State == State.Added)
            {
                var father = this.Session.Get<Patient>(family.Father.Id);
                patient.Father = father;
            }

            if (family.Mother != null && family.Mother.State == State.Added)
            {
                var mother = this.Session.Get<Patient>(family.Mother.Id);
                patient.Mother = mother;
            }

            foreach (var child in family.Children)
            {
                if (child.State == State.Added)
                {
                    var currentChild = this.Session.Get<Patient>(child.Id);
                    switch (patient.Gender)
                    {
                        case Gender.Male:
                            currentChild.Father = patient;
                            break;
                        case Gender.Female:
                            currentChild.Mother = patient;
                            break;
                        default:
                            Assert.FailOnEnumeration(patient.Gender);
                            break;
                    }
                    this.Session.Save(currentChild);
                }
            }
            this.Session.SaveOrUpdate(patient);
        }

        private IList<LightPatientDto> GetAllFamilyMembers(Patient patient)
        {
            var family = new List<Patient>();
            if (patient.Father != null) family.Add(patient.Father);
            if (patient.Mother != null) family.Add(patient.Mother);

            var children = (from p in this.Session.Query<Patient>()
                            where p.Father.Id == patient.Id
                               || p.Mother.Id == patient.Id
                            select p).ToList();
            family.AddRange(children);
            return Mapper.Map<IList<Patient>, IList<LightPatientDto>>(family);
        }

        private IList<Patient> GetChildren(Patient patient)
        {
            return (from child in this.Session.Query<Patient>()
                    where child.Father.Id == patient.Id
                       || child.Mother.Id == patient.Id
                    select child).ToList();
        }

        private ObservableCollection<LightPatientDto> GetChildrenAsObservable(Patient patient)
        {
            var children = this.GetChildren(patient);
            var childrenDto = Mapper.Map<IList<Patient>, IList<LightPatientDto>>(children);
            var result = new ObservableCollection<LightPatientDto>();
            result.Refill(childrenDto);
            return result;
        }

        private IList<LightPatientDto> RemoveFamilyMembers(IList<LightPatientDto> result, LightPatientDto patient)
        {
            var family = this.GetAllFamilyMembers(patient);
            family.Add(patient);
            return (from r in result
                    where !family.Contains(r, LightPatientDto.EqualityComparer)
                    select r).ToList();
        }

        private bool UpdateParent(LightPatientDto member, Patient entity)
        {
            var isUpdated = false;
            if (entity.Father != null && entity.Father.Id == member.Id)
            {
                entity.Father = null;
                isUpdated = true;
            }
            if (entity.Mother != null && entity.Mother.Id == member.Id)
            {
                entity.Mother = null;
                isUpdated = true;
            }

            if (isUpdated) this.Session.Update(entity);
            return isUpdated;
        }

        #endregion Methods
    }
}