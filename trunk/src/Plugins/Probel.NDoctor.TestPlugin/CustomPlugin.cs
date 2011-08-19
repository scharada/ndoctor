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
namespace Probel.NDoctor.TestPlugin
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    using Probel.NDoctor.PluginHost.Core;
    using Probel.NDoctor.TestPlugin.Properties;

    [Export(typeof(IPlugin))]
    public class CustomPlugin : Plugin
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPlugin"/> class.
        /// </summary>
        public CustomPlugin()
        {
            this.ContextualMenuName = "Custom menu";
            this.Constraint = new DatabaseConstraint()
            {
                Constraint = Constraints.Minimum,
                Version = new Version("0.0.0.0"),
            };

            this.Workbench = new Workbench();
            var menu = new MenuInfo()
            {
                Group = GROUP_MANAGERS,
                Icon = Resources.Menu,
                Name = "Test Plugin",
                Parent = Menus.Home,
                Command = new RelayCommand(() =>
                {
                    (this.Workbench as Workbench).SetTime();
                    this.Show();
                }, () => true)
            };
            this.RibbonMenu = menu;
            this.GlobalMenu = menu;

            var menus = new List<MenuInfo>();
            for (int i = 0; i < 4; i++)
            {
                menu.Group = "Test contextual menu";
                menus.Add(menu);
            }
            this.ContextMenus = menus.ToArray();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Save the state of the plugin.
        /// </summary>
        public override void Save()
        {
            // Do nothing
        }

        /// <summary>
        /// Display the plugin with an error message saying the plugin is on error.
        /// </summary>
        protected override void DisplayGuiOnError()
        {
            throw new ApplicationException("The plugin 'CustomPlugin' is on error");
        }

        #endregion Methods
    }
}