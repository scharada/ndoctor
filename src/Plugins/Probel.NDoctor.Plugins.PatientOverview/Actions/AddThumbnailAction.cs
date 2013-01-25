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
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientOverview.Actions.Base;

    internal class AddThumbnailAction : Action
    {
        #region Fields

        private readonly byte[] Thumbnail;

        #endregion Fields

        #region Constructors

        public AddThumbnailAction(IPatientDataComponent component, LightPatientDto patient, byte[] thumbnail)
            : base(component, patient)
        {
            this.Thumbnail = thumbnail;
        }

        #endregion Constructors

        #region Methods

        public override void Execute()
        {
            if (this.Thumbnail != null && this.Thumbnail.Length > 0)
            {
                this.Component.UpdateThumbnail(this.Patient, this.Thumbnail);
            }
            else { this.Logger.Warn("No thumbnail was updated because the specified byte array is null or contains 0 element"); }
        }

        #endregion Methods
    }
}