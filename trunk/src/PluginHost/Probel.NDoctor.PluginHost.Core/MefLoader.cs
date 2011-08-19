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
namespace Probel.NDoctor.PluginHost.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;

    public class MefLoader : IPluginLoader
    {
        #region Fields

        private string directory = string.Empty;

        #endregion Fields

        #region Constructors

        public MefLoader()
            : this(Environment.CurrentDirectory)
        {
        }

        public MefLoader(string directory)
        {
            this.directory = directory;
        }

        #endregion Constructors

        #region Properties

        [ImportMany]
        public List<IPlugin> Plugins
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public void RetrievePlugins(IPluginHost host)
        {
            var catalog = new DirectoryCatalog(this.directory);
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);

            foreach (var plugin in this.Plugins)
            {
                plugin.Host = host;
            }
        }

        #endregion Methods
    }
}