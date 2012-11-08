namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using System;
    using System.Collections.ObjectModel;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class AddDoctorViewModel : BaseBoxViewModel<DoctorDto>
    {
        #region Constructors

        public AddDoctorViewModel()
        {
            this.Specialisations = new ObservableCollection<TagDto>();
            this.BoxItem = new DoctorDto();
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<TagDto> Specialisations
        {
            get;
            private set;
        }

        private TagDto selectedTag;
        public TagDto SelectedTag
        {
            get { return this.selectedTag; }
            set
            {
                this.selectedTag = value;
                this.OnPropertyChanged(() => SelectedTag);
            }
        }
        #endregion Properties

        #region Methods

        public void Refresh()
        {
            this.Specialisations.Refill(this.Component.GetTags(TagCategory.Doctor));
        }

        protected override void AddItem()
        {
            this.Component.Create(this.BoxItem);
        }

        #endregion Methods
    }
}