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
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace Probel.Helpers.Assertion.Constraint
{
    /// <summary>
    /// Checks if the specified expression is a boolean with "True" value.
    /// </summary>
    internal class TrueConstraint : Constraint, IConstraint
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueConstraint"/> class.
        /// </summary>
        public TrueConstraint()
            : base("True", true)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Matches the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public override bool Match(object expression)
        {
            base.Match(expression);
            return ((bool)this.Actual == true);
        }

        #endregion Methods
    }
}