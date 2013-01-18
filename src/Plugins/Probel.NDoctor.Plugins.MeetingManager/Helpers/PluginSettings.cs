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

namespace Probel.NDoctor.Plugins.MeetingManager.Helpers
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.GoogleCalendar;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class PluginSettings : PluginSettingsBase
    {
        #region Fields

        private const string DEFAULT_CAL = "https://www.google.com/calendar/feeds/default/private/full";

        private static string DEFAULT = "Calendar";
        private static string GOOGLE = "GoogleCalendar";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginSettings"/> class.
        /// </summary>
        public PluginSettings()
            : base("MeetingManager")
        {
        }

        #endregion Constructors

        #region Properties

        public string BindedCalendar
        {
            get { return this.GetString(GOOGLE, "BindedCalendar", DEFAULT_CAL); }
            set { this.Set(GOOGLE, "BindedCalendar", value); }
        }

        public bool IsGoogleCalendarActivated
        {
            get { return this.GetBoolean(GOOGLE, "IsGoogleCalendarEnabled", false); }
            set { this.Set(GOOGLE, "IsGoogleCalendarEnabled", value); }
        }

        public string Password
        {
            get { return this.GetString(GOOGLE, "Password", string.Empty).Decrypt(); }
            set { this.Set(GOOGLE, "Password", value.Encrypt()); }
        }

        public SlotDuration SlotDuration
        {
            get
            {
                var value = this.GetString(DEFAULT, "SlotDuration", "ThirtyMinutes");
                return (SlotDuration)Enum.Parse(typeof(SlotDuration), value);
            }
            set
            {
                this.Set(DEFAULT, "SlotDuration", value.ToString());
            }
        }

        public string UserName
        {
            get { return this.GetString(GOOGLE, "UserName", string.Empty); }
            set { this.Set(GOOGLE, "UserName", value); }
        }

        public Workday Workday
        {
            get
            {
                return new Workday(this.WorkdayStart, this.WorkdayEnd, this.SlotDuration);
            }
        }

        public string WorkdayEnd
        {
            get { return this.GetString(DEFAULT, "WorkdayEnd", "17:00"); }
            set { this.Set(DEFAULT, "WorkdayEnd", value); }
        }

        public string WorkdayStart
        {
            get { return this.GetString(DEFAULT, "WorkdayStart", "08:00"); }
            set { this.Set(DEFAULT, "WorkdayStart", value); }
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
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Probel.NDoctor.Plugins.MeetingManager.Plugin.config");
            if (stream == null) { throw new NullReferenceException("The embedded default configuration can't be loaded or doesn't exist."); }
            else { return stream; }
        }

        #endregion Methods
    }
}