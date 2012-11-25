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
    using System.Linq;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

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

        [Test]
        public void UpdatePatient_WithNewInsurance_NoInsuranceDataLoss()
        {
            var name = this.RandomString;

            var insurance = this.CreateInsurance();

            this.WrapInTransaction(() => this.ComponentUnderTest.Create(insurance));

            var patient = this.HelperComponent.GetAllPatients()[0];
            patient.Insurance.Name = insurance.Name;

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
        }

        protected override void _Setup()
        {
            this.BuildComponent(session => new PatientDataComponent(session));
        }

        private InsuranceDto CreateInsurance()
        {
            var insurance = new InsuranceDto()
            {
                Name = this.RandomString,
                Address = new AddressDto()
                {
                    BoxNumber = this.RandomString,
                    City = this.RandomString,
                    PostalCode = this.RandomString,
                    Street = this.RandomString,
                    StreetNumber = this.RandomString,
                },
                Notes = this.RandomString,
                Phone = this.RandomString,
            };
            return insurance;
        }

        #endregion Methods
    }
}