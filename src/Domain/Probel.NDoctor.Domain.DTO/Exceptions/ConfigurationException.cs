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

    [Serializable]
    public class ConfigurationException : TranslateableException
    {
        #region Constructors

        public ConfigurationException()
            : base("You already configured the DAL", Messages.Ex_ConfigurationException)
        {
        }

        public ConfigurationException(string message, string translated)
            : base(message, translated)
        {
        }

        public ConfigurationException(string message, string translated, Exception inner)
            : base(message, translated, inner)
        {
        }

        protected ConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion Constructors
    }
}