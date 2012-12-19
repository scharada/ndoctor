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

namespace Probel.NDoctor.Domain.DTO.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    using Probel.NDoctor.Domain.DTO.Properties;

    [Serializable]
    public class ComponentException : TranslateableException
    {
        #region Fields

        private const string ErrorMsg = "An error occured in the DAL components. See inner exception for details.";

        #endregion Fields

        #region Constructors

        public ComponentException()
            : this(ErrorMsg, Messages.Ex_ComponentException)
        {
        }

        public ComponentException(Exception inner)
            : this(ErrorMsg, Messages.Ex_ComponentException, inner)
        {
        }

        public ComponentException(string message, string translated)
            : base(message, translated)
        {
        }

        public ComponentException(string message, string translated, Exception inner)
            : base(message, translated, inner)
        {
        }

        protected ComponentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion Constructors
    }
}