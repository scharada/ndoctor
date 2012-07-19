namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class AddProfessionViewModel : BaseBoxViewModel<ProfessionDto>
    {
        #region Constructors

        public AddProfessionViewModel()
        {
            this.BoxItem = new ProfessionDto();
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