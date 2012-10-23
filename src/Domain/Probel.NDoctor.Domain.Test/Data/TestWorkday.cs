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
namespace Probel.NDoctor.Domain.Test.Data
{
    using System;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.Test.Helpers;

    [TestFixture]
    [Category(Categories.UnitTest)]
    public class TestWorkday
    {
        #region Methods

        [Test]
        public void CanInsanciate()
        {
            var from = "08:03";
            var to = "17:03";

            var workDay = new Workday(from, to, SlotDuration.ThirtyMinutes);

            Assert.AreEqual(8, workDay.From.Hour, "From hour is wrong");
            Assert.AreEqual(3, workDay.From.Minute, "From minute is wrong");

            Assert.AreEqual(17, workDay.To.Hour, "To hour is wrong");
            Assert.AreEqual(3, workDay.To.Minute, "To minute is wrong");
        }

        [Test]
        public void CanTestInRange()
        {
            var dateTime = DateTime.Today.AddHours(16).AddMinutes(30);
            var workday = new Workday("08:00", "16:00", SlotDuration.ThirtyMinutes);

            Assert.IsFalse(workday.IsInRange(dateTime));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void FailInstanciate_BeginsAfterEnds()
        {
            var from = "15:00";
            var to = "8:00";

            var workDay = new Workday(from, to, SlotDuration.ThirtyMinutes);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void FailInstanciate_NotNumber()
        {
            var from = "wrong";
            var to = "15:00";

            var workDay = new Workday(from, to, SlotDuration.ThirtyMinutes);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void FailInstanciate_NotTime()
        {
            var from = "15:99";
            var to = "15:00";

            var workDay = new Workday(from, to, SlotDuration.ThirtyMinutes);
        }

        #endregion Methods
    }
}