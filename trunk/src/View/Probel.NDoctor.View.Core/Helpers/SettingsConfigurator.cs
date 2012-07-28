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

namespace Probel.NDoctor.View.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Controls;

    using Probel.Helpers.Strings;
    using Probel.NDoctor.View.Core.Properties;
    using Probel.NDoctor.View.Core.View;

    /// <summary>
    /// Contains all the plugin settings. This is used to store all the UserControls that are used
    /// for plugin configuration
    /// </summary>
    public class SettingsConfigurator
    {
        #region Fields

        private static readonly ObservableCollection<SettingUi> settingControls;

        #endregion Fields

        #region Constructors

        static SettingsConfigurator()
        {
            settingControls = new ObservableCollection<SettingUi>();
            settingControls.Add(new SettingUi(Messages.Title_DefaultSettings, new DefaultSettingsView()));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the setting controls.
        /// </summary>
        internal SettingUi[] Controls
        {
            get { return settingControls.ToArray(); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds a new settings UI for a plugin with a name for the setting tab.
        /// </summary>
        /// <param name="name">The name of the control. It will be used in the tabs to choose the settings control.</param>
        /// <param name="control">The control that contains all the settings logic.</param>
        public void Add(string name, UserControl control)
        {
            if (this.Contains(name)) { throw new ArgumentException("A setting UI has already been configured with the name '{0}'".FormatWith(name)); }
            else
            {
                settingControls.Add(new SettingUi(name, control));
            }
        }

        /// <summary>
        /// Determines whether a control with the specified name has already been configured.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if a control with the specified name has been configured; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string name)
        {
            foreach (var item in settingControls)
            {
                if (item.Name == name) return true;
            }
            return false;
        }

        #endregion Methods
    }

    public class SettingUi
    {
        #region Constructors

        internal SettingUi(string name, UserControl control)
        {
            this.Name = name;
            this.Control = control;
        }

        #endregion Constructors

        #region Properties

        public UserControl Control
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        #endregion Properties
    }
}