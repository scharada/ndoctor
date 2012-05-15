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
    using System.Windows.Controls.Primitives;

    public class CalendarTimeSlotItem : ButtonBase
    {
        #region Fields

        public static readonly RoutedEvent AddAppointmentEvent = 
            EventManager.RegisterRoutedEvent("AddAppointment", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(CalendarTimeSlotItem));

        #endregion Fields

        #region Constructors

        static CalendarTimeSlotItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarTimeSlotItem), new FrameworkPropertyMetadata(typeof(CalendarTimeSlotItem)));
        }

        #endregion Constructors

        #region Events

        public event RoutedEventHandler AddAppointment
        {
            add
            {
                AddHandler(AddAppointmentEvent, value);
            }
            remove
            {
                RemoveHandler(AddAppointmentEvent, value);
            }
        }

        #endregion Events

        #region Methods

        protected virtual void OnAddAppointment(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        protected override void OnClick()
        {
            base.OnClick();

            RaiseAddAppointmentEvent();
        }

        private void RaiseAddAppointmentEvent()
        {
            OnAddAppointment(new RoutedEventArgs(AddAppointmentEvent, this));
        }

        #endregion Methods
    }
}