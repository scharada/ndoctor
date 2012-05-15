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
namespace Probel.NDoctor.Plugins.PrescriptionManager.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Conversions;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PrescriptionManager.Helpers;
    using Probel.NDoctor.Plugins.PrescriptionManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    public class AddPrescriptionViewModel : BaseViewModel
    {
        #region Fields

        private IPrescriptionComponent component = ObjectFactory.GetInstance<IPrescriptionComponent>();
        private string criteria;
        private PrescriptionDto currentPrescription;
        private PrescriptionDocumentDto prescriptionDocumentToCreate;
        private bool searchOnTags;
        private DrugViewModel selectedDrug;
        private PrescriptionDto selectedPrescriptionToDelete;
        private TagDto selectedTag;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddPrescriptionViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public AddPrescriptionViewModel()
            : base()
        {
            this.Tags = new ObservableCollection<TagDto>();
            this.FoundDrugs = new ObservableCollection<DrugViewModel>();
            this.SearchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());
            this.PrescriptionDocumentToCreate = new PrescriptionDocumentDto() { CreationDate = DateTime.Today };

            this.SelectDrugCommand = new RelayCommand(() => this.SelectDrug());

            Notifyer.ItemChanged += (sender, e) => this.Refresh();
        }

        #endregion Constructors

        #region Properties

        public string Criteria
        {
            get { return this.criteria; }
            set
            {
                this.criteria = value;
                this.OnPropertyChanged(() => Criteria);
            }
        }

        public PrescriptionDto CurrentPrescription
        {
            get { return this.currentPrescription; }
            set
            {
                this.currentPrescription = value;
                this.OnPropertyChanged(() => CurrentPrescription);
            }
        }

        public ObservableCollection<DrugViewModel> FoundDrugs
        {
            get;
            private set;
        }

        public PrescriptionDocumentDto PrescriptionDocumentToCreate
        {
            get { return this.prescriptionDocumentToCreate; }
            set
            {
                this.prescriptionDocumentToCreate = value;
                this.OnPropertyChanged(() => PrescriptionDocumentToCreate);
            }
        }

        public ICommand SearchCommand
        {
            get;
            private set;
        }

        public bool SearchOnTags
        {
            get { return this.searchOnTags; }
            set
            {
                this.searchOnTags = value;
                this.OnPropertyChanged(() => SearchOnTags);
            }
        }

        public ICommand SelectDrugCommand
        {
            get;
            private set;
        }

        public DrugViewModel SelectedDrug
        {
            get { return this.selectedDrug; }
            set
            {
                this.selectedDrug = value;
                this.OnPropertyChanged(() => SelectedDrug);
            }
        }

        public PrescriptionDto SelectedPrescriptionToDelete
        {
            get { return this.selectedPrescriptionToDelete; }
            set
            {
                this.selectedPrescriptionToDelete = value;
                this.OnPropertyChanged(() => SelectedPrescriptionToDelete);
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

        public ICommand SelectPrescriptionCommand
        {
            get;
            private set;
        }

        public ObservableCollection<TagDto> Tags
        {
            get;
            private set;
        }

        public string TitlePrescriptionDocumentCreated
        {
            get { return Messages.Title_PrescriptionDocumentCreated; }
        }

        private bool IsDocumentInvalid
        {
            get
            {
                foreach (var prescription in this.PrescriptionDocumentToCreate.Prescriptions)
                {
                    if (string.IsNullOrWhiteSpace(prescription.Notes))
                    {
                        var dr = MessageBox.Show(Messages.Msg_EmptyNotesForPrescriptions
                            , Messages.Question
                            , MessageBoxButton.YesNo
                            , MessageBoxImage.Asterisk);

                        return (dr == MessageBoxResult.No);
                    }
                }
                return false;
            }
        }

        private bool IsPrescriptionEmpty
        {
            get
            {
                if (this.prescriptionDocumentToCreate.Prescriptions.Count == 0)
                {
                    MessageBox.Show(Messages.Msg_NothingToSave
                        , Messages.Warning
                        , MessageBoxButton.OK
                        , MessageBoxImage.Exclamation);
                    return true;
                }
                return false;
            }
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            using (this.component.UnitOfWork)
            {
                var tags = this.component.FindTags(TagCategory.Drug);
                this.Tags.Refill(tags);
            }
            PluginContext.Host.WriteStatusReady();
        }

        public void Save()
        {
            try
            {
                if (this.IsPrescriptionEmpty) return;
                if (this.IsDocumentInvalid) return;

                using (this.component.UnitOfWork)
                {

                    this.component.Create(this.PrescriptionDocumentToCreate, PluginContext.Host.SelectedPatient);
                }
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_PrescriptionSaved);
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_ErrorSavingPrescription);
            }
        }

        private bool CanSearch()
        {
            if (this.SearchOnTags)
            {
                return this.SelectedTag != null;
            }
            else
            {
                return !(string.IsNullOrWhiteSpace(this.Criteria));
            }
        }

        private void Search()
        {
            IList<DrugDto> drugs;
            using (this.component.UnitOfWork)
            {
                if (this.SearchOnTags) drugs = this.component.FindDrugsByTags(this.SelectedTag.Name);
                else drugs = this.component.FindDrugsByName(this.Criteria);
            }
            var mapped = Mapper.Map<IList<DrugDto>, IList<DrugViewModel>>(drugs);
            for (int i = 0; i < mapped.Count; i++) { mapped[i].Parent = this; }
            this.FoundDrugs.Refill(mapped);
        }

        private void SelectDrug()
        {
            this.PrescriptionDocumentToCreate.Prescriptions.Add(new PrescriptionDto()
            {
                Drug = this.SelectedDrug,
            });
        }

        #endregion Methods
    }
}