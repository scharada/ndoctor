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
namespace Probel.NDoctor.Plugins.Administration.Helpers
{
    using System;

    public static class Notifyer
    {
        #region Events

        public static event EventHandler ItemChanged;

        public static event EventHandler TagsChanged;

        #endregion Events

        #region Methods

        public static void OnItemChanged(object sender)
        {
            if (ItemChanged != null)
            {
                ItemChanged(sender, EventArgs.Empty);
            }
        }

        public static void OnTagsChanged(object sender)
        {
            if (TagsChanged != null)
            {
                TagsChanged(sender, EventArgs.Empty);
            }
        }

        #endregion Methods
    }
}