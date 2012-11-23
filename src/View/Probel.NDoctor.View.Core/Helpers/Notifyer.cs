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

namespace Probel.NDoctor.View.Core.Helpers
{
    using System;

    internal static class Notifyer
    {
        #region Events

        /// <summary>
        /// Occurs when user is saving the settings.
        /// </summary>
        public static event EventHandler SavingSettings;

        #endregion Events

        #region Methods

        /// <summary>
        /// Called when user is saving the settings.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void OnSavingSettings(object sender)
        {
            if (SavingSettings != null)
            {
                SavingSettings(sender, EventArgs.Empty);
            }
        }

        #endregion Methods
    }
}