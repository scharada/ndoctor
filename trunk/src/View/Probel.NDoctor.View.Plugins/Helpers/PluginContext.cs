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
namespace Probel.NDoctor.View.Plugins.Helpers
{
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.View.Plugins;

    /// <summary>
    /// Context of the plugin
    /// </summary>
    public static class PluginContext
    {
        #region Properties

        /// <summary>
        /// Gets the component factory.
        /// </summary>
        public static ComponentFactory ComponentFactory
        {
            get
            {
                return new ComponentFactory(Host.ConnectedUser, ComponentLogginEnabled);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether component loggin is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if component loggin is enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool ComponentLogginEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the door keeper. That's the instance that can specify if the demanded feature can be executed or not.
        /// </summary>
        public static DoorKeeper DoorKeeper
        {
            get
            {
                return new DoorKeeper(PluginContext.Host.ConnectedUser);
            }
        }

        /// <summary>
        /// Gets or sets the plugin host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public static IPluginHost Host
        {
            get;
            set;
        }

        #endregion Properties
    }
}