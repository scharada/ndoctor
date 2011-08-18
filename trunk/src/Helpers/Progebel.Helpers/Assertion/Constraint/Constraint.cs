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
    /// Base information of a constraint
    /// </summary>
    internal abstract class Constraint
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Constraint"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="expected">The expected.</param>
        public Constraint(string description, object expected)
        {
            this.Description = description;
            this.Expected = expected;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the last value used in the Match method of the constaint.
        /// </summary>
        /// <value>The actual.</value>
        public object Actual
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the of the expected value.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the description of the expected value of the constaint.
        /// </summary>
        /// <value>The description.</value>
        public object Expected
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Matches the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public virtual bool Match(object expression)
        {
            this.Actual = expression;
            return true;
        }

        #endregion Methods
    }
}