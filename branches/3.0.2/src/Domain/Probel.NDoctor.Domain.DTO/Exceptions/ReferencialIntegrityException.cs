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
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    using Probel.NDoctor.Domain.DTO.Properties;

    [Serializable]
    public class ReferencialIntegrityException : ApplicationException
    {
        #region Constructors

        public ReferencialIntegrityException()
            : this(Messages.Ex_ReferencialIntegrityException)
        {
        }

        public ReferencialIntegrityException(string message)
            : base(message)
        {
        }

        public ReferencialIntegrityException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ReferencialIntegrityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion Constructors
    }
}