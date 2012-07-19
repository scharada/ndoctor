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
    using System;

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PrescriptionManager.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class SearchService
    {
        #region Fields

        private IPrescriptionComponent component;

        #endregion Fields

        #region Constructors

        public SearchService(IPrescriptionComponent component)
        {
            this.component = component;
        }

        #endregion Constructors

        #region Methods

        public void SearchPrescription(DateTime from, DateTime to)
        {
            var found = this.component.FindPrescriptionsByDates(PluginContext.Host.SelectedPatient, from, to);
            Notifyer.OnPrescriptionFound(this, new PrescriptionResultDto(found, from, to));
            InnerWindow.Close();

            if (found.Count > 0) PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_FoundPrescription);
            else PluginContext.Host.WriteStatus(StatusType.Warning, Messages.Msg_NothingFound);
        }

        #endregion Methods
    }
}