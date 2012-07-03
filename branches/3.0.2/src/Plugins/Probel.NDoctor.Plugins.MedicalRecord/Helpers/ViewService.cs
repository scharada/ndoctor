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
        #region Methods

        public AddFolderViewModel GetViewModel(AddFolderView view)
        {
            return (AddFolderViewModel)view.DataContext;
        }

        public AddRecordViewModel GetViewModel(AddRecordView view)
        {
            return (AddRecordViewModel)view.DataContext;
        }

        public WorkbenchViewModel GetViewModel(WorkbenchView view)
        {
            return (WorkbenchViewModel)view.DataContext;
        }

        #endregion Methods
    }
}