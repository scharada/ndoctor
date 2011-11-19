namespace Probel.NDoctor.Plugins.MedicalRecord.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Probel.Helpers.Conversions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Helpers;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    public class AddRecordViewModel : BaseViewModel
    {
        #region Fields

        private ICommand addRecordCommand;
        private IMedicalRecordComponent component = ObjectFactory.GetInstance<IMedicalRecordComponent>();
        private MedicalRecordDto recordToAdd;
        private ObservableCollection<TagDto> tags = new ObservableCollection<TagDto>();

        #endregion Fields

        #region Constructors

        public AddRecordViewModel()
        {
            this.Refresh();
            this.recordToAdd = new MedicalRecordDto();
            this.addRecordCommand = new RelayCommand(() =>
            {
                using (this.component.UnitOfWork)
                {
                    this.component.Create(this.RecordToAdd, this.Host.SelectedPatient);
                }
                InnerWindow.Close();
                Notifyer.OnRefreshed();
                this.Host.WriteStatus(StatusType.Info, Messages.Msg_RecordAdded);
            }, () => this.RecordToAdd.Tag != null);
        }

        #endregion Constructors

        #region Properties

        public ICommand AddRecordCommand
        {
            get { return this.addRecordCommand; }
        }

        public MedicalRecordDto RecordToAdd
        {
            get { return this.recordToAdd; }
            set
            {
                this.recordToAdd = value;
                this.OnPropertyChanged("RecordToAdd");
            }
        }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public ObservableCollection<TagDto> Tags
        {
            get { return this.tags; }
            set
            {
                this.tags = value;
                this.OnPropertyChanged("Tags");
            }
        }

        #endregion Properties

        #region Methods

        private void Refresh()
        {
            using (this.component.UnitOfWork)
            {
                this.Tags.Refill(this.component.FindTags(TagCategory.MedicalRecord));
            }
        }

        #endregion Methods
    }
}