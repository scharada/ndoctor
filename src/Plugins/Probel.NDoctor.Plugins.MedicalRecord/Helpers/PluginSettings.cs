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
    using System.IO;
    using System.Windows.Media;

    using Nini.Config;

    using Probel.NDoctor.View.Core.Helpers;

    public class PluginSettings
    {
        #region Fields

        private const string DefaultFont = "Arial";
        private const int DefaultSize = 16;
        private const string PLUGIN_PATH = @"Plugins\MedicalRecord";

        private readonly IConfigSource Source = new XmlConfigSource(Path.Combine(PLUGIN_PATH, "Plugin.config"));

        private static string CONFIG = "TextEditor";

        #endregion Fields

        #region Properties

        public FontFamily FontFamily
        {
            get { return new FontFamily(this.Source.Configs[CONFIG].Get("FontFamily", DefaultFont)); }
            set { this.Source.Configs[CONFIG].Set("FontFamily", value.Source); }
        }

        public int FontSize
        {
            get { return this.Source.Configs[CONFIG].GetInt("FontSize", DefaultSize); }
            set { this.Source.Configs[CONFIG].Set("FontSize", value); }
        }

        #endregion Properties

        #region Methods

        public void Save()
        {
            this.Source.Save();
        }

        #endregion Methods
    }
}