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
    #region Enumerations

    /// <summary>
    /// Represents the groups the Home menu contains
    /// </summary>
    public enum Groups
    {
        /// <summary>
        /// The group of the home menu which contains all the managers
        /// </summary>
        Managers,
        /// <summary>
        /// The group of the home menu which contains all the generic tools such as search or save
        /// </summary>
        Tools,
        /// <summary>
        /// The of the home menu which contains all the global tools such as the calendar
        /// </summary>
        GlobalTools,

        /// <summary>
        /// Special menu only visible with command line args
        /// </summary>
        DebugTools,
    }

    #endregion Enumerations
}