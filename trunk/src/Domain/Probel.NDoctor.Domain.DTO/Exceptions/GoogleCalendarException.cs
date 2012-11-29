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

    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.Properties;

    /// <summary>
    /// This exception is thrown when an error occured during communication with Google Calendar
    /// </summary>
    [Serializable]
    public class GoogleCalendarException : TranslateableException
    {
        #region Fields

        private const string msg = "Google Calendar reports this error: {0}";
        private const string msgFormat = "An error occured when communicating with Google Calendar";

        #endregion Fields

        #region Constructors

        public GoogleCalendarException()
            : base(msg, Messages.Ex_GoogleCalendarException)
        {
        }

        public GoogleCalendarException(string message)
            : base(message, Messages.Ex_GoogleCalendarExceptionFormat.FormatWith(message))
        {
        }

        public GoogleCalendarException(string message, Exception inner)
            : base(msg.FormatWith(message), Messages.Ex_GoogleCalendarExceptionFormat.FormatWith(message), inner)
        {
        }

        protected GoogleCalendarException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion Constructors
    }
}