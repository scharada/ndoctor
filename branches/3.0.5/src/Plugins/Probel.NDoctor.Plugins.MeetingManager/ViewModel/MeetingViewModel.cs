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
    using System.Timers;
    using System.Windows.Input;

    using Probel.Helpers.Data;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;

    internal abstract class MeetingViewModel : BaseViewModel
    {
        #region Fields

        protected static readonly Timer Countdown = new Timer(250) { AutoReset = true };

        private ICalendarComponent component = PluginContext.ComponentFactory.GetInstance<ICalendarComponent>();
        private string criteria;
        private DateTime endDate;
        private TagDto selectedAppointmentTag;
        private LightPatientDto selectedPatient;
        private DateRange selectedSlot;
        private DateTime startDate;

        #endregion Fields

        #region Constructors

        public MeetingViewModel()
        {
            this.StartDate
                = this.startDate
                = DateTime.Today;
            this.FoundPatients = new ObservableCollection<LightPatientDto>();
            this.AppointmentTags = new ObservableCollection<TagDto>();

            this.SearchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<TagDto> AppointmentTags
        {
            get;
            private set;
        }

        public string Criteria
        {
            get { return this.criteria; }
            set
            {
                Countdown.Start();
                this.criteria = value;
                this.OnPropertyChanged(() => Criteria);
            }
        }

        public DateTime EndDate
        {
            get { return this.startDate; }
            set
            {
                this.startDate = value;
                this.OnPropertyChanged(() => EndDate);
            }
        }

        public ObservableCollection<LightPatientDto> FoundPatients
        {
            get;
            private set;
        }

        public ICommand SearchCommand
        {
            get;
            private set;
        }

        public TagDto SelectedAppointmentTag
        {
            get { return this.selectedAppointmentTag; }
            set
            {
                this.selectedAppointmentTag = value;
                this.OnPropertyChanged(() => SelectedAppointmentTag);
            }
        }

        public LightPatientDto SelectedPatient
        {
            get { return this.selectedPatient; }
            set
            {
                this.selectedPatient = value;
                this.SelectedSlot = null;
                this.ClearSlotZone();
                this.OnPropertyChanged(() => SelectedPatient);
            }
        }

        public DateRange SelectedSlot
        {
            get { return this.selectedSlot; }
            set
            {
                this.selectedSlot = value;
                this.OnPropertyChanged(() => SelectedSlot);
            }
        }

        public DateTime StartDate
        {
            get { return this.endDate; }
            set
            {
                this.endDate = value;
                this.OnPropertyChanged(() => StartDate);
            }
        }

        protected ICalendarComponent Component
        {
            get { return this.component; }
        }

        #endregion Properties

        #region Methods

        protected bool CanFindSlots()
        {
            if (this.SelectedPatient == null) return false;
            else if (this.EndDate < this.StartDate) return false;
            else return true;
        }

        protected abstract void ClearSlotZone();

        private bool CanSearch()
        {
            return !string.IsNullOrWhiteSpace(this.criteria);
        }

        private void Search()
        {
            try
            {
                var patients = this.component.GetPatientsByNameLight(this.Criteria, PluginContext.Configuration.SearchType);
                var tags = this.component.GetTags(TagCategory.Appointment);

                this.FoundPatients.Refill(patients);
                this.AppointmentTags.Refill(tags);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        #endregion Methods
    }
}