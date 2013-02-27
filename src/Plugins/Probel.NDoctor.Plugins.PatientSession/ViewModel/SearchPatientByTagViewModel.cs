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
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using System;
    using Probel.NDoctor.Domain.DTO.Specifications;
    using Probel.NDoctor.Plugins.PatientSession.Properties;

    internal class SearchPatientByTagViewModel : BaseViewModel
    {
        #region Fields

        private readonly IPatientSessionComponent Component = PluginContext.ComponentFactory.GetInstance<IPatientSessionComponent>();
        private readonly ICommand refreshCommand;
        private readonly ICommand searchCommand;
        private readonly ICommand selectPatientCommand;

        private bool isBusy;
        private LightPatientDto selectedPatient;

        #endregion Fields

        #region Constructors

        public SearchPatientByTagViewModel()
        {
            this.FoundPatients = new ObservableCollection<LightPatientDto>();
            this.Criterion = new ObservableCollection<SearchTagDto>();
            this.SearchTags = new ObservableCollection<SearchTagDto>();
            this.refreshCommand = new RelayCommand(() => this.Refresh(), () => this.CanRefresh());
            this.searchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());
            this.selectPatientCommand = new RelayCommand(() => this.SelectPatient(), () => this.CanSelectPatient());

            this.LogicalOperators = new ObservableCollection<Tuple<Operator, string>>();
            this.LogicalOperators.Add(new Tuple<Operator, string>(Operator.And, Messages.Search_And));
            this.LogicalOperators.Add(new Tuple<Operator, string>(Operator.Or, Messages.Search_Or));
            this.SelectedOperator = this.LogicalOperators[0];
        }

        private Tuple<Operator, string> selectedOperator;
        public Tuple<Operator, string> SelectedOperator
        {
            get { return this.selectedOperator; }
            set
            {
                this.selectedOperator = value;
                this.OnPropertyChanged(() => SelectedOperator);
            }
        }
        public ObservableCollection<Tuple<Operator, string>> LogicalOperators
        {
            get;
            private set;
        }
        #endregion Constructors

        #region Properties

        public ObservableCollection<SearchTagDto> Criterion
        {
            get;
            private set;
        }

        public ObservableCollection<LightPatientDto> FoundPatients
        {
            get;
            private set;
        }

        public bool IsBusy
        {
            get { return this.isBusy; }
            set
            {
                this.isBusy = value;
                this.OnPropertyChanged(() => IsBusy);
            }
        }

        public ICommand RefreshCommand
        {
            get { return this.refreshCommand; }
        }

        public ICommand SearchCommand
        {
            get { return this.searchCommand; }
        }

        public ObservableCollection<SearchTagDto> SearchTags
        {
            get;
            private set;
        }

        public LightPatientDto SelectedPatient
        {
            get { return this.selectedPatient; }
            set
            {
                this.selectedPatient = value;
                this.OnPropertyChanged(() => SelectedPatient);
            }
        }

        public ICommand SelectPatientCommand
        {
            get { return this.selectPatientCommand; }
        }

        #endregion Properties

        #region Methods

        private bool CanRefresh()
        {
            return true;
        }

        private bool CanSearch()
        {
            return true;
        }

        private bool CanSelectPatient()
        {
            return true;
        }

        private void Refresh()
        {
            this.SearchTags.Refill(this.Component.GetAllSearchTags());
        }

        private void Search()
        {
            this.IsBusy = true;

            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;

            var task = Task.Factory
                           .StartNew<IEnumerable<LightPatientDto>>(() => this.Component.GetPatientsWithTags(this.Criterion, this.SelectedOperator.Item1));
            task.ContinueWith(t =>
            {
                this.FoundPatients.Refill(t.Result);
                this.IsBusy = false;
            }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
        }

        private void SelectPatient()
        {
            PluginContext.Host.SelectedPatient = this.SelectedPatient;
            this.Close();
            PluginContext.Host.NavigateToStartPage();
        }

        #endregion Methods
    }
}