namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using System;
    using System.Collections.ObjectModel;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class AddDoctorViewModel : BaseBoxViewModel<DoctorDto>
    {
        #region Fields

        private TagDto selectedTag;

        #endregion Fields

        #region Constructors

        public AddDoctorViewModel()
        {
            this.Specialisations = new ObservableCollection<TagDto>();
            this.BoxItem = new DoctorDto();
            this.IsTypeEnabled = true;
        }

        #endregion Constructors

        #region Properties

        public TagDto SelectedTag
        {
            get { return this.selectedTag; }
            set
            {
                this.selectedTag = value;
                this.OnPropertyChanged(() => SelectedTag);
            }
        }

        public ObservableCollection<TagDto> Specialisations
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
                this.Specialisations.Refill(this.Component.GetTags(TagCategory.Doctor));
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        protected override void AddItem()
        {
            this.Component.Create(this.BoxItem);
        }

        #endregion Methods
        private bool isTypeEnabled ;
        public bool IsTypeEnabled
        {
            get { return this.isTypeEnabled; }
            set
            {
                this.isTypeEnabled = value;
                this.OnPropertyChanged(() => IsTypeEnabled);
            }
        }
    }
}