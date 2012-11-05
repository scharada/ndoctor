namespace Probel.NDoctor.Plugins.Administration.View
{
    using System.Windows;

    using Probel.NDoctor.Plugins.Administration.ViewModel;

    /// <summary>
    /// Interaction logic for AddSpecialisationView.xaml
    /// </summary>
    public partial class AddSpecialisationView : Window
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