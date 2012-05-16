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
namespace Probel.NDoctor.Plugins.PatientSession.ViewModel
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Conversions;
    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientSession.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using Probel.NDoctor.Domain.Components;

    public class SearchPatientViewModel : BaseViewModel
    {
        #region Fields

        private IPatientSessionComponent component;
        private string criterium = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchPatientViewModel"/> class.
        /// </summary>
        public SearchPatientViewModel()
            : base()
        {
            this.FoundPatients = new ObservableCollection<LightPatientViewModel>();
            this.Top10Patients = new ObservableCollection<LightPatientViewModel>();
            this.TodayPatients = new ObservableCollection<LightPatientViewModel>();
            this.SearchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());

            this.component = new ComponentFactory().GetInstance<IPatientSessionComponent>();
        }

        #endregion Constructors

        #region Properties

        public string ButtonSearchTitle
        {
            get { return Messages.Title_ButtonSearch; }
        }

        public string Criterium
        {
            get { return this.criterium; }
            set
            {
                this.criterium = value;
                this.OnPropertyChanged(() => Criterium);
            }
        }

        public ObservableCollection<LightPatientViewModel> FoundPatients
        {
            get;
            set;
        }

        public ICommand SearchCommand
        {
            get;
            private set;
        }

        public ObservableCollection<LightPatientViewModel> TodayPatients
        {
            get;
            set;
        }

        public ObservableCollection<LightPatientViewModel> Top10Patients
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            IList<LightPatientDto> result;
            using (this.component.UnitOfWork)
            {
                result = this.component.GetTopXPatient(10);
            }
            var patients = Mapper.Map<IList<LightPatientDto>, IList<LightPatientViewModel>>(result);
            this.Top10Patients.Refill(patients);
        }

        private bool CanSearch()
        {
            return !string.IsNullOrEmpty(this.Criterium);
        }

        private void Search()
        {
            IList<LightPatientDto> result;
            using (this.component.UnitOfWork)
            {
                result = this.component.FindPatientsByNameLight(this.Criterium, SearchOn.FirstAndLastName);
            }
            var patients = Mapper.Map<IList<LightPatientDto>, IList<LightPatientViewModel>>(result);
            this.FoundPatients.Refill(patients);

            PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_SearchExecuted.FormatWith(result.Count()));
        }

        #endregion Methods
    }
}