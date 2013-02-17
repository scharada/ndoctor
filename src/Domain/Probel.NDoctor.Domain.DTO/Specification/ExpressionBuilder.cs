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

namespace Probel.NDoctor.Domain.DTO.Specifications
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Builds an expression. It is used to build an expression when the user doesn't know when the expression will be instanciated
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExpressionBuilder<T>
    {
        #region Properties


        private readonly List<Specification<T>> Expressions = new List<Specification<T>>();

        #endregion Properties

        #region Methods


        /// <summary>
        /// Adds the specified spec into the internal specification collection.
        /// </summary>
        /// <param name="spec">The spec.</param>
        public void Add(Specification<T> spec)
        {
            this.Expressions.Add(spec);
        }

        public Specification<T> BuildOrExpression()
        {
            if (this.Expressions.Count == 0) { throw new NotSupportedException("The builder should contain al least 1 elements"); }
            else if (this.Expressions.Count == 1) { return this.Expressions[0]; }
            else 
            {
                Specification<T> expression = this.Expressions[0];
                for (int i = 1; i < Expressions.Count; i++)
                {
                    expression |= this.Expressions[i];
                }
                return expression;
            }
        }
        public Specification<T> BuildAndExpression()
        {
            if (this.Expressions.Count == 0) { throw new NotSupportedException("The builder should contain al least 1 elements"); }
            else if (this.Expressions.Count == 1) { return this.Expressions[0]; }
            else 
            {
                Specification<T> expression = this.Expressions[0];
                for (int i = 1; i < Expressions.Count; i++)
                {
                    expression &= this.Expressions[i];
                }
                return expression;
            }
        }
        public bool CanBuild
        {
            get { return this.Expressions.Count > 0; }
        }
        #endregion Methods
    }
}