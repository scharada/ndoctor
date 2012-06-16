﻿#region Header

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

namespace Probel.NDoctor.Plugins.MeetingManager.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Timers;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MeetingManager.Helpers;
    using Probel.NDoctor.Plugins.MeetingManager.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class RemoveMeetingViewModel : MeetingViewModel
    {
        #region Fields

        private AppointmentDto selectedAppointment;

        #endregion Fields

        #region Constructors

        public RemoveMeetingViewModel()
        {
            this.BusyAppointments = new ObservableCollection<AppointmentDto>();
            this.FindSlotsCommand = new RelayCommand(() => this.FindSlots(), () => this.CanFindSlots());
            this.RemoveAppointmentCommand = new RelayCommand(() => this.RemoveAppointment(), () => CanRemoveAppointment());
            Countdown.Elapsed += (sender, e) => PluginContext.Host.Invoke(() =>
            {
                this.SearchCommand.ExecuteIfCan();
                Countdown.Stop();
            });
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<AppointmentDto> BusyAppointments
        {
            get;
            private set;
        }

        public ICommand FindSlotsCommand
        {
            get;
            private set;
        }

        public ICommand RemoveAppointmentCommand
        {
            get;
            private set;
        }

        public AppointmentDto SelectedAppointment
        {
            get { return this.selectedAppointment; }
            set
            {
                this.selectedAppointment = value;
                this.OnPropertyChanged(() => SelectedAppointment);
            }
        }

        #endregion Properties

        #region Methods

        protected override void ClearSlotZone()
        {
            this.AppointmentTags.Clear();
        }

        private bool CanRemoveAppointment()
        {
            return this.SelectedAppointment != null
                && this.SelectedPatient != null;
        }

        private void FindSlots()
        {
            IList<AppointmentDto> slots;
            using (this.Component.UnitOfWork)
            {
                slots = this.Component.FindAppointments(this.SelectedPatient, this.StartDate, this.EndDate);
            }
            this.BusyAppointments.Refill(slots);

            if (slots.Count == 0) { MessageBox.Show(Messages.Msg_NothingFound, BaseText.Information, MessageBoxButton.OK, MessageBoxImage.Information); }
        }

        private void RemoveAppointment()
        {
            try
            {
                using (this.Component.UnitOfWork)
                {
                    this.Component.Remove(this.SelectedAppointment, this.SelectedPatient);
                }

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_AppointmentAdded);
                Notifyer.OnRefreshed(this);
                InnerWindow.Close();
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        #endregion Methods
    }
}