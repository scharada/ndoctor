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

namespace Probel.NDoctor.Plugins.PatientOverview.Actions.Base
{
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

    internal abstract class DoctorAction : Action
    {
        #region Fields

        protected readonly LightDoctorDto Doctor;

        #endregion Fields

        #region Constructors

        protected DoctorAction(IPatientDataComponent component, LightPatientDto patient, LightDoctorDto doctor)
            : base(component, patient)
        {
            this.Doctor = doctor;
        }

        #endregion Constructors
    }
}