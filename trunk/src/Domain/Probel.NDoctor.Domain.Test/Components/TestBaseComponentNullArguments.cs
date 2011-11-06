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

    using MyAssertionExcepiton = Probel.Helpers.Assertion.AssertionException;

    public class TestBaseComponentNullArguments : TestBase<BaseComponent>
    {
        #region Methods

        [Test]
        [ExpectedException(typeof(MyAssertionExcepiton))]
        public void FailOnCreateLightDoctor()
        {
            this.Component.Create((LightDoctorDto)null);
        }

        [Test]
        [ExpectedException(typeof(MyAssertionExcepiton))]
        public void FailOnCreateLightPatient()
        {
            this.Component.Create((LightPatientDto)null);
        }

        [Test]
        [ExpectedException(typeof(MyAssertionExcepiton))]
        public void FailOnCreatePatient()
        {
            this.Component.Create((PatientDto)null);
        }

        [Test]
        [ExpectedException(typeof(MyAssertionExcepiton))]
        public void FailOnCreateTag()
        {
            this.Component.Create((TagDto)null);
        }

        [Test]
        [ExpectedException(typeof(MyAssertionExcepiton))]
        public void FailOnCreateUser()
        {
            this.Component.Create((UserDto)null);
        }

        /// <summary>
        /// Gets the component instance.
        /// </summary>
        /// <returns></returns>
        protected override BaseComponent GetComponentInstance()
        {
            return new MockBaseComponent(Database.Scope.OpenSession());
        }

        #endregion Methods
    }
}