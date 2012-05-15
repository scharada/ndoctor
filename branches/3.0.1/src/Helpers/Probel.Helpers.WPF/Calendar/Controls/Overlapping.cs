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
namespace Probel.Helpers.WPF.Calendar.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Probel.Helpers.Data;

    public static class Overlapping
    {
        #region Methods

        public static double ItemCount(CalendarAppointmentItem currentApp, UIElementCollection children)
        {
            double count = 0;
            foreach (UIElement child in children)
            {
                if (child is CalendarAppointmentItem)
                {
                    var currentChild = child as CalendarAppointmentItem;

                    var cStart = currentApp.GetValue(TimeSlotPanel.StartTimeProperty) as DateTime?;
                    var cEnd = currentApp.GetValue(TimeSlotPanel.EndTimeProperty) as DateTime?;
                    var toTest = new DateRange(cStart.Value, cEnd.Value);

                    var aStart = currentChild.GetValue(TimeSlotPanel.StartTimeProperty) as DateTime?;
                    var aEnd = currentChild.GetValue(TimeSlotPanel.EndTimeProperty) as DateTime?;
                    var current = new DateRange(aStart.Value, aEnd.Value);

                    if (toTest.Overlaps(current))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        #endregion Methods
    }
}