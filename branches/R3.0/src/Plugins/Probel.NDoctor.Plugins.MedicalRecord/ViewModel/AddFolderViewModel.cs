namespace Probel.NDoctor.Plugins.MedicalRecord.ViewModel
{
    using System.Windows.Input;

    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Helpers;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    public class AddFolderViewModel : BaseViewModel
    {
        #region Fields

        private IMedicalRecordComponent component = ObjectFactory.GetInstance<IMedicalRecordComponent>();
        private TagDto tagToAdd;

        #endregion Fields

        #region Constructors

        public AddFolderViewModel()
            : base()
        {
            this.tagToAdd = new TagDto(TagCategory.MedicalRecord);
            this.AddFolderCommand = new RelayCommand(() =>
            {
                using (this.component.UnitOfWork)
                {
                    this.component.Create(this.tagToAdd);
                }
                InnerWindow.Close();
                this.Host.WriteStatus(StatusType.Info, Messages.Msg_TagAdded.StringFormat(this.tagToAdd.Name));

                Notifyer.OnRefreshed();

            }, () => !string.IsNullOrWhiteSpace(this.tagToAdd.Name));
        }

        #endregion Constructors

        #region Properties

        public ICommand AddFolderCommand
        {
            get; private set;
        }

        public TagDto TagToAdd
        {
            get { return this.tagToAdd; }
            set
            {
                this.tagToAdd = value;
                this.OnPropertyChanged("TagToAdd");
            }
        }

        #endregion Properties
    }
}