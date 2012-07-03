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

namespace Probel.NDoctor.Domain.DAL.EqualityComparers
{
    using System.Collections.Generic;

    using Probel.NDoctor.Domain.DAL.Entities;

    /// <summary>
    /// Check equality of task based on their name
    /// </summary>
    class TaskEqualityComparerOnName : IEqualityComparer<Task>
    {
        #region Methods

        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool Equals(Task x, Task y)
        {
            if (x == null && y == null) return true;
            else if (string.IsNullOrEmpty(x.Name) && string.IsNullOrEmpty(y.Name)) return true;
            else if (string.IsNullOrEmpty(x.Name) || string.IsNullOrEmpty(y.Name)) return false;
            else return x.Name.ToLower() == y.Name.ToLower();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(Task obj)
        {
            return obj.Name.ToLower().GetHashCode();
        }

        #endregion Methods
    }
}