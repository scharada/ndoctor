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
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;

    using Probel.NDoctor.Plugins.PatientSession.ViewModel;

    /// <summary>
    /// Interaction logic for SearchPatientControl.xaml
    /// </summary>
    public partial class SearchPatientControl : UserControl
    {
        #region Constructors

        public SearchPatientControl()
        {
            InitializeComponent();
            this.DataContext = new SearchPatientViewModel();
        }

        #endregion Constructors

        #region Methods

        private void this_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Keyboard.Focus(this.focused);
        }

        #endregion Methods
    }
}