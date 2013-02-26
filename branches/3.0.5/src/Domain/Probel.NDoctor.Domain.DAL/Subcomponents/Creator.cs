namespace Probel.NDoctor.Domain.DAL.Subcomponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using log4net;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.GoogleCalendar;
    using Probel.NDoctor.Domain.DAL.Helpers;
    using Probel.NDoctor.Domain.DAL.Macro;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.GoogleCalendar;
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class Creator
    {
        #region Fields

        private readonly ILog Logger = LogManager.GetLogger(typeof(Creator));
        private readonly ISession Session;

        #endregion Fields

        #region Constructors

        public Creator(ISession session)
        {
            this.Session = session;
        }

        #endregion Constructors

        #region Methods

        public long Create(MacroDto item)
        {
            Assert.IsNotNull(item, "item");

            if (!MacroBuilder.IsValidExpression(item.Expression)) { throw new InvalidMacroException(); }

            var exist = (from i in this.Session.Query<MacroDto>()
                         where i.Id == item.Id
                         select i).ToList().Count() > 0;
            if (exist) throw new ExistingItemException();

            var entity = Mapper.Map<MacroDto, Macro>(item);
            item.Id = (long)this.Session.Save(entity);
            return item.Id;
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        public long Create(DrugDto item)
        {
            Assert.IsNotNull(item, "item");

            var exist = (from i in this.Session.Query<Drug>()
                         where i.Name.ToUpper() == item.Name.ToUpper()
                            || i.Id == item.Id
                         select i).ToList().Count() > 0;
            if (exist) throw new ExistingItemException();

            var entity = Mapper.Map<DrugDto, Drug>(item);
            item.Id = (long)this.Session.Save(entity);
            return item.Id;
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        public long Create(PatientDto item)
        {
            Assert.IsNotNull(item, "item");

            var found = (from p in this.Session.Query<Patient>()
                         where p.Id == item.Id
                         select p).ToList().Count() > 0;
            if (found) throw new ExistingItemException();

            var entity = Mapper.Map<PatientDto, Patient>(item);
            item.Id = (long)this.Session.Save(entity);
            return item.Id;
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        public long Create(LightDoctorDto item)
        {
            Assert.IsNotNull(item, "item");

            var found = (from p in this.Session.Query<Doctor>()
                         where p.Id == item.Id
                         select p).ToList().Count() > 0;
            if (found) throw new ExistingItemException();

            var entity = Mapper.Map<LightDoctorDto, Doctor>(item);
            item.Id = (long)this.Session.Save(entity);
            return item.Id;
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        public long Create(TagDto item)
        {
            Assert.IsNotNull(item, "item");
            Assert.IsNotNull(item.Name, "item.Name");

            var exist = (from p in this.Session.Query<Tag>()
                         where (p.Name.ToUpper() == item.Name.ToUpper()
                             && p.Category == item.Category)
                             || p.Id == item.Id
                         select p).ToList().Count() > 0;
            if (exist) throw new ExistingItemException();

            var entity = Mapper.Map<TagDto, Tag>(item);
            item.Id = (long)this.Session.Save(entity);
            return item.Id;
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        public long Create(UserDto item)
        {
            Assert.IsNotNull(item, "item");

            var found = (from p in this.Session.Query<User>()
                         where p.Id == item.Id
                         select p).ToList().Count() > 0;
            if (found) throw new ExistingItemException();

            if (item.IsDefault) this.ReplaceDefaultUser();

            this.Session.Flush();

            var entity = Mapper.Map<UserDto, User>(item);
            if (string.IsNullOrWhiteSpace(entity.Password)) throw new EmptyPasswordException();

            if (this.IsFirstUser()) { entity.IsSuperAdmin = true; }

            item.Id = (long)this.Session.Save(entity);
            return item.Id;
        }

        /// <summary>
        /// Creates a new patient.
        /// </summary>
        /// <param name="item">The patient.</param>
        public long Create(LightPatientDto item)
        {
            Assert.IsNotNull(item, "item");

            var found = (from p in this.Session.Query<Patient>()
                         where p.Id == item.Id
                         || (
                            p.FirstName.ToLower() == item.FirstName.ToLower()
                            && p.LastName.ToLower() == item.LastName.ToLower()
                         )
                         select p).ToList().Count() > 0;

            var newEntity = Mapper.Map<LightPatientDto, Patient>(item);
            newEntity.InscriptionDate = DateTime.Today;

            if (found) throw new ExistingItemException();

            item.Id = (long)this.Session.Save(newEntity);
            return item.Id;
        }

        /// <summary>
        /// Creates the specified profession.
        /// </summary>
        /// <param name="item">The profession.</param>
        /// <returns></returns>
        public long Create(ProfessionDto item)
        {
            Assert.IsNotNull(item, "item");

            var found = (from p in this.Session.Query<Profession>()
                         where p.Id == item.Id
                            || item.Name.ToLower() == p.Name.ToLower()
                         select p).ToList().Count() > 0;
            if (found) throw new ExistingItemException();

            var entity = Mapper.Map<ProfessionDto, Profession>(item);

            item.Id = (long)this.Session.Save(entity);
            return item.Id;
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        public long Create(ReputationDto item)
        {
            Assert.IsNotNull(item, "item");

            var exist = (from p in this.Session.Query<Reputation>()
                         where item.Name.ToUpper() == p.Name.ToUpper()
                         || p.Id == item.Id
                         select p).ToList().Count() > 0;
            if (exist) throw new ExistingItemException();

            var entity = Mapper.Map<ReputationDto, Reputation>(item);

            item.Id = (long)this.Session.Save(entity);
            return item.Id;
        }

        /// <summary>
        /// Creates the specified pathology.
        /// </summary>
        /// <param name="item">The item.</param>
        public long Create(PathologyDto item)
        {
            var found = (from p in this.Session.Query<Pathology>()
                         where p.Id == item.Id
                            || item.Name.ToLower() == p.Name.ToLower()
                         select p).ToList().Count() > 0;
            if (found) throw new ExistingItemException();

            var entity = Mapper.Map<PathologyDto, Pathology>(item);

            item.Id = (long)this.Session.Save(entity);
            return item.Id;
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="reputation">The item to add in the database</param>
        public long Create(PracticeDto item)
        {
            Assert.IsNotNull(item, "item");

            var exist = (from i in this.Session.Query<Practice>()
                         where item.Name.ToUpper() == i.Name.ToUpper()
                            || i.Id == item.Id
                         select i).ToList().Count() > 0;
            if (exist) throw new ExistingItemException();

            var entity = Mapper.Map<PracticeDto, Practice>(item);

            item.Id = (long)this.Session.Save(entity);
            return item.Id;
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="reputation">The item to add in the database</param>
        public long Create(InsuranceDto item)
        {
            Assert.IsNotNull(item, "item");

            var exist = (from i in this.Session.Query<Insurance>()
                         where item.Name.ToUpper() == i.Name.ToUpper()
                            || i.Id == item.Id
                         select i).ToList().Count() > 0;
            if (exist) throw new ExistingItemException();

            var entity = Mapper.Map<InsuranceDto, Insurance>(item);

            item.Id = (long)this.Session.Save(entity);
            return item.Id;
        }

        /// <summary>
        /// Creates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        public long Create(SecurityUserDto item, string password)
        {
            Assert.IsNotNull(item, "item");
            if (string.IsNullOrEmpty(password)) throw new EmptyPasswordException();

            var found = (from p in this.Session.Query<User>()
                         where p.Id == item.Id
                           || (p.FirstName == item.FirstName
                            && p.LastName == item.LastName)

                         select p).ToList().Count() > 0;
            if (found) throw new ExistingItemException();

            var entity = Mapper.Map<SecurityUserDto, User>(item);
            entity.Password = password;

            if (entity.IsDefault) this.RemoveDefaultUser();
            if (this.IsFirstUser()) { entity.IsSuperAdmin = true; }

            item.Id = (long)this.Session.Save(entity);
            return item.Id;
        }

        /// <summary>
        /// Creates a new role in the repository.
        /// </summary>
        /// <param name="item">The role.</param>
        /// <returns></returns>
        public long Create(RoleDto item)
        {
            var entity = Mapper.Map<RoleDto, Role>(item);

            entity = this.Session.Merge(entity);
            this.Session.Save(entity);
            this.Session.Flush();

            Mapper.Map<Role, RoleDto>(entity, item);

            item.Id = entity.Id;
            return entity.Id;
        }

        /// <summary>
        /// Creates the specified task.
        /// </summary>
        /// <param name="item">The task.</param>
        /// <returns>
        /// The id of the created item
        /// </returns>
        public long Create(TaskDto item)
        {
            Assert.IsNotNull(item, "item");

            var exist = (from i in this.Session.Query<TaskDto>()
                         where i.Id == item.Id
                         select i).ToList().ToList().Count() > 0;
            if (exist) throw new ExistingItemException();

            var entity = Mapper.Map<TaskDto, Task>(item);
            item.Id = (long)this.Session.Save(entity);
            return item.Id;
        }

        /// <summary>
        /// Creates the specified meeting.
        /// </summary>
        /// <param name="appointment">The meeting.</param>
        /// <param name="patient">The patient.</param>
        public void Create(AppointmentDto appointment, LightPatientDto patient)
        {
            var patientEntity = this.Session.Get<Patient>(patient.Id);
            var meetingEntity = Mapper.Map<AppointmentDto, Appointment>(appointment);

            patientEntity.Appointments.Add(meetingEntity);
            this.Session.SaveOrUpdate(patientEntity);
        }

        /// <summary>
        /// Creates the specified appointment and use create it in Google Calendar if the binding is active.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <param name="patient">The patient.</param>
        /// <param name="config">The config.</param>
        public void Create(AppointmentDto appointment, LightPatientDto patient, GoogleConfiguration config)
        {
            var patientEntity = this.Session.Get<Patient>(patient.Id);
            var meetingEntity = Mapper.Map<AppointmentDto, Appointment>(appointment);

            meetingEntity.GoogleSynchronisationId = (config.IsActive)
                ? Guid.NewGuid() //This id allows to find Google appointments
                : new Guid(); //Empty Guid indicates this appointments is not binded with Google Calendar

            meetingEntity.User = this.Session.Get<User>(meetingEntity.User.Id);

            patientEntity.Appointments.Add(meetingEntity);
            this.Session.SaveOrUpdate(patientEntity);

            if (config.IsActive)
            {
                new GoogleService(config).AddAppointment(meetingEntity);
            }
        }

        /// <summary>
        /// Creates the specified record and link it to the specidied patient.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="forPatient">For patient.</param>
        public void Create(MedicalRecordDto record, LightPatientDto forPatient)
        {
            Assert.IsNotNull(record, "item");

            var foundPatient = (from p in this.Session.Query<Patient>()
                                where p.Id == forPatient.Id
                                select p).FirstOrDefault();
            if (foundPatient == null) throw new EntityNotFoundException(typeof(Patient));

            var recEntity = Mapper.Map<MedicalRecordDto, MedicalRecord>(record);

            foundPatient.MedicalRecords.Add(recEntity);

            this.Session.SaveOrUpdate(foundPatient);
        }

        /// <summary>
        /// Creates the specified patients.
        /// </summary>
        /// <param name="patients"></param>
        public void Create(IEnumerable<PatientFullDto> patients)
        {
            var entities = Mapper.Map<IEnumerable<PatientFullDto>, Patient[]>(patients);
            this.Save(entities);
        }

        /// <summary>
        /// Creates the specified illness period for the specified patient.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <param name="patient">The patient.</param>
        public void Create(IllnessPeriodDto period, LightPatientDto patient)
        {
            var entity = this.Session.Get<Patient>(patient.Id);
            var illnessPeriod = Mapper.Map<IllnessPeriodDto, IllnessPeriod>(period);

            entity.IllnessHistory.Add(illnessPeriod);
            this.Session.SaveOrUpdate(entity);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public long Create(DoctorDto item)
        {
            Assert.IsNotNull(item, "item");

            var found = (from p in this.Session.Query<Doctor>()
                         where p.Id == item.Id
                            || (item.FirstName.ToUpper() == p.FirstName.ToUpper()
                            && item.LastName.ToUpper() == p.LastName.ToUpper())
                         select p).ToList().Count() > 0;
            if (found) throw new ExistingItemException();

            var entity = Mapper.Map<DoctorDto, Doctor>(item);

            item.Id = (long)this.Session.Save(entity);
            return item.Id;
        }

        /// <summary>
        /// Creates the specified picture for the specified patient.
        /// </summary>
        /// <param name="picture">The picture.</param>
        /// <param name="patient">The patient.</param>
        public void Create(PictureDto picture, LightPatientDto forPatient)
        {
            Assert.IsNotNull(picture, "item");

            var foundPatient = (from p in this.Session.Query<Patient>()
                                where p.Id == forPatient.Id
                                select p).FirstOrDefault();
            if (foundPatient == null) throw new ExistingItemException();

            var newItem = Mapper.Map<PictureDto, Picture>(picture);
            newItem.Refresh(picture, this.Session);

            foundPatient.Pictures.Add(newItem);
            new ImageHelper().TryCreateThumbnail(foundPatient.Pictures);
            this.Session.Update(foundPatient);
        }

        /// <summary>
        /// Creates the specified prescription document for the specified patient.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="patient">The patient.</param>
        public void Create(PrescriptionDocumentDto document, LightPatientDto patient)
        {
            var entity = this.Session.Get<Patient>(patient.Id);
            var documentEntity = Mapper.Map<PrescriptionDocumentDto, PrescriptionDocument>(document);

            ReloadTagsFor(documentEntity);

            entity.PrescriptionDocuments.Add(documentEntity);
            this.Session.SaveOrUpdate(entity);
        }

        /// <summary>
        /// Adds a bmi entry to the specified patient.
        /// </summary>
        /// <param name="bmi">The bmi.</param>
        /// <param name="forPatient">The patient.</param>
        public void Create(BmiDto bmi, LightPatientDto forPatient)
        {
            var entity = this.Session.Get<Patient>(forPatient.Id);

            if (entity != null)
            {
                entity.Height = bmi.Height;
                entity.BmiHistory.Add(Mapper.Map<BmiDto, Bmi>(bmi));
                this.Session.Update(entity);

                forPatient.Height = bmi.Height;
            }
            else { throw new EntityNotFoundException(typeof(Bmi)); }
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        public void Create(MedicalRecordDto item)
        {
            Assert.IsNotNull(item, "item");

            var found = (from p in this.Session.Query<MedicalRecord>()
                         where p.Id == item.Id
                         select p).ToList().Count() > 0;
            if (found) throw new ExistingItemException();

            var entity = Mapper.Map<MedicalRecordDto, MedicalRecord>(item);
            this.Session.Save(entity);
        }

        private bool IsFirstUser()
        {
            var result = (from u in this.Session.Query<User>()
                          where u.IsSuperAdmin == true
                          select u).Take(2);

            return result.Count() == 0;
        }

        private void ReloadTagsFor(PrescriptionDocument prescriptionDocument)
        {
            foreach (var prescripiton in prescriptionDocument.Prescriptions)
            {
                if (prescripiton.Tag != null)
                {
                    prescripiton.Tag = (from t in this.Session.Query<Tag>()
                                        where t.Id == prescripiton.Tag.Id
                                        select t).First();
                }
                prescripiton.Drug = this.Session.Get<Drug>(prescripiton.Drug.Id);
            }
        }

        private void RemoveDefaultUser()
        {
            var defaultUsers = (from u in this.Session.Query<User>()
                                where u.IsDefault == true
                                select u);

            if (defaultUsers.Count() > 1) this.Logger.Warn("There's more than one default user in the database!");
            foreach (var user in defaultUsers)
            {
                user.IsDefault = false;
                this.Session.Update(user);
            }
        }

        private void ReplaceDefaultUser()
        {
            var user = (from u in this.Session.Query<User>()
                        where u.IsDefault == true
                        select u).FirstOrDefault();

            if (user == null) return;

            user.IsDefault = false;
            this.Session.Update(user);
        }

        private void Save(Entity[] entities)
        {
            try
            {
                using (var tx = this.Session.BeginTransaction())
                {
                    foreach (var entity in entities)
                    {
                        this.Session.SaveOrUpdate(entity);
                    }
                    tx.Commit();
                }
            }
            catch (Exception ex) { throw new QueryException(ex); }
        }

        #endregion Methods
    }
}