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

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.Test.Helpers;

    [TestFixture]
    [Category(Categories.FunctionalTest)]
    public class AdministrationComponentTest : BaseComponentTest<AdministrationComponent>
    {
        #region Methods
        
        [Test]
        public void RemoveInsurance_WhichIsReferenced_ClearExceptionThrown()
        {
            var users = this.HelperComponent.GetAllPatients();
            Assert.GreaterOrEqual(users.Count, 1);

            var insurance = new InsuranceDto() { Address = new AddressDto(), Name = string.Empty, Notes = string.Empty, Phone = string.Empty };
            long id = 0;

            this.WrapInTransaction(() =>
            {
                id = this.ComponentUnderTest.Create(insurance);
                users[0].Insurance = this.HelperComponent.GetInsurance(id);

                new PatientDataComponent(this.Session).Update(users[0]);
            });

            var insuranceToDelete = this.HelperComponent.GetInsurance(id);

            Assert.Throws<ReferencialIntegrityException>(() => this.ComponentUnderTest.Remove(insuranceToDelete));
        }

        [Test]
        public void RemovePathology_WhichIsReferenced_ClearExceptionThrown()
        {
            /* Look at the SQL script to understand that the pathology with ID 1 is already referenced in
             * the illness persion with id 1
             */
            var pathology = this.HelperComponent.GetPathology(1);
            Assert.Throws<ReferencialIntegrityException>(() => this.ComponentUnderTest.Remove(pathology));
        }

        /// <summary>
        /// issue 134
        /// </summary>
        [Test]
        public void RemovePathology_CheckWhetherCanRemove_ReturnsTheExectedValue()
        {
            /* Look at the SQL script to understand that the pathology with ID 1 is already referenced in
             * the illness persion with id 1
             */
            var pathology = this.HelperComponent.GetPathology(1);
            Assert.IsFalse(this.ComponentUnderTest.CanRemove(pathology));
        }

        protected override void _Setup()
        {
            this.BuildComponent(session => new AdministrationComponent(session));
        }

        #endregion Methods
    }
}