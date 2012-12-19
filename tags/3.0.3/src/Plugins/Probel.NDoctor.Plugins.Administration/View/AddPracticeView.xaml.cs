namespace Probel.NDoctor.Plugins.Administration.View
{
    using System.Windows;

    using Probel.NDoctor.Plugins.Administration.ViewModel;

    /// <summary>
    /// Interaction logic for AddPracticeView.xaml
    /// </summary>
    public partial class AddPracticeView : Window
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