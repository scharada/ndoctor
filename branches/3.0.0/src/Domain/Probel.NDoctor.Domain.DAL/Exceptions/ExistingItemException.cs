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
namespace Probel.NDoctor.Domain.DAL.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    using Probel.NDoctor.Domain.DAL.Properties;

    /// <summary>
    /// The exception that is thrown when 
    /// </summary>
    [Serializable]
    public class ExistingItemException : ApplicationException
    {
        #region Constructors

        public ExistingItemException()
            : this(Messages.Ex_ExistingItemException)
        {
        }

        public ExistingItemException(string message)
            : base(message)
        {
        }

        public ExistingItemException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ExistingItemException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion Constructors
    }
}