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
namespace Probel.NDoctor.Plugins.DebugTools
{
    using System;
    using System.IO;
    using System.Reflection;

    using Probel.NDoctor.View;
    using Probel.NDoctor.View.Plugins;

    public class PluginSettings : PluginSettingsBase
    {
        #region Fields

        private static string CONFIG = "Debug";

        #endregion Fields

        #region Constructors

        public PluginSettings()
            : base("DebugTools")
        {
        }

        #endregion Constructors

        #region Properties

        public bool DefaultGoogleCalendarConfig
        {
            get { return this.GetBoolean(CONFIG, "DefaultGoogleCalendarConfig", true); }
            set { this.Set(CONFIG, "DefaultGoogleCalendarConfig", value); }
        }

        public string DefaultPatient
        {
            get { return this.GetString(CONFIG, "DefaultPatient", "Wautier"); }
            set { this.Set(CONFIG, "DefaultPatient", value); }
        }

        public bool InjectDefaultData
        {
            get { return this.GetBoolean(CONFIG, "InjectDefaultData", true); }
            set { this.Set(CONFIG, "InjectDefaultData", value); }
        }

        public bool IsGoogleActivated
        {
            get { return this.GetBoolean(CONFIG, "IsGoogleActivated", true); }
            set { this.Set(CONFIG, "IsGoogleActivated", value); }
        }

        public bool LoadDefaultUser
        {
            get { return this.GetBoolean(CONFIG, "LoadDefaultUser", true); }
            set { this.Set(CONFIG, "LoadDefaultUser", value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Builds the default config file at the specified path with the
        /// specified value.
        /// </summary>
        /// <returns></returns>
        protected override Stream GetDefaultConfiguration()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Probel.NDoctor.Plugins.DebugTools.Plugin.config");

            if (stream == null) { throw new NullReferenceException("The embedded default configuration can't be loaded or doesn't exist."); }
            else { return stream; }
        }

        #endregion Methods
    }
}