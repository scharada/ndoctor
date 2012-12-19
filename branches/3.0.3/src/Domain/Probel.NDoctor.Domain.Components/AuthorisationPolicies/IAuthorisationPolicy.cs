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

namespace Probel.NDoctor.Domain.Components.AuthorisationPolicies
{
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Provide the algorithm for the authorisation allowance.
    /// </summary>
    internal interface IAuthorisationPolicy
    {
        #region Methods

        /// <summary>
        /// Determines whether the specified role is granted to execute the specified task.
        /// </summary>
        /// <param name="to">The level of authorisatio needed to execute the role.</param>
        /// <param name="user">The user to check the authorisations.</param>
        /// <returns>
        ///   <c>true</c> if the specified assigned role is granted; otherwise, <c>false</c>.
        /// </returns>
        bool IsGranted(string to, LightUserDto user);

        #endregion Methods
    }
}