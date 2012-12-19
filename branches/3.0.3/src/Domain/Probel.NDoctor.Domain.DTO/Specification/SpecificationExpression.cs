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

namespace Probel.NDoctor.Domain.DTO.Specification
{
    public class SpecificationExpression<T>
    {
        #region Fields

        private Specification<T> expression;

        #endregion Fields

        #region Constructors

        public SpecificationExpression(Specification<T> start)
        {
            this.expression = start;
        }

        public SpecificationExpression()
        {
            this.expression = new AllowAllSpecification<T>();
        }

        #endregion Constructors

        #region Methods

        public SpecificationExpression<T> And(Specification<T> spec)
        {
            this.expression = this.expression.And(spec);
            return this;
        }

        public bool IsSatisfiedBy(T obj)
        {
            return this.expression.IsSatisfiedBy(obj);
        }

        public SpecificationExpression<T> Not(Specification<T> spec)
        {
            this.expression = this.expression.Not(spec);
            return this;
        }

        public SpecificationExpression<T> Or(Specification<T> spec)
        {
            this.expression = this.expression.Or(spec);
            return this;
        }

        #endregion Methods
    }
}