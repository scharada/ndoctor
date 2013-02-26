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

namespace Probel.NDoctor.Domain.DTO.Remote
{
    using System;

    /// <summary>
    /// Checks and compare the current version of nDoctor with the remote database
    /// </summary>
    public interface IVersionNotifyer
    {
        #region Events

        /// <summary>
        /// Occurs when version is checked.
        /// </summary>
        event EventHandler<VersionEventArgs> Checked;

        #endregion Events

        #region Methods

        /// <summary>
        /// Checks the lastest version aynchronously
        /// </summary>
        /// <param name="version">The current version of the application.</param>
        void Check(Version version);

        #endregion Methods
    }

    public class VersionEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionEventArgs"/> class.
        /// </summary>
        /// <param name="hasNewVersion">if set to <c>true</c> [has new version].</param>
        /// <param name="remoteVersion">The remote version.</param>
        public VersionEventArgs(Version remoteVersion)
        {
            this.RemoteVersion = remoteVersion;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the remote version.
        /// </summary>
        public Version RemoteVersion
        {
            get; private set;
        }

        #endregion Properties
    }
}