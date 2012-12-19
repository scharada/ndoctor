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
    using System.Windows;
    using System.Windows.Controls;

    public class CalendarAppointmentItem : ContentControl
    {
        #region Fields

        public static readonly DependencyProperty EndTimeProperty = TimeSlotPanel.EndTimeProperty.AddOwner(typeof(CalendarAppointmentItem));
        public static readonly DependencyProperty IndentationProperty = TimeSlotPanel.IndentationProperty.AddOwner(typeof(CalendarAppointmentItem));
        public static readonly DependencyProperty StartTimeProperty = TimeSlotPanel.StartTimeProperty.AddOwner(typeof(CalendarAppointmentItem));

        #endregion Fields

        #region Constructors

        static CalendarAppointmentItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarAppointmentItem), new FrameworkPropertyMetadata(typeof(CalendarAppointmentItem)));
        }

        #endregion Constructors

        #region Properties

        public bool EndTime
        {
            get { return (bool)GetValue(EndTimeProperty); }
            set { SetValue(EndTimeProperty, value); }
        }

        public uint Indentation
        {
            get { return (uint)GetValue(IndentationProperty); }
            set { SetValue(IndentationProperty, value); }
        }

        public bool StartTime
        {
            get { return (bool)GetValue(StartTimeProperty); }
            set { SetValue(StartTimeProperty, value); }
        }

        #endregion Properties
    }
}