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

namespace Probel.NDoctor.Domain.DAL.Macro
{
    using System;

    using Probel.NDoctor.Domain.DAL.Properties;

    /// <summary>
    /// This exception is thrown when a macro is invalid or malformed
    /// </summary>
    [Serializable]
    public class InvalidMacroException : Exception
    {
        #region Constructors

        public InvalidMacroException()
            : this(Messages.Ex_InvalidMacroException)
        {
        }

        public InvalidMacroException(string message)
            : base(message)
        {
        }

        public InvalidMacroException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected InvalidMacroException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        #endregion Constructors
    }
}