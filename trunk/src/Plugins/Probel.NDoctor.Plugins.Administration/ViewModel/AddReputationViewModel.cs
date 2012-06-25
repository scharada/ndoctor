namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using Probel.NDoctor.Domain.DTO.Objects;

    public class AddReputationViewModel : BaseBoxViewModel<ReputationDto>
    {
        #region Constructors

        public AddReputationViewModel()
        {
            this.BoxItem = new ReputationDto();
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