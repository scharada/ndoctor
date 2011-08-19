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
namespace Probel.NDoctor.PluginHost.Core
{
    using System;

    public interface IPlugin
    {
        #region Properties

        /// <summary>
        /// Gets the cosntraint this plugin needs to fullfill to workkproperly.
        /// </summary>
        DatabaseConstraint Constraint
        {
            get;
        }

        /// <summary>
        /// Gets the name of the contextual menu.
        /// </summary>
        /// <value>
        /// The name of the contextual menu.
        /// </value>
        string ContextualMenuName
        {
            get;
        }

        IPluginHost Host
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the plugin is on error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if on error; otherwise, <c>false</c>.
        /// </value>
        bool OnError
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Save the state of the plugin.
        /// </summary>
        void Save();

        /// <summary>
        /// Setups the plugin. Typically, loads the plugin in memory
        /// </summary>
        void Setup();

        /// <summary>
        /// Displays the plugin into the GUI.
        /// </summary>
        void Show();

        void Validate(Version databaseVersion);

        #endregion Methods
    }
}