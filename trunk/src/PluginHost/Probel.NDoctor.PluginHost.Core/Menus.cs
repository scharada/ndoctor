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
    #region Enumerations

    /// <summary>
    /// Represents the default menus the host has.
    /// </summary>
    public enum Menus
    {
        /// <summary>
        /// Represents the "windows start" like menu.
        /// </summary>
        Global,
        /// <summary>
        /// Contains a menu group to manage a patient session and another menu group to manage the patient's managers.
        /// </summary>
        Home,
        /// <summary>
        /// Contains tools to improve the user experience of nDoctor.
        /// </summary>
        Tools,
        /// <summary>
        /// Contains help information about nDoctor.
        /// </summary>
        Help,
    }

    #endregion Enumerations
}