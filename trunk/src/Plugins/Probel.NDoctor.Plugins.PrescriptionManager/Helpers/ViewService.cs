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

namespace Probel.NDoctor.Plugins.PrescriptionManager.Helpers
{
    using Probel.NDoctor.Plugins.PrescriptionManager.View;
    using Probel.NDoctor.Plugins.PrescriptionManager.ViewModel;

    public class ViewService
    {
        #region Fields

        private static AddPrescriptionView addPrescriptionView;
        private static WorkbenchView workbenchView;

        #endregion Fields

        #region Properties

        public AddPrescriptionView AddPrescriptionView
        {
            get
            {
                if (addPrescriptionView == null) addPrescriptionView = new AddPrescriptionView();
                return addPrescriptionView;
            }
        }

        public AddPrescriptionViewModel AddPrescriptionViewModel
        {
            get
            {
                return (AddPrescriptionViewModel)this.AddPrescriptionView.DataContext;
            }
        }

        public WorkbenchView WorkbenchView
        {
            get
            {
                if (workbenchView == null) { workbenchView = new WorkbenchView(); }
                return workbenchView;
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
            this.CloseAddPrescriptionView();
        }

        private void CloseAddPrescriptionView()
        {
            addPrescriptionView = null;
        }

        private void CloseWorkbenchView()
        {
            workbenchView = null;
        }

        #endregion Methods
    }
}