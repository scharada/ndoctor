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

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.Test.Helpers;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    [Category(Categories.FunctionalTest)]
    public class FamilyComponentTest : BaseComponentTest<FamilyComponent>
    {
        #region Methods

        /// <summary>
        /// Issue 87
        /// </summary>
        [Test]
        public void UpdateFamily_AddFatherAndAChild_ThePatientHasAFatherAndAChild()
        {
            var cmp = new UnitTestComponent(this.Session);
            var users = cmp.GetPatientsByNameLight("*", SearchOn.FirstAndLastName);

            Assert.Greater(users.Count, 4, "Not enought data to execute the test");

            var family = new FamilyDto() { Current = users[0], };
            family.Fathers.Add(users[1]);

            this.ComponentUnderTest.AddNewChild(users[0], users[1]);

            var persistedFamily = this.ComponentUnderTest.GetFamily(users[0]);
            Assert.AreEqual(users[1].Id, family.Fathers[0].Id, "The father has an unexpected ID");

            var family2 = new FamilyDto() { Current = users[0], };
            family.Children.Add(users[2]);

            this.ComponentUnderTest.AddNewChild(users[0], users[2]);

            var persistedFamily2 = this.ComponentUnderTest.GetFamily(users[0]);
            Assert.AreEqual(users[1].Id, family.Fathers[0].Id, "The father has an unexpected ID");
            Assert.AreEqual(users[2].Id, family.Children[0].Id, "The child has an unexpected ID");
        }

        /// <summary>
        /// Issue 87
        /// </summary>
        [Test]
        public void UpdateFamily_AddFatherAndAMother_ThePatientHasAFatherAndAMother()
        {
            var cmp = new UnitTestComponent(this.Session);
            var users = cmp.GetPatientsByNameLight("*", SearchOn.FirstAndLastName);

            Assert.Greater(users.Count, 4, "Not enought data to execute the test");

            var family = new FamilyDto() { Current = users[0], };
            family.Fathers.Add(users[1]);

            this.ComponentUnderTest.AddNewParent(users[1], users[2]);

            var persistedFamily = this.ComponentUnderTest.GetFamily(users[0]);
            Assert.AreEqual(users[1].Id, family.Fathers[0].Id, "The father has an unexpected ID");

            var family2 = new FamilyDto() { Current = users[0], };
            family.Mothers.Add(users[2]);

            this.ComponentUnderTest.AddNewParent(users[1], users[3]);

            var persistedFamily2 = this.ComponentUnderTest.GetFamily(users[0]);
            Assert.AreEqual(users[1].Id, family.Fathers[0].Id, "The father has an unexpected ID");
            Assert.AreEqual(users[2].Id, family.Mothers[0].Id, "The child has an unexpected ID");
        }

        /// <summary>
        /// Issue 87
        /// </summary>
        [Test]
        public void UpdateFamily_AddFatherAndChangeIt_ThePatientHasANewFather()
        {
            var cmp = new UnitTestComponent(this.Session);
            var users = cmp.GetPatientsByNameLight("*", SearchOn.FirstAndLastName);

            var father1 = this.CreateNewPatient(Guid.NewGuid().ToString(), Gender.Male);
            var father2 = this.CreateNewPatient(Guid.NewGuid().ToString(), Gender.Male);
            cmp.Create(father1);
            cmp.Create(father2);

            this.ComponentUnderTest.AddNewParent(users[0], father1);

            var persistedFamily = this.ComponentUnderTest.GetFamily(users[0]);
            Assert.AreEqual(father1.Id, persistedFamily.Fathers[0].Id, "The father has an unexpected ID");

            this.ComponentUnderTest.AddNewParent(users[0], father2);

            var persistedFamily2 = this.ComponentUnderTest.GetFamily(users[0]);
            Assert.AreEqual(father2.Id, persistedFamily2.Fathers[0].Id, "The father has an unexpected ID");
        }

        /// <summary>
        /// Issue 87
        /// </summary>
        [Test]
        public void UpdateFamily_AddMotherAndChangeIt_ThePatientHasANewMother()
        {
            var cmp = new UnitTestComponent(this.Session);
            var users = cmp.GetPatientsByNameLight("*", SearchOn.FirstAndLastName);

            var mother1 = this.CreateNewPatient(Guid.NewGuid().ToString(), Gender.Female);
            var mother2 = this.CreateNewPatient(Guid.NewGuid().ToString(), Gender.Female);
            cmp.Create(mother1);
            cmp.Create(mother2);

            this.ComponentUnderTest.AddNewParent(users[0], mother1);

            var persistedFamily = this.ComponentUnderTest.GetFamily(users[0]);
            Assert.AreEqual(mother1.Id, persistedFamily.Mothers[0].Id, "The father has an unexpected ID");

            this.ComponentUnderTest.AddNewParent(users[0], mother2);

            var persistedFamily2 = this.ComponentUnderTest.GetFamily(users[0]);
            Assert.AreEqual(mother2.Id, persistedFamily2.Mothers[0].Id, "The father has an unexpected ID");
        }

        public override void _Setup()
        {
            this.BuildComponent(session => new FamilyComponent(session));
        }

        private LightPatientDto CreateNewPatient(string lastName, Gender gender)
        {
            return new LightPatientDto()
            {
                LastName = lastName,
                Gender = gender,
                Birthdate = DateTime.Now,
                Height = 180,
                Profession = new ProfessionDto(),
                FirstName = "FirstName"
            };
        }

        #endregion Methods
    }
}