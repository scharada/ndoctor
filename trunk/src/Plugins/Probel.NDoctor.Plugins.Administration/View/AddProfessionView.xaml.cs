namespace Probel.NDoctor.Plugins.Administration.View
{
    using System.Windows;

    using Probel.NDoctor.Plugins.Administration.ViewModel;

    /// <summary>
    /// Interaction logic for AddProfessionView.xaml
    /// </summary>
    public partial class AddProfessionView : Window
    {
        #region Constructors

        public AddProfessionView()
        {
            InitializeComponent();
            this.DataContext = new AddProfessionViewModel();
        }

        #endregion Constructors
    }
}