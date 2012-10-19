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

    using AutoMapper;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.Test.Helpers;

    public class TestUserSessionComponent : TestBase<UserSessionComponent>
    {
        #region Methods

        [Test]
        public void CanAddTwiceDefaultUser()
        {
            var user = Create.AUser(Create.Address(), Create.ARole(), Create.Practice(Create.Address()), true);
            var dto = Mapper.Map<User, UserDto>(user);

            this.Component.Create(dto);

            user = Create.AUser(Create.Address(), Create.ARole(), Create.Practice(Create.Address()), true);
            dto = Mapper.Map<User, UserDto>(user);

            this.Component.Create(dto);

            var defaultUser = this.Component.GetDefaultUser();
            Assert.NotNull(defaultUser);
            Assert.AreEqual(user.FirstName, defaultUser.FirstName);
        }

        [Test]
        public void CanConnect()
        {
            var result = this.Component.GetAllUsers();

            Assert.NotNull(result);
            Assert.Greater(result.Count, 0, "At least one user is expected");

            bool connected = this.Component.CanConnect(new LightUserDto() { Id = result[0].Id }, "aze");
            Assert.IsTrue(connected, "The user should be connected with the password 'aze'");
        }

        [Test]
        public void CanFindDefaultUser()
        {
            var result = this.Component.GetDefaultUser();
            Assert.NotNull(result, "Should be one default user.");
        }

        [Test]
        public void CanGetFullUser()
        {
            var users = this.Component.GetAllUsers();
            Assert.Greater(users.Count, 0, "At least one user should be in the database");

            var fullUser = this.Component.LoadUser(users[0]);
            Assert.NotNull(fullUser, "The selected user should be filled with all its data");
            Assert.NotNull(fullUser.AssignedRole, "The user should have an assigned role");
            Assert.NotNull(fullUser.Practice, "The user should have a practice");
        }

        [Test]
        public void CanUpdateUser()
        {
            var users = this.Component.GetAllUsers();
            Assert.Greater(users.Count, 0, "The db should have users in it.");
            var user = this.Component.LoadUser(users[0]);

            var firstName = user.FirstName;
            var lastName = user.LastName;
            var updatedValue = "New value" + Guid.NewGuid().ToString();

            user.FirstName
                = user.LastName
                = updatedValue;

            this.Component.Update(user);

            var updated = this.Component.LoadUser(users[0]);
            Assert.AreEqual(updatedValue, updated.FirstName, "The first name should be updated");
            Assert.AreEqual(updatedValue, updated.LastName, "The last name should be updated");
        }

        protected override UserSessionComponent GetComponentInstance()
        {
            return new UserSessionComponent(this.Database.Session);
        }

        #endregion Methods
    }
}