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
namespace Probel.NDoctor.Domain.DTO.Specifications
{
    public abstract class CompositeSpecification<T> : Specification<T>
    {
        #region Fields

        protected readonly Specification<T> leftSide;
        protected readonly Specification<T> rightSide;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeSpecification&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="leftSide">The left side.</param>
        /// <param name="rightSide">The right side.</param>
        internal CompositeSpecification(Specification<T> leftSide, Specification<T> rightSide)
        {
            this.leftSide = leftSide;
            this.rightSide = rightSide;
        }

        #endregion Constructors
    }
}