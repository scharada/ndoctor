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
    using Probel.NDoctor.Domain.DAL.Helpers;
    using Probel.NDoctor.Domain.DAL.Subcomponents;
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
        public void AddDoctorTo(LightPatientDto patient, LightDoctorDto doctor)
        {
            var patientEntity = this.Session.Get<Patient>(patient.Id);
            var doctorEntity = this.Session.Get<Doctor>(doctor.Id);

            if (patientEntity == null) throw new EntityNotFoundException(typeof(Patient));
            if (doctorEntity == null) throw new EntityNotFoundException(typeof(Doctor));

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
            return new Creator(this.Session).Create(profession);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="reputation">The item to add in the database</param>
        public long Create(ReputationDto reputation)
        {
            return new Creator(this.Session).Create(reputation);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="reputation">The item to add in the database</param>
        public long Create(InsuranceDto insurance)
        {
            return new Creator(this.Session).Create(insurance);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="reputation">The item to add in the database</param>
        public long Create(PracticeDto practice)
        {
            return new Creator(this.Session).Create(practice);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="doctor"></param>
        /// <returns></returns>
        public long Create(DoctorDto doctor)
        {
            return new Creator(this.Session).Create(doctor);
        }

        /// <summary>
        /// Gets the doctors linked to the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>A list of doctors</returns>
        public IList<LightDoctorDto> FindDoctorOf(LightPatientDto patient)
        {
            var entity = this.Session.Get<Patient>(patient.Id);
            return Mapper.Map<IList<Doctor>, IList<LightDoctorDto>>(entity.Doctors);
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
        public IList<LightDoctorDto> FindNotLinkedDoctorsFor(LightPatientDto patient, string criteria, SearchOn searchOn)
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
        /// Loads all the data of the patient represented by the specified id.
        /// </summary>
        /// <param name="patient">The id of the patient to load.</param>
        /// <returns>A DTO with the whole data</returns>
        /// <exception cref="Probel.NDoctor.Domain.DAL.Exceptions.EntityNotFoundException">If the id is not linked to a patient</exception>
        public PatientDto FindPatient(long id)
        {
            var fullPatient = (from p in this.Session.Query<Patient>()
                               where p.Id == id
                               select p).FirstOrDefault();

            if (fullPatient == null) { throw new EntityNotFoundException(typeof(Patient)); }

            // Fix for patient created before issue 76: if address is null
            // It's impossible to update the address with the databinding.
            if (fullPatient.Address == null) { fullPatient.Address = new Address(); }

            return Mapper.Map<Patient, PatientDto>(fullPatient);
        }

        /// <summary>
        /// Loads all the data of the patient.
        /// </summary>
        /// <param name="patient">The patient to load.</param>
        /// <returns>A DTO with the whole data</returns>
        /// <exception cref="Probel.NDoctor.Domain.DAL.Exceptions.EntityNotFoundException">If the patient doesn't exist</exception>
        public PatientDto FindPatient(LightPatientDto patient)
        {
            return this.FindPatient(patient.Id);
        }

        /// <summary>
        /// Removes the link that existed between the specified patient and the specified doctor.
        /// </summary>
        /// <exception cref="EntityNotFoundException">If there's no link between the doctor and the patient</exception>
        /// <param name="patient">The patient.</param>
        /// <param name="doctor">The doctor.</param>
        public void RemoveDoctorFor(LightPatientDto patient, LightDoctorDto doctor)
        {
            new Remover(this.Session).Remove(doctor, patient);
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