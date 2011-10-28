namespace Probel.NDoctor.Plugins.FamilyManager.ViewModel
{
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.ViewModel;

    public class MedicalCabinetViewModel : BaseViewModel
    {
        #region Fields

        private MedicalRecordCabinetDto medicalRecordCabinet;
        private MedicalRecordFolderDto selectedFolder;
        private MedicalRecordDto selectedRecord;

        #endregion Fields

        #region Properties

        public MedicalRecordCabinetDto MedicalRecordCabinet
        {
            get { return this.medicalRecordCabinet; }
            set
            {
                this.medicalRecordCabinet = value;
                this.OnPropertyChanged("MedicalRecordCabinet");
            }
        }

        public MedicalRecordFolderDto SelectedFolder
        {
            get { return this.selectedFolder; }
            set
            {
                this.selectedFolder = value;
                this.OnPropertyChanged("SelectedFolder");
            }
        }

        public MedicalRecordDto SelectedRecord
        {
            get { return this.selectedRecord; }
            set
            {
                this.selectedRecord = value;
                this.OnPropertyChanged("SelectedRecord");
            }
        }

        #endregion Properties
    }
}