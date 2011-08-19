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
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace Probel.NDoctor.PluginHost.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.PluginHost.Core.Exceptions;

    public class PluginContainer
    {
        #region Fields

        public List<IPlugin> plugins = new List<IPlugin>();

        private Version databaseVersion;
        private IPluginHost host;
        private IPluginLoader loader;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginContainer"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="loader">The loader.</param>
        /// <param name="databaseVersion">The database version.</param>
        public PluginContainer(IPluginHost host, IPluginLoader loader, Version databaseVersion)
        {
            Assert.IsNotNull(loader, "The plugin loader is null");
            Assert.IsNotNull(host, "The plugin host is null");

            this.host = host;
            this.loader = loader;
            this.databaseVersion = databaseVersion;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a value indicating whether on of the loaded plugins is on error.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has plugin on error; otherwise, <c>false</c>.
        /// </value>
        public bool HasPluginOnError
        {
            get
            {
                return (from plugin in this.plugins
                        where plugin.OnError == true
                        select plugin).Count() > 0;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Setups the plugins.
        /// </summary>
        public void SetupPlugins()
        {
            try
            {
                this.loader.RetrievePlugins(this.host);
                this.plugins = this.loader.Plugins;

                foreach (var plugin in this.plugins)
                {
                    plugin.Validate(databaseVersion);
                    plugin.Setup();
                }
            }
            catch (Exception ex) { throw new PluginException(ex.Message, ex); }
        }

        #endregion Methods
    }
}