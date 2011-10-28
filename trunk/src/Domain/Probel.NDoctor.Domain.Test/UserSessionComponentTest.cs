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

namespace Probel.NDoctor.Domain.DAL.Test
{
    using System;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Cfg;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Dto;
    using Probel.NDoctor.Domain.DAL.Test.Helpers;

    [TestFixture]
    public class UserSessionComponentTest : IDisposable
    {
        #region Fields

        private UserSessionComponent component;
        private InMemoryDatabase db = new InMemoryDatabase();

        #endregion Fields

        #region Methods

        [Test]
        public void CanConnect()
        {
            var result = this.component.GetAllUsers();

            Assert.NotNull(result);
            Assert.Greater(result.Count, 0, "At least one user is expected");

            bool connected = this.component.CanConnect(new ConnectedUserDto() { Id = result[0].Id }, "aze");
            Assert.IsTrue(connected, "The user should be connected with the password 'aze'");
        }

        [Test]
        public void CanGetAllPractices()
        {
            var practices = this.component.GetAllPractices();
            Assert.AreEqual(3, practices.Count);

            Assert.NotNull(practices[0].Address, "The task should have a description");
        }

        [Test]
        public void CanGetAllRoles()
        {
            var roles = this.component.GetAllRoles();
            Assert.AreEqual(1, roles.Count);

            Assert.IsFalse(string.IsNullOrEmpty(roles[0].Description), "The task should have a description");
            Assert.IsFalse(string.IsNullOrEmpty(roles[0].Name), "The task should have a name");
        }

        [Test]
        public void CanGetAllUsers()
        {
            var result = this.component.GetAllUsers();
            Assert.AreEqual(3, result.Count, "3 users are expected");
        }

        [Test]
        public void CanGetFullUser()
        {
            var users = this.component.GetAllUsers();
            Assert.Greater(users.Count, 0, "At least one user should be in the database");

            var fullUser = this.component.GetDataOfUser(users[0]);
            Assert.NotNull(fullUser, "The selected user should be filled with all its data");
            Assert.NotNull(fullUser.AssignedRole, "The user should have an assigned role");
            Assert.NotNull(fullUser.Practice, "The user should have a practice");
            Assert.NotNull(fullUser.Practice.Address, "The practice of the user should have an address");
        }

        [Test]
        public void CanUpdateUser()
        {
            var users = this.component.GetAllUsers();
            Assert.Greater(users.Count, 0, "The db should have users in it.");
            var user = this.component.GetDataOfUser(users[0]);

            var firstName = user.FirstName;
            var lastName = user.LastName;
            var updatedValue = "New value" + Guid.NewGuid().ToString();

            user.FirstName
                = user.LastName
                = updatedValue;

            this.component.UpdateUser(user);

            var updated = this.component.GetDataOfUser(users[0]);
            Assert.AreEqual(updatedValue, updated.FirstName, "The first name should be updated");
            Assert.AreEqual(updatedValue, updated.LastName, "The last name should be updated");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            db.Dispose();
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            new DalConfiguration().Configure();
            this.component = new UserSessionComponent(this.db.Session);
            Build.Database(this.db.Session);
        }

        #endregion Methods
    }
}