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

namespace Probel.NDoctor.View.Core.View
{
    using System.Diagnostics;
    using System.Windows.Controls;
    using System.Windows.Documents;

    using Probel.NDoctor.View.Core.ViewModel;

    /// <summary>
    /// Interaction logic for AboutBoxView.xaml
    /// </summary>
    public partial class AboutBoxView : UserControl
    {
        #region Constructors

        public AboutBoxView()
        {
            InitializeComponent();
            this.DataContext = new AboutBoxViewModel();
        }

        #endregion Constructors

        #region Methods

        private void Hyperlink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var uri = ((Hyperlink)sender).NavigateUri.AbsoluteUri;
            Process.Start(uri);
        }

        #endregion Methods
    }
}