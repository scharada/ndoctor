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

namespace Probel.NDoctor.Domain.Test.Components
{
    using System;
    using System.Linq;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.Test.TestHelpers;

    [TestFixture]
    public class PatientDataComponentTest : BaseComponentTest<PatientDataComponent>
    {
        #region Methods

        /// <summary>
        /// issue 122
        /// </summary>
        [Test]
        public void SearchDoctor_UserJokerSearch_ReturnsAllDoctors()
        {
            //The patient 3 has two doctors binded.
            var patient = this.HelperComponent.GetLightPatient(3);

            var count1 = this.ComponentUnderTest.GetNotLinkedDoctorsFor(patient, "*", SearchOn.FirstAndLastName).Count;
            var count2 = this.HelperComponent.GetAllDoctors().Count - 2;

            Assert.AreEqual(count1, count2);
        }

        /// <summary>
        /// Issue 127
        /// </summary>
        [Test]
        public void SearchPatient_GetByIdWithUnknownId_EntityNotFoundException()
        {
            var today = DateTime.Today;
            var patient = this.ComponentUnderTest.GetPatientById(3);
            patient.Birthdate = today;

            this.ComponentUnderTest.Update(patient);
            var found = this.ComponentUnderTest.GetLightPatientById(patient.Id);

            Assert.AreEqual(found.Id, patient.Id);
            Assert.AreEqual(today, found.Birthdate);
        }

        /// <summary>
        /// Issue 127
        /// </summary>
        [Test]
        public void SearchPatient_GetById_ItReturnsTheLastedData()
        {
            var today = DateTime.Today;
            var patient = this.ComponentUnderTest.GetPatientById(3);
            patient.Birthdate = today;

            this.ComponentUnderTest.Update(patient);
            var found = this.ComponentUnderTest.GetLightPatientById(patient.Id);

            Assert.AreEqual(found.Id, patient.Id);
            Assert.AreEqual(today, found.Birthdate);
        }

        /// <summary>
        /// Issue 127.
        /// </summary>
        [Test]
        public void SearchPatient_WithExistingId_PatientFound()
        {
            //Patient with ID 3 exists (Cf. InsertUsers.sql)
            var patient = this.ComponentUnderTest.GetLightPatientById(3);
            Assert.NotNull(patient);
        }

        /// <summary>
        /// Issue 127.
        /// </summary>
        [Test]
        public void SearchPatient_WithNotExistingId_EntityNotFoundException()
        {
            //Patient with ID 999 doesn't exist (Cf. InsertUsers.sql)
            Assert.Throws<EntityNotFoundException>(() => this.ComponentUnderTest.GetLightPatientById(999));
        }

        /// <summary>
        /// Issue 127.
        /// </summary>
        [Test]
        public void UpdataPatient_ChangeData_AllRecordsAreStillHere()
        {
            var today = DateTime.Today;
            //Patient with ID 3 exists (Cf. InsertUsers.sql)
            var patient = this.ComponentUnderTest.GetLightPatientById(3);

            var cabinet = new MedicalRecordComponent(this.Session).GetMedicalRecordCabinet(patient);
            Assert.Greater(cabinet.Folders.Count, 0, "Expecting folders in cabinet");
            Assert.Greater(cabinet.Folders[0].Records.Count, 0, "Expecting records in the first folder");

            this.WrapInTransaction(() =>
            {
                var item = this.GetPatient(patient.Id);
                item.Birthdate = today;
                this.ComponentUnderTest.Update(item);
            });

            cabinet = new MedicalRecordComponent(this.Session).GetMedicalRecordCabinet(patient);
            Assert.Greater(cabinet.Folders.Count, 0, "Expecting folders in cabinet");
            Assert.Greater(cabinet.Folders[0].Records.Count, 0, "Expecting records in the first folder");
            Assert.AreEqual(today, this.GetPatient(patient.Id).Birthdate);
        }

        [Test]
        public void UpdatePatient_TryToUpdateInsurance_NothingIsUpdated()
        {
            var name = GetRandom.String;
            var insuranceName = Guid.NewGuid().ToString();

            var patient = this.HelperComponent.GetAllPatients()[0];
            patient.Insurance.Name = insuranceName;

            this.WrapInTransaction(() => this.ComponentUnderTest.Update(patient));

            patient = this.HelperComponent.GetAllPatients()[0];
            Assert.AreNotEqual(insuranceName, patient.Insurance.Name);
        }

        [Test]
        public void UpdatePatient_WithNewInsurance_NoInsuranceDataLoss()
        {
            var name = GetRandom.String;

            var insurance = this.CreateInsurance();

            this.WrapInTransaction(() => this.ComponentUnderTest.Create(insurance));

            var patient = this.HelperComponent.GetAllPatients()[0];
            patient.Insurance = insurance;

            this.WrapInTransaction(() => this.ComponentUnderTest.Update(patient));

            Assert.AreNotEqual(0, insurance.Id);
            var updated = this.HelperComponent.GetInsurance(insurance.Id);

            Assert.AreEqual(insurance.Address.BoxNumber, updated.Address.BoxNumber);
            Assert.AreEqual(insurance.Address.City, updated.Address.City);
            Assert.AreEqual(insurance.Address.PostalCode, updated.Address.PostalCode);
            Assert.AreEqual(insurance.Address.Street, updated.Address.Street);
            Assert.AreEqual(insurance.Address.StreetNumber, updated.Address.StreetNumber);
            Assert.AreEqual(insurance.Name, updated.Name);
            Assert.AreEqual(insurance.Notes, updated.Notes);
            Assert.AreEqual(insurance.Phone, updated.Phone);

            patient = this.HelperComponent.GetAllPatients()[0];
            Assert.AreEqual(insurance.Id, patient.Insurance.Id);
            Assert.AreEqual(insurance.Name, patient.Insurance.Name);
        }

        [Test]
        public void UpdatePatient_WithNullAddress_PatientUpdated()
        {
            var patient = this.HelperComponent.GetAllPatients()[0];
            patient.Address = null;

            this.Session.Clear();
            this.ComponentUnderTest.Update(patient);
            patient = this.HelperComponent.GetAllPatients()[0];

            Assert.IsNotNull(patient.Address);
            Assert.IsTrue(string.IsNullOrEmpty(patient.Address.BoxNumber));
            Assert.IsTrue(string.IsNullOrEmpty(patient.Address.City));
            Assert.IsTrue(string.IsNullOrEmpty(patient.Address.PostalCode));
            Assert.IsTrue(string.IsNullOrEmpty(patient.Address.Street));
            Assert.IsTrue(string.IsNullOrEmpty(patient.Address.StreetNumber));
        }

        [Test]
        public void UpdatePatient_WithNullProperties_AllIsSaved()
        {
            var patient = this.HelperComponent.GetAllPatients()[0];
            patient.Profession = null;
            patient.Reputation = null;
            patient.Insurance = null;
            patient.Practice = null;

            this.ComponentUnderTest.Update(patient);
        }

        protected override void _Setup()
        {
            this.BuildComponent(session => new PatientDataComponent(session));
        }

        private InsuranceDto CreateInsurance()
        {
            var insurance = new InsuranceDto()
            {
                Name = GetRandom.String,
                Address = new AddressDto()
                {
                    BoxNumber = GetRandom.String,
                    City = GetRandom.String,
                    PostalCode = GetRandom.String,
                    Street = GetRandom.String,
                    StreetNumber = GetRandom.String,
                },
                Notes = GetRandom.String,
                Phone = GetRandom.String,
            };
            return insurance;
        }

        private PatientDto GetPatient(long id)
        {
            return (from p in this.HelperComponent.GetPatientsByName("*", SearchOn.FirstAndLastName)
                    where p.Id == id
                    select p).First();
        }

        #endregion Methods
    }
}