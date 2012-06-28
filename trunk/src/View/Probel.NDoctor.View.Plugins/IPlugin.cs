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
        /// Gets a value indicating whether this plugin is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this plugin is active; otherwise, <c>false</c>.
        /// </value>
        bool IsActive
        {
            get;
        }

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

        /// <summary>
        /// Gets the version of this plugin. This will be used with the <see cref="PluginValidator.cs"/>
        /// to check if this plugin can be validated or not.
        /// </summary>
        Version Version
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Activates this plugin. That's the PluginHost can display and the user can use this plugin.
        /// It means that all validations have succeeded on this plugin.
        /// </summary>
        void Activate();

        /// <summary>
        /// Closes this plugin. That's unload all the data. Typically used when the connected user disconnect.
        /// </summary>
        void Close();

        /// <summary>
        /// Deactivates this plugin. That's the PluginHost CAN'T display and the user CAN'T use this plugin.
        /// It means that at least one validation have FAILED on this plugin.
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Initialises this plugin. Basicaly it should configure the menus into the PluginHost
        /// </summary>
        void Initialise();

        /// <summary>
        /// Determines whether this plugin is valid refering to the host version.
        /// </summary>
        /// <param name="hostVersion">The host version.</param>
        /// <returns>
        ///   <c>true</c> if this plugin is valid; otherwise, <c>false</c>.
        /// </returns>
        bool IsValid(IPluginHost host);

        #endregion Methods
    }
}