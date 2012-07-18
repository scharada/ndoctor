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

namespace Probel.NDoctor.View.Plugins.Helpers
{
    public class Configuration
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the contextual menus should be automatically displayed when
        /// user navigates to a new plugin.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [automatic context menu]; otherwise, <c>false</c>.
        /// </value>
        public bool AutomaticContextMenu
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether component loggin is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if component loggin is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool ComponentLogginEnabled
        {
            get;
            set;
        }

        #endregion Properties
    }
}