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

namespace Probel.NDoctor.Domain.DTO.GoogleCalendar
{
    /// <summary>
    /// This binding will be used to connect to a Google Calendar and work with the appointments.
    /// For instance it'll create appointments into the configured Google Calendar or retrieve the
    /// appointments to retrieve and display in the nDoctor Agenda
    /// </summary>
    public class GoogleConfiguration
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleConfiguration"/> class.
        /// </summary>
        public GoogleConfiguration()
        {
            this.IsActive = false;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Provides an instance that won't use the Google Calendar Binding.
        /// </summary>
        public static GoogleConfiguration NotBinded
        {
            get { return new GoogleConfiguration(); }
        }

        /// <summary>
        /// Gets or sets the calendar URI used to retrieve or create new appointments.
        /// </summary>
        /// <value>
        /// The calendar URI.
        /// </value>
        public string CalendarUri
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the binding with Google Calendar is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password to connect to the configured Google Calendar.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the user that will be used to connect to the configured Google Calendar.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName
        {
            get;
            set;
        }

        #endregion Properties
    }
}