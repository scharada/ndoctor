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
namespace Probel.NDoctor.Domain.Test.Component
{
    using AutoMapper;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Exceptions;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.Test.Helpers;

    public class TestPatientSessionComponent : TestBase<PatientSessionComponent>
    {
        #region Methods

        [Test]
        public void CanGetTop2OfPatient()
        {
            var address = Create.Address();
            var patient1 = Create.Patient(address, Create.Tag(TagCategory.Patient), Create.Reputation(), Create.Insurance(), Create.Practice(address), Create.Profession());
            patient1.Counter = 150;

            var patient2 = Create.Patient(address, Create.Tag(TagCategory.Patient), Create.Reputation(), Create.Insurance(), Create.Practice(address), Create.Profession());
            patient2.Counter = 100;

            this.Transaction(() =>
            {
                this.Component.Create(Mapper.Map<Patient, PatientDto>(patient1));
                this.Component.Create(Mapper.Map<Patient, PatientDto>(patient2));
            });

            var result = this.Component.GetTopXPatient(2);

            Assert.AreEqual(2, result.Count, "Shoud have two elements in the array");

            Assert.AreEqual(patient1.FirstName, result[0].FirstName, "The first name of the user 1 is not the expected one");
            Assert.AreEqual(patient2.FirstName, result[1].FirstName, "The first name of the user 1 is not the expected one");
        }

        [Test]
        [ExpectedExceptionAttribute(typeof(ExistingItemException))]
        public void FailToCreatePatientWhenAlreadyExists()
        {
            var patients = this.Component.GetAllPatientsLight();
            Assert.Greater(patients.Count, 0, "At least one patient should be in the database");

            this.Component.Create(patients[0]);
        }

        /// <summary>
        /// Gets the component instance.
        /// </summary>
        /// <returns></returns>
        protected override PatientSessionComponent GetComponentInstance()
        {
            return new PatientSessionComponent(this.Database.Session);
        }

        #endregion Methods
    }
}