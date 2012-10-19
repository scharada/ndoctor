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
    using System.Linq;

    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Provide the algorithm for the authorisation allowance.
    /// </summary>
    internal class AuthorisationPolicy : IAuthorisationPolicy
    {
        #region Methods

        /// <summary>
        /// Determines whether the specified role is granted to execute the specified task.
        /// </summary>
        /// <param name="to">The level of authorisatio needed to execute the role.</param>
        /// <param name="assignedRole">The assigned role.</param>
        /// <returns>
        ///   <c>true</c> if the specified assigned role is granted; otherwise, <c>false</c>.
        /// </returns>
        public bool IsGranted(string to, LightUserDto user)
        {
            if (to == To.Everyone) return true;
            else if (user == null) return false;
            else if (user.AssignedRole == null && !user.IsSuperAdmin) { return false; }
            else if (user.IsSuperAdmin) { return true; }
            else
            {
                return (from task in user.AssignedRole.Tasks
                        where task.RefName.ToLower() == to
                        select task).ToList().Count() >0;
            }
        }

        #endregion Methods
    }
}