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
namespace Progebel.Helpers.Assertion.Constraint
{
    /// <summary>
    /// Checks if the specified expression matches the constraint.
    /// </summary>
    public interface IConstraint
    {
        #region Properties

        /// <summary>
        /// Gets or sets the last value used in the Match method of the constaint.
        /// </summary>
        /// <value>The actual.</value>
        object Actual
        {
            get;
        }

        /// <summary>
        /// Gets or sets the description of the constraint.
        /// </summary>
        /// <value>The description.</value>
        string Description
        {
            get;
        }

        /// <summary>
        /// Gets the description of the expected value of the constaint.
        /// </summary>
        /// <value>The description.</value>
        object Expected
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Check if the specified expression matches the constraint.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns><c>True</c> if the matc succeeded; otherwise returns <c>False</c></returns>
        bool Match(object expression);

        #endregion Methods
    }
}