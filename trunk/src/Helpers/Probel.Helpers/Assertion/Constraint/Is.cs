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
namespace Probel.Helpers.Assertion.Constraint
{
    using System;

    /// <summary>
    /// Constraint factory
    /// </summary>
    public static class Is
    {
        #region Properties

        /// <summary>
        /// Gets the False constaint.
        /// </summary>
        /// <value>The false.</value>
        public static IConstraint False
        {
            get { return new FalseConstraint(); }
        }

        /// <summary>
        /// Gets the null constraint.
        /// </summary>
        /// <value>The null.</value>
        public static IConstraint NotNull
        {
            get { return new NotNullConstraint(); }
        }

        /// <summary>
        /// Gets the null constraint.
        /// </summary>
        /// <value>The null.</value>
        public static IConstraint Null
        {
            get { return new NullConstraint(); }
        }

        /// <summary>
        /// Gets the True constraint.
        /// </summary>
        /// <value>The true.</value>
        public static IConstraint True
        {
            get { return new TrueConstraint(); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the constraint of Type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static IConstraint OfType(Type type)
        {
            return new OfTypeConstraint(type);
        }

        #endregion Methods
    }
}