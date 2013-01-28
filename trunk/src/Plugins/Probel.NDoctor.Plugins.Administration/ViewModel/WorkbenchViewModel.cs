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
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Administration.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    internal class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private IAdministrationComponent component;
        private Tuple<string, TagCategory> selectedCategory;
        private DoctorDto selectedDoctor;
        private DrugDto selectedDrug;
        private InsuranceDto selectedInsurance;
        private PathologyDto selectedPathology;
        private PracticeDto selectedPractice;
        private ProfessionDto selectedProfession;
        private ReputationDto selectedReputation;
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
                this.component = PluginContext.ComponentFactory.GetInstance<IAdministrationComponent>();
                PluginContext.Host.UserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IAdministrationComponent>();
            }

            #region Instanciates collections
            this.Insurances = new ObservableCollection<InsuranceDto>();
            this.Practices = new ObservableCollection<PracticeDto>();
            this.Pathologies = new ObservableCollection<PathologyDto>();
            this.Drugs = new ObservableCollection<DrugDto>();
            this.Reputations = new ObservableCollection<ReputationDto>();
            this.Professions = new ObservableCollection<ProfessionDto>();
            this.Tags = new ObservableCollection<TagViewModel>();
            this.Doctors = new ObservableCollection<DoctorDto>();

            #endregion

            #region Edition commands
            this.EditInsuranceCommand = new RelayCommand(() => this.EditInsurance(), () => this.SelectedInsurance != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.EditProfessionCommand = new RelayCommand(() => EditProfession(), () => this.SelectedProfession != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.EditPracticeCommand = new RelayCommand(() => this.EditPractice(), () => this.SelectedPractice != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.EditPathologyCommand = new RelayCommand(() => this.EditPathology(), () => this.selectedPathology != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.EditDrugCommand = new RelayCommand(() => this.EditDrug(), () => this.SelectedDrug != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.EditReputationCommand = new RelayCommand(() => this.EditReputation(), () => this.SelectedReputation != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.EditTagCommand = new RelayCommand(() => this.EditTag(), () => this.SelectedTag != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.EditDoctorCommand = new RelayCommand(() => this.EditDoctor(), () => this.SelectedDoctor != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            #endregion

            #region Suppression commands
            this.RemoveInsuranceCommand = new RelayCommand(() => this.RemoveInsurance(), () => this.SelectedInsurance != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.RemovePracticeCommand = new RelayCommand(() => this.RemovePractice(), () => this.SelectedPractice != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.RemovePathologyCommand = new RelayCommand(() => this.RemovePathology(), () => this.SelectedPathology != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.RemoveDrugCommand = new RelayCommand(() => this.RemoveDrug(), () => this.SelectedDrug != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.RemoveProfessionCommand = new RelayCommand(() => this.RemoveProfession(), () => this.SelectedProfession != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.RemoveReputationCommand = new RelayCommand(() => this.RemoveReputation(), () => this.SelectedReputation != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.RemoveTagCommand = new RelayCommand(() => this.RemoveTag(), () => this.SelectedTag != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.RemoveDoctorCommand = new RelayCommand(() => this.RemoveDoctor(), () => this.SelectedDoctor != null && PluginContext.DoorKeeper.IsUserGranted(To.Write));
            #endregion
        }

        #endregion Constructors

        #region Properties

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
            get;
            private set;
        }

        public ICommand EditDrugCommand
        {
            get;
            private set;
        }

        public ICommand EditInsuranceCommand
        {
            get;
            private set;
        }

        public ICommand EditPathologyCommand
        {
            get;
            private set;
        }

        public ICommand EditPracticeCommand
        {
            get;
            private set;
        }

        public ICommand EditProfessionCommand
        {
            get;
            private set;
        }

        public ICommand EditReputationCommand
        {
            get;
            private set;
        }

        public ICommand EditTagCommand
        {
            get;
            private set;
        }

        public ObservableCollection<InsuranceDto> Insurances
        {
            get;
            private set;
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
            get;
            private set;
        }

        public ICommand RemoveDrugCommand
        {
            get;
            private set;
        }

        public ICommand RemoveInsuranceCommand
        {
            get;
            private set;
        }

        public ICommand RemovePathologyCommand
        {
            get;
            private set;
        }

        public ICommand RemovePracticeCommand
        {
            get;
            private set;
        }

        public ICommand RemoveProfessionCommand
        {
            get;
            private set;
        }

        public ICommand RemoveReputationCommand
        {
            get;
            private set;
        }

        public ICommand RemoveTagCommand
        {
            get;
            private set;
        }

        public ObservableCollection<ReputationDto> Reputations
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

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            var task = Task.Factory
                .StartNew<TaskArgs>(() => this.GetAllListAsync())
                .ContinueWith(e => this.GetAllListCallback(e), context);
        }

        private void EditDoctor()
        {
            ViewService.Manager.ShowDialog<AddDoctorViewModel>(vm =>
            {
                vm.IsTypeEnabled = false;
                vm.AddCommand = new RelayCommand(() =>
                {
                    try
                    {
                        this.component.Update(this.SelectedDoctor);
                        vm.Close();
                    }
                    catch (Exception ex) { this.Handle.Error(ex); }
                });
                vm.BoxItem = this.SelectedDoctor;
            });
        }

        private void EditDrug()
        {
            ViewService.Manager.ShowDialog<AddDrugViewModel>(vm =>
            {
                vm.IsTypeEnabled = false;
                vm.AddCommand = new RelayCommand(() =>
                {
                    try
                    {
                        this.component.Update(this.SelectedDrug);
                        vm.Close();
                    }
                    catch (Exception ex) { this.Handle.Error(ex); }
                });
                vm.BoxItem = this.SelectedDrug;
            });
        }

        private void EditInsurance()
        {
            ViewService.Manager.ShowDialog<AddInsuranceViewModel>(vm =>
            {
                vm.AddCommand = new RelayCommand(() =>
                {
                    try
                    {
                        this.component.Update(this.SelectedInsurance);
                        vm.Close();
                    }
                    catch (Exception ex) { this.Handle.Error(ex); }
                });
                vm.BoxItem = this.SelectedInsurance;
            });
        }

        private void EditPathology()
        {
            ViewService.Manager.ShowDialog<AddPathologyViewModel>(vm =>
            {
                vm.IsTypeEnabled = false;
                vm.AddCommand = new RelayCommand(() =>
                {
                    try
                    {
                        this.component.Update(this.SelectedPathology);
                        vm.Close();
                    }
                    catch (Exception ex) { this.Handle.Error(ex); }
                });
                vm.BoxItem = this.SelectedPathology;
            });
        }

        private void EditPractice()
        {
            ViewService.Manager.ShowDialog<AddPracticeViewModel>(vm =>
            {
                vm.AddCommand = new RelayCommand(() =>
                {
                    try
                    {
                        this.component.Update(this.SelectedPractice);
                        vm.Close();
                    }
                    catch (Exception ex) { this.Handle.Error(ex); }
                });
                vm.BoxItem = this.SelectedPractice;
            });
        }

        private void EditProfession()
        {
            ViewService.Manager.ShowDialog<AddProfessionViewModel>(vm =>
            {
                vm.AddCommand = new RelayCommand(() =>
                {
                    try
                    {
                        this.component.Update(this.SelectedProfession);
                        vm.Close();
                    }
                    catch (Exception ex) { this.Handle.Error(ex); }
                });
                vm.BoxItem = this.SelectedProfession;
            });
        }

        private void EditReputation()
        {
            ViewService.Manager.ShowDialog<AddReputationViewModel>(vm =>
            {
                vm.AddCommand = new RelayCommand(() =>
                {
                    try
                    {
                        this.component.Update(this.SelectedReputation);
                        vm.Close();
                    }
                    catch (Exception ex) { this.Handle.Error(ex); }
                });
                vm.BoxItem = this.SelectedReputation;
            });
        }

        private void EditTag()
        {
            ViewService.Manager.ShowDialog<EditTagViewModel>(vm => vm.BoxItem = this.SelectedTag);
        }

        private TaskArgs GetAllListAsync()
        {
            var args = new TaskArgs();
            args.Insurances = this.component.GetAllInsurances();
            args.Practices = this.component.GetAllPractices();
            args.Pathologies = this.component.GetAllPathologies();
            args.Tags = this.component.GetAllTags();
            args.Drugs = this.component.GetAllDrugs();
            args.Reputations = this.component.GetAllReputations();
            args.Professions = this.component.GetAllProfessions();
            args.Doctors = this.component.GetAllDoctors();
            return args;
        }

        private void GetAllListCallback(Task<TaskArgs> e)
        {
            this.ExecuteIfTaskIsNotFaulted(e, () =>
            {
                var context = e.Result;
                this.Insurances.Refill(context.Insurances);
                this.Practices.Refill(context.Practices);
                this.Pathologies.Refill(context.Pathologies);
                this.Drugs.Refill(context.Drugs);
                this.Reputations.Refill(context.Reputations);
                this.Tags.Refill(Mapper.Map<IList<TagDto>, IList<TagViewModel>>(context.Tags));
                this.Professions.Refill(context.Professions);
                this.Doctors.Refill(context.Doctors);
            });
        }

        private void RemoveDoctor()
        {
            try
            {
                if (this.component.CanRemove(this.SelectedDoctor))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        this.component.Remove(this.SelectedDoctor);
                        this.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemoveDrug()
        {
            try
            {
                if (this.component.CanRemove(this.SelectedDrug))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        this.component.Remove(this.SelectedDrug);
                        this.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemoveInsurance()
        {
            try
            {
                if (this.component.CanRemove(this.SelectedInsurance))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        component.Remove(this.selectedInsurance);
                        this.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemovePathology()
        {
            try
            {
                if (this.component.CanRemove(this.SelectedPathology))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        this.component.Remove(this.SelectedPathology);
                        this.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemovePractice()
        {
            try
            {
                if (this.component.CanRemove(this.SelectedPractice))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        this.component.Remove(this.SelectedPractice);
                        this.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemoveProfession()
        {
            try
            {
                if (this.component.CanRemove(this.SelectedProfession))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        this.component.Remove(this.SelectedProfession);
                        this.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemoveReputation()
        {
            try
            {
                if (this.component.CanRemove(this.SelectedReputation))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        this.component.Remove(this.SelectedReputation);
                        this.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }

            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemoveTag()
        {
            try
            {
                if (this.component.CanRemove(this.SelectedTag))
                {
                    if (this.UserAcceptedDeletion())
                    {
                        this.component.Remove(this.SelectedTag);
                        this.Refresh();
                    }
                }
                else { ViewService.MessageBox.Warning(Messages.Msg_CantDelete); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private bool UserAcceptedDeletion()
        {
            return ViewService.MessageBox.Question(Messages.Msg_AskDelete);
        }

        #endregion Methods

        #region Nested Types

        private struct TaskArgs
        {
            #region Properties

            public IList<DoctorDto> Doctors
            {
                get;
                set;
            }

            public IList<DrugDto> Drugs
            {
                get;
                set;
            }

            public IList<InsuranceDto> Insurances
            {
                get;
                set;
            }

            public IList<PathologyDto> Pathologies
            {
                get;
                set;
            }

            public IList<PracticeDto> Practices
            {
                get;
                set;
            }

            public IList<ProfessionDto> Professions
            {
                get;
                set;
            }

            public IList<ReputationDto> Reputations
            {
                get;
                set;
            }

            public IList<TagDto> Tags
            {
                get;
                set;
            }

            #endregion Properties
        }

        #endregion Nested Types
    }
}