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

namespace Probel.NDoctor.View.Plugins
{
    using System;
    using System.Reflection;

    using Probel.NDoctor.Domain.DTO.Components;

    /// <summary>
    /// Configuration of the application
    /// </summary>
    public class Configuration
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the contextual menus should be automatically displayed when
        /// user navigates to a new plugin.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [automatic context menu]; otherwise, <c>false</c>.
        /// </value>
        public bool AutomaticContextMenu
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether component loggin is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if component loggin is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool BenchmarkEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the url where to download nDoctor.
        /// </summary>
        /// <value>
        /// The download site.
        /// </value>
        public string DownloadSite
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the execution time threshold. That's after how many milliseconds the execution time
        /// if concidered a high
        /// </summary>
        /// <value>
        /// The execution time threshold (in milliseconds).
        /// </value>
        public uint ExecutionTimeThreshold
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the search algorithm to apply on a search.
        /// </summary>
        /// <value>
        /// The type of the search.
        /// </value>
        public SearchOn SearchType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the version of nDoctor.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public Version Version
        {
            get
            {
                return Assembly
                    .GetExecutingAssembly()
                    .GetName()
                    .Version;
            }
        }

        #endregion Properties
    }
}