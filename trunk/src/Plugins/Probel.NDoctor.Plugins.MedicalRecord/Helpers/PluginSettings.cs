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

namespace Probel.NDoctor.Plugins.MedicalRecord.Helpers
{
    using System.Windows.Media;

    using Probel.NDoctor.View.Plugins.Helpers;
    using System.Reflection;
    using System;
    using System.IO;
    using System.Text;

    public class PluginSettings : PluginSettingsBase
    {
        #region Fields

        private const string DefaultFont = "Arial";
        private const int DefaultSize = 16;

        private static string CONFIG = "TextEditor";

        #endregion Fields

        #region Constructors

        public PluginSettings()
            : base("MedicalRecord")
        {
        }

        #endregion Constructors

        #region Properties

        public FontFamily FontFamily
        {
            get { return new FontFamily(this.GetString(CONFIG, "FontFamily", DefaultFont)); }
            set { this.Set(CONFIG, "FontFamily", value.Source); }
        }

        public int FontSize
        {
            get { return this.GetInt(CONFIG, "FontSize", DefaultSize); }
            set { this.Set(CONFIG, "FontSize", value); }
        }

        #endregion Properties
        /// <summary>
        /// Builds the default config file at the specified path with the
        /// specified value.
        /// </summary>
        /// <returns></returns>
        protected override Stream GetDefaultConfiguration()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Probel.NDoctor.Plugins.MedicalRecord.Plugin.config");
            if (stream == null) { throw new NullReferenceException("The embedded default configuration can't be loaded or doesn't exist."); }
            else { return stream; }
        }
    }
}