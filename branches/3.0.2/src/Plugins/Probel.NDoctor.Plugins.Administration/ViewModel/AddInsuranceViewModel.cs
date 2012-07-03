namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class AddInsuranceViewModel : BaseBoxViewModel<InsuranceDto>
    {
        #region Constructors

        public AddInsuranceViewModel()
        {
            this.BoxItem = new InsuranceDto();
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