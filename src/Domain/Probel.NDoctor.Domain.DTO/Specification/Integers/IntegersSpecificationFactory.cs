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

namespace Probel.NDoctor.Domain.DTO.Specification.Integers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Probel.NDoctor.Domain.DTO.Specification.Integers;

    public class IntegersSpecificationFactory
    {
        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="IntegersSpecificationFactory"/> class from being created.
        /// </summary>
        internal IntegersSpecificationFactory()
        {
        }

        #endregion Constructors

        #region Methods

        public Specification<int> EqualTo(int i)
        {
            return new IntegerEqualsToSpecification(i);
        }

        public Specification<int> GreaterThan(int i)
        {
            return new IntegerGreaterThanSpecification(i);
        }

        public Specification<int> LessThan(int i)
        {
            return new IntegerLessThanSpecification(i);
        }

        #endregion Methods
    }
}