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
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Select all the patient with the birth year specified in the ctor
    /// </summary>
    public class FindPatientByBirthYearSpecification : Specification<PatientDto>
    {
        #region Fields

        private int birthYear;

        #endregion Fields

        #region Constructors

        public FindPatientByBirthYearSpecification(int birthYear)
        {
            this.birthYear = birthYear;
        }

        #endregion Constructors

        #region Methods

        public override bool IsSatisfiedBy(PatientDto obj)
        {
            return obj.Birthdate.Year == this.birthYear;
        }

        #endregion Methods
    }
}