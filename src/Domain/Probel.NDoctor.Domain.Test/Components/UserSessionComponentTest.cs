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
    using Probel.NDoctor.Domain.DTO.Objects;

    [TestFixture]
    public class UserSessionComponentTest : BaseComponentTest<UserSessionComponent>
    {
        #region Methods

        [Test]
        public void ConnectUser_ConnectAUserWithEmptyPasswordSendNull_UserCannotConnect()
        {
            /* The doctor No Pazwordz is, oh surpsise, a doctor with an empty password.
             * It is set in the 'InsertUsers.sql' script file
             */
            var users = this.HelperComponent.GetUserByLastName("Pazwordz");
            var canConnect = this.ComponentUnderTest.CanConnect(users[0], null);

            Assert.IsFalse(canConnect);
        }

        [Test]
        public void ConnectUser_ConnectAUserWithEmptyPasswordSendStringEmpty_UserConnects()
        {
            /* The doctor No Pazwordz is, oh surpsise, a doctor with an empty password.
             * It is set in the 'InsertUsers.sql' script file
             */
            var users = this.HelperComponent.GetUserByLastName("Pazwordz");
            var canConnect = this.ComponentUnderTest.CanConnect(users[0], string.Empty);

            Assert.IsTrue(canConnect);
        }

        /// <summary>
        /// Issue 90
        /// </summary>
        [Test]
        public void ConnectUser_ConnectAUserWithNullPassword_UserCanConnects()
        {
            /* The doctor No Pazwordz is, oh surpsise, a doctor with an empty password.
             * It is set in the 'InsertUsers.sql' script file
             */
            var users = this.HelperComponent.GetUserByLastName("NullPazwordz");
            var canConnect = this.ComponentUnderTest.CanConnect(users[0], string.Empty);

            Assert.IsTrue(canConnect);
        }

        /// <summary>
        /// Issue 135
        /// </summary>
        [Test]
        public void CreateUser_CreateUsersWithSameFirstAndLastName_ExceptionIsExpected()
        {
            var firstName = "Robert";
            var lastName = "Dupont";

            var user1 = new LightUserDto() { FirstName = firstName, LastName = lastName };
            var user2 = new LightUserDto() { FirstName = firstName, LastName = lastName };

            this.ComponentUnderTest.Create(user1, "a");
            Assert.Throws<ExistingItemException>(() => this.ComponentUnderTest.Create(user2, "az"));
        }

        /// <summary>
        /// issue 117
        /// </summary>
        [Test]
        public void UpdateUserData_UpdateDefaultUser_OnlyOneDefaultUserInDb()
        {
            var users = (from u in this.HelperComponent.GetAllUsers()
                         where !u.IsDefault
                         select u).ToList();

            Assert.Greater(users.Count, 0);

            users[0].IsDefault = true;
            this.WrapInTransaction(() => this.ComponentUnderTest.Update(users[0]));

            var defaultUsersCount = (from u in this.HelperComponent.GetAllUsers()
                                     where u.IsDefault
                                     select u).Count();

            Assert.AreEqual(1, defaultUsersCount);
        }

        /// <summary>
        /// Issue 90
        /// </summary>
        [Test]
        public void UpdateUserData_UpdateUserWithEmptyPassword_TheUserIsUpdated()
        {
            /* The doctor No Pazwordz is, oh surpsise, a doctor with an empty password.
             * It is set in the 'InsertUsers.sql' script file
             */
            var users = this.HelperComponent.GetUserByLastName("Pazwordz");

            var fullUser = this.ComponentUnderTest.LoadUser(users[0]);
            fullUser.FirstName = Guid.NewGuid().ToString();

            this.ComponentUnderTest.Update(fullUser);
        }

        /// <summary>
        /// Issue 135
        /// </summary>
        [Test]
        public void UpdateUserData_UpdateWithExistingFirstAndLastName_ExceptionIsExpected()
        {
            var users = this.HelperComponent.GetAllUsers();

            Assert.GreaterOrEqual(users.Count, 2);

            users[0].FirstName = users[1].FirstName;
            users[0].LastName = users[1].LastName;

            Assert.Throws<ExistingItemException>(() => this.ComponentUnderTest.Update(users[0]));
        }

        protected override void _Setup()
        {
            this.BuildComponent(session => new UserSessionComponent(session));
        }

        #endregion Methods
    }
}