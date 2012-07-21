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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Timers;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientSession.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class SearchPatientViewModel : BaseViewModel
    {
        #region Fields

        public static readonly Timer Countdown = new Timer(250) { AutoReset = true };

        private IPatientSessionComponent component;
        private string criteria = string.Empty;

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

            this.component = PluginContext.ComponentFactory.GetInstance<IPatientSessionComponent>();
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPatientSessionComponent>();

            Countdown.Elapsed += (sender, e) => PluginContext.Host.Invoke(() =>
            {
                this.SearchCommand.TryExecute();
                Countdown.Stop();
            });
        }

        #endregion Constructors

        #region Properties

        public string ButtonSearchTitle
        {
            get { return Messages.Title_ButtonSearch; }
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
            var result = this.component.GetTopXPatient(10);
            var patients = Mapper.Map<IList<LightPatientDto>, IList<LightPatientViewModel>>(result);
            this.Top10Patients.Refill(patients);
        }

        private bool CanSearch()
        {
            return !string.IsNullOrEmpty(this.Criteria);
        }

        private void Search()
        {
            var result = this.component.FindPatientsByNameLight(this.Criteria, SearchOn.FirstAndLastName);
            var patients = Mapper.Map<IList<LightPatientDto>, IList<LightPatientViewModel>>(result);
            this.FoundPatients.Refill(patients);

            PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_SearchExecuted.FormatWith(result.Count()));
        }

        #endregion Methods
    }
}