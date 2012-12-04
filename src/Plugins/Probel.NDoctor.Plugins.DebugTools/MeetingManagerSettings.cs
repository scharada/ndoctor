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

namespace Probel.NDoctor.Plugins.DebugTools
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.GoogleCalendar;
    using Probel.NDoctor.View.Plugins.Helpers;

    internal class MeetingManagerSettings : PluginSettingsBase
    {
        #region Fields

        private const string DEFAULT_CAL = "https://www.google.com/calendar/feeds/default/private/full";

        private static string CONFIG = "GoogleCalendar";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginSettings"/> class.
        /// </summary>
        public MeetingManagerSettings()
            : base("MeetingManager")
        {
        }

        #endregion Constructors

        #region Properties

        public string BindedCalendar
        {
            get { return this.GetString(CONFIG, "BindedCalendar", DEFAULT_CAL); }
            set { this.Set(CONFIG, "BindedCalendar", value); }
        }

        public bool IsGoogleCalendarActivated
        {
            get { return this.GetBoolean(CONFIG, "IsGoogleCalendarEnabled", false); }
            set { this.Set(CONFIG, "IsGoogleCalendarEnabled", value); }
        }

        public string Password
        {
            get { return this.GetString(CONFIG, "Password", string.Empty).Decrypt(); }
            set { this.Set(CONFIG, "Password", value.Encrypt()); }
        }

        public string UserName
        {
            get { return this.GetString(CONFIG, "UserName", string.Empty); }
            set { this.Set(CONFIG, "UserName", value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets a google configuration using the settings.
        /// </summary>
        /// <returns></returns>
        public GoogleConfiguration GetGoogleConfiguration()
        {
            return new GoogleConfiguration()
            {
                CalendarUri = this.BindedCalendar,
                IsActive = this.IsGoogleCalendarActivated,
                Password = this.Password,
                UserName = this.UserName,
            };
        }

        /// <summary>
        /// Builds the default config file at the specified path with the
        /// specified value.
        /// </summary>
        /// <returns></returns>
        protected override Stream GetDefaultConfiguration()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Probel.NDoctor.Plugins.DebugTools.MeetingManager.config");
            if (stream == null) { throw new NullReferenceException("The embedded default configuration can't be loaded or doesn't exist."); }
            else { return stream; }
        }

        #endregion Methods
    }
}