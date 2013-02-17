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
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Specification&lt;T&gt;"/> class.
        /// </summary>
        internal Specification()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets an empty specification.
        /// </summary>
        public static Specification<T> Empty
        {
            get
            {
                return new EmptySpecification<T>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Operates a logical <c>Not</c> with the specified specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>A <c>Not</c> specification</returns>
        public static Specification<T> operator !(Specification<T> expression)
        {
            return new NotSpecification<T>(expression);
        }

        /// <summary>
        /// Operates a logical <c>And</c> with twith the left and the right part.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static CompositeSpecification<T> operator &(Specification<T> left, Specification<T> right)
        {
            return new AndSpecification<T>(left, right);
        }

        /// <summary>
        /// Operates a logical <c>Or</c> with twith the left and the right part.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static CompositeSpecification<T> operator |(Specification<T> left, Specification<T> right)
        {
            return new OrSpecification<T>(left, right);
        }

        /// <summary>
        /// Operates a logical <c>And</c> with the left and the right part.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>A <c>And</c> specification</returns>
        public CompositeSpecification<T> And(Specification<T> specification)
        {
            return this & specification;
        }

        /// <summary>
        /// Determines whether the specified obj is satisfied by this criteria.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the specified obj is satisfied by this specification; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsSatisfiedBy(T obj);

        /// <summary>
        /// Operates a logical <c>Not</c> with this specification and the specified one.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>A <c>Not</c> specification</returns>
        public Specification<T> Not()
        {
            return !this;
        }

        /// <summary>
        /// Operates a logical <c>Ot</c> with this specification and the specified one.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>A <c>Or</c> specification</returns>
        public CompositeSpecification<T> Or(Specification<T> specification)
        {
            return this | specification;
        }

        #endregion Methods
    }
}