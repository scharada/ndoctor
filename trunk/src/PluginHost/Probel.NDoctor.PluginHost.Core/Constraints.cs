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
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace Probel.NDoctor.PluginHost.Core
{
    #region Enumerations

    /// <summary>
    /// Represents how the plugin dependency should be applied.
    /// </summary>
    public enum Constraints
    {
        /// <summary>
        /// It means the plugin will be valid if the database has exactly the specified version
        /// </summary>
        Strict,
        /// <summary>
        /// It means the plugin will be valid if the database has the specified version or above
        /// </summary>
        Minimum,
    }

    #endregion Enumerations
}