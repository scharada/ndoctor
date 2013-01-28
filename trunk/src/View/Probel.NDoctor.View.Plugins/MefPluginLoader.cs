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

    using Probel.NDoctor.View.Plugins.Helpers;

    public class MefPluginLoader : LogObject, IPluginLoader
    {
        #region Fields

        private readonly string Repository = @".\Plugins";

        #endregion Fields

        #region Constructors

        public MefPluginLoader(string repository)
        {
            this.Repository = repository;
        }

        #endregion Constructors

        #region Methods

        public void RetrievePlugins(PluginContainer container, IPluginHost host)
        {
            this.Compose(container, host);
        }

        private void CheckName(ComposablePartDefinition def)
        {
            if (!def.Metadata.ContainsKey(Keys.PluginName))
            {
                var msg = "You forgot to give a name to a plugin. Check you've set the attribute Metadata to the plugin with the key 'PluginName'";
            #if DEBUG
                throw new NotImplementedException(msg);
            #else
                Logger.Warn(msg);
            #endif
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

            var filteredCatatog = new FilteredCatalog(catalog, def => this.IsPluginValid(def));

            var container = new CompositionContainer(filteredCatatog);
            var composition = new CompositionBatch();
            composition.AddPart(pluginContainer);

            container.Compose(composition);
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