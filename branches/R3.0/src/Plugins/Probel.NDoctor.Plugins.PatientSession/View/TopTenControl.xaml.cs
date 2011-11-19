namespace Probel.NDoctor.Plugins.PatientSession.View
{
    using System.Windows;
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.PatientSession.ViewModel;

    /// <summary>
    /// Interaction logic for TopTenControl.xaml
    /// </summary>
    public partial class TopTenControl : UserControl
    {
        #region Constructors

        public TopTenControl()
        {
            InitializeComponent();
            this.DataContext = new SearchPatientViewModel();
        }

        #endregion Constructors

        #region Methods

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is SearchPatientViewModel)
            {
                (this.DataContext as SearchPatientViewModel).Refresh();
            }
        }

        #endregion Methods
    }
}