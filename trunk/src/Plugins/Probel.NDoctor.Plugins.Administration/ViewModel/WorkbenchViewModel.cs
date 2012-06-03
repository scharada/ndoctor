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
    using System.Windows;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Conversions;
    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Administration.Helpers;
    using Probel.NDoctor.Plugins.Administration.Properties;
    using Probel.NDoctor.View.Core.Controls;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.Domain.DTO.Collections;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private IAdministrationComponent component;
        private Tuple<string, TagCategory> selectedCategory;
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
                PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IAdministrationComponent>();
            }

            Notifyer.ItemChanged += (sender, e) => this.Refresh();

            this.Insurances = new ObservableCollection<InsuranceDto>();
            this.Practices = new ObservableCollection<PracticeDto>();
            this.Pathologies = new ObservableCollection<PathologyDto>();
            this.Drugs = new ObservableCollection<DrugDto>();
            this.Reputations = new ObservableCollection<ReputationDto>();
            this.Professions = new ObservableCollection<ProfessionDto>();
            this.Tags = new ObservableCollection<TagViewModel>();

            #region Edition commands
            this.EditInsuranceCommand = new RelayCommand(() => this.EditInsurance(), () => this.SelectedInsurance != null);
            this.EditProfessionCommand = new RelayCommand(() => EditProfession(), () => this.SelectedProfession != null);
            this.EditPracticeCommand = new RelayCommand(() => this.EditPractice(), () => this.SelectedPractice != null);
            this.EditPathologyCommand = new RelayCommand(() => this.EditPathology(), () => this.selectedPathology != null);
            this.EditDrugCommand = new RelayCommand(() => this.EditDrug(), () => this.SelectedDrug != null);
            this.EditReputationCommand = new RelayCommand(() => this.EditReputation(), () => this.SelectedReputation != null);
            this.EditTagCommand = new RelayCommand(() => this.EditTag(), () => this.SelectedTag != null);
            #endregion

            #region Suppression commands
            this.RemoveInsuranceCommand = new RelayCommand(() => this.RemoveInsurance(), () => this.SelectedInsurance != null);
            this.RemovePracticeCommand = new RelayCommand(() => this.RemovePractice(), () => this.SelectedPractice != null);
            this.RemovePathlogyCommand = new RelayCommand(() => this.RemovePathlogy(), () => this.SelectedPathology != null);

            #endregion

            Notifyer.ItemChanged += (sender, e) => this.Refresh();
        }
        #endregion Constructors

        #region Properties

        public ObservableCollection<DrugDto> Drugs
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

        public ICommand RemoveInsuranceCommand
        {
            get;
            private set;
        }

        public ICommand RemovePathlogyCommand
        {
            get;
            private set;
        }

        public ICommand RemovePracticeCommand
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

        public void RemovePathlogy()
        {
            try
            {
                using (this.component.UnitOfWork)
                {
                    if (this.component.CanRemove(this.SelectedPathology))
                    {
                        this.component.Remove(this.SelectedPathology);
                    }
                    else { MessageBox.Show(Messages.Msg_CantDelete, BaseText.Warning, MessageBoxButton.OK, MessageBoxImage.Hand); }
                }
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private void EditDrug()
        {
            IList<TagDto> categories;
            using (this.component.UnitOfWork) { categories = this.component.FindTags(TagCategory.Drug); }

            InnerWindow.Show(Messages.Title_Edit, new DrugBox()
            {
                ButtonName = Messages.Btn_Apply,
                Drug = this.SelectedDrug,
                Categories = new ObservableCollection<TagDto>(categories),
                OkCommand = new RelayCommand(() =>
                {
                    try
                    {
                        using (this.component.UnitOfWork)
                        {
                            this.component.Update(this.SelectedDrug);
                        }
                        InnerWindow.Close();
                        Notifyer.OnItemChanged(this);
                    }
                    catch (Exception ex) { this.HandleError(ex); }
                }),
            });
        }

        private void EditInsurance()
        {
            InnerWindow.Show(Messages.Title_Edit, new InsuranceBox()
            {
                ButtonName = Messages.Btn_Apply,
                Insurance = this.SelectedInsurance,
                OkCommand = new RelayCommand(() =>
                {
                    try
                    {
                        using (this.component.UnitOfWork)
                        {
                            this.component.Update(this.SelectedInsurance);
                        }
                        InnerWindow.Close();
                        Notifyer.OnItemChanged(this);
                    }
                    catch (Exception ex) { this.HandleError(ex); }
                })
            });
        }

        private void EditPathology()
        {
            IList<TagDto> categories = new List<TagDto>();
            using (this.component.UnitOfWork) { categories = this.component.FindTags(TagCategory.Pathology); }

            InnerWindow.Show(Messages.Title_Edit, new PathologyBox()
            {
                ButtonName = Messages.Btn_Apply,
                Pathology = this.SelectedPathology,
                Tags = new ObservableCollection<TagDto>(categories),
                OkCommand = new RelayCommand(() =>
                {
                    try
                    {
                        using (this.component.UnitOfWork)
                        {
                            this.component.Update(this.SelectedPathology);
                        }
                        InnerWindow.Close();
                        Notifyer.OnItemChanged(this);
                    }
                    catch (Exception ex) { this.HandleError(ex); }
                }),
            });
        }

        private void EditPractice()
        {
            InnerWindow.Show(Messages.Title_Edit, new PracticeBox()
            {
                ButtonName = Messages.Btn_Apply,
                Profession = this.SelectedPractice,
                OkCommand = new RelayCommand(() =>
                {
                    try
                    {
                        using (this.component.UnitOfWork)
                        {
                            this.component.Update(this.SelectedPractice);
                        }
                        InnerWindow.Close();
                        Notifyer.OnItemChanged(this);
                    }
                    catch (Exception ex) { this.HandleError(ex); }
                }),
            });
        }

        private void EditProfession()
        {
            InnerWindow.Show(Messages.Title_Edit, new ProfessionBox()
            {
                ButtonName = Messages.Btn_Apply,
                Profession = this.SelectedProfession,
                OkCommand = new RelayCommand(() =>
                {
                    try
                    {
                        using (this.component.UnitOfWork)
                        {
                            this.component.Update(this.SelectedProfession);
                        }
                        InnerWindow.Close();
                        Notifyer.OnItemChanged(this);
                    }
                    catch (Exception ex) { this.HandleError(ex); }
                }),
            });
        }

        private void EditReputation()
        {
            InnerWindow.Show(Messages.Title_Edit, new ReputationBox()
            {
                ButtonName = Messages.Btn_Apply,
                Reputation = this.SelectedReputation,
                OkCommand = new RelayCommand(() =>
                {
                    try
                    {
                        using (this.component.UnitOfWork)
                        {
                            this.component.Update(this.SelectedReputation);
                        }
                        InnerWindow.Close();
                        Notifyer.OnItemChanged(this);
                    }
                    catch (Exception ex) { this.HandleError(ex); }
                }),
            });
        }

        private void EditTag()
        {
            var categories = TagCategoryCollection.Build();

            InnerWindow.Show(Messages.Title_Edit, new TagBox()
            {
                ButtonName = Messages.Btn_Apply,
                Stamp = this.SelectedTag,
                OkCommand = new RelayCommand(() =>
                {
                    try
                    {
                        using (this.component.UnitOfWork)
                        {
                            this.component.Update(this.SelectedTag);
                        }
                        InnerWindow.Close();
                        Notifyer.OnItemChanged(this);
                    }
                    catch (Exception ex) { this.HandleError(ex); }
                }),
            });
        }

        public void Refresh()
        {
            IList<InsuranceDto> insurances = null;
            IList<PracticeDto> practices = null;
            IList<PathologyDto> pathologies = null;
            IList<TagDto> tags = null;
            IList<DrugDto> drugs = null;
            IList<ReputationDto> reputations = null;
            IList<ProfessionDto> professions = null;

            using (this.component.UnitOfWork)
            {
                insurances = this.component.GetAllInsurances();
                practices = this.component.GetAllPractices();
                pathologies = this.component.GetAllPathologies();
                tags = this.component.GetAllTags();
                drugs = this.component.GetAllDrugs();
                reputations = this.component.GetAllReputations();
                professions = this.component.GetAllProfessions();
            }

            this.Insurances.Refill(insurances);
            this.Practices.Refill(practices);
            this.Pathologies.Refill(pathologies);
            this.Drugs.Refill(drugs);
            this.Reputations.Refill(reputations);
            this.Tags.Refill(Mapper.Map<IList<TagDto>, IList<TagViewModel>>(tags));
            this.Professions.Refill(professions);
        }

        private void RemoveInsurance()
        {
            try
            {
                using (this.component.UnitOfWork)
                {
                    if (this.component.CanRemove(this.selectedInsurance))
                    {
                        component.Remove(this.selectedInsurance);
                    }
                    else { MessageBox.Show(Messages.Msg_CantDelete, BaseText.Warning, MessageBoxButton.OK, MessageBoxImage.Hand); }
                }
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private void RemovePractice()
        {
            try
            {
                using (this.component.UnitOfWork)
                {
                    if (this.component.CanRemove(this.SelectedPractice))
                    {
                        this.component.Remove(this.SelectedPractice);
                    }
                    else { MessageBox.Show(Messages.Msg_CantDelete, BaseText.Warning, MessageBoxButton.OK, MessageBoxImage.Hand); }
                }
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        #endregion Methods
    }
}