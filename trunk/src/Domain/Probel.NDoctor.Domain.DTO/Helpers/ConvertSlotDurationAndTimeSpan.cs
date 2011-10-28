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

    using Probel.Helpers.Assertion;

    /// <summary>
    /// Extension to convert <see cref="TimeSpan"/> into <see cref="SlotDuration"/>
    /// and vice versa.
    /// </summary>
    public static class ConvertSlotDurationAndTimeSpan
    {
        #region Methods

        public static SlotDuration ToSlotDuration(this TimeSpan timespan)
        {
            if (timespan == new TimeSpan(0, 30, 0)) return SlotDuration.ThirtyMinutes;
            else if (timespan == new TimeSpan(1, 0, 0)) return SlotDuration.OneHour;
            else return SlotDuration.NotConfigured;
        }

        public static TimeSpan ToTimeSpan(this SlotDuration slot)
        {
            var thirtyMinute = new TimeSpan(0, 30, 0);
            var oneHour = new TimeSpan(1, 0, 0);
            TimeSpan result = new TimeSpan();
            switch (slot)
            {
                case SlotDuration.ThirtyMinutes:
                    result = thirtyMinute;
                    break;
                case SlotDuration.OneHour:
                    result = oneHour;
                    break;
                default:
                    Assert.FailOnEnumeration(slot);
                    break;
            }
            return result;
        }

        #endregion Methods
    }
}