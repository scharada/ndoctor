namespace Probel.NDoctor.Plugins.MedicalRecord.View
{
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.MedicalRecord.ViewModel;

    /// <summary>
    /// Interaction logic for AddFolderView.xaml
    /// </summary>
    public partial class AddFolderView : UserControl
    {
        #region Constructors

        public AddFolderView()
        {
            InitializeComponent();
            this.DataContext = new AddFolderViewModel();
        }

        #endregion Constructors
    }
}