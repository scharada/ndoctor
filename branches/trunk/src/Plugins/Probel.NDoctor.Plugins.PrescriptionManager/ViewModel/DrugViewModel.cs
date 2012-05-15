namespace Probel.NDoctor.Plugins.PrescriptionManager.ViewModel
{
    using System.Windows.Input;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PrescriptionManager.Properties;

    public class DrugViewModel : DrugDto
    {
        #region Fields

        private bool isSelected;

        #endregion Fields

        #region Properties

        public string BtnSelect
        {
            get { return Messages.Btn_Select; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected in the ListView.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

        public AddPrescriptionViewModel Parent
        {
            get; set;
        }

        public ICommand SelectDrugCommand
        {
            get
            {
                Assert.IsNotNull(this.Parent, "The parent of {0} should be set.", this.GetType());
                return this.Parent.SelectDrugCommand;
            }
        }

        #endregion Properties
    }
}