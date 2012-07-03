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
namespace Probel.NDoctor.Plugins.Authorisation.Helpers
{
    using System;

    /// <summary>
    /// Represent events that can be hendle throughout the plugin
    /// </summary>
    public static class Notifyer
    {
        #region Events

        /// <summary>
        /// Occurs before the plugin refreshes the roles.
        /// </summary>
        public static event EventHandler RoleRefreshing;

        /// <summary>
        /// Occurs when showing user management page.
        /// </summary>
        public static event EventHandler<PageEventArgs> Showing;

        /// <summary>
        /// Occurs before the plugin refreshes the users.
        /// </summary>
        public static event EventHandler UserRefreshing;

        #endregion Events

        #region Methods

        /// <summary>
        /// Called to ask a refresh of the plugin's data.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void OnRoleRefreshing(object sender)
        {
            if (RoleRefreshing != null)
            {
                RoleRefreshing(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when showing user management.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void OnShowing(object sender, PageEventArgs.DisplayedPage displayed)
        {
            if (Showing != null)
            {
                Showing(sender, new PageEventArgs(displayed));
            }
        }

        /// <summary>
        /// Called when the plugin refreshes the users.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void OnUserRefreshing(object sender)
        {
            if (UserRefreshing != null)
                UserRefreshing(sender, EventArgs.Empty);
        }

        #endregion Methods
    }
}