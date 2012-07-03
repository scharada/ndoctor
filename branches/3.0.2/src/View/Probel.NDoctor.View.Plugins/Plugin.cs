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
    using System.Globalization;
    using System.Windows.Controls;

    using log4net;

    using Probel.NDoctor.View.Plugins.Exceptions;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;
    using Probel.NDoctor.View.Plugins.Properties;

    public abstract class Plugin : IErrorHandler, IPlugin
    {
        #region Fields

        private ErrorHandler errorHandler;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="host">The host.</param>
        /// <param name="cultureInfo">The culture info.</param>
        [ImportingConstructor]
        public Plugin(Version version)
        {
            PluginContext.Host.PluginsClosing += (sender, e) => this.Close();
            this.errorHandler = new ErrorHandler(this);
            this.Version = version;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        /// <param name="version">The version of the plugin</param>
        /// <param name="host">The host.</param>
        /// <param name="cultureInfo">The culture info.</param>
        protected Plugin(string version)
            : this(new Version(version))
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this plugin is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this plugin is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public ILog Logger
        {
            get { return this.errorHandler.Logger; }
        }

        /// <summary>
        /// Gets or sets the page that represents the workbench of the plugin.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        public Page Page
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the version of this plugin. This will be used with the <see cref="PluginValidator.cs"/>
        /// to check if this plugin can be validated or not.
        /// </summary>
        public Version Version
        {
            get;
            protected set;
        }

        protected RibbonContextualTabGroupData contextualMenu
        {
            get;
            set;
        }

        protected CultureInfo CultureInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the validator.
        /// </summary>
        /// <value>
        /// The validator.
        /// </value>
        protected PluginValidator Validator
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Activates this plugin. That's the PluginHost can display and the user can use this plugin.
        /// It means that all validations have succeeded on this plugin.
        /// </summary>
        public virtual void Activate()
        {
            this.IsActive = true;
        }

        /// <summary>
        /// Closes this plugin. That's unload all the data. Typically used when the connected user disconnect.
        /// </summary>        
        public virtual void Close()
        {
            //By default it does nothing
        }

        /// <summary>
        /// Deactivates this plugin. That's the PluginHost CAN'T display and the user CAN'T use this plugin.
        /// It means that at least one validation have FAILED on this plugin.
        /// </summary>
        public virtual void Deactivate()
        {
            this.IsActive = false;
        }

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        public void HandleError(Exception ex)
        {
            this.errorHandler.HandleError(ex);
        }

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void HandleError(Exception ex, string format, params object[] args)
        {
            this.errorHandler.HandleError(ex, format, args);
        }

        /// <summary>
        /// Handles the error, log it and DOESN'T show a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void HandleErrorSilently(Exception ex, string format, params object[] args)
        {
            this.errorHandler.HandleErrorSilently(ex, format, args);
        }

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// This error is showed as a warning
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void HandleWarning(Exception ex, string format, params object[] args)
        {
            this.errorHandler.HandleWarning(ex, format, args);
        }

        /// <summary>
        /// Handles the error, log it and DOESN'T show a message box with the error.
        /// This error is showed as a warning
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void HandleWarningSilently(Exception ex, string format, params object[] args)
        {
            this.errorHandler.HandleWarningSilently(ex, format, args);
        }

        /// <summary>
        /// Initialises this plugin. Basicaly it should configure the menus into the PluginHost
        /// Every task that could throw exception should be in this method and not in the ctor.
        /// </summary>
        public abstract void Initialise();

        /// <summary>
        /// Determines whether this plugin is valid refering to the host version.
        /// </summary>
        /// <param name="host"></param>
        /// <returns>
        ///   <c>true</c> if this plugin is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid(IPluginHost host)
        {
            if (this.Validator == null) throw new PluginException(Messages.Ex_PluginException_NoValidator);

            return this.Validator.IsValid(host);
        }

        /// <summary>
        /// Shows the contextual menu of the current plugin.
        /// </summary>
        protected void ShowContextMenu()
        {
            if (this.contextualMenu != null)
            {
                this.contextualMenu.IsVisible = true;
                this.contextualMenu.TabDataCollection[0].IsSelected = PluginContext.Configuration.AutomaticContextMenu;
            }
            else { this.Logger.WarnFormat("The contextual menu of '{0}' is not set. [Null Reference]", this.GetType()); }
        }

        #endregion Methods
    }
}