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
namespace Probel.NDoctor.Domain.DTO.Specification
{
    /// <summary>
    /// Specification is used to make an expression to refine search after a query from the database.
    /// This pattern is explained here: http://devlicio.us/blogs/jeff_perrin/archive/2006/12/13/the-specification-pattern.aspx
    /// </summary>
    /// <typeparam name="T">The DTO to query</typeparam>
    public abstract class Specification<T>
    {
        #region Methods

        public AndSpecification<T> And(Specification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public abstract bool IsSatisfiedBy(T obj);

        public NotSpecification<T> Not(Specification<T> specification)
        {
            return new NotSpecification<T>(this, specification);
        }

        public OrSpecification<T> Or(Specification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        #endregion Methods
    }
}