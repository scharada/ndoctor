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
namespace Probel.NDoctor.Domain.DAL.Components
{
    using System.Linq;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Properties;
    using Probel.NDoctor.Domain.DAL.Subcomponents;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// This component manage the users that is connected into nDoctor
    /// </summary>
    public class UserSessionComponent : BaseComponent, IUserSessionComponent
    {
        #region Constructors

        public UserSessionComponent()
            : base()
        {
        }

        public UserSessionComponent(ISession Session)
            : base(Session)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Determines whether the specified user can connect into the application.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        ///   <c>true</c> if this instance can connect the specified user; otherwise, <c>false</c>.
        /// </returns>
        [Granted(To.Everyone)]
        public bool CanConnect(LightUserDto user, string password)
        {
            if (user == null || password == null) return false;

            if (!Session.IsOpen) throw new SessionException(Messages.Msg_ErrorSessionNotOpen);
            var foundUser = Session.Get<User>(user.Id);
            if (user == null) return false;

            return (password == foundUser.Password);
        }

        /// <summary>
        /// Creates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        [Granted(To.Everyone)]
        public long Create(LightUserDto item, string password)
        {
            return new Creator(this.Session).Create(item, password);
        }

        /// <summary>
        /// Gets user used for default connection or null if none is selected.
        /// </summary>
        /// <returns></returns>
        [Granted(To.Everyone)]
        public LightUserDto GetDefaultUser()
        {
            var result = (from user in this.Session.Query<User>()
                          where user.IsDefault == true
                          select user).ToList();

            if (result.Count == 0) return null;
            else if (result.Count > 1) throw new DalQueryException(Messages.Ex_QueryException_SeveralDefaultUsers);
            else return Mapper.Map<User, LightUserDto>(result[0]);
        }

        /// <summary>
        /// Gets the whole data of the specified connected user.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [Granted(To.Read)]
        public UserDto LoadUser(LightUserDto user)
        {
            var fullUser = this.Session.Get<User>(user.Id);

            if (fullUser == null) return null;
            var result = Mapper.Map<User, UserDto>(fullUser);
            return result;
        }

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="item">The user.</param>
        [Granted(To.MetaWrite)]
        public void Update(UserDto item)
        {
            new Updator(this.Session).Update(item);
        }

        /// <summary>
        /// Updates the password of the connected user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        [Granted(To.MetaWrite)]
        public void UpdatePassword(LightUserDto user, string password)
        {
            new Updator(this.Session).Update(user, password);
        }

        #endregion Methods
    }
}