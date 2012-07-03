namespace Probel.NDoctor.Plugins.Administration.View
{
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.Administration.ViewModel;

    /// <summary>
    /// Interaction logic for AddPracticeView.xaml
    /// </summary>
    public partial class AddPracticeView : UserControl
    {
        #region Constructors

        public AddPracticeView()
        {
            InitializeComponent();
            this.DataContext = new AddPracticeViewModel();
        }

        #endregion Constructors
    }
}