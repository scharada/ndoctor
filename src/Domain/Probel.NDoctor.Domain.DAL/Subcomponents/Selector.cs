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

namespace Probel.NDoctor.Domain.DAL.Subcomponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DAL.AopConfiguration;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.DAL.Helpers;

    internal class Selector
    {
        #region Fields

        private readonly ISession Session;

        #endregion Fields

        #region Constructors

        public Selector(ISession session)
        {
            this.Session = session;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets all doctors.
        /// </summary>
        /// <returns>The light weight version of the doctors</returns>
        public IList<LightDoctorDto> GetAllDoctorsLight()
        {
            var results = (from item in this.Session.Query<Doctor>()
                           select item).ToList();

            return Mapper.Map<IList<Doctor>, IList<LightDoctorDto>>(results);
        }

        /// <summary>
        /// Gets all drugs from the database.
        /// </summary>
        /// <returns></returns>
        public IList<DrugDto> GetAllDrugs()
        {
            var tags = (from tag in this.Session.Query<Drug>()
                        select tag).ToList();

            return Mapper.Map<IList<Drug>, IList<DrugDto>>(tags);
        }

        /// <summary>
        /// Gets all insurances stored in the database.
        /// </summary>
        /// <returns>A list of insurance</returns>
        public IList<InsuranceDto> GetAllInsurances()
        {
            var insurances = this.GetAllEntitiesInsurance();

            return Mapper.Map<IList<Insurance>, IList<InsuranceDto>>(insurances);
        }

        /// <summary>
        /// Gets all insurances stored in the database. Return a light version of the insurance
        /// </summary>
        /// <returns>A list of light weight insurance</returns>
        public IList<LightInsuranceDto> GetAllInsurancesLight()
        {
            var insurances = this.GetAllEntitiesInsurance();

            return Mapper.Map<IList<Insurance>, IList<LightInsuranceDto>>(insurances);
        }

        /// <summary>
        /// Gets all pathologies stored in the database.
        /// </summary>
        /// <returns></returns>
        public IList<PathologyDto> GetAllPathologies()
        {
            var pathologies = this.GetAllEntitiesPathologie();

            return Mapper.Map<IList<Pathology>, IList<PathologyDto>>(pathologies);
        }

        /// <summary>
        /// Gets all patients stored in the repository.
        /// </summary>
        /// <returns></returns>
        public IList<PatientDto> GetAllPatients()
        {
            var result = (from p in this.Session.Query<Patient>()
                          select p).ToList();
            return Mapper.Map<IList<Patient>, IList<PatientDto>>(result);
        }

        /// <summary>
        /// Gets all the patients in light version.
        /// </summary>
        /// <returns></returns>
        public IList<LightPatientDto> GetAllPatientsLight()
        {
            var result = GetAllEntitiesPatient();

            return Mapper.Map<IList<Patient>, IList<LightPatientDto>>(result);
        }

        /// <summary>
        /// Gets all practices stored in the database.
        /// </summary>
        /// <returns></returns>
        public IList<PracticeDto> GetAllPractices()
        {
            var practices = this.GetAllEntitiesPractice();
            return Mapper.Map<IList<Practice>, IList<PracticeDto>>(practices);
        }

        /// <summary>
        /// Gets all practices stored in the database.
        /// </summary>
        /// <returns></returns>
        public IList<LightPracticeDto> GetAllPracticesLight()
        {
            var practices = this.GetAllEntitiesPractice();
            return Mapper.Map<IList<Practice>, IList<LightPracticeDto>>(practices);
        }

        /// <summary>
        /// Gets all professions stored in the database.
        /// </summary>
        /// <returns></returns>
        public IList<ProfessionDto> GetAllProfessions()
        {
            var professions = (from profession in this.Session.Query<Profession>()
                               select profession)
                                  .OrderBy(e => e.Name)
                                  .ToList();

            return Mapper.Map<IList<Profession>, IList<ProfessionDto>>(professions);
        }

        /// <summary>
        /// Gets all reputations stored in the database.
        /// </summary>
        /// <returns></returns>
        public IList<ReputationDto> GetAllReputations()
        {
            var reputations = (from reputation in this.Session.Query<Reputation>()
                               select reputation)
                                    .OrderBy(e => e.Name)
                                    .ToList();

            return Mapper.Map<IList<Reputation>, IList<ReputationDto>>(reputations);
        }

        /// <summary>
        /// Gets all roles light.
        /// </summary>
        /// <returns>An array with all the roles</returns>
        public IList<RoleDto> GetAllRolesLight()
        {
            var roles = this.GetAllEntitiesRole();
            return Mapper.Map<IList<Role>, IList<RoleDto>>(roles);
        }

        /// <summary>
        /// Gets all the tags
        /// </summary>
        /// <returns></returns>
        public IList<TagDto> GetAllTags()
        {
            var tags = (from tag in this.Session.Query<Tag>()
                        select tag)
                            .OrderBy(e => e.Category)
                            .ToList();

            return Mapper.Map<IList<Tag>, IList<TagDto>>(tags);
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        public IList<LightUserDto> GetAllUsersLight()
        {
            try
            {
                var result = (from user in Session.Query<User>()
                              select user).ToList();
                return Mapper.Map<IList<User>, IList<LightUserDto>>(result);
            }
            catch (Exception ex)
            {
                throw new QueryException(ex);
            }
        }

        /// <summary>
        /// Gets the doctors by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="on">The on.</param>
        /// <returns></returns>
        public IList<LightDoctorDto> GetDoctorsByNameLight(string name, SearchOn on)
        {
            IList<Doctor> result = new List<Doctor>();
            switch (on)
            {
                case SearchOn.FirstName:
                    result = this.GetDoctorsByFirstName(name);
                    break;
                case SearchOn.LastName:
                    result = this.GetDoctorsByLastName(name);
                    break;
                case SearchOn.FirstAndLastName:
                    result = this.GetDoctorsByFirstAndLastName(name);
                    break;
                default:
                    Assert.FailOnEnumeration(on);
                    break;
            }

            return Mapper.Map<IList<Doctor>, IList<LightDoctorDto>>(result)
                         .ToList();
        }

        /// <summary>
        /// Gets the doctors by specialisation.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="on">The on.</param>
        /// <returns></returns>
        public IList<LightDoctorDto> GetDoctorsBySpecialisationLight(TagDto specialisation)
        {
            if (specialisation.Id <= 0) throw new DetachedEntityException();

            var results = (from item in this.Session.Query<Doctor>()
                           where item.Specialisation.Id == specialisation.Id
                           select item).ToList();

            return Mapper.Map<IList<Doctor>, IList<LightDoctorDto>>(results)
                         .ToList();
        }

        /// <summary>
        /// Gets the drugs which has in their name the specified criteria.
        /// </summary>
        /// <param name="name">The criteria.</param>
        /// <returns>A list of drugs</returns>
        public IList<DrugDto> GetDrugsByName(string name)
        {
            if (name == "*") { return this.GetAllDrugs(); }
            else
            {
                var result = (from drug in this.Session.Query<Drug>()
                              where drug.Name.Contains(name)
                              select drug).ToList();
                return Mapper.Map<IList<Drug>, IList<DrugDto>>(result);
            }
        }

        /// <summary>
        /// Gets the drugs by tags.
        /// </summary>
        /// <param name="tag">The tag name.</param>
        /// <returns>A list of drugs</returns>
        public IList<DrugDto> GetDrugsByTags(string criteria)
        {
            var result = (from drug in this.Session.Query<Drug>()
                          where drug.Tag.Name == criteria
                          select drug).ToList();
            return Mapper.Map<IList<Drug>, IList<DrugDto>>(result);
        }

        /// <summary>
        /// Gets all insurances that contain the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IList<InsuranceDto> GetInsurances(string name)
        {
            var insurances = (from insurance in this.Session.Query<Insurance>()
                              where insurance.Name.Contains(name)
                              select insurance).ToList();

            return Mapper.Map<IList<Insurance>, IList<InsuranceDto>>(insurances);
        }

        /// <summary>
        /// Gets the light doctor by specialisation.
        /// </summary>
        /// <param name="specialisation">The tag.</param>
        /// <returns></returns>
        public IList<LightDoctorDto> GetLightDoctor(TagDto specialisation)
        {
            var result = (from doctor in this.Session.Query<Doctor>()
                          where doctor.Specialisation.Id == specialisation.Id
                          select doctor).ToList();

            return Mapper.Map<IList<Doctor>, IList<LightDoctorDto>>(result);
        }

        /// <summary>
        /// Gets all the medical records of the specified patient. The records are packed into a 
        /// medical record cabinet which contains medical records folders. Each folder contains a list 
        /// of medical records.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public MedicalRecordCabinetDto GetMedicalRecordCabinet(LightPatientDto patient)
        {
            Assert.IsNotNull(patient, "patient");
            var selectedPatient = (from p in this.Session.Query<Patient>()
                                   where p.Id == patient.Id
                                   select p).FirstOrDefault();

            if (selectedPatient == null) throw new EntityNotFoundException(typeof(Patient));

            return Mapper.Map<Patient, MedicalRecordCabinetDto>(selectedPatient);
        }

        /// <summary>
        /// Gets all pathologies that contains the specified name.
        /// </summary>
        /// <returns></returns>
        public IList<PathologyDto> GetPathologiesByName(string name)
        {
            name = name ?? string.Empty;

            if (name == string.Empty || name == "*") { return this.GetAllPathologies(); }

            var pathologies = (from pahology in this.Session.Query<Pathology>()
                               where pahology.Name.Contains(name)
                               select pahology).ToList();

            return Mapper.Map<IList<Pathology>, IList<PathologyDto>>(pathologies);
        }

        /// <summary>
        /// Gets the patients that fullfill the specified criterium.
        /// </summary>
        /// <param name="criterium">The criterium.</param>
        /// <param name="search">The search should be done on the specified property.</param>
        /// <returns></returns>
        public IList<PatientDto> GetPatientByName(string criterium, SearchOn search)
        {
            var result = this.GetPatientEntities(criterium, search);
            return Mapper.Map<IList<Patient>, IList<PatientDto>>(result);
        }

        /// <summary>
        /// Gets the patients that fullfill the specified criterium.
        /// </summary>
        /// <param name="criterium">The criterium.</param>
        /// <param name="search">The search should be done on the specified property.</param>
        /// <returns></returns>
        [Granted(To.Everyone)]
        public IList<LightPatientDto> GetPatientByNameLight(string criterium, SearchOn search)
        {
            var result = this.GetPatientEntities(criterium, search);
            return Mapper.Map<IList<Patient>, IList<LightPatientDto>>(result);
        }

        /// <summary>
        /// Gets all practices that contains the specified name.
        /// </summary>
        /// <returns></returns>
        public IList<PracticeDto> GetPractices(string name)
        {
            var practices = (from practice in this.Session.Query<Practice>()
                             where practice.Name.Contains(name)
                             select practice).ToList();

            return Mapper.Map<IList<Practice>, IList<PracticeDto>>(practices);
        }

        /// <summary>
        /// Gets all professions that contains the specified name.
        /// </summary>
        /// <returns></returns>
        public IList<ProfessionDto> GetProfessions(string name)
        {
            var professions = (from profession in this.Session.Query<Profession>()
                               where profession.Name.Contains(name)
                               select profession).ToList();

            return Mapper.Map<IList<Profession>, IList<ProfessionDto>>(professions);
        }

        /// <summary>
        /// Gets all reputations that contains the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IList<ReputationDto> GetReputations(string name)
        {
            var reputations = (from reputation in this.Session.Query<Reputation>()
                               where reputation.Name.Contains(name)
                               select reputation).ToList();

            return Mapper.Map<IList<Reputation>, IList<ReputationDto>>(reputations);
        }

        /// <summary>
        /// Gets all the tags with the specified catagory.
        /// </summary>
        /// <returns></returns>
        public IList<TagDto> GetTags(TagCategory category)
        {

            var tags = (from tag in this.Session.Query<Tag>()
                        where tag.Category == category
                        select tag).ToList();

            if (category == TagCategory.Appointment)
            {
                tags = (from tag in tags
                        where tag.Name != Default.GoogleCalendarTagName
                        select tag).ToList();
            }

            return Mapper.Map<IEnumerable<Tag>, IList<TagDto>>(tags);
        }

        /// <summary>
        /// Gets the tag for patient that contain the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IList<TagDto> GetTags(string name, TagCategory type)
        {
            var tags = (from tag in this.Session.Query<Tag>()
                        where tag.Category == type
                           && tag.Name.Contains(name)
                        select tag).ToList();

            return Mapper.Map<IList<Tag>, IList<TagDto>>(tags);
        }

        /// <summary>
        /// Gets the user by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public UserDto GetUserById(long id)
        {
            var user = this.Session.Get<User>(id);
            return Mapper.Map<User, UserDto>(user);
        }

        public IList<LightUserDto> GetUserByLastName(string criteria)
        {
            criteria = criteria.ToLower();
            var entities = (from u in this.Session.Query<User>()
                            where u.LastName.ToLower() == criteria
                            select u).ToList();
            return Mapper.Map<IList<User>, IList<LightUserDto>>(entities);
        }

        internal IList<RoleDto> GetRoleByName(string name)
        {
            name = name.ToLower();
            var result = (from r in this.Session.Query<Role>()
                          where r.Name.ToLower() == name
                          select r).ToList();
            return Mapper.Map<IList<Role>, IList<RoleDto>>(result);
        }

        private List<Insurance> GetAllEntitiesInsurance()
        {
            var insurances = (from insurance in this.Session.Query<Insurance>()
                              select insurance)
                                .OrderBy(e => e.Name)
                                .ToList();
            return insurances;
        }

        private IList<Pathology> GetAllEntitiesPathologie()
        {
            var pathologies = (from insurance in this.Session.Query<Pathology>()
                               select insurance).ToList();
            return pathologies;
        }

        private List<Patient> GetAllEntitiesPatient()
        {
            var result = (from patient in this.Session.Query<Patient>()
                          select patient).ToList();
            return result;
        }

        private List<Practice> GetAllEntitiesPractice()
        {
            return (from practice in this.Session.Query<Practice>()
                    select practice)
                              .OrderBy(e => e.Name)
                              .ToList();
        }

        private IList<Role> GetAllEntitiesRole()
        {
            return (from role in this.Session.Query<Role>()
                    select role).ToList();
        }

        private IList<Doctor> GetDoctorsByFirstAndLastName(string name)
        {
            return (from doctor in this.Session.Query<Doctor>()
                    where doctor.FirstName.Contains(name)
                       || doctor.LastName.Contains(name)
                    select doctor).ToList();
        }

        private IList<Doctor> GetDoctorsByFirstName(string name)
        {
            return (from doctor in this.Session.Query<Doctor>()
                    where doctor.FirstName.Contains(name)
                    select doctor).ToList();
        }

        private IList<Doctor> GetDoctorsByLastName(string name)
        {
            return (from doctor in this.Session.Query<Doctor>()
                    where doctor.LastName.Contains(name)
                    select doctor).ToList();
        }

        private IList<Patient> GetPatientEntities(string criterium, SearchOn search)
        {
            if (string.IsNullOrEmpty(criterium)) return new List<Patient>().ToList();

            criterium = criterium.ToLower();
            var result = new List<Patient>();

            switch (search)
            {
                case SearchOn.FirstName:
                    {
                        result = GetPatientsByFirstName(criterium);
                        break;
                    }
                case SearchOn.LastName:
                    {
                        result = GetPatientsByLastName(criterium);
                        break;
                    }
                case SearchOn.FirstAndLastName:
                    {
                        result = GetPatientsByFirstAndLastName(criterium);
                        break;
                    }
                default:
                    {
                        Assert.FailOnEnumeration(search);
                        break;
                    }
            }
            return result;
        }

        private List<Patient> GetPatientsByFirstAndLastName(string criterium)
        {
            if (criterium != "*")
            {
                return (from patient in this.Session.Query<Patient>()
                        where patient.FirstName.Contains(criterium)
                           || patient.LastName.Contains(criterium)
                        select patient).ToList();
            }
            else
            {
                return (from patient in this.Session.Query<Patient>()
                        select patient).ToList();
            }
        }

        private List<Patient> GetPatientsByFirstName(string criterium)
        {
            if (criterium != "*")
            {
                return (from patient in this.Session.Query<Patient>()
                        where patient.FirstName.Contains(criterium)
                        select patient).ToList();
            }
            else
            {
                return (from patient in this.Session.Query<Patient>()
                        select patient).ToList();
            }
        }

        private List<Patient> GetPatientsByLastName(string criterium)
        {
            if (criterium != "*")
            {
                return (from patient in this.Session.Query<Patient>()
                        where patient.LastName.Contains(criterium)
                        select patient).ToList();
            }
            else
            {
                return (from patient in this.Session.Query<Patient>()
                        select patient).ToList();
            }
        }

        #endregion Methods
    }
}