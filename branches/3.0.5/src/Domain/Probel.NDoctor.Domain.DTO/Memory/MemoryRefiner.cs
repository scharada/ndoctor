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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents the base class of a memory refiner
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MemoryRefiner<T>
    {
        #region Fields

        public readonly IEnumerable<T> Items;

        #endregion Fields

        #region Constructors

        public MemoryRefiner(IEnumerable<T> items)
        {
            this.Items= (items== null)
                ? new List<T>()
                : items;
        }

        #endregion Constructors
    }
}