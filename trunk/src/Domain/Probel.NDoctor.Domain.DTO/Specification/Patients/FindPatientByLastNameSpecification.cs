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
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Select all the patient with the first and/or last name that contains
    /// the text specified in the contructor.
    /// This specification uses the lower case version of the Profession.Name to 
    /// make the selection.
    /// If the specified text is empty or null, all the row will be returned
    /// </summary>
    internal class FindPatientByLastNameSpecification : Specification<LightPatientDto>
    {
        #region Fields

        private string text;

        #endregion Fields

        #region Constructors

        public FindPatientByLastNameSpecification(string text)
        {
            this.text = text.ToLower();
        }

        #endregion Constructors

        #region Methods
        public override bool IsSatisfiedBy(LightPatientDto obj)
        {
            return obj.LastName.ToLower().Contains(this.text);
        }

        #endregion Methods
    }
}