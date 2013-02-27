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
namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Administration.Commands;
    using Probel.NDoctor.Plugins.Administration.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    internal class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private const double INTERVAL = 250;

        private static readonly System.Timers.Timer DoctorCountdown = new System.Timers.Timer(INTERVAL) { AutoReset = true };
        private static readonly System.Timers.Timer DrugCountdown = new System.Timers.Timer(INTERVAL) { AutoReset = true };
        private static readonly System.Timers.Timer InsuranceCountdown = new System.Timers.Timer(INTERVAL) { AutoReset = true };
        private static readonly System.Timers.Timer PathologyCountdown = new System.Timers.Timer(INTERVAL) { AutoReset = true };
        private static readonly System.Timers.Timer PracticeCountdown = new System.Timers.Timer(INTERVAL) { AutoReset = true };
        private static readonly System.Timers.Timer ProfessionCountdown = new System.Timers.Timer(INTERVAL) { AutoReset = true };
        private static readonly System.Timers.Timer ReputationCountdown = new System.Timers.Timer(INTERVAL) { AutoReset = true };
        private static readonly System.Timers.Timer TagCountdown = new System.Timers.Timer(INTERVAL) { AutoReset = true };
        private static readonly System.Timers.Timer SearchTagCountdown = new System.Timers.Timer(INTERVAL) { AutoReset = true };

        private readonly EditCommands Edit;
        private readonly WorkbenchRefresher Refresher;
        private readonly RemoveCommands Remove;

        private string criteriaDoctor;
        private string criteriaDrug;
        private string criteriaInsurance;
        private string criteriaPathology;
        private string criteriaPractice;
        private string criteriaProfession;
        private string criteriaReputation;
        private string criteriaSearchTag;
        private string criteriaTag;
        private bool isDoctorBusy;
        private bool isDrugBusy;
        private bool isInsuranceBusy;
        private bool isPathologyBusy;
        private bool isPracticeBusy;
        private bool isProfessionBusy;
        private bool isReputationBusy;
        private bool isSearchTypeBusy;
        private bool isTagBusy;
        private Tuple<string, TagCategory> selectedCategory;
        private DoctorDto selectedDoctor;
        private DrugDto selectedDrug;
        private InsuranceDto selectedInsurance;
        private PathologyDto selectedPathology;
        private PracticeDto selectedPractice;
        private ProfessionDto selectedProfession;
        private ReputationDto selectedReputation;
        private SearchTagDto selectedSearchTag;
        private TagDto selectedTag;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
        {
            if (!Designer.IsDesignMode)
            {
                this.Remove = new RemoveCommands(this, this.Handle);
                this.Edit = new EditCommands(this, this.Handle);

                ConfigureTimers();
                PluginContext.Host.UserConnected += (sender, e) =>
                {
                    this.Refresher.Component
                        = this.Remove.Component
                        = this.Edit.Component
                        = PluginContext.ComponentFactory.GetInstance<IAdministrationComponent>();
                };

                var component = PluginContext.ComponentFactory.GetInstance<IAdministrationComponent>();
                this.Refresher = new WorkbenchRefresher(this, component, this.Handle);
                this.Refresher.Component
                    = this.Remove.Component
                    = this.Edit.Component
                    = component;

            }
            #region Instanciate collections
            this.Insurances = new ObservableCollection<InsuranceDto>();
            this.Practices = new ObservableCollection<PracticeDto>();
            this.Pathologies = new ObservableCollection<PathologyDto>();
            this.Drugs = new ObservableCollection<DrugDto>();
            this.Reputations = new ObservableCollection<ReputationDto>();
            this.Professions = new ObservableCollection<ProfessionDto>();
            this.Tags = new ObservableCollection<TagViewModel>();
            this.Doctors = new ObservableCollection<DoctorDto>();
            this.SearchTags = new ObservableCollection<SearchTagDto>();
            #endregion
        }

        #endregion Constructors

        #region Properties

        public string CriteriaDoctor
        {
            get { return this.criteriaDoctor; }
            set
            {
                this.criteriaDoctor = value;
                this.OnPropertyChanged(() => CriteriaDoctor);
                DoctorCountdown.Start();
            }
        }

        public string CriteriaDrug
        {
            get { return this.criteriaDrug; }
            set
            {
                this.criteriaDrug = value;
                this.OnPropertyChanged(() => CriteriaDrug);
                DrugCountdown.Start();
            }
        }

        public string CriteriaInsurance
        {
            get { return this.criteriaInsurance; }
            set
            {
                this.criteriaInsurance = value;
                this.OnPropertyChanged(() => CriteriaInsurance);
                InsuranceCountdown.Start();
            }
        }

        public string CriteriaPathology
        {
            get { return this.criteriaPathology; }
            set
            {
                this.criteriaPathology = value;
                this.OnPropertyChanged(() => CriteriaPathology);
                PathologyCountdown.Start();
            }
        }

        public string CriteriaPractice
        {
            get { return this.criteriaPractice; }
            set
            {
                this.criteriaPractice = value;
                this.OnPropertyChanged(() => CriteriaPractice);
                PracticeCountdown.Start();
            }
        }

        public string CriteriaProfession
        {
            get { return this.criteriaProfession; }
            set
            {
                this.criteriaProfession = value;
                this.OnPropertyChanged(() => CriteriaProfession);
                ProfessionCountdown.Start();
            }
        }

        public string CriteriaReputation
        {
            get { return this.criteriaReputation; }
            set
            {
                this.criteriaReputation = value;
                this.OnPropertyChanged(() => CriteriaReputation);
                ReputationCountdown.Start();
            }
        }

        public string CriteriaSearchTag
        {
            get { return this.criteriaSearchTag; }
            set
            {
                this.criteriaSearchTag = value;
                this.OnPropertyChanged(() => CriteriaSearchTag);
                SearchTagCountdown.Start();
            }
        }

        public string CriteriaTag
        {
            get { return this.criteriaTag; }
            set
            {
                this.criteriaTag = value;
                this.OnPropertyChanged(() => CriteriaTag);
                TagCountdown.Start();
            }
        }

        public ObservableCollection<DoctorDto> Doctors
        {
            get;
            private set;
        }

        public ObservableCollection<DrugDto> Drugs
        {
            get;
            private set;
        }

        public ICommand EditDoctorCommand
        {
            get
            {
                return this.Edit.DoctorCommand;
            }
        }

        public ICommand EditDrugCommand
        {
            get
            {
                return this.Edit.DrugCommand;
            }
        }

        public ICommand EditInsuranceCommand
        {
            get
            {
                return this.Edit.InsuranceCommand;
            }
        }

        public ICommand EditPathologyCommand
        {
            get
            {
                return this.Edit.PathologyCommand;
            }
        }

        public ICommand EditPracticeCommand
        {
            get
            {
                return this.Edit.PracticeCommand;
            }
        }

        public ICommand EditProfessionCommand
        {
            get
            {
                return this.Edit.ProfessionCommand;
            }
        }

        public ICommand EditReputationCommand
        {
            get
            {
                return this.Edit.ReputationCommand;
            }
        }

        public ICommand EditSearchTagCommand
        {
            get { return Edit.SearchTagCommand; }
        }

        public ICommand EditTagCommand
        {
            get
            {
                return this.Edit.TagCommand;
            }
        }

        public ObservableCollection<InsuranceDto> Insurances
        {
            get;
            private set;
        }

        public bool IsDoctorBusy
        {
            get { return this.isDoctorBusy; }
            set
            {
                this.isDoctorBusy = value;
                this.OnPropertyChanged(() => IsDoctorBusy);
            }
        }

        public bool IsDrugBusy
        {
            get { return this.isDrugBusy; }
            set
            {
                this.isDrugBusy = value;
                this.OnPropertyChanged(() => IsDrugBusy);
            }
        }

        public bool IsInsuranceBusy
        {
            get { return this.isInsuranceBusy; }
            set
            {
                this.isInsuranceBusy = value;
                this.OnPropertyChanged(() => IsInsuranceBusy);
            }
        }

        public bool IsPathologyBusy
        {
            get { return this.isPathologyBusy; }
            set
            {
                this.isPathologyBusy = value;
                this.OnPropertyChanged(() => IsPathologyBusy);
            }
        }

        public bool IsPracticeBusy
        {
            get { return this.isPracticeBusy; }
            set
            {
                this.isPracticeBusy = value;
                this.OnPropertyChanged(() => IsPracticeBusy);
            }
        }

        public bool IsProfessionBusy
        {
            get { return this.isProfessionBusy; }
            set
            {
                this.isProfessionBusy = value;
                this.OnPropertyChanged(() => IsProfessionBusy);
            }
        }

        public bool IsReputationBusy
        {
            get { return this.isReputationBusy; }
            set
            {
                this.isReputationBusy = value;
                this.OnPropertyChanged(() => IsReputationBusy);
            }
        }

        public bool IsSearchTagBusy
        {
            get { return this.isSearchTypeBusy; }
            set
            {
                this.isSearchTypeBusy = value;
                this.OnPropertyChanged(() => IsSearchTagBusy);
            }
        }

        public bool IsTagBusy
        {
            get { return this.isTagBusy; }
            set
            {
                this.isTagBusy = value;
                this.OnPropertyChanged(() => IsTagBusy);
            }
        }

        public ObservableCollection<PathologyDto> Pathologies
        {
            get;
            private set;
        }

        public ObservableCollection<PracticeDto> Practices
        {
            get;
            private set;
        }

        public ObservableCollection<ProfessionDto> Professions
        {
            get;
            private set;
        }

        public ICommand RemoveDoctorCommand
        {
            get
            {
                return this.Remove.DoctorCommand;
            }
        }

        public ICommand RemoveDrugCommand
        {
            get
            {
                return this.Remove.DrugCommand;
            }
        }

        public ICommand RemoveInsuranceCommand
        {
            get
            {
                return this.Remove.InsuranceCommand;
            }
        }

        public ICommand RemovePathologyCommand
        {
            get
            {
                return this.Remove.PathologyCommand;
            }
        }

        public ICommand RemovePracticeCommand
        {
            get
            {
                return this.Remove.PracticeCommand;
            }
        }

        public ICommand RemoveProfessionCommand
        {
            get
            {
                return this.Remove.ProfessionCommand;
            }
        }

        public ICommand RemoveReputationCommand
        {
            get
            {
                return this.Remove.ReputationCommand;
            }
        }

        public ICommand RemoveSearchTagCommand
        {
            get { return this.Remove.SearchTagCommand; }
        }

        public ICommand RemoveTagCommand
        {
            get
            {
                return this.Remove.TagCommand;
            }
        }

        public ObservableCollection<ReputationDto> Reputations
        {
            get;
            private set;
        }

        public ObservableCollection<SearchTagDto> SearchTags
        {
            get;
            private set;
        }

        public Tuple<string, TagCategory> SelectedCategory
        {
            get { return this.selectedCategory; }
            set
            {
                this.selectedCategory = value;
                this.OnPropertyChanged(() => SelectedCategory);
            }
        }

        public DoctorDto SelectedDoctor
        {
            get { return this.selectedDoctor; }
            set
            {
                this.selectedDoctor = value;
                this.OnPropertyChanged(() => SelectedDoctor);
            }
        }

        public DrugDto SelectedDrug
        {
            get { return this.selectedDrug; }
            set
            {
                this.selectedDrug = value;
                this.OnPropertyChanged(() => SelectedDrug);
            }
        }

        public InsuranceDto SelectedInsurance
        {
            get { return this.selectedInsurance; }
            set
            {
                this.selectedInsurance = value;
                this.OnPropertyChanged(() => SelectedInsurance);
            }
        }

        public PathologyDto SelectedPathology
        {
            get { return this.selectedPathology; }
            set
            {
                this.selectedPathology = value;
                this.OnPropertyChanged(() => SelectedPathology);
            }
        }

        public PracticeDto SelectedPractice
        {
            get { return this.selectedPractice; }
            set
            {
                this.selectedPractice = value;
                this.OnPropertyChanged(() => SelectedPractice);
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

        public ReputationDto SelectedReputation
        {
            get { return this.selectedReputation; }
            set
            {
                this.selectedReputation = value;
                this.OnPropertyChanged(() => SelectedReputation);
            }
        }

        public SearchTagDto SelectedSearchTag
        {
            get { return this.selectedSearchTag; }
            set
            {
                this.selectedSearchTag = value;
                this.OnPropertyChanged(() => SelectedSearchTag);
            }
        }

        public TagDto SelectedTag
        {
            get { return this.selectedTag; }
            set
            {
                this.selectedTag = value;
                this.OnPropertyChanged(() => SelectedTag);
            }
        }

        public ObservableCollection<TagViewModel> Tags
        {
            get;
            private set;
        }

        internal TaskScheduler Scheduler
        {
            get
            {
                return TaskScheduler.FromCurrentSynchronizationContext();
            }
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            this.CriteriaDoctor = string.Empty;
            this.Refresher.Refresh();
        }

        private void ConfigureTimers()
        {
            DoctorCountdown.Elapsed += (sender, e) =>
            {
                this.Refresher.RefreshDoctorInMemory(this.CriteriaDoctor);
                DoctorCountdown.Stop();
            };
            DrugCountdown.Elapsed += (sender, e) =>
            {
                this.Refresher.RefreshDrugInMemory(this.CriteriaDrug);
                DrugCountdown.Stop();
            };
            InsuranceCountdown.Elapsed += (sender, e) =>
            {
                this.Refresher.RefreshInsurancesInMemory(this.CriteriaInsurance);
                InsuranceCountdown.Stop();
            };
            PathologyCountdown.Elapsed += (sender, e) =>
            {
                this.Refresher.RefreshPathologyInMemory(this.CriteriaPathology);
                PathologyCountdown.Stop();
            };
            PracticeCountdown.Elapsed += (sender, e) =>
            {
                this.Refresher.RefreshPracticeInMemory(this.CriteriaPractice);
                PracticeCountdown.Stop();
            };
            ProfessionCountdown.Elapsed += (sender, e) =>
            {
                this.Refresher.RefreshProfessionInMemory(this.CriteriaProfession);
                ProfessionCountdown.Stop();
            };
            ReputationCountdown.Elapsed += (sender, e) =>
            {
                this.Refresher.RefreshReputationInMemory(this.CriteriaReputation);
                ReputationCountdown.Stop();
            };
            TagCountdown.Elapsed += (sender, e) =>
            {
                this.Refresher.RefreshTagInMemory(this.CriteriaTag);
                TagCountdown.Stop();
            };
            SearchTagCountdown.Elapsed += (sender, e) =>
            {
                this.Refresher.RefreshSearchTagInMemory(this.CriteriaSearchTag);
                TagCountdown.Stop();
            };
        }

        #endregion Methods
    }
}