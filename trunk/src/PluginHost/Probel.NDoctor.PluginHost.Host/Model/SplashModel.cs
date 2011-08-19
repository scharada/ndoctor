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

namespace Probel.NDoctor.PluginHost.Host.Model
{
    using System;

    using log4net;

    using Probel.NDoctor.Domain.DAL.Confirugration;
    using Probel.NDoctor.PluginHost.Core;
    using Probel.NDoctor.PluginHost.Host.View;
    using Probel.NDoctor.PluginHost.Host.Properties;

    /// <summary>
    /// Work done by the splash screen
    /// </summary>
    public class SplashModel
    {
        #region Fields

        IPluginHost host;
        private ILog log = LogManager.GetLogger(typeof(SplashModel));

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SplashModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public SplashModel(IPluginHost host)
        {
            this.host = host;

            this.SplashWork = (Action<ISplashScreen>)delegate(ISplashScreen splash)
            {
                this.log.Warn("This configuration code should be in the spashscreen!");

                splash.SetStatus(10, "Configuring nHibernate...");
                var manager = new SQLiteManager();
                manager.ConfigureDatabaseAsFile(DalSettings.DbPath)
                       .CreateDb()
                       .CreateSessionFactory();

                splash.SetStatus(50, "Configuring plugins...");
                var container = new PluginContainer(this.host, new MefLoader(), new Version("0.0.0.0"));
                container.SetupPlugins();
                this.host.WriteStatus(StatusType.Info, Messages.Msg_Ready);
            };
        }

        #endregion Constructors

        #region Methods

        public Action<ISplashScreen> SplashWork
        {
            get;
            private set;
        }

        #endregion Methods
    }
}