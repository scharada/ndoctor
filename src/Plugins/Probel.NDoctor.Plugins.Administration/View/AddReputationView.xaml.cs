namespace Probel.NDoctor.Plugins.Administration.View
{
    using System.Windows;
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.Administration.ViewModel;

    /// <summary>
    /// Interaction logic for AddReputationView.xaml
    /// </summary>
    public partial class AddReputationView : Window
    {
        #region Constructors

        public AddReputationView()
        {
            InitializeComponent();
            this.DataContext = new AddReputationViewModel();
        }

        #endregion Constructors
    }
}