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
namespace Probel.NDoctor.Plugins.PatientSession.Helpers
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.GoogleCalendar;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.View.Plugins;

    public class PluginSettings : PluginSettingsBase
    {
        #region Fields

        private const string BIRTHDATE = "ShowBirthDate";
        private const string CITY = "ShowCity";
        private const string INSCRIPTION = "ShowInscriptionDate";
        private const string LAST_UPDATE = "ShowLastUpdate";
        private const string PROFESSION = "ShowProfession";
        private const string REASON = "ShowReason";
        private const string SECTION = "Criterion";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginSettings"/> class.
        /// </summary>
        public PluginSettings()
            : base("PatientSession")
        {
        }

        #endregion Constructors

        #region Properties

        public bool ShowBirthdate
        {
            get { return this.GetValue(BIRTHDATE); }
            set { this.SetValue(BIRTHDATE, value); }
        }

        public bool ShowCity
        {
            get { return this.GetValue(BIRTHDATE); }
            set { this.SetValue(CITY, value); }
        }

        public bool ShowInscription
        {
            get { return this.GetValue(INSCRIPTION); }
            set { this.SetValue(INSCRIPTION, value); }
        }

        public bool ShowLastUpdate
        {
            get { return this.GetValue(LAST_UPDATE); }
            set { this.SetValue(LAST_UPDATE, value); }
        }

        public bool ShowProfession
        {
            get { return this.GetValue(PROFESSION); }
            set { this.SetValue(PROFESSION, value); }
        }

        public bool SHowReason
        {
            get { return this.GetValue(REASON); }
            set { this.SetValue(REASON, value); }
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
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Probel.NDoctor.Plugins.PatientSession.Plugin.config");
            if (stream == null) { throw new NullReferenceException("The embedded default configuration can't be loaded or doesn't exist."); }
            else { return stream; }
        }

        private bool GetValue(string key)
        {
            return this.GetBoolean(SECTION, key, true);
        }

        private void SetValue(string key, bool value)
        {
            this.Set(SECTION, key, value);
        }

        #endregion Methods
    }
}