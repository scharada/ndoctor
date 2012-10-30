using NUnit.Framework;
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
using Probel.NDoctor.Domain.DAL.Components;
using Probel.NDoctor.Domain.DTO.Objects;
using Probel.NDoctor.Domain.DTO.Components;

namespace Probel.NDoctor.Domain.Test.Components
{
    [TestFixture]
    public class BmiComponentTest : BaseComponentTest<BmiComponent>
    {
        protected override void _Setup()
        {
            this.BuildComponent(session => new BmiComponent(session));
        }

        [Test]
        public void CreateBmiEntry_CreateBmiWithNewHeight_PatientHasNewHeightInDatabase()
        {
            var patients = this.HelperComponent.GetAllPatientsLight();

            this.ComponentUnderTest.CreateBmi(new BmiDto() { Height = 199 }, patients[0]);

            var updated = this.HelperComponent.GetPatientsByName(patients[0].LastName, SearchOn.LastName);

            Assert.AreEqual(199, updated[0].Height);
        }

        [Test]
        public void CreateBmiEntry_CreateBmiWithNewHeight_DtoIsRefreshedWithNewHeight()
        {
            var patients = this.HelperComponent.GetAllPatientsLight();

            this.ComponentUnderTest.CreateBmi(new BmiDto() { Height = 199 }, patients[0]);

            var updated = this.HelperComponent.GetPatientsByName(patients[0].LastName, SearchOn.LastName);

            Assert.AreEqual(patients[0].Height, updated[0].Height);
        }
    }
}
