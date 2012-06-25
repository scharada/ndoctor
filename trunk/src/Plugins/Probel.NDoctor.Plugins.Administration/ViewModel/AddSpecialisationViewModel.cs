namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using Probel.NDoctor.Domain.DTO.Objects;

    public class AddSpecialisationViewModel : BaseBoxViewModel<TagDto>
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
            using (this.Component.UnitOfWork)
            {
                this.Component.Create(this.BoxItem);
            }
        }

        #endregion Methods
    }
}