namespace Probel.NDoctor.Plugins.PatientSession.View
{
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.PatientSession.ViewModel;

    /// <summary>
    /// Interaction logic for SearchPatientControl.xaml
    /// </summary>
    public partial class SearchPatientControl : UserControl
    {
        #region Constructors

        public SearchPatientControl()
        {
            InitializeComponent();
            this.DataContext = new SearchPatientViewModel();
        }

        #endregion Constructors
    }
}