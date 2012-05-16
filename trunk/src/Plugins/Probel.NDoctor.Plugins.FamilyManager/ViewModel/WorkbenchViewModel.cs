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
namespace Probel.NDoctor.Plugins.FamilyManager.ViewModel
{
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.Domain.Components;



    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private FamilyDto family;
        private IFamilyComponent familyComponent = new ComponentFactory().GetInstance<IFamilyComponent>();
        private MedicalRecordCabinetDto medicalRecordCabinet = new MedicalRecordCabinetDto();
        private IMedicalRecordComponent medicalRecordComponent = new ComponentFactory().GetInstance<IMedicalRecordComponent>();
        private MedicalRecordFolderDto selectedFolder;
        private LightPatientDto selectedMember = new LightPatientDto();
        private MedicalRecordDto selectedRecord;

        #endregion Fields

        #region Constructors

        public WorkbenchViewModel()
            : base()
        {
            this.MemberSelectedCommand = new RelayCommand(() => this.LoadCabinet());

            Notifyer.Refreshed += (sender, e) => this.Refresh();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the family of the selected patient.
        /// </summary>
        /// <value>
        /// The family.
        /// </value>
        public FamilyDto Family
        {
            get { return this.family; }
            set
            {
                this.family = value;
                this.OnPropertyChanged(() => Family);
            }
        }

        public bool HasFather
        {
            get
            {
                if (this.Family == null) return false;
                else return (this.Family.Fathers != null
                    && this.Family.Fathers.Count > 0);
            }
        }

        public bool HasMother
        {
            get
            {
                if (this.Family == null) return false;
                else return (this.Family.Mothers != null
                    && this.Family.Mothers.Count > 0);
            }
        }

        public MedicalRecordCabinetDto MedicalRecordCabinet
        {
            get { return this.medicalRecordCabinet; }
            private set
            {
                this.medicalRecordCabinet = value;

                if (value.Folders.Length > 0)
                {
                    this.SelectedFolder = value.Folders[0];
                    if (value.Folders[0].Records.Length > 0)
                    {
                        this.SelectedRecord = value.Folders[0].Records[0];
                    }
                }

                this.OnPropertyChanged(() => MedicalRecordCabinet);
            }
        }

        public ICommand MemberSelectedCommand
        {
            get;
            private set;
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

        public void Refresh()
        {
            using (this.familyComponent.UnitOfWork)
            {
                this.Family = this.familyComponent.FindFamily(PluginContext.Host.SelectedPatient);
            }

            this.Logger.Debug("Load family");
        }

        /// <summary>
        /// Reset the frame to empty. That's set the selected member to null.
        /// </summary>
        public void Reset()
        {
            this.SelectedMember = null;
        }

        private void LoadCabinet()
        {
            if (this.SelectedMember != null)
            {
                using (this.medicalRecordComponent.UnitOfWork)
                {
                    this.MedicalRecordCabinet = this.medicalRecordComponent.GetMedicalRecordCabinet(this.SelectedMember);
                }
                this.Logger.Debug("Load medical records cabinet");
            }
            else { this.Logger.Warn("Impossible to load the medical record cabinet because the selected member is null"); }
        }

        #endregion Methods
    }
}