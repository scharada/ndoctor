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
    public class TestTimeSlotCollection
    {
        #region Methods

        [Test]
        public void CanCreateTimeSlotCollection()
        {
            var from = DateTime.Today.AddHours(12);
            var to = DateTime.Today.AddHours(16);
            var workday = new Workday("13:00", "16:00", SlotDuration.ThirtyMinutes);
            var collection = TimeSlotCollection.Create(from, to, workday);

            Assert.AreEqual(6, collection.Count);

            Assert.AreEqual(13, collection[0].StartTime.Hour, "Start time hour should be 16");
            Assert.AreEqual(0, collection[0].StartTime.Minute, "Start time minute should be 0");

            Assert.AreEqual(16, collection[collection.Count - 1].EndTime.Hour, "End time hour should be 16");
            Assert.AreEqual(0, collection[collection.Count - 1].EndTime.Minute, "End time minute should be 0");
        }

        #endregion Methods
    }
}