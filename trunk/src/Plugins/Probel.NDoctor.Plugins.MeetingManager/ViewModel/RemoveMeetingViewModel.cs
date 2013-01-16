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
namespace Probel.NDoctor.Plugins.MeetingManager.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MeetingManager.Helpers;
    using Probel.NDoctor.Plugins.MeetingManager.Properties;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Toolbox;
    using Probel.Mvvm.Gui;

    internal class RemoveMeetingViewModel : MeetingViewModel
    {
        #region Fields

        private AppointmentDto selectedAppointment;

        #endregion Fields

        #region Constructors

        public RemoveMeetingViewModel()
        {
            this.BusyAppointments = new ObservableCollection<AppointmentDto>();
            this.GetSlotsCommand = new RelayCommand(() => this.GetSlots(), () => this.CanFindSlots());
            this.RemoveAppointmentCommand = new RelayCommand(() => this.RemoveAppointment(), () => CanRemoveAppointment());
            Countdown.Elapsed += (sender, e) => PluginContext.Host.Invoke(() =>
            {
                this.SearchCommand.TryExecute();
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

        public ICommand GetSlotsCommand
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

        private void GetSlots()
        {
            var slots = this.Component.GetAppointments(this.SelectedPatient, this.StartDate, this.EndDate);

            this.BusyAppointments.Refill(slots);

            if (slots.Count == 0) { ViewService.MessageBox.Information(Messages.Msg_NothingFound); }
        }

        private void RemoveAppointment()
        {
            try
            {
                this.Component.Remove(this.SelectedAppointment, this.SelectedPatient, new PluginSettings().GetGoogleConfiguration());

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_AppointmentAdded);
                this.Close();
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        #endregion Methods
    }
}