namespace Probel.NDoctor.Plugins.UserSession.View
{
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.UserSession.ViewModel;

    /// <summary>
    /// Interaction logic for UpdateUserControl.xaml
    /// </summary>
    public partial class UpdateUserControl : UserControl
    {
        #region Constructors

        public UpdateUserControl()
        {
            InitializeComponent();
            this.DataContext = new UpdateUserViewModel();
        }

        #endregion Constructors
    }
}