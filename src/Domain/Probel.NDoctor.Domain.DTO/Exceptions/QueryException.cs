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
namespace Probel.NDoctor.Domain.DTO.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    using Probel.NDoctor.Domain.DTO.Properties;

    /// <summary>
    /// The exception that is thrown when query failed 
    /// </summary>
    [Serializable]
    public class DalQueryException : TranslateableException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DalQueryException"/> class.
        /// </summary>
        public DalQueryException()
            : this("An error occured during the execution of a query.", Messages.Ex_QueryException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DalQueryException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DalQueryException(string message, string translated)
            : base(message, translated)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DalQueryException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public DalQueryException(string message, string translated, Exception inner)
            : base(message, translated, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DalQueryException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected DalQueryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion Constructors
    }
}