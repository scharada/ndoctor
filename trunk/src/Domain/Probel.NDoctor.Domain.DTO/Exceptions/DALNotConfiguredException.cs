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
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    using Probel.NDoctor.Domain.DTO.Properties;

    /// <summary>
    /// Thrown when StructureMap is not configured in the DAL library
    /// </summary>
    [Serializable]
    public class DALNotConfiguredException : Exception
    {
        #region Constructors

        public DALNotConfiguredException()
            : base(Messages.Ex_DALNotConfiguredException)
        {
        }

        public DALNotConfiguredException(string message)
            : base(message)
        {
        }

        public DALNotConfiguredException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DALNotConfiguredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion Constructors
    }
}