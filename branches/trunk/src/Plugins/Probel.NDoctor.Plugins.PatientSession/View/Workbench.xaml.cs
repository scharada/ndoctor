namespace Probel.NDoctor.Plugins.PatientSession.View
{
    using System.Windows;
    using System.Windows.Controls;

    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientSession.ViewModel;

    /// <summary>
    /// Interaction logic for Workbench.xaml
    /// </summary>
    public partial class Workbench : Page
    {
        #region Constructors

        public Workbench()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is WorkbenchViewModel)
            {
                (this.DataContext as WorkbenchViewModel).Refresh();
            }
        }

        #endregion Methods
    }
}