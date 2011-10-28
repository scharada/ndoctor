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
namespace Probel.NDoctor.View.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Globalization;
    using System.Threading;

    using Probel.NDoctor.View.Plugins.Configuration;

    using StructureMap;

    public class MefPluginLoader : LogObject, IPluginLoader
    {
        #region Fields

        private IPluginConfigurationLoader configurationLoader = ObjectFactory.GetInstance<IPluginConfigurationLoader>();
        private List<PluginConfiguration> pluginConfiguration = new List<PluginConfiguration>();

        #endregion Fields

        #region Methods

        public void RetrievePlugins(PluginContainer container, IPluginHost host)
        {
            this.pluginConfiguration = configurationLoader.LoadConfiguration();
            this.Compose(container, host);
        }

        private void Compose(PluginContainer pluginContainer, IPluginHost host)
        {
            Logger.DebugFormat("Found {0} plugin(s) in configuration file", this.pluginConfiguration.Count);

            var activatedPluginCount = 0;
            var deactivatedPluginCount = 0;

            var catalog = new AggregateCatalog();

            foreach (var plugin in this.pluginConfiguration)
            {
                var directory = configurationLoader.GetPluginDir(plugin);
                this.Logger.DebugFormat("\tDir: {0}", directory);

                if (plugin.IsActivated)
                {
                    catalog.Catalogs.Add(new DirectoryCatalog(directory));
                    activatedPluginCount++;
                }
                else deactivatedPluginCount++;
            }

            this.Logger.DebugFormat("Activated plugin(s): {0}. Deactivated plugin(s): {1}", activatedPluginCount, deactivatedPluginCount);

            var container = new CompositionContainer(catalog);
            container.ComposeExportedValue<Version>("version", host.HostVersion);
            container.ComposeExportedValue<IPluginHost>("host", host);
            container.ComposeExportedValue<CultureInfo>("cultureInfo", Thread.CurrentThread.CurrentUICulture);

            var composition = new CompositionBatch();
            composition.AddPart(pluginContainer);

            container.Compose(composition);
        }

        #endregion Methods
    }
}