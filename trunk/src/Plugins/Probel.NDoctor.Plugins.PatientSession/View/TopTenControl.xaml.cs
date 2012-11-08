/*
    This file is part of NDoctor.

    NDoctor is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    NDoctor is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with NDoctor.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace Probel.NDoctor.Plugins.PatientSession.View
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Probel.NDoctor.Plugins.PatientSession.ViewModel;

    /// <summary>
    /// Interaction logic for TopTenControl.xaml
    /// </summary>
    public partial class TopTenControl : Window
    {
        #region Constructors

        public TopTenControl()
        {
            InitializeComponent();
            this.DataContext = new TopTenViewModel();
        }

        #endregion Constructors

        #region Methods

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(this.focused);
            if (this.DataContext is SearchPatientViewModel)
            {
                (this.DataContext as TopTenViewModel).Refresh();
            }
        }

        #endregion Methods
    }
}