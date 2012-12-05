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

    public class NavigationRouteEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationRouteEventArgs"/> class.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="destination">The destination.</param>
        public NavigationRouteEventArgs(object current, object destination)
        {
            this.Destination = destination;
            this.Current = current;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the current workbench. That's where the user starts from
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public object Current
        {
            get; set;
        }

        /// <summary>
        /// Gets the destination. That is the workbench where to user navigates.
        /// </summary>
        public object Destination
        {
            get; private set;
        }

        #endregion Properties
    }
}