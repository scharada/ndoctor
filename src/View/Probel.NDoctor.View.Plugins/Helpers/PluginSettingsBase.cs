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

namespace Probel.NDoctor.View.Plugins.Helpers
{
    using System;
    using System.IO;

    using Nini.Config;
    using System.Text;

    /// <summary>
    /// Represents the base of a plugin configuration. It's meant to hide the underneath mechanism
    /// </summary>
    public abstract class PluginSettingsBase
    {
        #region Fields

        private readonly XmlConfigSource Source;

        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginSettingsBase"/> class.
        /// </summary>
        /// <param name="pluginName">Name of the plugin.</param>
        public PluginSettingsBase(string pluginName)
        {
            var fileName = string.Format("{0}\\Probel\\nDoctor\\{1}.plugin.config"
                   , Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                   , pluginName);

            this.BuildDefaultFileStream(fileName);

            Source = new XmlConfigSource(fileName);
        }

        private void BuildDefaultFileStream(string fileName)
        {
            if (!File.Exists(fileName))
            {
                var configStream = this.GetDefaultConfiguration();

                using (var reader = new StreamReader(configStream, Encoding.UTF8))
                using (var stream = File.Create(fileName))
                {
                    var writer = new StreamWriter(stream);
                    writer.Write(reader.ReadToEnd());
                    writer.Flush();
                }
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Saves the configuration into a file.
        /// </summary>
        public void Save()
        {
            this.Source.Save();
        }

        /// <summary>
        /// Gets the bool defined in the specified section and key of the plugin config file.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value if nothing is found.</param>
        /// <returns></returns>
        protected bool GetBoolean(string section, string key, bool defaultValue)
        {
            return Source.Configs[section].GetBoolean(key, defaultValue);
        }

        /// <summary>
        /// Gets the integer defined in the specified section and key of the plugin config file.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value if nothing is found.</param>
        /// <returns></returns>
        protected int GetInt(string section, string key, int defaultValue)
        {
            return Source.Configs[section].GetInt(key, defaultValue);
        }

        /// <summary>
        /// Gets the string defined in the specified section and key of the plugin config file.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value if nothing is found.</param>
        /// <returns></returns>
        protected string GetString(string section, string key, string defaultValue)
        {
            return Source.Configs[section].Get(key, defaultValue);
        }

        /// <summary>
        /// Sets the specified string in the specified section and key of the plugin config file.
        /// </summary>
        /// <param name="section">The config.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected void Set(string section, string key, object value)
        {
            this.Source.Configs[section].Set(key, value);
        }

        #endregion Methods

        /// <summary>
        /// Builds the default config file at the specified path with the
        /// specified value.
        /// </summary>
        /// <param name="fullname">The full name of the file to build.</param>
        /// <param name="value">The content of the configuration.</param>
        protected abstract Stream GetDefaultConfiguration();
    }
}