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

    public abstract class MemoryRefinerWithNameProperty<T> : MemoryRefiner<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryRefinerWithNameProperty&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        protected MemoryRefinerWithNameProperty(IEnumerable<T> items)
            : base(items)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Finds all the elements that contains the name in the
        /// property <c>Name</c>
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>An enumeration of <paramref name="T"/></returns>
        public IEnumerable<T> GetByName(string name)
        {
            name = name.ToUpper();
            var result = (from dynamic item in this.Items
                          where item.Name.ToUpper().Contains(name)
                          select (T)item).ToList();
            return (IEnumerable<T>)result;
        }

        #endregion Methods
    }
}