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

namespace Probel.NDoctor.Domain.DAL.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using MongoDB.Bson;

    /// <summary>
    /// Contains information about session duration
    /// </summary>
    internal class SessionDuration
    {
        #region Properties

        /// <summary>
        /// Gets or sets the duration of the session.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public ObjectId Id
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the date of the session.
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        public DateTime TimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the version of nDoctor for this session.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version
        {
            get; set;
        }

        #endregion Properties
    }
}