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
namespace Probel.NDoctor.Plugins.FamilyViewer.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Data;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.FamilyViewer.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private readonly ICommand loadRecordsCommand;
        private readonly ICommand removeRelationCommand;

        private IFamilyComponent component = PluginContext.ComponentFactory.GetInstance<IFamilyComponent>();
        private bool isBusy;
        private MedicalRecordCabinetDto medicalRecordCabinet;
        private MedicalRecordFolderDto selectedFolder;
        private LightPatientDto selectedMember;
        private MedicalRecordDto selectedRecord;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
        {
            PluginContext.Host.PatientConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IFamilyComponent>();

            this.FamilyMembers = new ObservableCollection<LightPatientDto>();

            this.loadRecordsCommand = new RelayArgCommand(e => this.LoadRecords(e as object[]), e => this.CanLoadRecords());
            this.removeRelationCommand = new RelayCommand(() => this.RemoveRelation(), () => this.CanRemoveRelation());
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<LightPatientDto> FamilyMembers
        {
            get;
            private set;
        }

        public ICollectionView FamilyMembersView
        {
            get
            {
                var icv = CollectionViewSource.GetDefaultView(this.FamilyMembers);
                icv.GroupDescriptions.Add(new PropertyGroupDescription("Segretator"));
                return icv;
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

        public ICommand LoadRecordsCommand
        {
            get { return this.loadRecordsCommand; }
        }

        public MedicalRecordCabinetDto MedicalRecordCabinet
        {
            get { return this.medicalRecordCabinet; }
            set
            {
                this.medicalRecordCabinet = value;
                this.OnPropertyChanged(() => MedicalRecordCabinet);
            }
        }

        public ICommand RemoveRelationCommand
        {
            get { return this.removeRelationCommand; }
        }

        public MedicalRecordFolderDto SelectedFolder
        {
            get { return this.selectedFolder; }
            set
            {
                this.selectedFolder = value;
                this.OnPropertyChanged(() => SelectedFolder);
            }
        }

        public LightPatientDto SelectedMember
        {
            get { return this.selectedMember; }
            set
            {
                this.selectedMember = value;
                this.OnPropertyChanged(() => SelectedMember);
            }
        }

        public MedicalRecordDto SelectedRecord
        {
            get { return this.selectedRecord; }
            set
            {
                this.selectedRecord = value;
                this.OnPropertyChanged(() => SelectedRecord);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Refreshes the whole data of this instance.
        /// </summary>
        public void Refresh()
        {
            try
            {
                var family = this.component.GetFamily(PluginContext.Host.SelectedPatient);
                this.FamilyMembers.Refill(family.ToPatients());
                this.SelectedFolder = null;
                this.SelectedRecord = null;
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private bool CanLoadRecords()
        {
            return PluginContext.Host.ConnectedUser != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Read);
        }

        private bool CanRemoveRelation()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private void LoadRecords(object[] addedItems)
        {
            try
            {
                if (addedItems != null && addedItems.Length > 0)
                {
                    this.MedicalRecordCabinet = this.component.GetMedicalRecordCabinet(addedItems[0] as LightPatientDto);
                    PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_MedicalRecordLoaded);

                    if (this.MedicalRecordCabinet != null && this.MedicalRecordCabinet.Folders.Count > 0)
                    {
                        this.SelectedFolder = this.MedicalRecordCabinet.Folders[0];
                    }
                    if (this.SelectedFolder != null && this.SelectedFolder.Records.Count > 0)
                    {
                        this.SelectedRecord = this.SelectedFolder.Records[0];
                    }
                }
                else { return; }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemoveRelation()
        {
            this.IsBusy = true;
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(e => RemoveRelationAsync(e as LightPatientDto), PluginContext.Host.SelectedPatient)
                        .ContinueWith(e => RemoveRelationCallback(e), context);
        }

        private void RemoveRelationAsync(LightPatientDto selectedPatient)
        {
            try
            {
                this.component.RemoveFamilyMember(this.SelectedMember, selectedPatient);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void RemoveRelationCallback(Task e)
        {
            this.ExecuteIfTaskIsNotFaulted(e, () =>
            {
                this.Refresh();
                this.IsBusy = false;
            });
        }

        #endregion Methods
    }
}