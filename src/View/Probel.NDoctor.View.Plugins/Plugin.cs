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
    using Probel.NDoctor.View.Toolbox.Navigation;

    public abstract class Plugin : IPlugin
    {
        #region Fields

        protected IErrorHandler Handle;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="host">The host.</param>
        /// <param name="cultureInfo">The culture info.</param>
        [ImportingConstructor]
        public Plugin()
        {
            this.Handle = new ErrorHandlerFactory().New(this);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public ILog Logger
        {
            get { return this.Handle.Logger; }
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

        protected RibbonContextualTabGroupData ContextualMenu
        {
            get;
            set;
        }

        protected CultureInfo CultureInfo
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initialises this plugin. Basicaly it should configure the menus into the PluginHost
        /// Every task that could throw exception should be in this method and not in the ctor.
        /// </summary>
        public abstract void Initialise();

        /// <summary>
        /// Shows the contextual menu of the current plugin.
        /// </summary>
        protected void ShowContextMenu()
        {
            if (this.ContextualMenu != null)
            {
                this.ContextualMenu.IsVisible = true;
                this.ContextualMenu.TabDataCollection[0].IsSelected = PluginContext.Configuration.AutomaticContextMenu;
            }
            else { this.Logger.WarnFormat("The contextual menu of '{0}' is not set. [Null Reference]", this.GetType()); }
        }

        #endregion Methods
    }
}