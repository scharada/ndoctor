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
namespace Probel.Helpers.WPF.Calendar.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public sealed class AppointmentCollection : ObservableCollection<Appointment>
    {
        #region Constructors

        public AppointmentCollection()
        {
        }

        public AppointmentCollection(IEnumerable<Appointment> list)
        {
            foreach (var item in list)
            {
                this.Add(item);
            }
        }

        #endregion Constructors

        #region Properties

        public static AppointmentCollection TestData
        {
            get
            {
                var result = new AppointmentCollection();
                var now = DateTime.Today;
                var i = 0;

                result.Add(new Appointment() { Subject = string.Format("{0} - 00:00 - 01:00", i++), StartTime = now, EndTime = now.AddHours(1) });
                result.Add(new Appointment() { Subject = string.Format("{0} - 01:00 - 02:00", i++), StartTime = now.AddHours(1), EndTime = now.AddHours(2) });
                result.Add(new Appointment() { Subject = string.Format("{0} - 02:00 - 03:00", i++), StartTime = now.AddHours(2), EndTime = now.AddHours(3) });
                result.Add(new Appointment() { Subject = string.Format("{0} - 02:00 - 03:00", i++), StartTime = now.AddHours(2), EndTime = now.AddHours(3) });
                result.Add(new Appointment() { Subject = string.Format("{0} - 04:00 - 05:00", i++), StartTime = now.AddHours(4), EndTime = now.AddHours(5) });
                result.Add(new Appointment() { Subject = string.Format("{0} - 05:00 - 06:00", i++), StartTime = now.AddHours(5), EndTime = now.AddHours(6) });
                result.Add(new Appointment() { Subject = string.Format("{0} - 02:00 - 04:00", i++), StartTime = now.AddHours(2), EndTime = now.AddHours(4) });
                result.Add(new Appointment() { Subject = string.Format("{0} - 03:00 - 04:00", i++), StartTime = now.AddHours(3), EndTime = now.AddHours(4) });

                return result;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds an object to the end of the System.Collections.ObjectModel.Collection<T>.
        /// </summary>
        /// <param name="item">The object to be added to the end of the System.Collections.ObjectModel.Collection<T>.
        /// The value can be null for reference types.</param>
        public new void Add(Appointment item)
        {
            base.Add(item);
            this.InternalSort();

            var items = (from i in this
                         where i.DateRange.Equals(item.DateRange)
                            || i.DateRange.Overlaps(item.DateRange)
                         select i).ToList();

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Indentation = (uint)i;
            }
        }

        /// <summary>
        /// Sorts the appointments on their duration to avoir displaying bus that will overlaps
        /// the meetings.
        /// </summary>
        private void InternalSort()
        {
            var sortedItemsList = this.OrderByDescending(e => e.DateRange.TimeSpan).ToList();

            foreach (var item in sortedItemsList)
            {
                base.Move(IndexOf(item), sortedItemsList.IndexOf(item));
            }
        }

        #endregion Methods
    }
}