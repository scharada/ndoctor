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

namespace Probel.NDoctor.Plugins.PatientSession.ViewModel
{
    using System.Collections.ObjectModel;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class TopTenViewModel : SearchPatientViewModel
    {
        #region Constructors

        public TopTenViewModel()
        {
            this.Top10Patients = new ObservableCollection<LightPatientDto>();
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<LightPatientDto> Top10Patients
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            var result = this.component.GetTopXPatient(10);
            this.Top10Patients.Refill(result);
        }

        #endregion Methods
    }
}