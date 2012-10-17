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

namespace Probel.NDoctor.Plugins.PatientSession.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.DTO.Specification;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class SearchPatientExtendedViewModel : BaseViewModel
    {
        #region Fields

        private readonly IPatientSessionComponent Component;

        private bool isBusy;
        private bool isByProfession;
        private string name;
        private LightPatientDto selectedPatient;
        private ProfessionDto selectedProfession;

        #endregion Fields

        #region Constructors

        public SearchPatientExtendedViewModel()
        {
            this.Component = PluginContext.ComponentFactory.GetInstance<IPatientSessionComponent>();

            this.FoundPatients = new ObservableCollection<LightPatientDto>();
            this.Professions = new ObservableCollection<ProfessionDto>();

            this.SearchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());
            this.RefreshCommand = new RelayCommand(() => this.Refresh());
            this.SelectPatientCommand = new RelayCommand(() => this.SelectPatient(), () => this.CanSelectPatient());

            this.Name = "*";
            this.IsBusy = false;
        }

        #endregion Constructors

        #region Properties

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

        public bool IsByProfession
        {
            get { return this.isByProfession; }
            set
            {
                this.isByProfession = value;
                this.OnPropertyChanged(() => IsByProfession);
            }
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged(() => Name);
            }
        }

        public ObservableCollection<ProfessionDto> Professions
        {
            get;
            private set;
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public ICommand SearchCommand
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

        public ProfessionDto SelectedProfession
        {
            get { return this.selectedProfession; }
            set
            {
                this.selectedProfession = value;
                this.OnPropertyChanged(() => SelectedProfession);
            }
        }

        public ICommand SelectPatientCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private bool CanSearch()
        {
            return this.SelectedProfession != null
                && !string.IsNullOrWhiteSpace(this.Name);
        }

        private bool CanSelectPatient()
        {
            return this.SelectedPatient != null;
        }

        private void Refresh()
        {
            try
            {
                this.Professions.Refill(this.Component.GetAllProfessions());
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void Search()
        {
            try
            {
                var expression = new SpecificationExpression<PatientDto>();

                if (this.IsByProfession) { expression.And(new FindPatientByProfessionSpecification(this.SelectedProfession)); }

                var context = TaskScheduler.FromCurrentSynchronizationContext();
                var task = Task.Factory
                    .StartNew<IList<LightPatientDto>>(() => this.SearchAsync(expression))
                    .ContinueWith(e => this.SearchCallback(e), context);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private IList<LightPatientDto> SearchAsync(SpecificationExpression<PatientDto> expression)
        {
            this.IsBusy = true;
            return this.Component.FindPatientsByNameLight(this.Name, expression);
        }

        private void SearchCallback(Task<IList<LightPatientDto>> e)
        {
            this.FoundPatients.Refill(e.Result);
            this.IsBusy = false;
        }

        private void SelectPatient()
        {
            PluginContext.Host.SelectedPatient = this.SelectedPatient;
            InnerWindow.Close();
            PluginContext.Host.NavigateToStartPage();
        }

        #endregion Methods
    }
}