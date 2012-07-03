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

    internal class ViewService
    {
        #region Methods

        public AddPrescriptionViewModel GetViewModel(AddPrescriptionView view)
        {
            return (AddPrescriptionViewModel)view.DataContext;
        }

        public AddDrugTypeViewModel GetViewModel(AddDrugTypeView view)
        {
            return (AddDrugTypeViewModel)view.DataContext;
        }

        public WorkbenchViewModel GetViewModel(WorkbenchView view)
        {
            return (WorkbenchViewModel)view.DataContext;
        }

        internal SearchDrugViewModel GetViewModel(SearchDrugView view)
        {
            return (SearchDrugViewModel)view.DataContext;
        }

        #endregion Methods
    }
}