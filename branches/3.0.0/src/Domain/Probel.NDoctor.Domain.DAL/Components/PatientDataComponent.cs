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

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Exceptions;
    using Probel.NDoctor.Domain.DAL.Helpers;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;

    public class PatientDataComponent : BaseComponent, IPatientDataComponent
    {
        #region Constructors

        public PatientDataComponent(ISession session)
            : base(session)
        {
        }

        public PatientDataComponent()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Adds the specified doctor to the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="doctor">The doctor.</param>
        /// <exception cref="EntityNotFoundException">If there's no link between the doctor and the patient</exception>
        public void AddLink(LightPatientDto patient, LightDoctorDto doctor)
        {
            this.CheckSession();
            var patientEntity = this.Session.Get<Patient>(patient.Id);
            var doctorEntity = this.Session.Get<Doctor>(doctor.Id);

            if (patientEntity == null || doctorEntity == null) throw new EntityNotFoundException();

            patientEntity.Doctors.Add(doctorEntity);
            doctorEntity.Patients.Add(patientEntity);

            using (var tx = this.Session.Transaction)
            {
                tx.Begin();
                this.Session.Update(patientEntity);
                this.Session.Update(doctorEntity);
                tx.Commit();
            }
        }

        /// <summary>
        /// Creates the specified profession.
        /// </summary>
        /// <param name="profession">The profession.</param>
        public long Create(ProfessionDto profession)
        {
            Assert.IsNotNull(profession, "The item to create shouldn't be null");
            this.CheckSession();

            var found = (from p in this.Session.Query<Profession>()
                         where p.Id == profession.Id
                            || profession.Name.ToLower() == p.Name.ToLower()
                         select p).Count() > 0;
            if (found) throw new ExistingItemException();

            var entity = Mapper.Map<ProfessionDto, Profession>(profession);
            entity.Id = 0;

            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="reputation">The item to add in the database</param>
        public long Create(ReputationDto reputation)
        {
            Assert.IsNotNull(reputation, "The item to create shouldn't be null");
            this.CheckSession();

            var exist = (from p in this.Session.Query<Reputation>()
                         where reputation.Name.ToUpper() == p.Name.ToUpper()
                         || p.Id == reputation.Id
                         select p).Count() > 0;
            if (exist) throw new ExistingItemException();

            var entity = Mapper.Map<ReputationDto, Reputation>(reputation);
            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="reputation">The item to add in the database</param>
        public long Create(InsuranceDto insurance)
        {
            Assert.IsNotNull(insurance, "The item to create shouldn't be null");
            this.CheckSession();

            var exist = (from i in this.Session.Query<Insurance>()
                         where insurance.Name.ToUpper() == i.Name.ToUpper()
                            || i.Id == insurance.Id
                         select i).Count() > 0;
            if (exist) throw new ExistingItemException();

            var entity = Mapper.Map<InsuranceDto, Insurance>(insurance);
            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="reputation">The item to add in the database</param>
        public long Create(PracticeDto practice)
        {
            Assert.IsNotNull(practice, "The item to create shouldn't be null");
            this.CheckSession();

            var exist = (from i in this.Session.Query<Practice>()
                         where practice.Name.ToUpper() == i.Name.ToUpper()
                            || i.Id == practice.Id
                         select i).Count() > 0;
            if (exist) throw new ExistingItemException();

            var entity = Mapper.Map<PracticeDto, Practice>(practice);
            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="doctor"></param>
        /// <returns></returns>
        public long Create(DoctorDto doctor)
        {
            Assert.IsNotNull(doctor, "The item to create shouldn't be null");
            this.CheckSession();

            var found = (from p in this.Session.Query<Doctor>()
                         where p.Id == doctor.Id
                            || (doctor.FirstName.ToUpper() == p.FirstName.ToUpper()
                            && doctor.LastName.ToUpper() == doctor.LastName.ToUpper())
                         select p).Count() > 0;
            if (found) throw new ExistingItemException();

            var entity = Mapper.Map<DoctorDto, Doctor>(doctor);
            entity.Id = 0;

            return (long)this.Session.Save(entity);
        }

        /// <summary>
        /// Finds the doctors that can be linked to the specified doctor.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="on">Indicate where the search should be executed.</param>
        /// <returns>
        /// A list of doctor
        /// </returns>
        public IList<LightDoctorDto> FindDoctorsFor(LightPatientDto patient, string criteria, SearchOn searchOn)
        {
            var patientEntity = this.Session.Get<Patient>(patient.Id);

            List<Doctor> result = new List<Doctor>();

            switch (searchOn)
            {
                case SearchOn.FirstName:
                    result = this.SearchDoctorOnFirstName(criteria);
                    break;
                case SearchOn.LastName:
                    result = this.SearchDoctorOnLastName(criteria);
                    break;
                case SearchOn.FirstAndLastName:
                    result = this.SearchDoctorOnFirstNameAndLastName(criteria);
                    break;
                default:
                    Assert.FailOnEnumeration(searchOn);
                    break;
            }

            result = this.RemoveDoctorsOfPatient(result, patientEntity);

            return Mapper.Map<IList<Doctor>, IList<LightDoctorDto>>(result);
        }

        /// <summary>
        /// Gets the doctors linked to the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>A list of doctors</returns>
        public IList<LightDoctorDto> GetDoctorOf(LightPatientDto patient)
        {
            this.CheckSession();
            var entity = this.Session.Get<Patient>(patient.Id);
            return Mapper.Map<IList<Doctor>, IList<LightDoctorDto>>(entity.Doctors);
        }

        /// <summary>
        /// Loads all the data of the patient represented by the specified id.
        /// </summary>
        /// <param name="patient">The id of the patient to load.</param>
        /// <returns>A DTO with the whole data</returns>
        /// <exception cref="Probel.NDoctor.Domain.DAL.Exceptions.EntityNotFoundException">If the id is not linked to a patient</exception>
        public PatientDto GetPatient(long id)
        {
            this.CheckSession();
            var fullPatient = (from p in this.Session.Query<Patient>()
                               where p.Id == id
                               select p).FirstOrDefault();

            if (fullPatient == null) throw new EntityNotFoundException();

            return Mapper.Map<Patient, PatientDto>(fullPatient);
        }

        /// <summary>
        /// Loads all the data of the patient.
        /// </summary>
        /// <param name="patient">The patient to load.</param>
        /// <returns>A DTO with the whole data</returns>
        /// <exception cref="Probel.NDoctor.Domain.DAL.Exceptions.EntityNotFoundException">If the patient doesn't exist</exception>
        public PatientDto GetPatient(LightPatientDto patient)
        {
            return this.GetPatient(patient.Id);
        }

        /// <summary>
        /// Removes the link that existed between the specified patient and the specified doctor.
        /// </summary>
        /// <exception cref="EntityNotFoundException">If there's no link between the doctor and the patient</exception>
        /// <param name="patient">The patient.</param>
        /// <param name="doctor">The doctor.</param>
        public void RemoveLink(LightPatientDto patient, LightDoctorDto doctor)
        {
            var patientEntity = this.Session.Get<Patient>(patient.Id);
            var doctorEntity = this.Session.Get<Doctor>(doctor.Id);

            var doctorToDel = this.FindDoctorToDel(doctor, patientEntity);
            var patientToDel = this.FindPatientToDel(patient, doctorEntity);

            patientEntity.Doctors.Remove(doctorToDel);
            doctorEntity.Patients.Remove(patientToDel);

            using (var tx = this.Session.Transaction)
            {
                tx.Begin();
                this.Session.Update(patientEntity);
                this.Session.Update(doctorEntity);
                tx.Commit();
            }
        }

        private Doctor FindDoctorToDel(LightDoctorDto doctor, Patient patientEntity)
        {
            var doctorToDel = (from d in patientEntity.Doctors
                               where d.Id == doctor.Id
                               select d).FirstOrDefault();

            if (doctorToDel == null) throw new EntityNotFoundException();
            else return doctorToDel;
        }

        private Patient FindPatientToDel(LightPatientDto patient, Doctor doctorEntity)
        {
            var patientToDel = (from p in doctorEntity.Patients
                                where p.Id == patient.Id
                                select p).FirstOrDefault();

            if (patientToDel == null) throw new EntityNotFoundException();
            else return patientToDel;
        }

        private bool NotIn(Patient patient, Doctor toCheck)
        {
            return (from d in patient.Doctors
                    where d.Id == toCheck.Id
                    select d).Count() == 0;
        }

        private List<Doctor> RemoveDoctorsOfPatient(List<Doctor> result, Patient patientEntity)
        {
            return (from d in result
                    where !patientEntity.Doctors.Contains(d, new EntityEqualityComparer())
                    select d).ToList();
        }

        private List<Doctor> SearchDoctorOnFirstName(string criteria)
        {
            return (from d in this.Session.Query<Doctor>()
                    where d.FirstName.Contains(criteria)
                    select d).ToList();
        }

        private List<Doctor> SearchDoctorOnFirstNameAndLastName(string criteria)
        {
            return (from d in this.Session.Query<Doctor>()
                    where d.FirstName.Contains(criteria)
                       || d.LastName.Contains(criteria)
                    select d).ToList();
        }

        private List<Doctor> SearchDoctorOnLastName(string criteria)
        {
            return (from d in this.Session.Query<Doctor>()
                    where d.LastName.Contains(criteria)
                    select d).ToList();
        }

        #endregion Methods
    }
}