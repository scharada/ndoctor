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

namespace Probel.NDoctor.Domain.DTO.Helpers
{
    using System.Collections.Generic;

    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.DTO.Properties;

    public static class DtoExtentions
    {
        #region Methods

        /// <summary>
        /// Convert a family into a list of patient. It fills up the Segratator to allow grouping
        /// the patients by family link
        /// </summary>
        /// <param name="family">The family.</param>
        /// <returns></returns>
        public static IEnumerable<LightPatientDto> ToPatients(this FamilyDto family)
        {
            var patients = new List<LightPatientDto>();

            if (family.Father != null)
            {
                family.Father.Segretator = Messages.Msg_Father;
                patients.Add(family.Father);
            }
            if (family.Mother != null)
            {
                family.Mother.Segretator = Messages.Msg_Mother;
                patients.Add(family.Mother);
            }
            foreach (var child in family.Children)
            {
                child.Segretator = Messages.Msg_Children;
                patients.Add(child);
            }

            return patients.ToArray();
        }

        #endregion Methods
    }
}