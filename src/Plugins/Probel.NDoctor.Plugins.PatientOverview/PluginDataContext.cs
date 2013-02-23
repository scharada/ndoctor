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

namespace Probel.NDoctor.Plugins.PatientOverview
{
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientOverview.Actions;
    using System;
    using Probel.Helpers.Events;

    internal class PluginDataContext
    {
        #region Fields

        public static readonly PluginDataContext Instance = new PluginDataContext();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="PluginDataContext"/> class from being created.
        /// </summary>
        private PluginDataContext()
        {
            this.Actions = new ActionInvoker();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the selected patient with all the pending changes.
        /// </summary>
        /// <value>
        /// The selected patient.
        /// </value>
        public ActionInvoker Actions
        {
            get;
            set;
        }

        #endregion Properties
        /// <summary>
        /// Occurs when a new doctor is binded to the connected patient.
        /// </summary>
        public event EventHandler<EventArgs<LightDoctorDto>> DoctorUnbinded;
        /// <summary>
        /// Called when a new doctor is binded to the connected patient.
        /// </summary>
        /// <param name="doctor">The doctor.</param>
        public void OnDoctorUnbinded(LightDoctorDto doctor)
        {
            if (this.DoctorUnbinded != null)
            {
                this.DoctorUnbinded(this, new EventArgs<LightDoctorDto>(doctor));
            }
        }

        /// <summary>
        /// Occurs when a new doctor is binded to the connected patient.
        /// </summary>
        public event EventHandler<EventArgs<LightDoctorDto>> DoctorBinded;
        /// <summary>
        /// Called when a new doctor is binded to the connected patient.
        /// </summary>
        /// <param name="doctor">The doctor.</param>
        public void OnDoctorBinded(LightDoctorDto doctor)
        {
            if (this.DoctorBinded != null)
            {
                this.DoctorBinded(this, new EventArgs<LightDoctorDto>(doctor));
            }
        }
    }
}