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

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.Test.Helpers;

    [TestFixture]
    [Category(Categories.FunctionalTest)]
    public class AdministrationComponentTest : BaseComponentTest<AdministrationComponent>
    {
        #region Methods

        /// <summary>
        /// Issue 157
        /// </summary>
        [Test]
        public void RemoveInsurance_CheckWhetherCanRemoveNull_PartialEvaluationExceptionExpression()
        {
            Assert.Throws<NotSupportedException>(() => this.ComponentUnderTest.CanRemove((InsuranceDto)null));
        }

        /// <summary>
        /// Issue 157
        /// </summary>
        [Test]
        public void RemoveInsurance_CheckWhetherCanRemove_ReturnsTheExectedValue()
        {
            /* Look at the SQL script to understand that the pathology with ID 1 is already referenced in
             * the illness persion with id 1
             */
            var insurance = this.HelperComponent.GetInsurance(1);
            Assert.IsFalse(this.ComponentUnderTest.CanRemove(insurance));
        }

        [Test]
        public void RemoveInsurance_WhichIsReferenced_ClearExceptionThrown()
        {
            var users = this.HelperComponent.GetAllPatients();
            Assert.GreaterOrEqual(users.Count, 1);

            var insurance = new InsuranceDto() { Address = new AddressDto(), Name = string.Empty, Notes = string.Empty, Phone = string.Empty };
            long id = 0;

            this.WrapInTransaction(() =>
            {
                id = this.ComponentUnderTest.Create(insurance);
                users[0].Insurance = this.HelperComponent.GetInsurance(id);

                new PatientDataComponent(this.Session).Update(users[0]);
            });

            var insuranceToDelete = this.HelperComponent.GetInsurance(id);

            Assert.Throws<ReferencialIntegrityException>(() => this.ComponentUnderTest.Remove(insuranceToDelete));
        }

        /// <summary>
        /// issue 134
        /// </summary>
        [Test]
        public void RemovePathology_CheckWhetherCanRemove_ReturnsTheExectedValue()
        {
            /* Look at the SQL script to understand that the pathology with ID 1 is already referenced in
             * the illness persion with id 1
             */
            var pathology = this.HelperComponent.GetPathology(1);
            Assert.IsFalse(this.ComponentUnderTest.CanRemove(pathology));
        }

        [Test]
        public void RemovePathology_WhichIsReferenced_ClearExceptionThrown()
        {
            /* Look at the SQL script to understand that the pathology with ID 1 is already referenced in
             * the illness persion with id 1
             */
            var pathology = this.HelperComponent.GetPathology(1);
            Assert.Throws<ReferencialIntegrityException>(() => this.ComponentUnderTest.Remove(pathology));
        }

        [Test]
        public void UpdateInsurance_ModifyAddress_AddressChanged()
        {
            var insurance = this.ComponentUnderTest.GetAllInsurances()[0];

            var guid = Guid.NewGuid().ToString();
            insurance.Address.BoxNumber = guid;
            insurance.Address.City = guid;
            insurance.Address.PostalCode = guid;
            insurance.Address.Street = guid;
            insurance.Address.StreetNumber = guid;

            this.Session.Clear();
            this.ComponentUnderTest.Update(insurance);
            insurance = this.ComponentUnderTest.GetAllInsurances()[0];

            Assert.AreEqual(insurance.Address.BoxNumber, guid);
            Assert.AreEqual(insurance.Address.City, guid);
            Assert.AreEqual(insurance.Address.PostalCode, guid);
            Assert.AreEqual(insurance.Address.Street, guid);
            Assert.AreEqual(insurance.Address.StreetNumber, guid);

        }
        [Test]
        public void UpdateInsurance_ModifyAddressWhenNull_AddressChanged()
        {
            var insurance = this.ComponentUnderTest.GetAllInsurances()[0];
            insurance.Address = null;

            this.Session.Clear();
            this.ComponentUnderTest.Update(insurance);
            insurance = this.ComponentUnderTest.GetAllInsurances()[0];

            Assert.IsNotNull(insurance.Address, "Address shouldn't ne null");
            Assert.IsTrue(string.IsNullOrEmpty(insurance.Address.BoxNumber));
            Assert.IsTrue(string.IsNullOrEmpty(insurance.Address.City));
            Assert.IsTrue(string.IsNullOrEmpty(insurance.Address.PostalCode));
            Assert.IsTrue(string.IsNullOrEmpty(insurance.Address.Street));
            Assert.IsTrue(string.IsNullOrEmpty(insurance.Address.StreetNumber));
        }
        [Test]
        public void UpdatePractice_ModifyAddress_AddressChanged()
        {
            var practice = this.ComponentUnderTest.GetAllPractices()[0];

            var guid = Guid.NewGuid().ToString();
            practice.Address.BoxNumber = guid;
            practice.Address.City = guid;
            practice.Address.PostalCode = guid;
            practice.Address.Street = guid;
            practice.Address.StreetNumber = guid;

            this.Session.Clear();
            this.ComponentUnderTest.Update(practice);

            Assert.AreEqual(practice.Address.BoxNumber, guid);
            Assert.AreEqual(practice.Address.City, guid);
            Assert.AreEqual(practice.Address.PostalCode, guid);
            Assert.AreEqual(practice.Address.Street, guid);
            Assert.AreEqual(practice.Address.StreetNumber, guid);
        }
        [Test]
        public void UpdatePractice_ModifyAddressWhenNull_AddressChanged()
        {
            var practice = this.ComponentUnderTest.GetAllPractices()[0];
            practice.Address = null;

            this.Session.Clear();
            this.ComponentUnderTest.Update(practice);
            practice = this.ComponentUnderTest.GetAllPractices()[0];

            Assert.IsNotNull(practice.Address, "Address shouldn't ne null");
            Assert.IsTrue(string.IsNullOrEmpty(practice.Address.BoxNumber));
            Assert.IsTrue(string.IsNullOrEmpty(practice.Address.City));
            Assert.IsTrue(string.IsNullOrEmpty(practice.Address.PostalCode));
            Assert.IsTrue(string.IsNullOrEmpty(practice.Address.Street));
            Assert.IsTrue(string.IsNullOrEmpty(practice.Address.StreetNumber));
        }

        [Test]
        public void UpdateDoctor_ModifyAddress_AddressChanged()
        {
            var doctor = this.ComponentUnderTest.GetAllDoctors()[0];

            var guid = Guid.NewGuid().ToString();
            doctor.Address.BoxNumber = guid;
            doctor.Address.City = guid;
            doctor.Address.PostalCode = guid;
            doctor.Address.Street = guid;
            doctor.Address.StreetNumber = guid;

            this.Session.Clear();
            this.ComponentUnderTest.Update(doctor);

            Assert.AreEqual(doctor.Address.BoxNumber, guid);
            Assert.AreEqual(doctor.Address.City, guid);
            Assert.AreEqual(doctor.Address.PostalCode, guid);
            Assert.AreEqual(doctor.Address.Street, guid);
            Assert.AreEqual(doctor.Address.StreetNumber, guid);
        }
        [Test]
        public void UpdateDoctor_ModifyAddressWhenNull_AddressChanged()
        {
            var doctor = this.ComponentUnderTest.GetAllDoctors()[0];
            doctor.Address = null;

            this.Session.Clear();
            this.ComponentUnderTest.Update(doctor);
            doctor = this.ComponentUnderTest.GetAllDoctors()[0];

            Assert.IsNotNull(doctor.Address, "Address shouldn't ne null");
            Assert.IsTrue(string.IsNullOrEmpty(doctor.Address.BoxNumber));
            Assert.IsTrue(string.IsNullOrEmpty(doctor.Address.City));
            Assert.IsTrue(string.IsNullOrEmpty(doctor.Address.PostalCode));
            Assert.IsTrue(string.IsNullOrEmpty(doctor.Address.Street));
            Assert.IsTrue(string.IsNullOrEmpty(doctor.Address.StreetNumber));
        }

        protected override void _Setup()
        {
            this.BuildComponent(session => new AdministrationComponent(session));
        }

        #endregion Methods
    }
}