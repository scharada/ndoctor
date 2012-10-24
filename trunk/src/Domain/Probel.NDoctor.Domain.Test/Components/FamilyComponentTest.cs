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
    using System.Collections.Generic;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class FamilyComponentTest : BaseComponentTest<FamilyComponent>
    {
        #region Methods


        /// <summary>
        /// Issue 87
        /// </summary>
        [Test]
        public void GetValueFromFamily_ExecutionTimeShouldBeLow_ExecutionTimeIsLow()
        {
            var cmp = new UnitTestComponent(this.Session);
            var users = cmp.GetPatientsByNameLight("*", SearchOn.FirstAndLastName);

            Assert.Greater(users.Count, 4, "Not enought data to execute the test");

            var family = new FamilyDto() { Current = users[0], };
            family.Fathers.Add(users[1]);

            this.ComponentUnderTest.Update(family);

            var persistedFamily = this.ComponentUnderTest.GetFamily(users[0]);
            Assert.AreEqual(users[1].Id, family.Fathers[0].Id, "The father has an unexpected ID");


            var family2 = new FamilyDto() { Current = users[0], };
            family.Fathers.Add(users[2]);

            this.ComponentUnderTest.Update(family);

            var persistedFamily2 = this.ComponentUnderTest.GetFamily(users[0]);
            Assert.AreEqual(users[2].Id, family.Fathers[0].Id, "The father has an unexpected ID");
        }

        public override void _Setup()
        {
            this.BuildComponent(session => new FamilyComponent(session));
        }

        #endregion Methods
    }
}