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
namespace Probel.NDoctor.PluginHost.Host.ViewModel
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    using log4net;

    using Probel.NDoctor.Domain.DAL.Confirugration;
    using Probel.NDoctor.PluginHost.Core;
    using Probel.NDoctor.PluginHost.Host.Properties;

    /// <summary>
    /// ViewModel of the spash screen
    /// </summary>
    public class SpashScreenViewModel : ViewModel
    {
        #region Fields

        private IPluginHost host;
        private ILog log = LogManager.GetLogger(typeof(SpashScreenViewModel));
        private string message = string.Empty;
        private uint percentage = 0;

        #endregion Fields

        #region Constructors

        public SpashScreenViewModel(IPluginHost host)
        {
            this.host = host;
        }

        #endregion Constructors

        #region Events

        public event EventHandler Finished;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get { return this.message; }
            set
            {
                this.message = value;
                this.OnPropertyChanged("Message");
            }
        }

        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>
        /// The percentage.
        /// </value>
        public uint Percentage
        {
            get { return this.percentage; }
            set
            {
                this.percentage = value;
                this.OnPropertyChanged("Percentage");
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initialises the configuration of the application.
        /// </summary>
        public void Initialise()
        {
            this.log.Debug("Application started.");
            this.ConfigureNHibernate();
            this.Percentage = 50;

            this.ConfigurePlugins();
            this.Percentage = 100;
            this.host.WriteStatus(StatusType.Info, Messages.Msg_Ready);

            if (this.host is Window)
            {
                (this.host as Window).Show();
            }
            else
            {
                this.log.WarnFormat("Impossible to show the main window. The type of the host is '{0}' while '{1}' is expected."
                    , this.host.GetType()
                    , typeof(Window));
            }

            this.OnFinished();
        }

        private void ConfigureNHibernate()
        {
            try
            {
                this.Message = Messages.Msg_ConfiguringNHibernate;

                var manager = new SQLiteManager();
                manager.ConfigureDatabaseAsFile(DalSettings.DbPath)
                       .CreateDb()
                       .CreateSessionFactory();

                this.log.Debug("nHibernate is configured.");
            }
            catch (Exception ex)
            {
                this.log.Error("An error occured when configuring nHibernate.", ex);
                this.host.WriteStatus(StatusType.Error, Messages.Msg_ErrorOccuredOnNHibernateConfig);
                throw;
            }
        }

        private void ConfigurePlugins()
        {
            try
            {
                this.log.Warn("Plugin issue: the database versioning is not yet implemented. The version is hardcoded to '0.0.0.0'");
                this.Message = Messages.Msg_ConfiguringPlugins;

                var container = new PluginContainer(this.host, new MefLoader(), new Version("0.0.0.0"));
                container.SetupPlugins();
                this.log.Debug("Plugins are configured");
            }
            catch (Exception ex)
            {
                this.log.Error("An error occured when configuring the plugins", ex);
                this.host.WriteStatus(StatusType.Error, Messages.Msg_ErrorOccuredOnPluginConfig);
                throw;
            }
        }

        private void OnFinished()
        {
            if (this.Finished != null)
                this.Finished(this, EventArgs.Empty);
        }

        #endregion Methods
    }
}