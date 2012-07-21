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

namespace Probel.NDoctor.Plugins.PrescriptionManager.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PrescriptionManager.Helpers;
    using Probel.NDoctor.Plugins.PrescriptionManager.Properties;
    using Probel.NDoctor.Plugins.PrescriptionManager.View;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    internal class AddPrescriptionViewModel : BaseViewModel
    {
        #region Fields

        private readonly ViewService ViewService = new ViewService();

        private IPrescriptionComponent component = PluginContext.ComponentFactory.GetInstance<IPrescriptionComponent>();
        private DateTime creationDate;

        #endregion Fields

        #region Constructors

        public AddPrescriptionViewModel()
        {
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPrescriptionComponent>();
            Notifyer.DrugSelected += (sender, e) => this.AddDrug(e.Data);
            Notifyer.PrescriptionRemoving += (sender, e) => this.Remove(e.Data);

            this.SaveCommand = new RelayCommand(() => this.Save(), () => this.CanSave());
            this.SearchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());

            this.Prescriptions = new ObservableCollection<PrescriptionDto>();
            this.Tags = new ObservableCollection<TagDto>();
            this.CreationDate = DateTime.Today;
        }

        #endregion Constructors

        #region Properties

        public DateTime CreationDate
        {
            get { return this.creationDate; }
            set
            {
                this.creationDate = value;
                this.OnPropertyChanged(() => CreationDate);
            }
        }

        public ObservableCollection<PrescriptionDto> Prescriptions
        {
            get;
            private set;
        }

        public ICommand RemoveCommand
        {
            get;
            private set;
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand SearchCommand
        {
            get;
            private set;
        }

        public ObservableCollection<TagDto> Tags
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            try
            {
                var results = this.component.FindTags(TagCategory.Prescription);
                this.Tags.Refill(results);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        public void ResetPage()
        {
            this.CreationDate = DateTime.Today;
            this.Prescriptions.Clear();
        }

        private void AddDrug(DrugDto drug)
        {
            var prescription = new PrescriptionDto() { Drug = drug };
            this.Prescriptions.Add(prescription);
        }

        private bool CanSave()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write)
                && this.Prescriptions.Count > 0;
        }

        private bool CanSearch()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Read);
        }

        private bool HasEmptyPrescriptions()
        {
            foreach (var prescription in this.Prescriptions)
            {
                if (string.IsNullOrWhiteSpace(prescription.Notes)) return true;
            }
            return false;
        }

        private void Remove(PrescriptionDto prescription)
        {
            this.Prescriptions.Remove(prescription);
        }

        private void Save()
        {
            try
            {
                if (this.HasEmptyPrescriptions())
                {
                    var dr = MessageBox.Show(Messages.Msg_EmptyNotesForPrescriptions, BaseText.Question, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (dr == MessageBoxResult.No) return;
                }

                var document = new PrescriptionDocumentDto() { CreationDate = this.CreationDate };
                document.Prescriptions.AddRange(this.Prescriptions);

                this.component.Create(document, PluginContext.Host.SelectedPatient);

                this.ResetPage();
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private void Search()
        {
            var view = new SearchDrugView();
            this.ViewService.GetViewModel(view).Refresh();
            InnerWindow.Show(Messages.Btn_AddDrug, view);
        }

        #endregion Methods
    }
}