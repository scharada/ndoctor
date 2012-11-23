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
namespace Probel.NDoctor.Domain.Test.Validations
{
    using System;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DTO.Objects;

    [TestFixture]
    public class DrugValidation
    {
        #region Methods

        [Test]
        public void IsInvalid_Drug()
        {
            var item = new DrugDto()
            {
                Name = string.Empty,
            };
            Assert.IsFalse(item.IsValid());
        }

        [Test]
        public void IsValid_Drug()
        {
            var item = new DrugDto()
            {
                Name = Guid.NewGuid().ToString(),
            };
            Assert.IsTrue(item.IsValid());
        }

        #endregion Methods
    }
}