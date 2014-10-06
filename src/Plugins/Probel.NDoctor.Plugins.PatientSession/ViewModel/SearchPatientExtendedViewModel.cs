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

    using Probel.Helpers.Assertion;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.DTO.Specifications;
    using Probel.NDoctor.Plugins.PatientSession.Helpers;
    using Probel.NDoctor.Plugins.PatientSession.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;

    internal class SearchPatientExtendedViewModel : BaseViewModel
    {
        #region Fields

        private readonly IPatientSessionComponent Component;
        private readonly PluginSettings Settings = new PluginSettings();

        private DateTime birthdateAfterDate = DateTime.Today.AddMonths(-1);
        private DateTime birthdayBeforeDate = DateTime.Today;
        private string cityCriteria = string.Empty;
        private DateTime inscriptionAfterDate = DateTime.Today.AddMonths(-1);
        private DateTime inscriptionBeforeDate = DateTime.Today;
        private bool isBusy;
        private bool isByBirthdate;
        private bool isByCity;
        private bool isByInscription;
        private bool isByLastUpdate;
        private bool isByName;
        private bool isByProfession;
        private bool isByReason;
        private string nameCriteria = string.Empty;
        private string reasonCriteria = string.Empty;
        private Tuple<Operator, string> selectedOperator;
        private LightPatientDto selectedPatient;
        private ProfessionDto selectedProfession;
        private DateTime updateAfterDate = DateTime.Today.AddMonths(-1);
        private DateTime updateBeforeDate = DateTime.Today;

        #endregion Fields

        #region Constructors

        public SearchPatientExtendedViewModel()
        {
            this.Component = PluginContext.ComponentFactory.GetInstance<IPatientSessionComponent>();

            this.FoundPatients = new ObservableCollection<LightPatientDto>();
            this.Professions = new ObservableCollection<ProfessionDto>();
            this.LogicalOperators = new ObservableCollection<Tuple<Operator, string>>();
            this.LogicalOperators.Add(new Tuple<Operator, string>(Operator.And, Messages.Search_And));
            this.LogicalOperators.Add(new Tuple<Operator, string>(Operator.Or, Messages.Search_Or));
            this.SelectedOperator = this.LogicalOperators[0];

            this.SearchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());
            this.RefreshCommand = new RelayCommand(() => this.Refresh());
            this.SelectPatientCommand = new RelayCommand(() => this.SelectPatient(), () => this.CanSelectPatient());

            this.IsBusy = false;
        }

        #endregion Constructors

        #region Properties

        public DateTime BirthdateAfterDate
        {
            get { return this.birthdateAfterDate; }
            set
            {
                this.birthdateAfterDate = value;
                this.OnPropertyChanged(() => BirthdateAfterDate);
            }
        }

        public DateTime BirthdateBeforeDate
        {
            get { return this.birthdayBeforeDate; }
            set
            {
                this.birthdayBeforeDate = value;
                this.OnPropertyChanged(() => BirthdateBeforeDate);
            }
        }

        public string CityCriteria
        {
            get { return this.cityCriteria; }
            set
            {
                this.cityCriteria = value;
                this.OnPropertyChanged(() => CityCriteria);
            }
        }

        public ObservableCollection<LightPatientDto> FoundPatients
        {
            get;
            private set;
        }

        public DateTime InscriptionAfterDate
        {
            get { return this.inscriptionAfterDate; }
            set
            {
                this.inscriptionAfterDate = value;
                this.OnPropertyChanged(() => InscriptionAfterDate);
            }
        }

        public DateTime InscriptionBeforeDate
        {
            get { return this.inscriptionBeforeDate; }
            set
            {
                this.inscriptionBeforeDate = value;
                this.OnPropertyChanged(() => InscriptionBeforeDate);
            }
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

        public bool IsByBirthdate
        {
            get { return this.isByBirthdate; }
            set
            {
                this.isByBirthdate = value;
                this.OnPropertyChanged(() => IsByBirthdate);
            }
        }

        public bool IsByCity
        {
            get { return this.isByCity; }
            set
            {
                this.isByCity = value;
                this.OnPropertyChanged(() => IsByCity);
            }
        }

        public bool IsByInscription
        {
            get { return this.isByInscription; }
            set
            {
                this.isByInscription = value;
                this.OnPropertyChanged(() => IsByInscription);
            }
        }

        public bool IsByLastUpdate
        {
            get { return this.isByLastUpdate; }
            set
            {
                this.isByLastUpdate = value;
                this.OnPropertyChanged(() => IsByLastUpdate);
            }
        }

        public bool IsByName
        {
            get { return this.isByName; }
            set
            {
                this.isByName = value;
                this.OnPropertyChanged(() => IsByName);
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

        public bool IsByReason
        {
            get { return this.isByReason; }
            set
            {
                this.isByReason = value;
                this.OnPropertyChanged(() => IsByReason);
            }
        }

        public ObservableCollection<Tuple<Operator, string>> LogicalOperators
        {
            get;
            private set;
        }

        public string NameCriteria
        {
            get { return this.nameCriteria; }
            set
            {
                this.nameCriteria = value;
                this.OnPropertyChanged(() => NameCriteria);
            }
        }

        public ObservableCollection<ProfessionDto> Professions
        {
            get;
            private set;
        }

        public string ReasonCriteria
        {
            get { return this.reasonCriteria; }
            set
            {
                this.reasonCriteria = value;
                this.OnPropertyChanged(() => ReasonCriteria);
            }
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

        public Tuple<Operator, string> SelectedOperator
        {
            get { return this.selectedOperator; }
            set
            {
                this.selectedOperator = value;
                this.OnPropertyChanged(() => SelectedOperator);
            }
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

        public bool ShowBirthdate
        {
            get { return this.Settings.ShowBirthdate; }
            set
            {
                this.Settings.ShowBirthdate = value;
                this.OnPropertyChanged(() => ShowBirthdate);
            }
        }

        public bool ShowCity
        {
            get { return this.Settings.ShowCity; }
            set
            {
                this.Settings.ShowCity = value;
                this.OnPropertyChanged(() => ShowCity);
            }
        }

        public bool ShowInscriptionDate
        {
            get { return this.Settings.ShowInscription; }
            set
            {
                this.Settings.ShowInscription = value;
                this.OnPropertyChanged(() => ShowInscriptionDate);
            }
        }

        public bool ShowLastUpdate
        {
            get { return this.Settings.ShowLastUpdate; }
            set
            {
                this.Settings.ShowLastUpdate = value;
                this.OnPropertyChanged(() => ShowLastUpdate);
            }
        }

        public bool ShowProfession
        {
            get { return this.Settings.ShowProfession; }
            set
            {
                this.Settings.ShowProfession = value;
                this.OnPropertyChanged(() => ShowProfession);
            }
        }

        public bool ShowReason
        {
            get { return this.Settings.ShowReason; }
            set
            {
                this.Settings.ShowReason = value;
                this.OnPropertyChanged(() => ShowReason);
            }
        }

        public DateTime UpdateAfterDate
        {
            get { return this.updateAfterDate; }
            set
            {
                this.updateAfterDate = value;
                this.OnPropertyChanged(() => UpdateAfterDate);
            }
        }

        public DateTime UpdateBeforeDate
        {
            get { return this.updateBeforeDate; }
            set
            {
                this.updateBeforeDate = value;
                this.OnPropertyChanged(() => UpdateBeforeDate);
            }
        }

        #endregion Properties

        #region Methods

        private Specification<LightPatientDto> BuidOrExpression()
        {
            var builder = new ExpressionBuilder<LightPatientDto>();
            if (this.IsByName) { builder.Add(When.Patient.LastNameContains(this.NameCriteria)); }
            if (this.IsByProfession) { builder.Add(When.Patient.ProfessionIs(this.SelectedProfession)); }
            if (this.IsByBirthdate) { builder.Add(When.Patient.BirthdateIsBetween(this.BirthdateAfterDate, this.BirthdateBeforeDate)); }
            if (this.IsByLastUpdate) { builder.Add(When.Patient.LastUpdateIsBetween(this.UpdateAfterDate, this.UpdateBeforeDate)); }
            if (this.IsByInscription) { builder.Add(When.Patient.InscriptionIsBetween(this.InscriptionAfterDate, this.InscriptionBeforeDate)); }
            if (this.IsByCity) { builder.Add(When.Patient.CityContains(this.CityCriteria)); }
            if (this.IsByReason) { builder.Add(When.Patient.ReasonContains(this.ReasonCriteria)); }
            return (builder.CanBuild)
                ? builder.BuildOrExpression()
                : When.Patient.None();
        }

        private Specification<LightPatientDto> BuildAndExpression()
        {
            var builder = new ExpressionBuilder<LightPatientDto>();
            if (this.IsByName) { builder.Add(When.Patient.LastNameContains(this.NameCriteria)); }
            if (this.IsByProfession) { builder.Add(When.Patient.ProfessionIs(this.SelectedProfession)); }
            if (this.IsByBirthdate) { builder.Add(When.Patient.BirthdateIsBetween(this.BirthdateAfterDate, this.BirthdateBeforeDate)); }
            if (this.IsByLastUpdate) { builder.Add(When.Patient.LastUpdateIsBetween(this.UpdateAfterDate, this.UpdateBeforeDate)); }
            if (this.IsByInscription) { builder.Add(When.Patient.InscriptionIsBetween(this.InscriptionAfterDate, this.InscriptionBeforeDate)); }
            if (this.IsByCity) { builder.Add(When.Patient.CityContains(this.CityCriteria)); }
            if (this.IsByReason) { builder.Add(When.Patient.ReasonContains(this.ReasonCriteria)); }
            return (builder.CanBuild)
                ? builder.BuildAndExpression()
                : When.Patient.None();
        }

        private bool CanSearch()
        {
            //" " (white space) or String.Empty will disable the search on the name
            return true;
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
                var expression = (this.SelectedOperator.Item1 == Operator.And)
                    ? this.BuildAndExpression()
                    : this.BuidOrExpression();

                var context = TaskScheduler.FromCurrentSynchronizationContext();
                var task = Task.Factory
                    .StartNew<IList<LightPatientDto>>(() => this.SearchAsync(expression));
                task.ContinueWith(e => this.SearchCallback(e), context);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private IList<LightPatientDto> SearchAsync(Specification<LightPatientDto> expression)
        {
            this.IsBusy = true;
            return this.Component.GetPatientsByNameLight(expression);
        }

        private void SearchCallback(Task<IList<LightPatientDto>> e)
        {
            this.ExecuteIfTaskIsNotFaulted(e, () =>
            {
                this.FoundPatients.Refill(e.Result);
                this.IsBusy = false;
            });
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