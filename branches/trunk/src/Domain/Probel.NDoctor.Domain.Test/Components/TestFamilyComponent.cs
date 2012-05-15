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
    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Cfg;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.Test.Helpers;

    public class TestFamilyComponent : TestBase<FamilyComponent>
    {
        #region Methods

        [Test]
        public void CanGetFamily()
        {
            // The ID of the patient is 7 for the test. And god said, for this time
            // I'll allow a magic number. But never do it twice ;)
            var family = this.Component.FindFamily(new LightPatientDto() { Id = 7 });

            Assert.NotNull(family.Current, "No current patient found");
            Assert.NotNull(family.Father, "No father found");
            Assert.NotNull(family.Mother, "No mother found");
            Assert.AreEqual(5, family.Children.Count, "It is not the number of children expected");
        }

        /// <summary>
        /// Gets the component instance.
        /// </summary>
        /// <returns></returns>
        protected override FamilyComponent GetComponentInstance()
        {
            return new FamilyComponent(SQLiteDatabase.Scope.OpenSession());
        }

        #endregion Methods
    }
}