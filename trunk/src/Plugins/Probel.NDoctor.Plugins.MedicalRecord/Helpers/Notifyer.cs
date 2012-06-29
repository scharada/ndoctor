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
namespace Probel.NDoctor.Plugins.MedicalRecord.Helpers
{
    using System;

    public static class Notifyer
    {
        #region Events

        /// <summary>
        /// Occurs when refreshed the plugin needs a refresh of all the data.
        /// </summary>
        public static event EventHandler Refreshed;

        /// <summary>
        /// Occurs when use is saving medical records.
        /// </summary>
        public static event EventHandler Saving;

        #endregion Events

        #region Methods

        /// <summary>
        /// Called to trigger the <see cref="Refreshed"/> event 
        /// </summary>
        public static void OnRefreshed()
        {
            if (Refreshed != null)
                Refreshed(new Object(), EventArgs.Empty);
        }

        /// <summary>
        /// Called when user is saving medical records.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void OnSaving(object sender)
        {
            if (Saving != null)
            {
                Saving(sender, EventArgs.Empty);
            }
        }

        #endregion Methods
    }
}