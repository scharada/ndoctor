namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using Probel.NDoctor.Domain.DTO.Objects;

    public class AddDoctorViewModel : BaseBoxViewModel<DoctorDto>
    {
        #region Constructors

        public AddDoctorViewModel()
        {
            this.BoxItem = new DoctorDto();
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