namespace Probel.NDoctor.Plugins.FamilyManager.View
{
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.FamilyManager.ViewModel;

    /// <summary>
    /// Interaction logic for ManageFamilyWorkbench.xaml
    /// </summary>
    public partial class AddFamilyWorkbench : UserControl
    {
        #region Constructors

        public AddFamilyWorkbench()
        {
            InitializeComponent();
            this.DataContext = new AddFamilyViewModel();
        }

        #endregion Constructors
    }
}