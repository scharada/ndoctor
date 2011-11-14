namespace Probel.NDoctor.Plugins.FamilyManager.View
{
    using System.Windows.Controls;
    using Probel.NDoctor.Plugins.FamilyManager.ViewModel;

    /// <summary>
    /// Interaction logic for RemoveFamilyWorkbench.xaml
    /// </summary>
    public partial class RemoveFamilyWorkbench : UserControl
    {
        #region Constructors

        public RemoveFamilyWorkbench()
        {
            InitializeComponent();
            var dataContext = new RemoveFamilyViewModel();
            dataContext.Refresh();
            this.DataContext = dataContext;
        }

        #endregion Constructors
    }
}