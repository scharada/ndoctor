namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class AddPracticeViewModel : BaseBoxViewModel<PracticeDto>
    {
        #region Constructors

        public AddPracticeViewModel()
        {
            this.BoxItem = new PracticeDto();
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