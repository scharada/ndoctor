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
namespace Progebel.Helpers.Assertion.Message
{
    using Progebel.Helpers.Assertion.Constraint;

    /// <summary>
    /// It formats the assertion message into a generic way.S
    /// </summary>
    public interface IMessageWriter
    {
        #region Methods

        /// <summary>
        /// Writes the difference between the constaint and the actual value into a message.
        /// </summary>
        /// <param name="constraint">The constraint.</param>
        string BuildMessage(IConstraint constraint);

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        string ToString();

        #endregion Methods
    }
}