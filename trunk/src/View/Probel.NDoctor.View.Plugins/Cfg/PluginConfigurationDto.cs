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

namespace Probel.NDoctor.View.Plugins.Cfg
{
    using System;

    using Probel.Mvvm.DataBinding;

    public class PluginConfigurationDto : ObservableObject
    {
        #region Fields

        private string explanations;
        private Guid id;
        private bool isActivated;
        private bool isMandatory;
        private bool isRecommended;
        private string name;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the explanations on what the plugin does.
        /// </summary>
        /// <value>
        /// The explanations.
        /// </value>
        public string Explanations
        {
            get { return this.explanations; }
            set
            {
                this.explanations = value;
                this.OnPropertyChanged(() => explanations);
            }
        }

        /// <summary>
        /// Gets or sets the id of the plugin.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
                this.OnPropertyChanged(() => Id);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the plugin shoud be activated or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActivated
        {
            get { return this.isActivated; }
            set
            {
                this.isActivated = value;
                this.OnPropertyChanged(() => IsActivated);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this plugin is mandatory.
        /// </summary>
        /// <value>
        ///   <c>true</c> if mandatory; otherwise, <c>false</c>.
        /// </value>
        public bool IsMandatory
        {
            get { return this.isMandatory; }
            set
            {
                this.isMandatory = value;
                this.OnPropertyChanged(() => IsMandatory);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this plugin is recommended.
        /// </summary>
        /// <value>
        ///   <c>true</c> if recommended; otherwise, <c>false</c>.
        /// </value>
        public bool IsRecommended
        {
            get { return this.isRecommended; }
            set
            {
                this.isRecommended = value;
                this.OnPropertyChanged(() => IsRecommended);
            }
        }

        /// <summary>
        /// Gets or sets the name of the plugin. This is used in the GUI to have a human
        /// readable name
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged(() => Name);
            }
        }

        #endregion Properties
    }
}