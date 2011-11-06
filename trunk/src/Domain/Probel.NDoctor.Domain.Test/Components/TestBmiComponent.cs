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
    using System;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Cfg;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.Test.Helpers;

    public class TestBmiComponent : TestBase<BmiComponent>
    {
        #region Methods

        /// <summary>
        /// Determines whether this instance [can add bmi enty].
        /// </summary>
        [Test]
        public void CanAddBmiEntry()
        {
            var foundPatient = this.Component.GetPatientLightById(3);
            Assert.NotNull(foundPatient, "No patient were found");

            var patient = this.Component.GetPatientWithBmiHistory(foundPatient);
            var before = patient.BmiHistory.Count;

            this.Component.AddBmi(new BmiDto() { Date = DateTime.Now, Height = 180, Weight = 85 }, foundPatient);

            foundPatient = this.Component.GetPatientLightById(3);
            Assert.NotNull(foundPatient, "No patient were found after add");

            patient = this.Component.GetPatientWithBmiHistory(foundPatient);
            Assert.AreEqual(patient.BmiHistory.Count, before + 1);
        }

        [Test]
        public void CanDeleteBmyEntry()
        {
            var foundPatient = this.Component.GetPatientLightById(3);
            Assert.NotNull(foundPatient, "No patient were found");

            var patient = this.Component.GetPatientWithBmiHistory(foundPatient);
            Assert.GreaterOrEqual(patient.BmiHistory.Count, 10);

            var count = this.Component.GetPatientWithBmiHistory(patient).BmiHistory.Count;

            var dateTime = new DateTime(2000, 01, 01).Date;
            this.Component.AddBmi(new BmiDto() { Date = dateTime, Height = 10, Weight = 10 }, patient);
            this.Component.AddBmi(new BmiDto() { Date = dateTime, Height = 10, Weight = 10 }, patient);
            this.Component.AddBmi(new BmiDto() { Date = dateTime, Height = 10, Weight = 10 }, patient);
            this.Component.AddBmi(new BmiDto() { Date = dateTime, Height = 10, Weight = 10 }, patient);

            Assert.AreEqual(count + 4
                , this.Component.GetPatientWithBmiHistory(patient).BmiHistory.Count
                , "The count is not the expected one after insertion");

            this.Transaction(() => this.Component.DeleteBmiWithDate(patient, dateTime));

            Assert.AreEqual(count
                , this.Component.GetPatientWithBmiHistory(patient).BmiHistory.Count
                , "The count is not the expected one after delete");
        }

        [Test]
        public void CanGetBmiHistory()
        {
            var patients = this.Component.GetAllPatientsLight();
            Assert.Greater(patients.Count, 1, "No patient were found");

            var patient = this.Component.GetPatientWithBmiHistory(patients[0]);
            Assert.GreaterOrEqual(patient.BmiHistory.Count, 10);
        }

        /// <summary>
        /// Gets the component instance.
        /// </summary>
        /// <returns></returns>
        protected override BmiComponent GetComponentInstance()
        {
            return new BmiComponent(Database.Scope.OpenSession());
        }

        #endregion Methods
    }
}