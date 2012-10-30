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

    using NHibernate;

    using Probel.NDoctor.Domain.DAL.Subcomponents;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

    public class UnitTestComponent : BaseComponent
    {
        #region Constructors

        public UnitTestComponent(ISession session)
            : base(session)
        {
        }

        #endregion Constructors

        #region Methods

        public long Create(LightPatientDto patient)
        {
            return new Creator(this.Session).Create(patient);
        }

        public IList<PatientDto> GetPatientsByName(string criteria, SearchOn searchOn)
        {
            return new Selector(this.Session).GetPatientByName(criteria, searchOn);
        }

        public IList<LightPatientDto> GetPatientsByNameLight(string criteria, SearchOn searchOn)
        {
            return new Selector(this.Session).GetPatientByNameLight(criteria, searchOn);
        }

        #endregion Methods

        public IList<LightUserDto> GetAllUsers()
        {
            return new Selector(this.Session).GetAllUsers();
        }

        public IList<LightUserDto> GetUserByLastName(string criteria)
        {
            return new Selector(this.Session).GetUserByLastName(criteria);

        }
    }
}