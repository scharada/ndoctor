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
namespace Probel.NDoctor.Plugins.UserSession.Helpers
{
    using System;

    /// <summary>
    /// Notifies the whole plugin when event occur
    /// </summary>
    public static class Notifyer
    {
        #region Events

        /// <summary>
        /// Occurs when a user is added.
        /// </summary>
        public static event EventHandler UserAdded;

        #endregion Events

        #region Methods

        /// <summary>
        /// Called when a user is added.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void OnUserAdded(object sender)
        {
            if (UserAdded != null)
                UserAdded(sender, EventArgs.Empty);
        }

        #endregion Methods
    }
}