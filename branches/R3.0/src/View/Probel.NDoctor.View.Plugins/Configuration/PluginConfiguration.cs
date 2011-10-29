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
namespace Probel.NDoctor.View.Plugins.Configuration
{
    /// <summary>
    /// Configuration of a plugin
    /// </summary>
    public struct PluginConfiguration
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the plugin is activated.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is activated; otherwise, <c>false</c>.
        /// </value>
        public bool IsActivated
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the name of the plugin as it should be displayed in the plugin
        /// management module.
        /// </summary>
        /// <value>
        /// The menu.
        /// </value>
        public string Menu
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the path to the files of the plugin.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path
        {
            get;
            internal set;
        }

        #endregion Properties
    }
}