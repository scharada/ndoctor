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

namespace Probel.NDoctor.Domain.DTO.Specifications.Patients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Verifies that the specified <see cref="LightPatientDto"/> has got an inscription data between the specified dates
    /// </summary>
    internal class FindPatientByInscriptionSpecification : Specification<LightPatientDto>
    {
        #region Fields

        private readonly DateTime After;
        private readonly DateTime Before;

        #endregion Fields

        #region Constructors

        public FindPatientByInscriptionSpecification(DateTime after, DateTime before)
        {
            this.After = after;
            this.Before = before;
        }

        #endregion Constructors

        #region Methods

        public override bool IsSatisfiedBy(LightPatientDto obj)
        {
            return obj.InscriptionDate >= this.After
                && obj.InscriptionDate <= this.Before;
        }

        #endregion Methods
    }
}