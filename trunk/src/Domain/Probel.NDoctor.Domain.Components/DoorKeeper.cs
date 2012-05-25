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

namespace Probel.NDoctor.Domain.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Probel.NDoctor.Domain.Components.AuthorisationPolicies;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

    using StructureMap;

    /// <summary>
    /// This class checks whether the demanded right is granted to the connected user or not.
    /// </summary>
    public class DoorKeeper
    {
        #region Fields

        private static readonly IAuthorisationPolicy policy = ObjectFactory.GetInstance<IAuthorisationPolicy>();

        private LightUserDto user;

        #endregion Fields

        #region Constructors

        public DoorKeeper(LightUserDto user)
        {
            this.user = user;
        }

        #endregion Constructors

        #region Methods

        public bool Grants(string to)
        {
            return policy.IsGranted(to, this.user);
        }

        #endregion Methods
    }
}