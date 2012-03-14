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
        private TagDto selectedTag;
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
                    this.recordToAdd.Tag = this.SelectedTag;
                    this.component.Create(this.recordToAdd, PluginContext.Host.SelectedPatient);
                }
                InnerWindow.Close();
                Notifyer.OnRefreshed();
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_RecordAdded);
            }, () => this.selectedTag != null);
        }

        #endregion Constructors

        #region Properties

        public ICommand AddRecordCommand
        {
            get { return this.addRecordCommand; }
        }

        public TagDto SelectedTag
        {
            get { return this.selectedTag; }
            set
            {
                this.selectedTag = value;
                this.OnPropertyChanged("SelectedTag");
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

        public string Title
        {
            get { return this.recordToAdd.Name; }
            set
            {
                this.recordToAdd.Name = value;
                this.OnPropertyChanged("Title");
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