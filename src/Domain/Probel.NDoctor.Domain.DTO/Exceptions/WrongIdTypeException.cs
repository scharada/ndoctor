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

    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.Properties;

    [Serializable]
    public class WrongIdTypeException : TranslateableException
    {
        #region Constructors

        public WrongIdTypeException(Type expected, Type current)
            : this("The ID type is '{0}' while expecting '{1}'".FormatWith(expected.ToString(), current.ToString())
            , Messages.Ex_DalSessionException_UnknownId.FormatWith(expected.ToString(), current.ToString()))
        {
        }

        public WrongIdTypeException(string message, string translated)
            : base(message, translated)
        {
        }

        public WrongIdTypeException(string message, string translated, Exception inner)
            : base(message, translated, inner)
        {
        }

        protected WrongIdTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion Constructors
    }
}