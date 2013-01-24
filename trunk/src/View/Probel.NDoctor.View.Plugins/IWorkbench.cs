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

namespace Probel.NDoctor.View.Plugins
{
    /// <summary>
    /// Any Page that implements this interface has features to check whether the user can quit this page toward another
    /// </summary>
    public interface IWorkbench
    {
        #region Methods

        /// <summary>
        /// Ask to the user to confirm he/she want to leave the current page.
        /// </summary>
        /// <returns></returns>
        bool AskToLeave();

        /// <summary>
        /// Determines whether the user can leave the current page without data loss
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can navigate; otherwise, <c>false</c>.
        /// </returns>
        bool CanLeave();

        #endregion Methods
    }
}