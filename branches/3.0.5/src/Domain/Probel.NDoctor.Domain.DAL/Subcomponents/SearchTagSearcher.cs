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
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class SearchTagSearcher
    {
        #region Fields

        private readonly ISession Session;

        #endregion Fields

        #region Constructors

        public SearchTagSearcher(ISession session)
        {
            this.Session = session;
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<LightPatientDto> GetPatientBySearchTagOperatorAnd(IEnumerable<SearchTagDto> tags)
        {
            var ids = tags.Select(e => e.Id).ToList();

            var entities = (from p in this.Session.Query<Patient>()
                            where (from t in p.SearchTags
                                   where ids.Contains(t.Id)
                                   select t).Count() == ids.Count()
                            select p).ToList();
            return Mapper.Map<IEnumerable<Patient>, IEnumerable<LightPatientDto>>(entities);
        }

        public IEnumerable<LightPatientDto> GetPatientBySearchTagOperatorOr(IEnumerable<SearchTagDto> tags)
        {
            var ids = tags.Select(e => e.Id).ToList();
            var entities = (from p in this.Session.Query<Patient>()
                            where (from t in p.SearchTags
                                   where ids.Contains(t.Id)
                                   select t).Count() > 0
                            select p).ToList();
            return Mapper.Map<IEnumerable<Patient>, IEnumerable<LightPatientDto>>(entities);
        }

        #endregion Methods
    }
}