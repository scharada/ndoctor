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
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using Probel.NDoctor.View.Plugins.Helpers;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Primitives;

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
            if (def.Metadata.ContainsKey(Constraint.Name))
            {
                return new Constraint(def.Metadata[Constraint.Name].ToString()).IsValid(PluginContext.Host);
            }
            else { return false; }
        }

        #endregion Methods
    }
}