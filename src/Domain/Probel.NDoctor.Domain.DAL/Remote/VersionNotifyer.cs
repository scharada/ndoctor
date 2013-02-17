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

namespace Probel.NDoctor.Domain.DAL.Remote
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    using Probel.NDoctor.Domain.DTO.Remote;

    public class VersionNotifyer : IVersionNotifyer
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(VersionNotifyer));

        private readonly string ConnectionString;
        private readonly string DatabaseName;

        #endregion Fields

        #region Constructors

        internal VersionNotifyer(string connectionString, string databaseName)
        {
            this.DatabaseName = databaseName;
            this.ConnectionString = connectionString;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when version is checked.
        /// </summary>
        public event EventHandler<VersionEventArgs> Checked;

        #endregion Events

        #region Methods

        /// <summary>
        /// Checks the lastest version aynchronously. Any exception is swallowed and logged.
        /// This methods shouldn't crash. The application stability before aside features.
        /// </summary>
        /// <param name="version">The current version of the application.</param>
        public void Check(Version version)
        {
            version = new Version(version.Major, version.Minor, version.Build, 0);
            var task = Task.Factory
                .StartNew<Version>(ctx => this.CheckAsync(ctx as Version), version);
            task.ContinueWith(c =>
            {
                if (c.Result as Version > version)
                {
                    this.OnChecked(c.Result as Version);
                }
            }, new CancellationTokenSource().Token);
        }

        protected void OnChecked(Version remoteVersion)
        {
            lock (this)
            {
                if (this.Checked != null)
                {
                    this.Checked(this, new VersionEventArgs(remoteVersion));
                }
            }
        }

        private Version CheckAsync(Version version)
        {
            try
            {
                var versions = new MongoClient(this.ConnectionString)
                    .GetServer()
                    .GetDatabase(this.DatabaseName)
                    .GetCollection<RemoteVersion>("versions")
                    .AsQueryable();

                var max = versions.Max(v => v.Version);
                var remote = (from v in versions
                              where v.Version == max
                              select v).Single();

                return Version.Parse(remote.Version);
            }
            catch (Exception ex)
            {
                Logger.Error("Impossible to check the remote versions", ex);
                return new Version();
            }
        }

        #endregion Methods
    }
}