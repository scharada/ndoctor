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
    using Probel.NDoctor.Domain.DTO.Objects;

    [TestFixture]
    public class RescueToolsComponentTest : BaseComponentTest<RescueToolsComponent>
    {
        #region Methods

        [Test]
        public void FindDoubloons_OfDoctor_OneDoubloonFound()
        {
            var doubloons = this.ComponentUnderTest.GetDoctorDoubloons();
            Assert.AreEqual(1, doubloons.Count());
        }

        [Test]
        public void FindDoubloon_OfSpecifiedDoctor_OneDoubloonFound()
        {
            var doctor = this.HelperComponent.GetDoctorById(18);

            var doubloons = this.ComponentUnderTest.GetDoubloonsOf(doctor);

            Assert.AreEqual(1, doubloons.Count());
        }

        [Test]
        public void ReplaceDoubloons_OfDoctor_NewDoctorReplaced()
        {
        }

        protected override void _Setup()
        {
            this.BuildComponent(session => new RescueToolsComponent(session));
        }

        #endregion Methods
    }
}