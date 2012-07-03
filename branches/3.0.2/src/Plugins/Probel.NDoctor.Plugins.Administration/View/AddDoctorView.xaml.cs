namespace Probel.NDoctor.Plugins.Administration.View
{
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.Administration.ViewModel;

    /// <summary>
    /// Interaction logic for AddDoctorView.xaml
    /// </summary>
    public partial class AddDoctorView : UserControl
    {
        #region Constructors

        public AddDoctorView()
        {
            InitializeComponent();
            this.DataContext = new AddDoctorViewModel();
        }

        #endregion Constructors
    }
}