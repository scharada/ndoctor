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
    using System.ComponentModel.Composition.Primitives;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Input;

    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Cfg;

    public class MefPluginLoader : LogObject, IPluginLoader
    {
        #region Fields

        private const string DEBUG_PLUGIN = "{1A5224ED-3E37-4AD8-AB2B-FBC0115434FA}";

        private readonly List<Guid> IdCollection = new List<Guid>();
        private readonly string Repository = @".\Plugins";

        #endregion Fields

        #region Constructors

        public MefPluginLoader(string repository, PluginsConfigurationFolder folder)
        {
            this.Repository = repository;
            this.PluginConfiguration = folder;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the plugin configuration.
        /// </summary>
        /// <value>
        /// The plugin configuration.
        /// </value>
        public PluginsConfigurationFolder PluginConfiguration
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public void RetrievePlugins(PluginContainer container, IPluginHost host)
        {
            this.Compose(container, host);
        }

        private void CheckName(ComposablePartDefinition def)
        {
            if (!def.Metadata.ContainsKey(Keys.PluginId))
            {
                var msg = string.Format("There's a plugin without id. Check you've set the attribute Metadata to the plugin with the key '{0}'", Keys.PluginId);
            #if DEBUG
                throw new NotImplementedException(msg);
            #else
                Logger.Warn(msg);
            #endif
            }
            else
            {
                Guid id;
                if (!(Guid.TryParse(def.Metadata[Keys.PluginId].ToString(), out id)))
                {
                    throw new Exception(string.Format(
                        "The plugin doen't have a valid id. Check you've set the attribute Metadata to the plugin with the key '{0}'", Keys.PluginId));
                }
                if (this.IdCollection.Contains(id))
                {
                    throw new Exception(string.Format(
                        "A plugin with the same id already exist in the repository [id: {0}]", id.ToString()));
                }
            }
        }

        private void Compose(PluginContainer pluginContainer, IPluginHost host)
        {
            var catalog = new AggregateCatalog();
            var count = 0;
            foreach (var directory in Directory.GetDirectories(Repository))
            {
                Logger.DebugFormat("Found plugin in '{0}'", directory);
                catalog.Catalogs.Add(new DirectoryCatalog(directory));
                count++;
            }
            Logger.InfoFormat("Found {0} plugin(s) in the repository. (Validation not done)", count);

            var filteredCatatog = new FilteredCatalog(catalog, def => this.IsPluginValid(def) && this.IsActivated(def));

            var container = new CompositionContainer(filteredCatatog);
            var composition = new CompositionBatch();
            composition.AddPart(pluginContainer);

            container.Compose(composition);
        }

        private bool IsActivated(ComposablePartDefinition def)
        {
            if (def.Metadata.ContainsKey(Keys.PluginId))
            {
                var id = def.Metadata[Keys.PluginId].ToString();
            #if DEBUG
                //The debug plugin should be activated if in debug mode
                if (this.PluginConfiguration.Exist(Guid.Parse(DEBUG_PLUGIN)) && id == DEBUG_PLUGIN) { return true; }
            #endif
                return (this.PluginConfiguration[id].IsMandatory)
                    ? true
                    : this.PluginConfiguration[id].IsActivated;

            }
            else { return false; }
        }

        private bool IsPluginValid(ComposablePartDefinition def)
        {
            this.CheckName(def);

            if (def.Metadata.ContainsKey(Keys.Constraint))
            {
                return new Constraint(def.Metadata[Keys.Constraint].ToString()).IsValid(PluginContext.Host);
            }
            else
            {
            #if DEBUG
                throw new NotImplementedException("A plugin without validation contract was found. It is ignored. Check that you decorated the plugin with a 'PartMetadata' attribute.");
            #else
                Logger.Warn("A plugin without validation contract was found. It is ignored. Check that you decorated the plugin with a 'PartMetadata' attribute.");
                return false;
            #endif
            }
        }

        #endregion Methods
    }
}