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
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    using Probel.NDoctor.View.Plugins.Exceptions;
    using Probel.NDoctor.View.Plugins;

    public class PluginContainer : LogObject
    {
        #region Fields

        private IPluginLoader loader;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginContainer"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public PluginContainer(IPluginHost host, IPluginLoader loader)
        {
            PluginContext.Host = host;
            this.loader = loader;
            this.Plugins = new List<IPlugin>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the plugins.
        /// </summary>
        /// <value>
        /// The plugins.
        /// </value>
        [ImportMany]
        public IList<IPlugin> Plugins
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Loads the plugins.
        /// </summary>
        public void LoadPlugins()
        {
            this.loader.RetrievePlugins(this, PluginContext.Host);
            if (this.Plugins == null) throw new PluginsNotLoadedException();

            foreach (var plugin in this.Plugins)
            {
                plugin.Initialise();
            }
            this.Logger.InfoFormat("Loaded {0} valid plugin(s) for the version of the host [v{1}].", this.Plugins.Count, PluginContext.Host.HostVersion);
        }

        #endregion Methods
    }
}