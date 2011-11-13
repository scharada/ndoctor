namespace Probel.NDoctor.Plugins.UserSession.View
{
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