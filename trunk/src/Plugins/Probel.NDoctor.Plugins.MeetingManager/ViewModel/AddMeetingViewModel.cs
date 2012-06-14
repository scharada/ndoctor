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

namespace Probel.NDoctor.Plugins.MeetingManager.ViewModel
{
    using System;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MeetingManager.Helpers;
    using Probel.NDoctor.Plugins.MeetingManager.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class AddMeetingViewModel : MeetingViewModel
    {
        #region Constructors

        public AddMeetingViewModel()
        {
            this.FreeSlots = new TimeSlotCollection();
            this.AddAppointmentCommand = new RelayCommand(() => this.AddAppointment(), () => this.CanAddAppointment());
            this.FindFreeSlotsCommand = new RelayCommand(() => this.FindFreeSlots(), () => this.CanFindSlots());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddAppointmentCommand
        {
            get;
            private set;
        }

        public ICommand FindFreeSlotsCommand
        {
            get;
            private set;
        }

        public TimeSlotCollection FreeSlots
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        protected override void ClearSlotZone()
        {
            this.FreeSlots.Clear();
        }

        private void AddAppointment()
        {
            try
            {
                using (this.Component.UnitOfWork)
                {
                    var appointment = new AppointmentDto()
                    {
                        StartTime = this.SelectedSlot.StartTime,
                        EndTime = this.SelectedSlot.EndTime,
                        Subject = string.Format("{0} - {1}", this.SelectedAppointmentTag.Name, this.SelectedPatient.DisplayedName),
                        User = PluginContext.Host.ConnectedUser,
                        Tag = this.SelectedAppointmentTag,
                    };
                    this.Component.Create(appointment, this.SelectedPatient);
                }

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_AppointmentAdded);
                Notifyer.OnRefreshed(this);
                InnerWindow.Close();
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private bool CanAddAppointment()
        {
            return this.SelectedSlot != null
                && this.SelectedAppointmentTag != null;
        }

        private void FindFreeSlots()
        {
            try
            {
                var freeSlots = new TimeSlotCollection();
                using (this.Component.UnitOfWork)
                {
                    freeSlots = this.Component.FindSlots(this.StartDate, this.EndDate, PluginContext.Host.Workday);
                }
                this.FreeSlots.Refill(freeSlots);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        #endregion Methods
    }
}