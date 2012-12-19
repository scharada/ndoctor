namespace Probel.NDoctor.Plugins.PathologyManager.View
{
    using System.Windows;
    using System.Windows.Input;

    using Probel.NDoctor.Plugins.PathologyManager.ViewModel;

    /// <summary>
    /// Interaction logic for AddPathologyCategoryView.xaml
    /// </summary>
    public partial class AddPathologyCategoryView : Window
    {
        #region Constructors

        public AddPathologyCategoryView()
        {
            InitializeComponent();
            this.DataContext = new AddPathologyCategoryViewModel();
        }

        #endregion Constructors

        #region Methods

        private void this_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Keyboard.Focus(this.focused);
        }

        #endregion Methods
    }
}