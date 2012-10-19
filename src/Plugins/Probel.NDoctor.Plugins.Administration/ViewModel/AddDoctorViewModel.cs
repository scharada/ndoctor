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

            try
            {
                var result = this.Component.GetTags(TagCategory.Doctor);

                this.Specialisations.Refill(result);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<TagDto> Specialisations
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        protected override void AddItem()
        {
            this.Component.Create(this.BoxItem);
        }

        #endregion Methods
    }
}