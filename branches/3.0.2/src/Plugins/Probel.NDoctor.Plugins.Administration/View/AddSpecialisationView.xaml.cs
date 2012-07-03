namespace Probel.NDoctor.Plugins.Administration.View
{
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.Administration.ViewModel;

    /// <summary>
    /// Interaction logic for AddSpecialisationView.xaml
    /// </summary>
    public partial class AddSpecialisationView : UserControl
    {
        #region Constructors

        public AddSpecialisationView()
        {
            InitializeComponent();
            this.DataContext = new AddSpecialisationViewModel();
        }

        #endregion Constructors
    }
}