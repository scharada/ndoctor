﻿/*
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Probel.NDoctor.Domain.DAL.Components;
using Probel.NDoctor.Domain.DTO.Objects;
using AutoMapper;

namespace Probel.NDoctor.Domain.Test.Components
{
    [TestFixture]
    public class UserSessionComponentTest : BaseComponentTest<UserSessionComponent>
    {
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
        protected override void _Setup()
        {
            this.BuildComponent(session => new UserSessionComponent(session));
        }
    }
}