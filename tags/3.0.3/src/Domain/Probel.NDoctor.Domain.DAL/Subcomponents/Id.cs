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

namespace Probel.NDoctor.Domain.DAL.Subcomponents
{
    using System.Linq;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.NDoctor.Domain.DAL.Entities;

    internal class Id
    {
        #region Fields

        private readonly ISession Session;

        #endregion Fields

        #region Constructors

        public Id(ISession session)
        {
            this.Session = session;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Checks if user with specified ID exist into the database
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public bool ExistForUser(long id)
        {
            return (from i in this.Session.Query<User>()
                    where i.Id == id
                    select i).Count() == 1;
        }

        #endregion Methods
    }
}