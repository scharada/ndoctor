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

namespace Probel.NDoctor.Domain.DAL.Components
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.EqualityComparers;
    using Probel.NDoctor.Domain.DAL.Properties;
    using Probel.NDoctor.Domain.DAL.Subcomponents;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Provide tools to manage authorisation management. These features are granted only for administrators
    /// </summary>
    [Granted(To.Administer)]
    public class AuthorisationComponent : BaseComponent, IAuthorisationComponent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorisationComponent"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public AuthorisationComponent(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorisationComponent"/> class.
        /// </summary>
        public AuthorisationComponent()
            : base()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Determines whether the specified role can be removed.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>
        ///   <c>true</c> if this the specified role can be removed; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRemove(RoleDto role)
        {
            return (from u in this.Session.Query<User>()
                    where u.AssignedRole.Id == role.Id
                    select u).Count() == 0;
        }

        /// <summary>
        /// Creates a new role in the repository.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public long Create(RoleDto role)
        {
            return new Creator(this.Session).Create(role);
        }

        /// <summary>
        /// Creates the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns>
        /// The id of the created item
        /// </returns>
        public long Create(TaskDto task)
        {
            return new Creator(this.Session).Create(task);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        /// <returns>
        /// The id of the just created item
        /// </returns>
        public long Create(TagDto item)
        {
            return new Creator(this.Session).Create(item);
        }

        /// <summary>
        /// Gets all users stored in the database.
        /// </summary>
        /// <returns></returns>
        public LightUserDto[] GetAllLightUsers()
        {
            var entities = (from u in this.Session.Query<User>()
                            select u);
            return Mapper.Map<IEnumerable<User>, LightUserDto[]>(entities);
        }

        /// <summary>
        /// Gets all roles the repository contains.
        /// </summary>
        /// <returns></returns>
        public RoleDto[] GetAllRoles()
        {
            var entities = (from r in this.Session.Query<Role>()
                            select r).ToList();
            return Mapper.Map<IList<Role>, RoleDto[]>(entities);
        }

        /// <summary>
        /// Gets the tasks that are not yet binded to the specified role.
        /// If the specified role is null, it'll return all the tasks stored in the 
        /// database
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>A list of tasks</returns>
        public TaskDto[] GetAvailableTasks(RoleDto role)
        {
            if (role == null) return new TaskDto[] { };

            var tasks = Mapper.Map<IEnumerable<TaskDto>, IList<Task>>(role.Tasks);
            var entities = (from t in this.Session.Query<Task>()
                            select t).ToList();

            var result = new List<Task>();
            foreach (var item in entities)
            {
                if (!tasks.Contains(item, new TaskEqualityComparerOnName()))
                {
                    result.Add(item);
                }
            }

            return Mapper.Map<IEnumerable<Task>, TaskDto[]>(result);
        }

        /// <summary>
        /// Gets the task by its reference name.
        /// </summary>
        /// <param name="refName">Name of the ref.</param>
        /// <returns>The found task or <c>Null</c> if nothing is found</returns>
        public TaskDto GetTaskByReference(string refName)
        {
            var result = (from t in this.Session.Query<Task>()
                          where t.RefName == refName
                          select t).FirstOrDefault();
            return Mapper.Map<Task, TaskDto>(result);
        }

        /// <summary>
        /// Determines whether this specified usr is super admin.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        ///   <c>true</c> if the specified useris super admin; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSuperAdmin(LightUserDto user)
        {
            var superadmin = (from u in this.Session.Query<User>()
                              where u.Id == user.Id
                              select u).FirstOrDefault();
            if (superadmin == null) { throw new BusinessLogicException(Messages.Ex_NoSuperAdmin); }
            return superadmin.IsSuperAdmin;
        }

        /// <summary>
        /// Removes the role with the specified id.
        /// </summary>
        /// <param name="role">The role to remove.</param>
        public void Remove(RoleDto item)
        {
            Assert.IsNotNull(item, "item");
            new Remover(this.Session).Remove<Role>(item);
        }

        /// <summary>
        /// Removes the specified user from the repository.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Remove(LightUserDto user)
        {
            var aptEntities = (from a in this.Session.Query<Appointment>()
                               where a.User.Id == user.Id
                               select a);

            foreach (var item in aptEntities)
            {
                this.Session.Delete(item);
            }

            var userEntities = (from u in this.Session.Query<User>()
                                where u.Id == user.Id
                                select u);

            foreach (var item in userEntities)
            {
                this.Session.Delete(item);
            }
        }

        /// <summary>
        /// Updates the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        public void Update(RoleDto role)
        {
            new Updator(this.Session).Update(role);
        }

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Update(LightUserDto user)
        {
            new Updator(this.Session).Update(user);
        }

        #endregion Methods
    }
}