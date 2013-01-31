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

    using MongoDB.Bson;

    /// <summary>
    /// Represents an anonymous user used to manage statistics about nDoctor usage
    /// </summary>
    internal class AnonymousUser
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousUser"/> class.
        /// </summary>
        public AnonymousUser()
        {
            this.SessionDurations = new List<TimeSpan>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the application key.
        /// </summary>
        /// <value>
        /// The application key.
        /// </value>
        public Guid ApplicationKey
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
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the installation date.
        /// </summary>
        /// <value>
        /// The installation date.
        /// </value>
        public DateTime InstallationDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last update.
        /// </summary>
        /// <value>
        /// The last update.
        /// </value>
        public DateTime LastUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the version of nDoctor the user is using.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version
        {
            get;
            set;
        }

        #endregion Properties

        /// <summary>
        /// Gets or sets the date and time when the user has updated nDoctor.
        /// </summary>
        /// <value>
        /// The update version.
        /// </value>
        public DateTime UpdateVersion { get; set; }

        /// <summary>
        /// Gets or sets the session durations.
        /// </summary>
        /// <value>
        /// The session durations.
        /// </value>
        public List<TimeSpan> SessionDurations { get; set; }
    }
}