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

    /// <summary>
    /// Makes in memory search for doctors
    /// </summary>
    public class DoctorRefiner : MemoryRefiner<DoctorDto>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorRefiner"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public DoctorRefiner(IEnumerable<DoctorDto> items)
            : base(items)
        {
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<DoctorDto> GetByName(string name)
        {
            name = name.ToUpper();
            return (from item in this.Items
                    where item.LastName.ToUpper().Contains(name)
                    select item).ToArray();
        }

        #endregion Methods
    }
}