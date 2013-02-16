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

namespace Probel.NDoctor.Domain.DAL.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class InDebugMode
    {
        #region Constructors

        static InDebugMode()
        {
            Value = false;
        }

        #endregion Constructors

        #region Properties

        public static bool Value
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="Probel.NDoctor.Domain.DAL.Helpers.InDebugMode"/> to <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator bool(InDebugMode mode)
        {
            return InDebugMode.Value;
        }

        #endregion Methods
    }
}