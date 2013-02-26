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
    using System.Xml.Serialization;

    /// <summary>
    /// Represents the configuration of the plugin. That's, the information used
    /// to know whether the plugin is activated or not.
    /// </summary>
    [Serializable]
    public class PluginConfiguration
    {
        #region Properties

        /// <summary>
        /// Gets or sets the explanations on what the plugin does.
        /// </summary>
        /// <value>
        /// The explanations.
        /// </value>
        [XmlElement]
        public string Explanations
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id of the plugin.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [XmlElement]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the plugin shoud be activated or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [XmlElement]
        public bool IsActivated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this plugin is mandatory.
        /// </summary>
        /// <value>
        ///   <c>true</c> if mandatory; otherwise, <c>false</c>.
        /// </value>
        [XmlElement]
        public bool IsMandatory
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this plugin is recommended.
        /// </summary>
        /// <value>
        ///   <c>true</c> if recommended; otherwise, <c>false</c>.
        /// </value>
        [XmlElement]
        public bool IsRecommended
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the plugin. This is used in the GUI to have a human
        /// readable name
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlElement]
        public string Name
        {
            get;
            set;
        }

        #endregion Properties
    }
}