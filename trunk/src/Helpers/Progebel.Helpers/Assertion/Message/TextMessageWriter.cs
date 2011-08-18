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
    /// Provides featurs to format a message
    /// </summary>
    public class TextMessageWriter : IMessageWriter
    {
        #region Fields

        private string header = "The assertion failed: ";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextMessageWriter"/> class.
        /// </summary>
        public TextMessageWriter()
        {
            this.Message = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextMessageWriter"/> class.
        /// </summary>
        /// <param name="header">The header.</param>
        public TextMessageWriter(object header)
            : this()
        {
            this.header += header.ToString();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        private string Message
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Writes the difference between the constraint and the actual value into a message.
        /// </summary>
        /// <param name="constraint">The constraint.</param>
        public string BuildMessage(IConstraint constraint)
        {
            this.Message = string.Format("Expected: '{0}' But was : '{1}'"
                , constraint.Description
                , this.GetString(constraint.Actual));

            this.BuildHeader(this.Message, constraint);
            return this.Message;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Message;
        }

        private void BuildHeader(string message, IConstraint constraint)
        {
            if (this.header != string.Empty)
                this.Message = header + " - " + this.Message;
        }

        private string GetString(object value)
        {
            if (value == null) return "NULL";
            else return value.ToString();
        }

        #endregion Methods
    }
}