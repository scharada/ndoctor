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

using NUnit.Framework;
using Probel.NDoctor.Domain.DAL.Components;
using Probel.NDoctor.Domain.DTO.Objects;

namespace Probel.NDoctor.Domain.Test.Components
{
    [TestFixture]
    public class AuthorisationComponentTest : BaseComponentTest<AuthorisationComponent>
    {
        protected override void _Setup()
        {
            this.BuildComponent(session => new AuthorisationComponent(session));
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
    }
}
