namespace Probel.NDoctor.Plugins.PathologyManager.View
{
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.PathologyManager.ViewModel;

    /// <summary>
    /// Interaction logic for AddIllnessPeriodView.xaml
    /// </summary>
    public partial class AddIllnessPeriodView : UserControl
    {
        #region Constructors

        public AddIllnessPeriodView()
        {
            InitializeComponent();
            this.DataContext = new IllnessPeriodListViewModel();
        }

        #endregion Constructors
    }
}