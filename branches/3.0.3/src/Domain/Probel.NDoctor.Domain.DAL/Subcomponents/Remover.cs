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

    using log4net;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class Remover
    {
        #region Fields

        private readonly ILog Logger = LogManager.GetLogger(typeof(Creator));
        private readonly ISession Session;

        #endregion Fields

        #region Constructors

        public Remover(ISession session)
        {
            this.Session = session;
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
        /// Determines whether this instance can remove the specified drug dto.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified drug dto; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRemove(DrugDto item)
        {
            return (from t in this.Session.Query<Patient>()
                    where t.PrescriptionDocuments
                        .Where(e => e.Prescriptions
                            .Where(p => p.Drug.Id == item.Id).Count() > 0)
                            .Count() > 0
                    select t).Count() == 0;
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
            return (from t in this.Session.Query<Patient>()
                    where t.Reputation.Id == item.Id
                    select t).Count() == 0;
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
            var entity = this.Session.Get<Tag>(item.Id);
            switch (item.Category)
            {
                case TagCategory.Doctor: return this.CanRemoveSpecialisation(entity);
                case TagCategory.Picture: return this.CanRemovePictureTag(entity);
                case TagCategory.MedicalRecord: return this.CanRemoveRecordTag(entity);
                case TagCategory.Patient: return this.CanRemovePersonTag(entity);
                case TagCategory.Drug: return this.CanRemoveDrugTag(entity);
                case TagCategory.Prescription: return this.CanRemovePrescriptionTag(entity);
                case TagCategory.PrescriptionDocument: return this.CanRemovePrescriptionDocumentTag(entity);
                case TagCategory.Pathology: return this.CanRemovePathologyTag(entity);
                case TagCategory.Appointment: return this.CanRemoveAppointmentTag(entity);
                default:
                    Assert.FailOnEnumeration(item.Category);
                    return false;
            }
        }

        /// <summary>
        /// Determines whether this instance can remove the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRemove(ProfessionDto item)
        {
            return (from t in this.Session.Query<Patient>()
                    where t.Profession.Id == item.Id
                    select t).Count() == 0;
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
            return (from t in this.Session.Query<Patient>()
                    where t.Doctors.Where(e => e.Id == item.Id).Count() > 0
                    select t).Count() == 0;
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove</typeparam>
        /// <param name="id">The id of the item to remove.</param>
        public void Remove(MedicalRecordDto item)
        {
            Assert.IsNotNull(item, "item");
            this.Remove<MedicalRecord>(item);
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <param name="item"></param>
        public void Remove(TagDto item)
        {
            Assert.IsNotNull(item, "item");
            if (!this.CanRemove(item)) throw new ReferencialIntegrityException();
            this.Remove<Tag>(item);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove</param>
        public void Remove(PathologyDto item)
        {
            Assert.IsNotNull(item, "item");
            if (!this.CanRemove(item)) throw new ReferencialIntegrityException();
            this.Remove<Pathology>(item);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove</param>
        public void Remove(DrugDto item)
        {
            Assert.IsNotNull(item, "item");
            if (!this.CanRemove(item)) throw new ReferencialIntegrityException();
            this.Remove<Drug>(item);
        }

        /// <summary>
        /// Removes item with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of the item to remove</typeparam>
        /// <param name="id">The id of the item to remove.</param>
        public void Remove(InsuranceDto item)
        {
            Assert.IsNotNull(item, "item");
            if (!this.CanRemove(item)) throw new ReferencialIntegrityException();
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
            if (!this.CanRemove(item)) throw new ReferencialIntegrityException();
            this.Remove<Practice>(item);
        }

        /// <summary>
        /// Removes the specified doctor.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(DoctorDto item)
        {
            Assert.IsNotNull(item, "item");
            if (!this.CanRemove(item)) throw new ReferencialIntegrityException();
            this.Remove<Doctor>(item);
        }

        /// <summary>
        /// Removes the specified BMI entry from the specified patient.
        /// </summary>
        /// <param name="forPatient">The patient.</param>
        /// <param name="dto">The dto to remove.</param>
        public void Remove(BmiDto bmi, LightPatientDto forPatient)
        {
            var ebmi = this.Session.Get<Bmi>(bmi.Id);
            this.Session.Delete(ebmi);

            var patient = this.Session.Get<Patient>(forPatient.Id);
            this.Session.Update(patient);
        }

        /// <summary>
        /// Removes the specified meeting.
        /// </summary>
        /// <param name="meeting">The meeting.</param>
        /// <param name="forPatient">The patient.</param>
        public void Remove(AppointmentDto meeting, LightPatientDto forPatient)
        {
            var patientEntity = this.Session.Get<Patient>(forPatient.Id);

            var toRemove = (from a in patientEntity.Appointments
                            where a.Id == meeting.Id
                            select a).FirstOrDefault();
            if (toRemove == null) throw new EntityNotFoundException(typeof(Appointment));

            patientEntity.Appointments.Remove(toRemove);

            this.Session.Update(patientEntity);
            this.Session.Delete(toRemove);
        }

        /// <summary>
        /// Removes the specified illness period list from the specified patient's
        /// illness history.
        /// </summary>
        /// <param name="illnessPeriods">The illness periods.</param>
        /// <param name="forPatient">The patient.</param>
        public void Remove(IList<IllnessPeriodDto> illnessPeriods, LightPatientDto forPatient)
        {
            foreach (var illnessPeriod in illnessPeriods)
            {
                this.Remove(illnessPeriod, forPatient);
            }
        }

        /// <summary>
        /// Removes the specified illness period from the specified patient's
        /// illness history.
        /// </summary>
        /// <param name="illnessPeriod">The illness period.</param>
        /// <param name="forPatient">The patient.</param>
        public void Remove(IllnessPeriodDto illnessPeriod, LightPatientDto forPatient)
        {
            Assert.IsNotNull(illnessPeriod, "illnessPeriod");
            Assert.IsNotNull(forPatient, "patient");

            var entity = this.Session.Get<Patient>(forPatient.Id);

            for (int i = 0; i < entity.IllnessHistory.Count; i++)
            {
                if (entity.IllnessHistory[i].Id == illnessPeriod.Id)
                {
                    entity.IllnessHistory.RemoveAt(i);
                    break;
                }
            }
            this.Session.Update(entity);
        }

        /// <summary>
        /// Removes the link that existed between the specified patient and the specified doctor.
        /// </summary>
        /// <param name="doctor">The doctor to remove of the specified patient.</param>
        /// <param name="forPatient">The doctor will be unbound for this patient.</param>
        /// <exception cref="EntityNotFoundException">If there's no link between the doctor and the patient</exception>
        public void Remove(LightDoctorDto doctor, LightPatientDto forPatient)
        {
            var patientEntity = this.Session.Get<Patient>(forPatient.Id);
            var doctorEntity = this.Session.Get<Doctor>(doctor.Id);

            var doctorToDel = this.GetDoctorToDel(doctor, patientEntity);
            var patientToDel = this.GetPatientToDel(forPatient, doctorEntity);

            patientEntity.Doctors.Remove(doctorToDel);
            doctorEntity.Patients.Remove(patientToDel);

            this.Session.Update(patientEntity);
            this.Session.Update(doctorEntity);
        }

        /// <summary>
        /// Removes the specified item but doesn't touch the drugs liked to it.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(PrescriptionDocumentDto item)
        {
            foreach (var prescription in item.Prescriptions)
            {
                this.Remove<Prescription>(prescription);
            }

            this.Remove<PrescriptionDocument>(item);
        }

        /// <summary>
        /// Removes the specified prescription.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(PrescriptionDto item)
        {
            this.Remove<Prescription>(item);
        }

        public void Remove(MacroDto item)
        {
            Assert.IsNotNull(item, "item");
            this.Remove<Macro>(item);
        }

        /// <summary>
        /// Deletes the bmi with the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        public void RemoveBmi(LightPatientDto patient, DateTime date)
        {
            var current = (from p in this.Session.Query<Patient>()
                           where p.Id == patient.Id
                           select p).FirstOrDefault();

            var deletedCount = 0;
            for (int i = 0; i < current.BmiHistory.Count; i++)
            {
                if (current.BmiHistory[i].Date.Date == date.Date)
                {
                    current.BmiHistory.Remove(current.BmiHistory[i]);
                    deletedCount++;
                    i--; //I've deleted one item, I step back and continue to check for deletion
                }
            }
            this.Session.Update(current);
        }

        internal void Remove<TEntity>(BaseDto item)
            where TEntity : Entity
        {
            var loaded = this.Session.Get<TEntity>(item.Id);
            if (loaded != null)
            {
                this.Session.Delete(loaded);
            }
        }

        internal void Remove<TEntity, TDto>(TDto item, Action<TEntity> updateBefore)
            where TEntity : Entity
            where TDto : BaseDto
        {
            var entity = this.Session.Get<TEntity>(item.Id);
            updateBefore(entity);
            this.Session.Update(entity);

            this.Session.Update(entity);
            this.Remove<TEntity>(item);
        }

        private bool CanRemoveAppointmentTag(Tag entity)
        {
            return (from p in this.Session.Query<Appointment>()
                    where p.Tag.Id == entity.Id
                    select p).Count() == 0;
        }

        private bool CanRemoveDrugTag(Tag entity)
        {
            return (from p in this.Session.Query<Drug>()
                    where p.Tag.Id == entity.Id
                    select p).Count() == 0;
        }

        private bool CanRemovePathologyTag(Tag entity)
        {
            return (from p in this.Session.Query<Pathology>()
                    where p.Tag.Id == entity.Id
                    select p).Count() == 0;
        }

        private bool CanRemovePersonTag(Tag entity)
        {
            return (from p in this.Session.Query<Person>()
                    where p.Tag.Id == entity.Id
                    select p).Count() == 0;
        }

        private bool CanRemovePictureTag(Tag entity)
        {
            return (from p in this.Session.Query<Picture>()
                    where p.Tag.Id == entity.Id
                    select p).Count() == 0;
        }

        private bool CanRemovePrescriptionDocumentTag(Tag entity)
        {
            return (from p in this.Session.Query<Prescription>()
                    where p.Tag.Id == entity.Id
                    select p).Count() == 0;
        }

        private bool CanRemovePrescriptionTag(Tag entity)
        {
            return (from p in this.Session.Query<Prescription>()
                    where p.Tag.Id == entity.Id
                    select p).Count() == 0;
        }

        private bool CanRemoveRecordTag(Tag entity)
        {
            return (from r in this.Session.Query<MedicalRecord>()
                    where r.Tag.Id == entity.Id
                    select r).Count() == 0;
        }

        private bool CanRemoveSpecialisation(Tag tag)
        {
            return (from d in this.Session.Query<Doctor>()
                    where d.Specialisation.Id == tag.Id
                    select d).Count() == 0;
        }

        private Doctor GetDoctorToDel(LightDoctorDto doctor, Patient patientEntity)
        {
            var doctorToDel = (from d in patientEntity.Doctors
                               where d.Id == doctor.Id
                               select d).FirstOrDefault();

            if (doctorToDel == null) throw new EntityNotFoundException(typeof(Doctor));
            else return doctorToDel;
        }

        private Patient GetPatientToDel(LightPatientDto patient, Doctor doctorEntity)
        {
            var patientToDel = (from p in doctorEntity.Patients
                                where p.Id == patient.Id
                                select p).FirstOrDefault();

            if (patientToDel == null) throw new EntityNotFoundException(typeof(Patient));
            else return patientToDel;
        }

        #endregion Methods
    }
}