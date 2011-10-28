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

namespace Probel.NDoctor.Domain.DTO.Helpers
{
    using System;

    using Probel.Helpers.Data;
    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.Properties;

    /// <summary>
    /// Represents a work day. That's it contain the start hour and the end hour of the working day.
    /// The two <see cref="DateTime"/> are MinValue and ctor add the specified hour and minutes to it
    /// </summary>
    public class Workday
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Workday"/> class.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="duration">The duration.</param>
        public Workday(string startTime, string endTime, SlotDuration duration)
        {
            this.Duration = duration;
            this.From = this.GetTime(startTime);
            this.To = this.GetTime(endTime);

            if (this.From > this.To) throw new ArgumentException(Messages.Error_BeginBiggerThanEnd);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Represents a slot duration. Tha's how much time last a meeting
        /// </summary>
        public SlotDuration Duration
        {
            get;
            private set;
        }

        public DateTime From
        {
            get;
            private set;
        }

        public DateTime To
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Determines whether the time of the specified <see cref="DateTime"/>
        /// is in the range of the workday. That's between the start time and the end time.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        ///   <c>true</c> if is in range; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInRange(DateTime other)
        {
            var time = DateTime.MinValue.AddHours(other.Hour).AddMinutes(other.Minute);
            return time >= this.From && time <= this.To;
        }

        /// <summary>
        /// Determines whether the time of the specified <see cref="DateRange"/>
        /// is in the range of the workday. That's whether the <see cref="DateRange"/>
        /// starts after the workday start and finishes before the workday end.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        ///   <c>true</c> if is in range; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInRange(DateRange other)
        {
            var from = DateTime.MinValue.AddHours(other.StartTime.Hour).AddMinutes(other.StartTime.Minute);
            var to = DateTime.MinValue.AddHours(other.StartTime.Hour).AddMinutes(other.EndTime.Minute);

            return from >= this.From && from <= this.To
                && to >= this.From && to <= this.To;
        }

        private DateTime GetTime(string value)
        {
            var parts = value.Split(':');
            if (parts.Length != 2) throw new ArgumentException(Messages.Error_WrongTextForDate.StringFormat(value));

            int hours, minutes;
            if (!Int32.TryParse(parts[0], out hours)) throw new ArgumentException(Messages.Error_WrongTextForDate);
            if (!Int32.TryParse(parts[1], out minutes)) throw new ArgumentException(Messages.Error_WrongTextForDate);

            if (hours < 0 || hours >= 24) throw new ArgumentException(Messages.Error_NotSupportedDate_Hour);
            if (minutes < 0 || minutes >= 60) throw new ArgumentException(Messages.Error_NotSupportedDate_Minutes);

            return DateTime.MinValue.AddHours(hours).AddMinutes(minutes);
        }

        #endregion Methods
    }
}