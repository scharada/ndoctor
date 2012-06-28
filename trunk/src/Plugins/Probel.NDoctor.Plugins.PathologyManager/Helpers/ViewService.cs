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

namespace Probel.NDoctor.Plugins.PathologyManager.Helpers
{
    using Probel.NDoctor.Plugins.PathologyManager.View;
    using Probel.NDoctor.Plugins.PathologyManager.ViewModel;

    public class ViewService
    {
        #region Fields

        private static WorkbenchView workbench;

        #endregion Fields

        #region Properties

        public WorkbenchView WorkbenchView
        {
            get
            {
                if (workbench == null) { workbench = new WorkbenchView(); }
                return workbench;
            }
        }

        public WorkbenchViewModel WorkbenchViewModel
        {
            get
            {
                return (WorkbenchViewModel)this.WorkbenchView.DataContext;
            }
        }

        #endregion Properties

        #region Methods

        public void CloseAll()
        {
            this.CloseWorkbenchView();
        }

        private void CloseWorkbenchView()
        {
            workbench = null;
        }

        #endregion Methods
    }
}