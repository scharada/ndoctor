namespace Probel.NDoctor.Plugins.PathologyManager.View
{
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.PathologyManager.ViewModel;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for AddPathologyCategoryView.xaml
    /// </summary>
    public partial class AddPathologyCategoryView : UserControl
    {
        #region Constructors

        public AddPathologyCategoryView()
        {
            InitializeComponent();
            this.DataContext = new AddPathologyCategoryViewModel();
        }

        #endregion Constructors

        private void this_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Keyboard.Focus(this.focused);
        }
    }
}