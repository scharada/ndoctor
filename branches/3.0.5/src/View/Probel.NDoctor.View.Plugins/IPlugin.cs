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
namespace Probel.NDoctor.View.Plugins
{
    using System;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a plugin and how it can be manipulated from the PluginHost
    /// </summary>
    public interface IPlugin
    {
        #region Properties

        /// <summary>
        /// Gets the page that represents the workbench of the plugin.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        Page Page
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initialises this plugin. Basicaly it should configure the menus into the PluginHost
        /// </summary>
        void Initialise();

        #endregion Methods
    }
}