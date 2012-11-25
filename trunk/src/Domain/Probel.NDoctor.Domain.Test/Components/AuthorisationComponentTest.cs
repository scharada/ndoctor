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
    public class AuthorisationComponentTest : BaseComponentTest<AuthorisationComponent>
    {
        #region Methods

        [Test]
        public void CheckDefaultAdAuthorisationData_GetRoleCount_MoreThanARoleInDB()
        {
            var result = this.ComponentUnderTest.GetAllRoles();
            Assert.Greater(result.Length, 1);
        }

        [Test]
        public void CreateNewRole_CreateWithExistingTasks_RoleIsCreated()
        {
            var name = Guid.NewGuid().ToString();

            /* First you should create the tasks in the database before using it.
             * Cascade insertion will be implemented when it'll be needed
             */
            var a = new TaskDto("a");
            var b = new TaskDto("b");
            var c = new TaskDto("c");
            this.ComponentUnderTest.Create(a);
            this.ComponentUnderTest.Create(b);
            this.ComponentUnderTest.Create(c);

            var role = new RoleDto() { Name = name };
            role.Tasks.Add(a);
            role.Tasks.Add(b);
            role.Tasks.Add(c);

            this.ComponentUnderTest.Create(role);

            Assert.AreEqual(3, this.ComponentUnderTest.GetAllRoles().Where(e => e.Name == name).First().Tasks.Count);
        }

        [Test]
        public void CreateNewRole_CreateWithNONExistingTasks_CreatesTheNewTasks()
        {
            var name = Guid.NewGuid().ToString();

            var a = new TaskDto("a");
            var b = new TaskDto("b");
            var c = new TaskDto("c");

            var role = new RoleDto() { Name = name };
            role.Tasks.Add(a);
            role.Tasks.Add(b);
            role.Tasks.Add(c);

            this.ComponentUnderTest.Create(role);

            Assert.AreEqual(3, this.ComponentUnderTest.GetAllRoles().Where(e => e.Name == name).First().Tasks.Count);
        }

        /// <summary>
        /// Issue 93
        /// </summary>
        [Test]
        public void ManageAuthorisation_CreateNewRole_RoleCreated()
        {
            var name = this.RandomString;
            this.WrapInTransaction(() =>
            {
                var role = new RoleDto()
                {
                    Description = this.RandomString,
                    Name = name,
                };
                this.ComponentUnderTest.Create(role);

                var found = this.HelperComponent.GetRoleByName(name);

                Assert.NotNull(found);
                Assert.AreEqual(1, found.Count);
                Assert.AreEqual(name, found[0].Name);
            });
        }

        /// <summary>
        /// Issue 94
        /// </summary>
        [Test]
        [ExpectedException(typeof(NullItemInListException))]
        public void ManageAuthorisation_UpdateARoleWithAnEmptyTask_NullItemInListExceptionIsThrown()
        {
            var role = new RoleDto()
            {
                Description = this.RandomString,
                Name = this.RandomString,
            };
            this.ComponentUnderTest.Create(role);
            this.Session.Flush();

            role.Tasks.Add(new TaskDto(this.RandomString));
            role.Tasks.Add(new TaskDto(this.RandomString));
            role.Tasks.Add(null);

            this.ComponentUnderTest.Update(role);
        }

        [Test]
        public void RemoveRole_RemoveRole_RoleRemovedAndNoTasksRemovedFromDb()
        {
            int count = 0;
            var name = Guid.NewGuid().ToString();
            var tasks = this.ComponentUnderTest.GetAllTasks();

            RoleDto role = new RoleDto() { Name = name };
            role.Tasks.Add(tasks[1]);
            role.Tasks.Add(tasks[2]);
            role.Tasks.Add(tasks[3]);

            RoleDto role2 = new RoleDto() { Name = "azer" };
            role.Tasks.Add(tasks[1]);
            role.Tasks.Add(tasks[2]);
            role.Tasks.Add(tasks[3]);

            this.ComponentUnderTest.Create(role);
            this.ComponentUnderTest.Create(role2);
            this.Session.Flush();

            count = this.ComponentUnderTest.GetAllTasks().Length;

            this.ComponentUnderTest.Remove(role);
            this.Session.Flush();
            Assert.AreEqual(count, this.ComponentUnderTest.GetAllTasks().Length);
        }

        [Test]
        public void UpdateRole_RemoveTasksFromRole_TheTaskIsUnbindedAndStillInDb()
        {
            int count = 0;
            var name = Guid.NewGuid().ToString();

            RoleDto role = new RoleDto() { Name = name };
            role.Tasks.Add(new TaskDto("a"));
            role.Tasks.Add(new TaskDto("b"));
            role.Tasks.Add(new TaskDto("c"));

            this.ComponentUnderTest.Create(role);
            this.Session.Flush();

            count = this.ComponentUnderTest.GetAllTasks().Length;

            role.Tasks.Clear();
            this.ComponentUnderTest.Update(role);
            this.Session.Flush();
            Assert.AreEqual(count, this.ComponentUnderTest.GetAllTasks().Length);
        }

        protected override void _Setup()
        {
            this.BuildComponent(session => new AuthorisationComponent(session));
        }

        #endregion Methods
    }
}