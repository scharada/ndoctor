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

namespace Probel.NDoctor.Plugins.MedicalRecord.Helpers
{
    using Probel.NDoctor.Plugins.MedicalRecord.View;
    using Probel.NDoctor.Plugins.MedicalRecord.ViewModel;

    public class ViewService
    {
        #region Fields

        private static AddFolderView addFolderView;
        private static AddRecordView addRecordView;
        private static WorkbenchView workbenchView;

        #endregion Fields

        #region Properties

        public AddFolderView AddFolderView
        {
            get
            {
                if (addFolderView == null) { addFolderView = new AddFolderView(); }
                return addFolderView;
            }
        }

        public AddFolderViewModel AddFolderViewModel
        {
            get { return (AddFolderViewModel)AddFolderView.DataContext; }
        }

        public AddRecordView AddRecordView
        {
            get
            {
                if (addRecordView == null) { addRecordView = new AddRecordView(); }
                return addRecordView;
            }
        }

        public AddRecordViewModel AddRecordViewModel
        {
            get { return (AddRecordViewModel)AddRecordView.DataContext; }
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
            get { return (WorkbenchViewModel)WorkbenchView.DataContext; }
        }

        #endregion Properties

        #region Methods

        public void CloseAll()
        {
            this.CloseAddFolderView();
            this.CloseAddRecordView();
            this.CloseWorkbenchView();
        }

        private void CloseAddFolderView()
        {
            addFolderView = null;
        }

        private void CloseAddRecordView()
        {
            addRecordView = null;
        }

        private void CloseWorkbenchView()
        {
            workbenchView = null;
        }

        #endregion Methods
    }
}