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

namespace Probel.NDoctor.Plugins.PatientSession.View
{
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.PatientSession.ViewModel;

    /// <summary>
    /// Interaction logic for SearchPatientExtendedControl.xaml
    /// </summary>
    public partial class SearchPatientExtendedControl : UserControl
    {
        #region Constructors

        public SearchPatientExtendedControl()
        {
            InitializeComponent();
            this.DataContext = new SearchPatientExtendedViewModel();
        }

        #endregion Constructors
    }
}