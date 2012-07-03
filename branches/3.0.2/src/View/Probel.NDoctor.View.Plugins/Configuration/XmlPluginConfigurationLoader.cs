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
namespace Probel.NDoctor.View.Plugins.Configuration
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using log4net;

    using Probel.Helpers.Strings;
    using Probel.NDoctor.View.Plugins.Exceptions;
    using Probel.NDoctor.View.Plugins.Properties;

    /// <summary>
    /// Retrieve the configuration of a plugin
    /// </summary>
    public class XmlPluginConfigurationLoader : IPluginConfigurationLoader
    {
        #region Fields

        private string CONFIG_FILE;
        private string CONFIG_PATH;
        private ILog logger = LogManager.GetLogger(typeof(XmlPluginConfigurationLoader));
        private string REPOSITORY_PATH;

        #endregion Fields

        #region Constructors

        public XmlPluginConfigurationLoader(string configPath, string configFile, string xPath)
        {
            logger.DebugFormat("Config path: {0}", configPath);
            logger.DebugFormat("Config file: {0}", configFile);
            logger.DebugFormat("XPath: {0}", xPath);

            this.REPOSITORY_PATH = xPath;
            this.CONFIG_FILE = configFile;
            this.CONFIG_PATH = configPath;
        }

        #endregion Constructors

        #region Methods

        public string GetPluginDir(PluginConfiguration plugin)
        {
            var pluginDir = Path.Combine(CONFIG_PATH, plugin.Path);
            if (!Directory.Exists(pluginDir))
            {
                throw new FileNotFoundException(Messages.Ex_PluginNotFoundException.FormatWith(plugin.Menu, pluginDir));
            }
            return pluginDir;
        }

        public List<PluginConfiguration> LoadConfiguration()
        {
            List<PluginConfiguration> configuration = new List<PluginConfiguration>();

            var elements = XDocument.Load(Path.Combine(CONFIG_PATH, CONFIG_FILE))
                .XPathSelectElements(REPOSITORY_PATH);
            foreach (var element in elements)
            {
                string path, menu;

                if (element.Attribute("path") == null)
                {
                    var msg = string.Format(Messages.Msg_MissingParameterInConfiguration, "path");
                    throw new PluginConfigurationException(msg);
                }
                else path = element.Attribute("path").Value;

                if (element.Attribute("name") == null)
                {
                    var msg = string.Format(Messages.Msg_MissingParameterInConfiguration, "name");
                    throw new PluginConfigurationException(msg);
                }
                else menu = element.Attribute("name").Value;

                var activated = false;
                if (element.Attribute("activated") != null)
                    bool.TryParse(element.Attribute("activated").Value, out activated);

                configuration.Add(new PluginConfiguration()
                {
                    Path = (string.IsNullOrEmpty(path) ? string.Empty : path),
                    Menu = (string.IsNullOrEmpty(menu) ? string.Empty : menu),
                    IsActivated = activated,
                });

            }
            return configuration;
        }

        #endregion Methods
    }
}