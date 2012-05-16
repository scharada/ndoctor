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
    using System.Collections.Generic;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Conversions;
    using Probel.Helpers.Data;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MeetingManager.Helpers;
    using Probel.NDoctor.Plugins.MeetingManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class PatientViewModel : BaseViewModel
    {
        #region Fields

        private ICalendarComponent component = new ComponentFactory(PluginContext.Host.ConnectedUser).GetInstance<ICalendarComponent>();
        private DateTime fromDate;
        private bool isSelected;
        private LightPatientDto patient;
        private DateTime toDate;

        #endregion Fields

        #region Constructors

        public PatientViewModel()
            : base()
        {
            this.FoundSlotsToAdd = new TimeSlotCollection();
            this.FoundSlotsToRemove = new TimeSlotCollection();
            this.SearchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());

            this.fromDate
                = this.toDate
                = DateTime.Today;

            Notifyer.Refreshed += (sender, e) =>
            {
                if (CanSearch()) { this.Search(); }
            };
        }

        #endregion Constructors

        #region Properties

        public string DisplayedName
        {
            get
            {
                return (this.Patient == null)
                    ? string.Empty
                    : this.Patient.DisplayedName;
            }
        }

        public TimeSlotCollection FoundSlotsToAdd
        {
            get;
            private set;
        }

        public TimeSlotCollection FoundSlotsToRemove
        {
            get;
            private set;
        }

        public DateTime FromDate
        {
            get { return this.fromDate; }
            set
            {
                this.fromDate = value;
                this.ToDate = this.FromDate;
                this.OnPropertyChanged(() => FromDate);
            }
        }

        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                if (value == false)
                {
                    this.FoundSlotsToAdd.Clear();
                    this.FoundSlotsToRemove.Clear();
                }
                this.isSelected = value;
                this.OnPropertyChanged(() => IsSelected);
            }
        }

        public LightPatientDto Patient
        {
            get { return this.patient; }
            set
            {
                this.patient = value;
                this.OnPropertyChanged(() => Patient);
            }
        }

        public ICommand SearchCommand
        {
            get;
            private set;
        }

        public DateTime ToDate
        {
            get { return this.toDate; }
            set
            {
                this.toDate = value;
                this.OnPropertyChanged(() => ToDate);
            }
        }

        #endregion Properties

        #region Methods

        private bool CanSearch()
        {
            return this.FromDate <= this.ToDate;
        }

        private void Search()
        {
            try
            {
                using (this.component.UnitOfWork)
                {
                    this.SearchAppointmentToAdd();
                    this.SearchAppointmentToRemove();
                }
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Error_ErrorSearching);
            }
        }

        private void SearchAppointmentToAdd()
        {
            var result = this.component.FindSlots(this.FromDate, this.ToDate, PluginContext.Host.Workday);
            DateRangeViewModel.RefreshTags();
            var mappedResult = Mapper.Map<IList<DateRange>, IList<DateRangeViewModel>>(result);

            // Fills the default subject to all the found appointments and the
            // good patient.
            foreach (var item in mappedResult)
            {
                item.Patient = this.Patient;
            }
            this.FoundSlotsToAdd.Refill(mappedResult);
        }

        private void SearchAppointmentToRemove()
        {
            var result = this.component.FindAppointments(this.Patient, this.FromDate, this.ToDate);
            var mappedResult = Mapper.Map<IList<AppointmentDto>, IList<DateRangeViewModel>>(result);

            // Fills the default subject to all the found appointments and the
            // good patient.
            foreach (var item in mappedResult)
            {
                item.Patient = this.Patient;
            }

            this.FoundSlotsToRemove.Refill(mappedResult);
        }

        #endregion Methods
    }
}