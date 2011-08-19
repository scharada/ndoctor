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
    using System.Windows.Controls;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.PluginHost.Core.Properties;

    public abstract class Plugin : IPlugin
    {
        #region Fields

        public readonly string GROUP_MANAGERS = Messages.Msg_GroupManager;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the cosntraint this plugin needs to fullfill to workkproperly.
        /// </summary>
        public DatabaseConstraint Constraint
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the name of the contextual menu.
        /// </summary>
        /// <value>
        /// The name of the contextual menu.
        /// </value>
        public string ContextualMenuName
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the plugin host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public IPluginHost Host
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the plugin is on error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if on error; otherwise, <c>false</c>.
        /// </value>
        public bool OnError
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the context menu of this plugin.
        /// </summary>
        /// <value>
        /// The context menu.
        /// </value>
        protected MenuInfo[] ContextMenus
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the global menu. Usually used if this plugin is not a patient session 
        /// based plugin
        /// </summary>
        /// <value>
        /// The global menu.
        /// </value>
        protected MenuInfo GlobalMenu
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ribbon menu. Usually used to add a patient session based plugin
        /// </summary>
        /// <value>
        /// The ribbon menu.
        /// </value>
        protected MenuInfo RibbonMenu
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the workbench. That's the main gui item to manage this plugin
        /// </summary>
        /// <value>
        /// The workbench.
        /// </value>
        protected UserControl Workbench
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Save the state of the plugin.
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// Displays the plugin into the GUI.
        /// </summary>
        public virtual void Setup()
        {
            Assert.IsNotNull(this.Host, "The plugin host is null!");

            if (this.GlobalMenu != null) this.Host.SetGlobalMenu(this.GlobalMenu);
            if (this.RibbonMenu != null) this.Host.SetRibbonMenu(this.RibbonMenu);
        }

        /// <summary>
        /// Setups the plugin. Typically, loads the plugin in memory
        /// </summary>
        public void Show()
        {
            if (this.OnError) this.DisplayGuiOnError();
            else this.DisplayGui();
        }

        public void Validate(Version databaseVersion)
        {
            Assert.IsNotNull(databaseVersion, "The database version is null");
            Assert.IsNotNull(this.Constraint, "The constraint is null");

            if (this.Constraint.Version <= databaseVersion && this.Constraint.Constraint == Constraints.Minimum)
            {
                this.OnError = false;
            }
            else if (this.Constraint.Version == databaseVersion && this.Constraint.Constraint == Constraints.Strict)
            {
                this.OnError = false;
            }
            else
            {
                this.OnError = true;
            }
        }

        /// <summary>
        /// Display the plugin with an error message saying the plugin is on error.
        /// </summary>
        protected abstract void DisplayGuiOnError();

        /// <summary>
        /// Display the gui items of the plugin into the host.
        /// </summary>
        private void DisplayGui()
        {
            this.Host.SetContextualMenu(this.ContextualMenuName, this.ContextMenus);
            this.Host.SetWorkbench(this.Workbench);
        }

        #endregion Methods
    }
}