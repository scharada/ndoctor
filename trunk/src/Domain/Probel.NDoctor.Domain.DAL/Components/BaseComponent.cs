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

    using log4net;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Management;
    using Probel.NDoctor.Domain.DAL.Cfg;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Helpers;
    using Probel.NDoctor.Domain.DAL.Properties;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;

    /* REFACTOR: 1027 lines of code => refactor this god object.
     * I should split the responsibilities....
     */
    public abstract class BaseComponent : IBaseComponent
    {
        #region Fields

        private Action<object> dispose;
        private Func<object, object> init;

        #endregion Fields

        #region Constructors

        public BaseComponent(ISession session)
            : this()
        {
            this.Session = session;
        }

        public BaseComponent()
        {
            this.Logger = LogManager.GetLogger(this.GetType());
            this.init = (context) =>
            {
                this.Logger.DebugFormat("\t[{0}] Opening session", this.GetType().Name);
                this.OpenSession();
                return null;
            };

            this.dispose = (context) =>
            {
                this.Logger.DebugFormat("\t[{0}] Closing session", this.GetType().Name);
                this.CloseSession();
            };
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is session open.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is session open; otherwise, <c>false</c>.
        /// </value>
        public bool IsSessionOpen
        {
            [InspectionIgnored]
            get
            {
                if (this.Session == null) return false;
                return this.Session.IsOpen;
            }
        }

        public UnitOfWork UnitOfWork
        {
            [InspectionIgnored]
            get
            {
                if (this.Session != null && this.Session.IsOpen) { throw new SessionException(Messages.Msg_ErrorSessionAlreadyOpenException); }
                else
                {
                    return new UnitOfWork(init, dispose);
                }
            }
        }

        internal ISession Session
        {
            get;
            private set;
        }

        protected ILog Logger
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        public long Create(DrugDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var exist = (from i in this.Session.Query<Drug>()
                         where i.Name.ToUpper() == item.Name.ToUpper()
                            || i.Id == item.Id
                         select i).Count() > 0;
            if (exist) throw new ExistingItemException();

            var entity = Mapper.Map<DrugDto, Drug>(item);
            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        public long Create(PatientDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var found = (from p in this.Session.Query<Patient>()
                         where p.Id == item.Id
                         select p).Count() > 0;
            if (found) throw new ExistingItemException();

            var entity = Mapper.Map<PatientDto, Patient>(item);
            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        public long Create(LightDoctorDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var found = (from p in this.Session.Query<Doctor>()
                         where p.Id == item.Id
                         select p).Count() > 0;
            if (found) throw new ExistingItemException();

            var entity = Mapper.Map<LightDoctorDto, Doctor>(item);
            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        public long Create(TagDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var exist = (from p in this.Session.Query<Tag>()
                         where (p.Name.ToUpper() == item.Name.ToUpper()
                             && p.Category == item.Category)
                             || p.Id == item.Id
                         select p).Count() > 0;
            if (exist) throw new ExistingItemException();

            var entity = Mapper.Map<TagDto, Tag>(item);
            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        [Granted(To.Everyone)]
        public long Create(UserDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var found = (from p in this.Session.Query<User>()
                         where p.Id == item.Id
                         select p).Count() > 0;
            if (found) throw new ExistingItemException();

            if (item.IsDefault) this.ReplaceDefaultUser();
            this.Session.Flush();

            var entity = Mapper.Map<UserDto, User>(item);

            if (this.IsFirstUser()) { entity.IsSuperAdmin = true; }
            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Creates a new patient.
        /// </summary>
        /// <param name="item">The patient.</param>
        public long Create(LightPatientDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var found = (from p in this.Session.Query<Patient>()
                         where p.Id == item.Id
                         select p).Count() > 0;

            var newPatient = Mapper.Map<LightPatientDto, Patient>(item);
            newPatient.InscriptionDate = DateTime.Today;

            if (found) throw new ExistingItemException();

            return (long)this.Session.Save(newPatient);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [InspectionIgnored]
        public void Dispose()
        {
            this.CloseSession();
        }

        /// <summary>
        /// Finds the doctors by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="on">The on.</param>
        /// <returns></returns>
        public IList<LightDoctorDto> FindDoctorsByNameLight(string name, SearchOn on)
        {
            IList<Doctor> result = new List<Doctor>();
            switch (on)
            {
                case SearchOn.FirstName:
                    result = this.FindDoctorsByFirstName(name);
                    break;
                case SearchOn.LastName:
                    result = this.FindDoctorsByLastName(name);
                    break;
                case SearchOn.FirstAndLastName:
                    result = this.FindDoctorsByFirstAndLastName(name);
                    break;
                default:
                    Assert.FailOnEnumeration(on);
                    break;
            }

            return Mapper.Map<IList<Doctor>, IList<LightDoctorDto>>(result)
                         .ToList();
        }

        /// <summary>
        /// Finds the doctors by specialisation.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="on">The on.</param>
        /// <returns></returns>
        public IList<LightDoctorDto> FindDoctorsBySpecialisationLight(TagDto specialisation)
        {
            if (specialisation.Id <= 0) throw new DetachedEntityException();

            var results = (from item in this.Session.Query<Doctor>()
                           where item.Specialisation.Id == specialisation.Id
                           select item).ToList();

            return Mapper.Map<IList<Doctor>, IList<LightDoctorDto>>(results)
                         .ToList();
        }

        /// <summary>
        /// Finds the drugs which has in their name the specified criteria.
        /// </summary>
        /// <param name="name">The criteria.</param>
        /// <returns>A list of drugs</returns>
        public IList<DrugDto> FindDrugsByName(string name)
        {
            var result = (from drug in this.Session.Query<Drug>()
                          where drug.Name.Contains(name)
                          select drug).ToList();
            return Mapper.Map<IList<Drug>, IList<DrugDto>>(result);
        }

        /// <summary>
        /// Finds the drugs by tags.
        /// </summary>
        /// <param name="tag">The tag name.</param>
        /// <returns>A list of drugs</returns>
        public IList<DrugDto> FindDrugsByTags(string criteria)
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
        public IList<InsuranceDto> FindInsurances(string name)
        {
            var insurances = (from insurance in this.Session.Query<Insurance>()
                              where insurance.Name.Contains(name)
                              select insurance).ToList();

            return Mapper.Map<IList<Insurance>, IList<InsuranceDto>>(insurances);
        }

        /// <summary>
        /// Finds the light doctor by specialisation.
        /// </summary>
        /// <param name="specialisation">The tag.</param>
        /// <returns></returns>
        public IList<LightDoctorDto> FindLightDoctor(TagDto specialisation)
        {
            var result = (from doctor in this.Session.Query<Doctor>()
                          where doctor.Specialisation.Id == specialisation.Id
                          select doctor).ToList();

            return Mapper.Map<IList<Doctor>, IList<LightDoctorDto>>(result);
        }

        /// <summary>
        /// Gets all pathologies that contains the specified name.
        /// </summary>
        /// <returns></returns>
        public IList<PathologyDto> FindPathology(string name)
        {
            var pathologies = (from pahology in this.Session.Query<Pathology>()
                               where pahology.Name.Contains(name)
                               select pahology).ToList();

            return Mapper.Map<IList<Pathology>, IList<PathologyDto>>(pathologies);
        }

        /// <summary>
        /// Finds the patients that fullfill the specified criterium.
        /// </summary>
        /// <param name="criterium">The criterium.</param>
        /// <param name="search">The search should be done on the specified property.</param>
        /// <returns></returns>
        [Granted(To.Everyone)]
        public IList<LightPatientDto> FindPatientsByNameLight(string criterium, SearchOn search)
        {
            if (string.IsNullOrEmpty(criterium)) return new List<LightPatientDto>().ToList();

            criterium = criterium.ToLower();
            var result = new List<Patient>();

            switch (search)
            {
                case SearchOn.FirstName:
                    {
                        result = FindPatientsByOnFirstName(criterium);
                        break;
                    }
                case SearchOn.LastName:
                    {
                        result = FindPatientsByOnLastName(criterium);
                        break;
                    }
                case SearchOn.FirstAndLastName:
                    {
                        result = FindPatientsByFirstAndLastName(criterium);
                        break;
                    }
                default:
                    {
                        Assert.FailOnEnumeration(search);
                        return null;
                    }
            }
            return Mapper.Map<IList<Patient>, IList<LightPatientDto>>(result);
        }

        /// <summary>
        /// Gets all practices that contains the specified name.
        /// </summary>
        /// <returns></returns>
        public IList<PracticeDto> FindPractices(string name)
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
        public IList<ProfessionDto> FindProfessions(string name)
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
        public IList<ReputationDto> FindReputations(string name)
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
        public IList<TagDto> FindTags(TagCategory category)
        {
            var tags = (from tag in this.Session.Query<Tag>()
                        where tag.Category == category
                        select tag).ToList();

            return Mapper.Map<IList<Tag>, IList<TagDto>>(tags);
        }

        /// <summary>
        /// Gets the tag for patient that contain the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IList<TagDto> FindTags(string name, TagCategory type)
        {
            var tags = (from tag in this.Session.Query<Tag>()
                        where tag.Category == type
                           && tag.Name.Contains(name)
                        select tag).ToList();

            return Mapper.Map<IList<Tag>, IList<TagDto>>(tags);
        }

        /// <summary>
        /// Finds all doctors.
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
            var insurances = this.GetAllEntitiesInsurances();

            return Mapper.Map<IList<Insurance>, IList<InsuranceDto>>(insurances);
        }

        /// <summary>
        /// Gets all insurances stored in the database. Return a light version of the insurance
        /// </summary>
        /// <returns>A list of light weight insurance</returns>
        public IList<LightInsuranceDto> GetAllInsurancesLight()
        {
            var insurances = this.GetAllEntitiesInsurances();

            return Mapper.Map<IList<Insurance>, IList<LightInsuranceDto>>(insurances);
        }

        /// <summary>
        /// Gets all pathologies stored in the database.
        /// </summary>
        /// <returns></returns>
        public IList<PathologyDto> GetAllPathologies()
        {
            var pathologies = this.GetAllEntitiesPathologies();

            return Mapper.Map<IList<Pathology>, IList<PathologyDto>>(pathologies);
        }

        /// <summary>
        /// Finds all the patients in light version.
        /// </summary>
        /// <returns></returns>
        public IList<LightPatientDto> GetAllPatientsLight()
        {
            var result = GetAllPatientEntities();

            return Mapper.Map<IList<Patient>, IList<LightPatientDto>>(result);
        }

        /// <summary>
        /// Gets all practices stored in the database.
        /// </summary>
        /// <returns></returns>
        public IList<PracticeDto> GetAllPractices()
        {
            var practices = this.GetAllEntitiesPractices();
            return Mapper.Map<IList<Practice>, IList<PracticeDto>>(practices);
        }

        /// <summary>
        /// Gets all practices stored in the database.
        /// </summary>
        /// <returns></returns>
        public IList<LightPracticeDto> GetAllPracticesLight()
        {
            var practices = this.GetAllEntitiesPractices();
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
            var roles = this.GetAllEntitiesRoles();
            return Mapper.Map<IList<Role>, IList<RoleDto>>(roles);
        }

        /// <summary>
        /// Gets all the tags
        /// </summary>
        /// <returns></returns>
        public IList<TagDto> GetAllTags()
        {
            var tags = (from tag in this.Session.Query<Tag>()
                        select tag).ToList();

            return Mapper.Map<IList<Tag>, IList<TagDto>>(tags);
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        [Granted(To.Everyone)]
        public IList<LightUserDto> GetAllUsers()
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
        /// Gets the user by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public UserDto GetUserById(long id)
        {
            var user = this.Session.Get<User>(id);
            return Mapper.Map<User, UserDto>(user);
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove</typeparam>
        /// <param name="id">The id of the item to remove.</param>
        public void Remove(InsuranceDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var entity = this.Session.Get<Insurance>(item.Id);
            this.Remove(entity);
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove</typeparam>
        /// <param name="id">The id of the item to remove.</param>
        public void Remove(PracticeDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var entity = this.Session.Get<Practice>(item.Id);
            this.Remove(entity);
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove</typeparam>
        /// <param name="id">The id of the item to remove.</param>
        public void Remove(ProfessionDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var entity = this.Session.Get<Profession>(item.Id);
            this.Remove(entity);
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove</typeparam>
        /// <param name="id">The id of the item to remove.</param>
        public void Remove(ReputationDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var entity = this.Session.Get<Reputation>(item.Id);
            this.Remove(entity);
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove</typeparam>
        /// <param name="id">The id of the item to remove.</param>
        public void Remove(TagDto item)
        {
            var entity = this.Session.Get<Tag>(item.Id);
            this.Remove(entity);
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove</typeparam>
        /// <param name="id">The id of the item to remove.</param>
        public void Remove(LightDoctorDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var entity = this.Session.Get<Doctor>(item.Id);
            this.Remove(entity);
        }

        /// <summary>
        /// Removas the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(LightPatientDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var entity = this.Session.Get<Patient>(item.Id);
            this.Remove(entity);
        }

        /// <summary>
        /// Removas the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(PatientDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var entity = this.Session.Get<Patient>(item.Id);
            this.Remove(entity);
        }

        /// <summary>
        /// Updates the patient with the new data.
        /// </summary>
        /// <param name="item">The patient.</param>
        public void Update(PatientDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");
            item.LastUpdate = DateTime.Today;

            var entity = this.Session.Get<Patient>(item.Id);

            Mapper.Map<PatientDto, Patient>(item, entity);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified picture.
        /// </summary>
        /// <param name="picture">The picture.</param>
        public void Update(PictureDto item)
        {
            var entity = this.Session.Get<Picture>(item.Id);
            if (entity == null) throw new EntityNotFoundException(typeof(Picture));

            Mapper.Map<PictureDto, Picture>(item, entity);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Check if the current session can be used to query the database
        /// </summary>
        /// <exception cref="DalSessionException">Is thrown when the session is not configured correctly</exception>
        internal void CheckSession()
        {
            Assert.IsNotNull(this.Session, "The session is not configured. Every query to a component should be in a UnitOfWork!");
            if (!Session.IsOpen) throw new DalSessionException(Messages.Msg_ErrorSessionNotOpen);
        }

        /// <summary>
        /// Sets the session. This for test purpose
        /// </summary>
        /// <param name="session">The session.</param>
        internal void SetSession(ISession session)
        {
            this.Session = session;
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        protected void Create(MedicalRecordDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var found = (from p in this.Session.Query<MedicalRecord>()
                         where p.Id == item.Id
                         select p).Count() > 0;
            if (found) throw new ExistingItemException();

            var entity = Mapper.Map<MedicalRecordDto, MedicalRecord>(item);
            this.Session.Save(entity);
        }

        protected bool IsFirstUser()
        {
            var result = (from u in this.Session.Query<User>()
                          where u.IsSuperAdmin == true
                          select u).Take(2);

            return result.Count() == 0;
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove</typeparam>
        /// <param name="id">The id of the item to remove.</param>
        protected void Remove(MedicalRecordDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var entity = this.Session.Get<MedicalRecord>(item.Id);
            this.Remove(entity);
        }

        protected void Remove(object item)
        {
            if (item != null)
            {
                this.Session.Delete(item);
            }
        }

        /// <summary>
        /// Updates the specified medical record.
        /// </summary>
        /// <param name="item">The insurance.</param>
        protected void Update(MedicalRecordDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var entity = this.Session.Get<MedicalRecord>(item.Id);
            var tag = this.Session.Get<Tag>(entity.Tag.Id);

            //Reload the tag to avoid exception from nhibernate
            Mapper.Map<MedicalRecordDto, MedicalRecord>(item, entity);

            entity.Tag = tag;
            this.Session.Merge(entity);
        }

        private void CloseSession()
        {
            if (this.Session != null && Session.IsOpen)
            {
                this.Session.Flush();
                this.Session.Close();
            }
            else throw new DalSessionException(Messages.Msg_ErrorSessionNotOpen);
        }

        private IList<Doctor> FindDoctorsByFirstAndLastName(string name)
        {
            return (from doctor in this.Session.Query<Doctor>()
                    where doctor.FirstName.Contains(name)
                       || doctor.LastName.Contains(name)
                    select doctor).ToList();
        }

        private IList<Doctor> FindDoctorsByFirstName(string name)
        {
            return (from doctor in this.Session.Query<Doctor>()
                    where doctor.FirstName.Contains(name)
                    select doctor).ToList();
        }

        private IList<Doctor> FindDoctorsByLastName(string name)
        {
            return (from doctor in this.Session.Query<Doctor>()
                    where doctor.LastName.Contains(name)
                    select doctor).ToList();
        }

        private List<Patient> FindPatientsByFirstAndLastName(string criterium)
        {
            return (from patient in this.Session.Query<Patient>()
                    where patient.FirstName.Contains(criterium)
                       || patient.LastName.Contains(criterium)
                    select patient).ToList();
        }

        private List<Patient> FindPatientsByOnFirstName(string criterium)
        {
            return (from patient in this.Session.Query<Patient>()
                    where patient.FirstName.Contains(criterium)
                    select patient).ToList();
        }

        private List<Patient> FindPatientsByOnLastName(string criterium)
        {
            return (from patient in this.Session.Query<Patient>()
                    where patient.LastName.Contains(criterium)
                    select patient).ToList();
        }

        private List<Insurance> GetAllEntitiesInsurances()
        {
            var insurances = (from insurance in this.Session.Query<Insurance>()
                              select insurance)
                                .OrderBy(e => e.Name)
                                .ToList();
            return insurances;
        }

        private IList<Pathology> GetAllEntitiesPathologies()
        {
            var pathologies = (from insurance in this.Session.Query<Pathology>()
                               select insurance).ToList();
            return pathologies;
        }

        private List<Practice> GetAllEntitiesPractices()
        {
            return (from practice in this.Session.Query<Practice>()
                    select practice)
                              .OrderBy(e => e.Name)
                              .ToList();
        }

        private IList<Role> GetAllEntitiesRoles()
        {
            return (from role in this.Session.Query<Role>()
                    select role).ToList();
        }

        private List<Patient> GetAllPatientEntities()
        {
            var result = (from patient in this.Session.Query<Patient>()
                          select patient).ToList();
            return result;
        }

        private void OpenSession()
        {
            if (Session == null || !Session.IsOpen)
                this.Session = DAL.SessionFactory.OpenSession();
            else throw new DalSessionException(Messages.Msg_ErrorSessionAlreadyOpenException);
        }

        private void ReplaceDefaultUser()
        {
            var users = (from user in this.Session.Query<User>()
                         where user.IsDefault == true
                         select user).ToList();

            if (users.Count == 0) return;

            users[0].IsDefault = false;
            this.Session.Update(users[0]);
        }

        #endregion Methods
    }
}