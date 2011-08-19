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
    /// Indicates the type of the status
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        /// An error is an unrecoverable error.
        /// </summary>
        Error,
        /// <summary>
        /// An informative message displays some info to the user.
        /// </summary>
        Info,
        /// <summary>
        /// A warning message is a recoverable error.
        /// </summary>
        Warning,
    }

    #endregion Enumerations
}