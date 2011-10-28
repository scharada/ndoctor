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

namespace Probel.NDoctor.Plugins.PatientData.Helpers
{
    using System;

    /// <summary>
    /// Maintains globel event to allow wide range communication between components
    /// </summary>
    public static class Notifyer
    {
        #region Events

        /// <summary>
        /// Occurs when a link bewteen a doctor and a patient changed.
        /// </summary>
        public static event EventHandler DoctorLinkChanged;

        /// <summary>
        /// Occurs when a specialisation changed.
        /// </summary>
        public static event EventHandler SpecialisationChanged;

        #endregion Events

        #region Methods

        /// <summary>
        /// Notifies a doctor was linked to a patinent
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void OnDoctorLinkChanged(object sender)
        {
            if (DoctorLinkChanged != null)
            {
                DoctorLinkChanged(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when a specialisation changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void OnSpecialisationChanged(object sender)
        {
            if (SpecialisationChanged != null)
            {
                SpecialisationChanged(sender, EventArgs.Empty);
            }
        }

        #endregion Methods
    }
}