namespace Probel.NDoctor.Plugins.MedicalRecord.View
{
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.MedicalRecord.ViewModel;

    /// <summary>
    /// Interaction logic for AddRecordView.xaml
    /// </summary>
    public partial class AddRecordView : UserControl
    {
        #region Constructors

        public AddRecordView()
        {
            InitializeComponent();
            this.DataContext = new AddRecordViewModel();
        }

        #endregion Constructors
    }
}