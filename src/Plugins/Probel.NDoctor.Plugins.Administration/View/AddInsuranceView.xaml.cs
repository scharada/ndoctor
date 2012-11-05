namespace Probel.NDoctor.Plugins.Administration.View
{
    using System.Windows;
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.Administration.ViewModel;

    /// <summary>
    /// Interaction logic for AddInsuranceView.xaml
    /// </summary>
    public partial class AddInsuranceView : Window
    {
        #region Constructors

        public AddInsuranceView()
        {
            InitializeComponent();
            this.DataContext = new AddInsuranceViewModel();
        }

        #endregion Constructors
    }
}