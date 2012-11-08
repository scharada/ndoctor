#region Header

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

#endregion Header

namespace Probel.NDoctor.Plugins.PrescriptionManager.View
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Probel.NDoctor.Plugins.PrescriptionManager.ViewModel;

    /// <summary>
    /// Interaction logic for SearchDrugView.xaml
    /// </summary>
    public partial class SearchDrugView : Window
    {
        #region Constructors

        public SearchDrugView()
        {
            InitializeComponent();
            this.DataContext = new SearchDrugViewModel();
            this.rb_OnName.IsChecked = true;
        }

        #endregion Constructors

        #region Methods

        private void this_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(this.focused);
        }

        #endregion Methods
    }
}