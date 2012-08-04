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

namespace Probel.NDoctor.Domain.DTO.Specification
{
    using System;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Select all the patient with the profession specified in the contructor.
    /// This specification uses the lower case version of the Profession.Name to 
    /// make the selection
    /// </summary>
    public class FindPatientByProfessionSpecification : Specification<PatientDto>
    {
        #region Fields

        private string profession;

        #endregion Fields

        #region Constructors

        public FindPatientByProfessionSpecification(ProfessionDto profession)
        {
            this.profession = profession.Name.ToLower();
        }

        #endregion Constructors

        #region Methods

        public override bool IsSatisfiedBy(PatientDto obj)
        {
            Assert.IsNotNull(obj);
            if (obj.Profession == null || string.IsNullOrWhiteSpace(obj.Profession.Name)) return false;
            else
            {
                return obj.Profession.Name.ToLower().Contains(profession);
            }
        }

        #endregion Methods
    }
}