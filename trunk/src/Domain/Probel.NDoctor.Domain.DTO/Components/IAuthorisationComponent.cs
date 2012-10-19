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
    using Probel.NDoctor.Domain.DTO.Objects;

    public interface IAuthorisationComponent : IBaseComponent
    {
        #region Methods

        /// <summary>
        /// Determines whether the specified role can be removed.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>
        ///   <c>true</c> if this the specified role can be removed; otherwise, <c>false</c>.
        /// </returns>
        bool CanRemove(RoleDto role);

        /// <summary>
        /// Creates a new role in the repository.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        long Create(RoleDto role);

        /// <summary>
        /// Creates the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns>The id of the created item</returns>
        long Create(TaskDto task);

        /// <summary>
        /// Gets the task by its reference name.
        /// </summary>
        /// <param name="refName">Name of the ref.</param>
        /// <returns>The found task or <c>Null</c> if nothing is found</returns>
        TaskDto GetTaskByReference(string refName);

        /// <summary>
        /// Gets all users stored in the database.
        /// </summary>
        /// <returns></returns>
        LightUserDto[] GetAllLightUsers();

        /// Gets all roles the repository contains.
        /// </summary>
        /// <returns></returns>
        RoleDto[] GetAllRoles();

        /// <summary>
        /// Gets the tasks that are not yet binded to the specified role.
        /// If the specified role is null, it'll return all the tasks stored in the 
        /// database
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>A list of tasks</returns>
        TaskDto[] GetAvailableTasks(RoleDto role);

        /// <summary>
        /// Determines whether this specified usr is super admin.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        ///   <c>true</c> if the specified useris super admin; otherwise, <c>false</c>.
        /// </returns>
        bool IsSuperAdmin(LightUserDto user);

        /// <summary>
        /// Removes the role with the specified id.
        /// </summary>
        /// <param name="role">The role to remove.</param>
        void Remove(RoleDto role);

        /// <summary>
        /// Removes the specified user from the repository.
        /// </summary>
        /// <param name="user">The user.</param>
        void Remove(LightUserDto user);

        /// <summary>
        /// Updates the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        void Update(RoleDto role);

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        void Update(LightUserDto user);

        #endregion Methods
    }
}