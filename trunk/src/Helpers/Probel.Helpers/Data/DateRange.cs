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
namespace Probel.Helpers.Data
{
    using System;

    using Probel.Helpers.Properties;
    using Probel.Mvvm.DataBinding;

    /// <summary>
    /// 
    /// </summary>
    public class DateRange : ObservableObject, IEquatable<DateRange>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRange"/> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        public DateRange(DateTime start, DateTime end)
        {
            if (start > end) throw new ArgumentException("The start date of the range should be before the end date.");
            this.StartTime = start;
            this.EndTime = end;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the end date of the range.
        /// </summary>
        public DateTime EndTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the start date of the range.
        /// </summary>
        public DateTime StartTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the time span of this time range.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get
            {
                return this.EndTime - this.StartTime;
            }
        }

        public string TimeToDisplay
        {
            get
            {
                var dateFrom = this.StartTime.ToShortDateString();
                var dateTo = (this.StartTime.Date == this.EndTime.Date)
                    ? string.Empty
                    : this.EndTime.ToShortDateString();

                var timeFrom = this.StartTime.ToString("HH:mm");
                var timeTo = this.EndTime.ToString("HH:mm");

                return string.Format(Messages.Title_FromTo, dateFrom, timeFrom, dateTo, timeTo);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool AreEqualTimeSpan(DateRange other)
        {
            return this.StartTime == other.StartTime
                && this.EndTime == other.EndTime;
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(DateRange other)
        {
            return this.StartTime == other.StartTime
                && this.EndTime == other.EndTime;
        }

        /// <summary>
        /// Indicates if the specified date range overlapses with this instance
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns><c>True</c> if the ranges overlap; otherwise <c>False</c></returns>
        public bool Overlaps(DateRange range)
        {
            return !(this.StartTime <= range.StartTime && this.EndTime <= range.StartTime
                  || range.EndTime <= this.StartTime && range.EndTime <= this.EndTime);
        }

        public override string ToString()
        {
            return string.Format("[{0}] From {1} - To: {2}"
                , this.GetType()
                , this.StartTime
                , this.EndTime);
        }

        #endregion Methods
    }
}