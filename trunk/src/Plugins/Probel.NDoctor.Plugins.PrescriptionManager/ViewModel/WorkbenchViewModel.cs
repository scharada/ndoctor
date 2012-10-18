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
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PrescriptionManager.Helpers;
    using Probel.NDoctor.Plugins.PrescriptionManager.Properties;
    using Probel.NDoctor.View.Core.Controls;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.Services.Messaging;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    internal class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private IPrescriptionComponent component = PluginContext.ComponentFactory.GetInstance<IPrescriptionComponent>();
        private DateTime endCriteria;
        private SearchService searcher;
        private PrescriptionDto selectedPrescription;
        private PrescriptionDocumentDto selectPrescriptionDocument;
        private DateTime startCriteria;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
            : base()
        {
            this.searcher = new SearchService(this.component);

            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPrescriptionComponent>();
            PluginContext.Host.NewPatientConnected += (sender, e) => this.FoundPrescriptions.Clear();

            this.RemovePrescriptionDocumentCommand = new RelayCommand(() => this.RemovePrescriptionDocument(), () => this.SelectedPrescriptionDocument != null);
            this.RemovePrescriptionCommand = new RelayCommand(() => this.RemovePrescription(), () => this.SelectedPrescription != null);
            this.EditPrescriptionCommand = new RelayCommand(() => this.EditPrescription(), () => this.SelectedPrescription != null);

            this.StartCriteria = DateTime.Today.AddMonths(-1);
            this.EndCriteria = DateTime.Today.AddMonths(1);

            this.FoundPrescriptions = new ObservableCollection<PrescriptionDocumentDto>();
            Notifyer.PrescriptionFound += (sender, e) =>
            {
                this.FoundPrescriptions.Refill(e.Data.Prescriptions);
                this.StartCriteria = e.Data.From;
                this.EndCriteria = e.Data.To;
            };
        }

        #endregion Constructors

        #region Properties

        public ICommand EditPrescriptionCommand
        {
            get;
            private set;
        }

        public DateTime EndCriteria
        {
            get { return this.endCriteria; }
            set
            {
                this.endCriteria = value;
                this.OnPropertyChanged(() => EndCriteria);
                this.OnPropertyChanged(() => PrescriptionHeader);
            }
        }

        public ObservableCollection<PrescriptionDocumentDto> FoundPrescriptions
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the prescription header. The binding is done when the <see cref="EndCriteria"/> is updated
        /// </summary>
        public string PrescriptionHeader
        {
            get
            {
                return Messages.Title_PrescriptionHeader.FormatWith(
                    this.StartCriteria.ToShortDateString()
                    , this.EndCriteria.ToShortDateString());
            }
        }

        public ICommand RemovePrescriptionCommand
        {
            get;
            private set;
        }

        public ICommand RemovePrescriptionDocumentCommand
        {
            get;
            private set;
        }

        public PrescriptionDto SelectedPrescription
        {
            get { return this.selectedPrescription; }
            set
            {
                this.selectedPrescription = value;
                this.OnPropertyChanged(() => SelectedPrescription);
            }
        }

        public PrescriptionDocumentDto SelectedPrescriptionDocument
        {
            get { return this.selectPrescriptionDocument; }
            set
            {
                this.selectPrescriptionDocument = value;
                this.OnPropertyChanged(() => SelectedPrescriptionDocument);
            }
        }

        public DateTime StartCriteria
        {
            get { return this.startCriteria; }
            set
            {
                this.startCriteria = value;
                // I update the  header 'FromTo' from the EndCriteria
                this.OnPropertyChanged(() => StartCriteria);
            }
        }

        #endregion Properties

        #region Methods

        private void EditPrescription()
        {
            try
            {
                var refObj = new ReferencedObject<string>(this.SelectedPrescription.Notes);
                InnerWindow.Show(BaseText.Edit, new EditionBox()
                {
                    Value = refObj,
                    ButtonName = BaseText.OK,
                    OkCommand = new RelayCommand(() =>
                    {
                        this.SelectedPrescription.Notes = refObj.Value;
                        this.component.Update(this.SelectedPrescription);

                        InnerWindow.Close();
                    }, () => this.SelectedPrescription != null),
                });
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemovePrescription()
        {
            try
            {
                var dr = MessageBox.Show(BaseText.Question_Delete, BaseText.Question, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dr == MessageBoxResult.No) return;

                this.component.Remove(this.SelectedPrescription);

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_PrescriptionDeleted);
                this.searcher.SearchPrescription(this.StartCriteria, this.EndCriteria);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemovePrescriptionDocument()
        {
            try
            {
                var dr = MessageBox.Show(BaseText.Question_Delete, BaseText.Question, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dr == MessageBoxResult.No) return;

                this.component.Remove(this.SelectedPrescriptionDocument);

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_PrescriptionDeleted);
                this.searcher.SearchPrescription(this.StartCriteria, this.EndCriteria);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        #endregion Methods
    }
}