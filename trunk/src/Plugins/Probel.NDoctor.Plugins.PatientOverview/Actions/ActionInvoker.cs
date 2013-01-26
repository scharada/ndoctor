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

namespace Probel.NDoctor.Plugins.PatientOverview.Actions
{
    using System.Collections.Generic;

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientOverview.Actions.Base;

    internal class ActionInvoker : IAction
    {
        #region Fields

        private readonly List<IAction> Actions = new List<IAction>();

        #endregion Fields

        #region Methods

        public void AddThumbnail(IPatientDataComponent component, LightPatientDto patient, byte[] thumbnail)
        {
            this.Add(new AddThumbnailAction(component, patient, thumbnail));
        }

        public void Bind(IPatientDataComponent component, LightPatientDto patient, LightDoctorDto doctor)
        {
            this.Add(new AddDoctorAction(component, patient, doctor));
        }

        public void Execute()
        {
            foreach (var action in this.Actions)
            {
                action.Execute();
            }
            this.Actions.Clear();
        }

        public void Unbind(IPatientDataComponent component, LightPatientDto patient, LightDoctorDto doctor)
        {
            this.Add(new RemoveDoctorAction(component, patient, doctor));
        }

        private void Add(IAction action)
        {
            this.Actions.Add(action);
        }

        #endregion Methods
    }
}