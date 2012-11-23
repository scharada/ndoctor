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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;

    public class PatientSessionComponentTest : BaseComponentTest<PatientSessionComponent>
    {
        #region Methods

        [Test]
        public void LoadPatient_CheckHeightOfPatientWithJokerSearchOnFirstLastName_HeightIsSet()
        {
            var patients = this.ComponentUnderTest.GetPatientsByNameLight("*", SearchOn.FirstAndLastName);

            Assert.AreEqual(180, patients[0].Height);
        }

        [Test]
        public void LoadPatient_CheckHeightOfPatientWithJokerSearchOnFirstName_HeightIsSet()
        {
            var patients = this.ComponentUnderTest.GetPatientsByNameLight("*", SearchOn.FirstName);

            Assert.AreEqual(180, patients[0].Height);
        }

        [Test]
        public void LoadPatient_CheckHeightOfPatientWithJokerSearchOnLastName_HeightIsSet()
        {
            var patients = this.ComponentUnderTest.GetPatientsByNameLight("*", SearchOn.LastName);

            Assert.AreEqual(180, patients[0].Height);
        }

        [Test]
        public void LoadPatient_CheckHeightOfPatientWithoutJokerSearchOnFirstLastName_HeightIsSet()
        {
            var patients = this.ComponentUnderTest.GetPatientsByNameLight("Vroumiz", SearchOn.FirstAndLastName);

            Assert.AreEqual(180, patients[0].Height);
        }

        [Test]
        public void LoadPatient_CheckHeightOfPatientWithoutJokerSearchOnFirstName_HeightIsSet()
        {
            var patients = this.ComponentUnderTest.GetPatientsByNameLight("Caroline", SearchOn.FirstName);

            Assert.AreEqual(180, patients[0].Height);
        }

        [Test]
        public void LoadPatient_CheckHeightOfPatientWithoutJokerSearchOnLastName_HeightIsSet()
        {
            var patients = this.ComponentUnderTest.GetPatientsByNameLight("Vroumiz", SearchOn.LastName);

            Assert.AreEqual(180, patients[0].Height);
        }

        protected override void _Setup()
        {
            this.BuildComponent(s => new PatientSessionComponent(s));
        }

        #endregion Methods
    }
}