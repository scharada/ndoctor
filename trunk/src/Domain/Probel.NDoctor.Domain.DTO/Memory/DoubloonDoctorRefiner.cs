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

namespace Probel.NDoctor.Domain.DTO.Memory
{
    using System.Collections.Generic;
    using System.Linq;

    using Probel.NDoctor.Domain.DTO.Objects;

    public class DoubloonDoctorRefiner : MemoryRefiner<DoubloonDoctorDto>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubloonDoctorRefiner"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        public DoubloonDoctorRefiner(IEnumerable<DoubloonDoctorDto> list)
            : base(list)
        {
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<DoubloonDoctorDto> FindByFirstOrLastName(string criteria)
        {
            criteria = criteria.ToUpper();
            return (from doc in this.Items
                    where doc.FirstName.ToUpper().Contains(criteria)
                       || doc.LastName.ToUpper().Contains(criteria)
                    select doc).AsEnumerable();
        }

        #endregion Methods
    }
}