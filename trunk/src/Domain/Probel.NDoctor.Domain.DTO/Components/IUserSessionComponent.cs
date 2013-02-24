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
namespace Probel.NDoctor.Domain.DTO.Components
{
    using System.Collections.Generic;

    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// This component manage the users that is connected into nDoctor
    /// </summary>
    public interface IUserSessionComponent : IBaseComponent
    {
        #region Methods

        /// <summary>
        /// Determines whether the specified user can connect into the application.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        ///   <c>true</c> if this instance can connect the specified user; otherwise, <c>false</c>.
        /// </returns>
        bool CanConnect(SecurityUserDto user, string password);

        /// <summary>
        /// Creates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        long Create(SecurityUserDto item, string password);

        /// <summary>
        /// Gets all practices stored in the database.
        /// </summary>
        /// <returns></returns>
        IList<PracticeDto> GetAllPractices();

        /// <summary>
        /// Gets all roles light.
        /// </summary>
        /// <returns>An array with all the roles</returns>
        IList<RoleDto> GetAllRolesLight();

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        IList<SecurityUserDto> GetAllUsers();

        /// <summary>
        /// Gets user used for default connection or null if none is selected.
        /// </summary>
        /// <returns></returns>
        SecurityUserDto GetDefaultUser();

        /// <summary>
        /// Gets the user by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        UserDto GetUserById(long id);

        /// <summary>
        /// Gets the whole data of the specified connected user.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        UserDto LoadUser(SecurityUserDto user);

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="item">The user.</param>
        void Update(UserDto item);

        /// <summary>
        /// Updates the password of the connected user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        void UpdatePassword(SecurityUserDto user, string password);

        #endregion Methods
    }
}