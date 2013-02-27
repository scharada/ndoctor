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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.NDoctor.Domain.DAL.Entities;

    internal class Query
    {
        #region Fields

        private readonly ISession Session;

        #endregion Fields

        #region Constructors

        public Query(ISession session)
        {
            this.Session = session;
        }

        #endregion Constructors

        #region Methods

        public bool CheckSearchTagExists(string name)
        {
            name = name.ToLower();
            return (from t in this.Session.Query<SearchTag>()
                    where t.Name.ToLower() == name
                    select t).Count() > 0;
        }

        #endregion Methods
    }
}