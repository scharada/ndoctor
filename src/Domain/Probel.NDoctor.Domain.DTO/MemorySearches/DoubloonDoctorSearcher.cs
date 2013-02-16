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

namespace Probel.NDoctor.Domain.DTO.MemorySearches
{
    using System.Collections.Generic;
    using System.Linq;

    using Probel.NDoctor.Domain.DTO.Objects;

    public class DoubloonDoctorSearcher
    {
        #region Fields

        public readonly IEnumerable<DoubloonDoctorDto> List;

        #endregion Fields

        #region Constructors

        public DoubloonDoctorSearcher(IEnumerable<DoubloonDoctorDto> list = null)
        {
            this.List = (list == null)
                ? new List<DoubloonDoctorDto>()
                : list;
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<DoubloonDoctorDto> FindByFirstOrLastName(string criteria)
        {
            IEnumerable<DoubloonDoctorDto> result;

            if (string.IsNullOrWhiteSpace(criteria)) { result = this.List; }
            else
            {
                var uCriteria = criteria.ToUpper();
                result = (from doc in this.List
                          where doc.FirstName.ToUpper().Contains(uCriteria)
                             || doc.LastName.ToUpper().Contains(uCriteria)
                          select doc).AsEnumerable();
            }
            return result;
        }

        #endregion Methods
    }
}