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
    using System.IO;

    using Nini.Config;

    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.GoogleCalendar;

    public class PluginSettings
    {
        #region Fields

        private const string DEFAULT_CAL = "https://www.google.com/calendar/feeds/default/private/full";
        private const string PLUGIN_PATH = @"Plugins\MeetingManager";

        private readonly IConfigSource Source = new XmlConfigSource(Path.Combine(PLUGIN_PATH, "Plugin.config"));

        private static string CONFIG = "GoogleCalendar";

        #endregion Fields

        #region Properties

        public string BindedCalendar
        {
            get { return this.Source.Configs[CONFIG].Get("BindedCalendar", DEFAULT_CAL); }
            set { this.Source.Configs[CONFIG].Set("BindedCalendar", value); }
        }

        public bool IsGoogleCalendarActivated
        {
            get { return this.Source.Configs[CONFIG].GetBoolean("IsGoogleCalendarEnabled", false); }
            set { this.Source.Configs[CONFIG].Set("IsGoogleCalendarEnabled", value); }
        }

        public string Password
        {
            get { return this.Source.Configs[CONFIG].Get("Password", string.Empty).Decrypt(); }
            set { this.Source.Configs[CONFIG].Set("Password", value.Encrypt()); }
        }

        public string UserName
        {
            get { return this.Source.Configs[CONFIG].Get("UserName", string.Empty); }
            set { this.Source.Configs[CONFIG].Set("UserName", value); }
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

        public void Save()
        {
            this.Source.Save();
        }

        #endregion Methods
    }
}