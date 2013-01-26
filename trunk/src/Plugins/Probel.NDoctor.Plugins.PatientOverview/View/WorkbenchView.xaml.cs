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
namespace Probel.NDoctor.Plugins.PatientOverview.View
{
    using System.Windows.Controls;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Plugins.PatientOverview.ViewModel;
    using Probel.NDoctor.View.Plugins;

    /// <summary>
    /// Interaction logic for Workbench.xaml
    /// </summary>
    public partial class WorkbenchView : Page, ILeaveCheckable
    {
        #region Constructors

        public WorkbenchView()
        {
            InitializeComponent();
            this.DataContext = new WorkbenchViewModel();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Ask to the user to confirm he/she want to navigate out of the current page.
        /// </summary>
        /// <returns></returns>
        public bool AskToLeave()
        {
            return this.As<WorkbenchViewModel>().AskToLeave();
        }

        /// <summary>
        /// Determines whether navigate out of this page is possible without data loss
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can navigate; otherwise, <c>false</c>.
        /// </returns>
        public bool CanLeave()
        {
            return this.As<WorkbenchViewModel>().CanLeave();
        }

        #endregion Methods
    }
}