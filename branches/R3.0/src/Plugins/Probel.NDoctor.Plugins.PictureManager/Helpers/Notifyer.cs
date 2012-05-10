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
namespace Probel.NDoctor.Plugins.PictureManager.Helpers
{
    using System;

    /// <summary>
    /// Static repository that fires statis events
    /// </summary>
    public static class Notifyer
    {
        #region Events

        /// <summary>
        /// Occurs when an item is added/removed/updated.
        /// </summary>
        public static event EventHandler ItemChanged;

        #endregion Events

        #region Methods

        /// <summary>
        /// Called when an item was added/removed/updated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void OnItemChanged(object sender)
        {
            if (ItemChanged != null)
                ItemChanged(sender, EventArgs.Empty);
        }

        #endregion Methods
    }
}