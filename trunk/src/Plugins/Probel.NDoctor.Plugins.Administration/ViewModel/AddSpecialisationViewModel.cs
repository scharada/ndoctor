namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class AddSpecialisationViewModel : BaseBoxViewModel<TagDto>
    {
        #region Constructors

        public AddSpecialisationViewModel()
        {
            this.BoxItem = new TagDto(TagCategory.Doctor);
        }

        #endregion Constructors

        #region Methods

        protected override void AddItem()
        {
            this.Component.Create(this.BoxItem);
        }

        #endregion Methods
    }
}