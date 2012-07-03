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

namespace Probel.NDoctor.Domain.Components
{
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;

    using Probel.NDoctor.Domain.Components.Properties;

    /// <summary>
    /// Whenever the execution of a method is not granted, this exception is thrown
    /// </summary>
    [Serializable]
    public class AuthorisationException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorisationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AuthorisationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorisationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public AuthorisationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        internal AuthorisationException(Type type, MethodInfo methodInfo)
            : this()
        {
            this.TargetType = type;
            this.CalledMethod = methodInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorisationException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected AuthorisationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorisationException"/> class.
        /// </summary>
        private AuthorisationException()
            : this(Messages.Ex_AuthorisationException)
        {
        }

        #endregion Constructors

        #region Properties

        public MethodInfo CalledMethod
        {
            get; private set;
        }

        public Type TargetType
        {
            get; private set;
        }

        #endregion Properties
    }
}