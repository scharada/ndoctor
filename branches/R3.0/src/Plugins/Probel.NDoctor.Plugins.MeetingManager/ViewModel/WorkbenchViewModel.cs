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
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Conversions;
    using Probel.Helpers.WPF.Calendar.Model;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MeetingManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private ICalendarComponent component = ObjectFactory.GetInstance<ICalendarComponent>();
        private string criteria;
        private DateTime dateToDisplay;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
            : base()
        {
            this.DateToDisplay = DateTime.Today;
            this.FoundPatients = new ObservableCollection<PatientViewModel>();
            this.DayAppointments = new AppointmentCollection();

            this.SearchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());

            this.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "DateToDisplay")
                    this.RefreshCalendar();
            };
        }

        #endregion Constructors

        #region Properties

        public string Criteria
        {
            get { return this.criteria; }
            set
            {
                this.criteria = value;
                this.OnPropertyChanged("Criteria");
            }
        }

        public DateTime DateToDisplay
        {
            get { return this.dateToDisplay; }
            set
            {
                this.dateToDisplay = value;
                this.OnPropertyChanged("DateToDisplay");
            }
        }

        public AppointmentCollection DayAppointments
        {
            get;
            private set;
        }

        public ObservableCollection<PatientViewModel> FoundPatients
        {
            get;
            private set;
        }

        public ICommand SearchCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private bool CanSearch()
        {
            return !string.IsNullOrWhiteSpace(this.Criteria);
        }

        private void RefreshCalendar()
        {
            try
            {
                using (this.component.UnitOfWork)
                {
                    var result = this.component.FindAppointments(this.DateToDisplay);
                    var mappedResult = Mapper.Map<IList<AppointmentDto>, AppointmentCollection>(result);
                    this.DayAppointments.Refill(mappedResult);
                }
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Error_RefreshingCalendar);
            }
        }

        private void Search()
        {
            try
            {
                IList<LightPatientDto> result = new List<LightPatientDto>();
                using (this.component.UnitOfWork)
                {
                    result = this.component.FindPatientsByNameLight(this.Criteria, SearchOn.FirstAndLastName);
                }
                var mappedResult = Mapper.Map<IList<LightPatientDto>, IList<PatientViewModel>>(result);

                foreach (var item in mappedResult)
                {
                    item.Refreshed += (sender, e) =>
                    {
                        this.RefreshCalendar();
                        this.DateToDisplay = this.DateToDisplay.AddMilliseconds(1);
                    };
                }
                this.FoundPatients.Refill(mappedResult);
                this.DateToDisplay = this.DateToDisplay.AddMilliseconds(1);
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Error_ErrorSearching);
            }
        }

        #endregion Methods
    }
}