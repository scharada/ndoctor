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
    using System.Linq;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Assertion;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Helpers;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Contains all the query to manage the medical records of a patient
    /// </summary>
    public class MedicalRecordComponent : BaseComponent, IMedicalRecordComponent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicalRecordComponent"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public MedicalRecordComponent(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicalRecordComponent"/> class.
        /// </summary>
        public MedicalRecordComponent()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates the specified record and link it to the specidied patient.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="forPatient">For patient.</param>
        public void Create(MedicalRecordDto record, LightPatientDto forPatient)
        {
            Assert.IsNotNull(record, "The item to create shouldn't be null");
            this.CheckSession();

            var foundPatient = (from p in this.Session.Query<Patient>()
                                where p.Id == forPatient.Id
                                select p).FirstOrDefault();
            if (foundPatient == null) throw new EntityNotFoundException(typeof(Patient));

            var recEntity = Mapper.Map<MedicalRecordDto, MedicalRecord>(record);

            foundPatient.MedicalRecords.Add(recEntity);

            this.Session.SaveOrUpdate(foundPatient);
        }

        /// <summary>
        /// Finds the medical record by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The medical record or <c>Null</c> if not found
        /// </returns>
        public MedicalRecordDto FindMedicalRecordById(long id)
        {
            this.CheckSession();

            var result = this.Session.Get<MedicalRecord>(id);

            return (result != null)
                ? Mapper.Map<MedicalRecord, MedicalRecordDto>(result)
                : null;
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
            this.CheckSession();
            var selectedPatient = (from p in this.Session.Query<Patient>()
                                   where p.Id == patient.Id
                                   select p).FirstOrDefault();

            if (selectedPatient == null) throw new EntityNotFoundException(string.Format("No patient with id '{0}' was found.", patient.Id));

            return Mapper.Map<Patient, MedicalRecordCabinetDto>(selectedPatient);
        }

        /// <summary>
        /// Commits the changes on medical record cabinet.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="cabinet">The cabinet.</param>
        public void UpdateCabinet(LightPatientDto patient, MedicalRecordCabinetDto cabinet)
        {
            this.CheckSession();
            using (var tx = this.Session.BeginTransaction())
            {
                cabinet.ForeachRecords(record =>
                {
                    switch (record.State)
                    {
                        case State.Clean:
                            //Nothing to do
                            break;
                        case State.Updated:
                            record.LastUpdate = DateTime.Now;
                            this.Update(record);
                            break;
                        case State.Created:
                            record.LastUpdate
                                = record.CreationDate
                                = DateTime.Now;
                            this.Create(record);
                            break;
                        case State.Removed:
                            this.Remove(record);
                            break;
                        default:
                            Assert.FailOnEnumeration(record.State);
                            break;
                    }
                });
                tx.Commit();
            }
        }

        #endregion Methods
    }
}