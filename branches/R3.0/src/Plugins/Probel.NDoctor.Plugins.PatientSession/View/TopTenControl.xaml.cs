using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Probel.NDoctor.Plugins.PatientSession.ViewModel;

namespace Probel.NDoctor.Plugins.PatientSession.View
{
    /// <summary>
    /// Interaction logic for TopTenControl.xaml
    /// </summary>
    public partial class TopTenControl : UserControl
    {
        public TopTenControl()
        {
            InitializeComponent();
            this.DataContext = new WorkbenchViewModel();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is WorkbenchViewModel)
            {
                (this.DataContext as WorkbenchViewModel).Refresh();
            }
        }
    }
}
