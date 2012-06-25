namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using Probel.NDoctor.Domain.DTO.Objects;

    public class AddProfessionViewModel : BaseBoxViewModel<ProfessionDto>
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
            using (this.Component.UnitOfWork)
            {
                this.Component.Create(this.BoxItem);
            }
        }

        #endregion Methods
    }
}