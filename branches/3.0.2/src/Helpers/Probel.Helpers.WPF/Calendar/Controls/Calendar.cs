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
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Probel.Helpers.WPF.Calendar.Model;

    public class Calendar : Control
    {
        #region Fields

        public static readonly RoutedEvent AddAppointmentEvent = 
            CalendarTimeSlotItem.AddAppointmentEvent.AddOwner(typeof(CalendarDay));
        public static readonly DependencyProperty AppointmentsProperty = 
            DependencyProperty.Register("Appointments", typeof(IEnumerable<Appointment>), typeof(Calendar),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Calendar.OnAppointmentsChanged)));

        /// <summary>
        /// CurrentDate Dependency Property
        /// </summary>
        public static readonly DependencyProperty CurrentDateProperty = 
            DependencyProperty.Register("CurrentDate", typeof(DateTime), typeof(Calendar),
                new FrameworkPropertyMetadata((DateTime)DateTime.Now,
                    new PropertyChangedCallback(OnCurrentDateChanged)));
        public static readonly RoutedCommand NextDay = new RoutedCommand("NextDay", typeof(Calendar));
        public static readonly RoutedCommand PreviousDay = new RoutedCommand("PreviousDay", typeof(Calendar));

        /// <summary>
        /// Refreshes the calendar
        /// </summary>
        public static readonly RoutedCommand Refresh = new RoutedCommand("Refresh", typeof(Calendar));

        #endregion Fields

        #region Constructors

        static Calendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(typeof(Calendar)));

            CommandManager.RegisterClassCommandBinding(typeof(Calendar)
                , new CommandBinding(NextDay, new ExecutedRoutedEventHandler(OnExecutedNextDay), new CanExecuteRoutedEventHandler(OnCanExecuteNextDay)));

            CommandManager.RegisterClassCommandBinding(typeof(Calendar)
                , new CommandBinding(PreviousDay, new ExecutedRoutedEventHandler(OnExecutedPreviousDay), new CanExecuteRoutedEventHandler(OnCanExecutePreviousDay)));

            CommandManager.RegisterClassCommandBinding(typeof(Calendar)
                , new CommandBinding(Refresh, new ExecutedRoutedEventHandler(OnExecutedRefresh), new CanExecuteRoutedEventHandler(OnCanExecuteRefresh)));
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

        #region Properties

        public IEnumerable<Appointment> Appointments
        {
            get { return (IEnumerable<Appointment>)GetValue(AppointmentsProperty); }
            set
            {
                SetValue(AppointmentsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the CurrentDate property.  This dependency property 
        /// indicates ....
        /// </summary>
        public DateTime CurrentDate
        {
            get { return (DateTime)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }

        #endregion Properties

        #region Methods

        public static void OnExecutedRefresh(object sender, ExecutedRoutedEventArgs e)
        {
            ((Calendar)sender).OnExecutedRefresh(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            /* REFACTOR: Check whether this code is elegant and not error prone
             * The first time OnCurrentDateChanged is called, the template
             * children aren't found in the method FilterAppointment. Therefore
             * no appointments was displayed on the screen. I force the appointment's
             * refresh everytime this method is called. It smells refactoring ;)
             */
            this.FilterAppointments();
        }

        public void RefreshData()
        {
            this.CurrentDate += TimeSpan.FromMilliseconds(1);
        }

        protected virtual void OnAppointmentsChanged(DependencyPropertyChangedEventArgs e)
        {
            FilterAppointments();
        }

        protected virtual void OnCanExecuteNextDay(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = false;
        }

        protected virtual void OnCanExecutePreviousDay(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = false;
        }

        protected virtual void OnCanExecuteRefresh(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = false;
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CurrentDate property.
        /// </summary>
        protected virtual void OnCurrentDateChanged(DependencyPropertyChangedEventArgs e)
        {
            FilterAppointments();
        }

        protected virtual void OnExecutedNextDay(ExecutedRoutedEventArgs e)
        {
            CurrentDate += TimeSpan.FromDays(1);
            e.Handled = true;
        }

        protected virtual void OnExecutedPreviousDay(ExecutedRoutedEventArgs e)
        {
            CurrentDate -= TimeSpan.FromDays(1);
            e.Handled = true;
        }

        protected virtual void OnExecutedRefresh(ExecutedRoutedEventArgs e)
        {
            CurrentDate += TimeSpan.FromMilliseconds(1);
            e.Handled = true;
        }

        private static void OnAppointmentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Calendar)d).OnAppointmentsChanged(e);
        }

        private static void OnCanExecuteNextDay(object sender, CanExecuteRoutedEventArgs e)
        {
            ((Calendar)sender).OnCanExecuteNextDay(e);
        }

        private static void OnCanExecutePreviousDay(object sender, CanExecuteRoutedEventArgs e)
        {
            ((Calendar)sender).OnCanExecutePreviousDay(e);
        }

        private static void OnCanExecuteRefresh(object sender, CanExecuteRoutedEventArgs e)
        {
            ((Calendar)sender).OnCanExecuteRefresh(e);
        }

        /// <summary>
        /// Handles changes to the CurrentDate property.
        /// </summary>
        private static void OnCurrentDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Calendar)d).OnCurrentDateChanged(e);
        }

        private static void OnExecutedNextDay(object sender, ExecutedRoutedEventArgs e)
        {
            ((Calendar)sender).OnExecutedNextDay(e);
        }

        private static void OnExecutedPreviousDay(object sender, ExecutedRoutedEventArgs e)
        {
            ((Calendar)sender).OnExecutedPreviousDay(e);
        }

        private void FilterAppointments()
        {
            DateTime byDate = CurrentDate;
            CalendarDay day = this.GetTemplateChild("day") as CalendarDay;
            if (day != null) day.ItemsSource = Appointments.ByDate(byDate);

            TextBlock dayHeader = this.GetTemplateChild("dayHeader") as TextBlock;
            if (dayHeader != null) dayHeader.Text = byDate.DayOfWeek.ToString() + " " + byDate.ToShortDateString();
        }

        #endregion Methods
    }
}