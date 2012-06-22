namespace Probel.NDoctor.Plugins.MeetingManager.View
{
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.MeetingManager.ViewModel;

    /// <summary>
    /// Interaction logic for AddCategoryView.xaml
    /// </summary>
    public partial class AddCategoryView : UserControl
    {
        #region Constructors

        public AddCategoryView()
        {
            InitializeComponent();
            this.DataContext = new AddCategoryViewModel();
        }

        #endregion Constructors
    }
}