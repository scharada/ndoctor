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
    using NUnit.Framework;

    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.GoogleCalendar;
    using Probel.NDoctor.Domain.DTO.Objects;

    using StructureMap;

    [TestFixture]
    public class CalendarComponentTest : BaseComponentTest<CalendarComponent>
    {
        #region Fields

        private readonly ComponentFactory Factory = ComponentFactory.TestInstance();

        #endregion Fields

        #region Methods

        [Test]
        public void CheckAuthorisation_SecretaryAddsAppointments_SecretaryIsGrantedTo()
        {
            ObjectFactory.Configure(c => c.For<ICalendarComponent>().Use(this.ComponentUnderTest));

            var task = new TaskDto(To.EditCalendar);
            var user = this.HelperComponent.GetLightUserById(1);
            user.AssignedRole.Tasks.Add(new TaskDto(To.EditCalendar));

            this.Factory.ConnectUser(user);
            var component = this.Factory.GetInstance<ICalendarComponent>();

            // This code should pass
            component.Create(new AppointmentDto() { User = user }, this.HelperComponent.GetAllPatientsLight()[0], GoogleConfiguration.NotBinded);
        }

        [Test]
        public void CheckAuthorisation_SecretaryRemovesAppointments_SecretaryIsGrantedTo()
        {
            ObjectFactory.Configure(c => c.For<ICalendarComponent>().Use(this.ComponentUnderTest));

            var task = new TaskDto(To.EditCalendar);
            var user = this.HelperComponent.GetLightUserById(1);
            user.AssignedRole.Tasks.Add(new TaskDto(To.EditCalendar));

            this.Factory.ConnectUser(user);
            var component = this.Factory.GetInstance<ICalendarComponent>();

            // This code should pass
            var patient = this.HelperComponent.GetAllPatientsLight()[0];
            var meeting = new AppointmentDto() { User = user };

            component.Create(meeting, patient, GoogleConfiguration.NotBinded);

            component.Remove(meeting, patient, GoogleConfiguration.NotBinded);
        }

        /// <summary>
        /// Issue 159
        /// </summary>
        [Test]
        public void ManageAppointments_AddNewAppointment_PasswordOfUserNotReset()
        {
            var user = this.HelperComponent.GetLightUserById(1);

            // This code should pass
            var patient = this.HelperComponent.GetAllPatientsLight()[0];
            var meeting = new AppointmentDto() { User = user };

            this.WrapInTransaction(() => this.ComponentUnderTest.Create(meeting, patient, GoogleConfiguration.NotBinded));

            Assert.IsTrue(new UserSessionComponent(this.Session).CanConnect(user, "aze"));
        }

        protected override void _Setup()
        {
            this.BuildComponent(s => new CalendarComponent(s));
        }

        #endregion Methods
    }
}