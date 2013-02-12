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
    using System.Linq;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;

    [TestFixture]
    public class DbSettingsComponentTest : BaseComponentTest<DbSettingsComponent>
    {
        #region Methods

        [Test]
        public void CheckKey_KeyDoesntExists_ReturnsFalse()
        {
            var key = this.RandomString;

            bool exist = this.ComponentUnderTest.Exists(key);
            Assert.IsFalse(exist);
        }

        [Test]
        public void CheckKey_KeyExists_ReturnsTrue()
        {
            var key = this.RandomString;
            var value = this.RandomString;

            using (var tx = this.Session.BeginTransaction())
            {
                this.ComponentUnderTest[key] = value;
                tx.Commit();
            }

            bool exist = this.ComponentUnderTest.Exists(key);
            Assert.IsTrue(exist);
        }

        [Test]
        public void Insert_NewKey_ValueInserted()
        {
            var key = this.RandomString;
            var value = this.RandomString;

            using (var tx = this.Session.BeginTransaction())
            {
                this.ComponentUnderTest[key] = value;
                tx.Commit();
            }

            var rValue = this.ComponentUnderTest[key];
            Assert.AreEqual(value, rValue);
        }

        [Test]
        public void Insert_SameKeyTwice_ValueInserted()
        {
            var key = this.RandomString;
            var value = this.RandomString;

            using (var tx = this.Session.BeginTransaction())
            {
                this.ComponentUnderTest[key] = Guid.NewGuid().ToString();
                this.ComponentUnderTest[key] = value;
                tx.Commit();
            }

            var rValue = this.ComponentUnderTest[key];

            Assert.AreEqual(value, rValue);
        }

        [Test]
        public void Retrieve_AllSettings_AllAreReturned()
        {
            var settings = this.ComponentUnderTest.Settings;
            Assert.AreEqual(5, settings.Count());
        }

        [Test]
        public void Retrieve_ExistingValueWithDifferentCase_ValueReturned()
        {
            var value = this.ComponentUnderTest["ISDEBUG"];

            Assert.IsFalse(bool.Parse(value));
        }

        [Test]
        public void Retrieve_ExistingValueWithSameCase_ValueReturned()
        {
            var value = this.ComponentUnderTest["IsDebug"];

            Assert.IsFalse(bool.Parse(value));
        }

        [Test]
        public void Retrieve_NotExistingValue_ExceptionThrown()
        {
            Assert.Throws<EntityNotFoundException>(() =>
            {
                var value = this.ComponentUnderTest[this.RandomString];
            });
        }

        protected override void _Setup()
        {
            this.BuildComponent(session => new DbSettingsComponent(session));
        }

        #endregion Methods
    }
}