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

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
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
            var patients = this.Component.FindPatientsByNameLight("Patient", SearchOn.FirstAndLastName);
            Assert.Greater(patients.Count, 1, "No patient were found");

            var patient = this.Component.GetPatientWithBmiHistory(patients[0]);
            var before = patient.BmiHistory.Length;

            this.Component.CreateBmi(new BmiDto() { Date = DateTime.Now, Height = 180, Weight = 85 }, patients[0]);

            patients = this.Component.FindPatientsByNameLight("Patient", SearchOn.FirstAndLastName);
            Assert.Greater(patients.Count, 1, "No patient were found after add");

            patient = this.Component.GetPatientWithBmiHistory(patients[0]);
            Assert.AreEqual(patient.BmiHistory.Length, before + 1);
        }

        [Test]
        public void CanDeleteBmyEntry()
        {
            var patients = this.Component.FindPatientsByNameLight("patient", SearchOn.FirstAndLastName);
            Assert.Greater(patients.Count, 1, "No patient were found");

            var patient = this.Component.GetPatientWithBmiHistory(patients[0]);
            Assert.GreaterOrEqual(patient.BmiHistory.Length, 10);

            var count = this.Component.GetPatientWithBmiHistory(patient).BmiHistory.Length;

            var dateTime = DateTime.Today.AddDays(-4);
            this.Component.CreateBmi(new BmiDto() { Date = dateTime, Height = 10, Weight = 10 }, patient);
            this.Component.CreateBmi(new BmiDto() { Date = dateTime, Height = 10, Weight = 10 }, patient);
            this.Component.CreateBmi(new BmiDto() { Date = dateTime, Height = 10, Weight = 10 }, patient);
            this.Component.CreateBmi(new BmiDto() { Date = dateTime, Height = 10, Weight = 10 }, patient);

            Assert.AreEqual(count + 4
                , this.Component.GetPatientWithBmiHistory(patient).BmiHistory.Length
                , "The count is not the expected one after insertion");

            this.Component.RemoveBmiWithDate(patient, dateTime);

            Assert.AreEqual(count
                , this.Component.GetPatientWithBmiHistory(patient).BmiHistory.Length
                , "The count is not the expected one after deletion");
        }

        [Test]
        public void CanGetBmiHistory()
        {
            var patients = this.Component.FindPatientsByNameLight("patient", SearchOn.FirstAndLastName);
            Assert.Greater(patients.Count, 1, "No patient were found");

            var patient = this.Component.GetPatientWithBmiHistory(patients[0]);
            Assert.GreaterOrEqual(patient.BmiHistory.Length, 10);
        }

        /// <summary>
        /// Gets the component instance.
        /// </summary>
        /// <returns></returns>
        protected override BmiComponent GetComponentInstance()
        {
            return new BmiComponent(this.Database.Session);
        }

        #endregion Methods
    }
}