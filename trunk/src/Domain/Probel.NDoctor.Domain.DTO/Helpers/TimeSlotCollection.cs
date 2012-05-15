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
namespace Probel.NDoctor.Domain.DTO.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Data;
    using Probel.NDoctor.Domain.DTO.Properties;

    public class TimeSlotCollection : ObservableCollection<DateRange>
    {
        #region Methods

        /// <summary>
        /// Creates the specified items as a <see cref="TimeSlotCollection"/>.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static TimeSlotCollection Create(IEnumerable<DateRange> items)
        {
            var result = new TimeSlotCollection();
            foreach (var item in items)
            {
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Creates the specified items as a <see cref="TimeSlotCollection"/> which start at the specified
        /// start date, ends at the specified date and where the start and end time are in range of a workday.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="workday">The workday.</param>
        /// <returns></returns>
        public static TimeSlotCollection Create(DateTime from, DateTime to, Workday workday)
        {
            Assert.IsFalse(workday.Duration == SlotDuration.NotConfigured, "The duration of the workday should be configured");

            var result = new List<DateRange>();
            var duration = workday.Duration.ToTimeSpan();

            var counter = 0;
            var i = from.Date;
            while (i < to.AddDays(1).Date)
            {
                var j = i.Add(duration);

                if (workday.IsInRange(new DateRange(i, j)))
                {
                    result.Add(new DateRange(i, j));
                }
                i = j;

                // It is easy to make a mistake an fall into an infinite loops in such a case.
                // I test to avoid this, just in case ;)
                if (++counter >= 65535) { throw new IndexOutOfRangeException(Messages.Error_TooManyLoops); }
            }
            return TimeSlotCollection.Create(result);
        }

        #endregion Methods
    }
}