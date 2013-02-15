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
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using log4net;

    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    using Probel.Helpers.Data;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Components;

    /// <summary>
    /// Export the statistics about the usage of the current session of nDoctor
    /// </summary>
    internal class StatisticsExporter : Probel.NDoctor.Domain.DAL.Statistics.IStatisticsExporter
    {
        #region Fields

        private const string APPKEY = "AppKey";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(StatisticsExporter));

        private readonly string ConnectionString; // = "mongodb://statistics:ndoctor@ds049157.mongolab.com:49157/ndoctor-statistics";
        private readonly MongoDatabase Database;
        private readonly TimeSpan Duration;
        private readonly IDbSettingsComponent Settings = new DbSettingsComponent();
        private readonly string Version;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsExporter"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="sessionDuration">Duration of the session.</param>
        internal StatisticsExporter(string connectionString, string databaseName, Version version, TimeSpan sessionDuration)
        {
            this.ConnectionString = connectionString;

            this.Version = version.ToString();
            this.Duration = sessionDuration;

            this.Database = new MongoClient(ConnectionString)
                .GetServer()
                .GetDatabase(databaseName);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Exports the specified history into the global database.
        /// If this action fails, it'll be silently. That's, the thrown
        /// exception is swallowed and logged.
        /// </summary>
        /// <param name="executions">The executions.</param>
        public void Export(IEnumerable<ApplicationStatistics> executions, Guid appKey)
        {
            try
            {
                var user = this.GetUser(appKey);

                var history = Mapper.Map<IEnumerable<ApplicationStatistics>, IEnumerable<StatisticEntry>>(executions);

                foreach (var item in history) { item.UserId = user.Id; }

                this.Update(history, user);

                Logger.Info("History exported to the database...");
            }
            catch (Exception ex)
            {
                Logger.Error("Impossible to export the history to the distant database", ex);
            }
        }

        private AnonymousUser GetUser(Guid appKey)
        {
            var users = this.Database.GetCollection<AnonymousUser>("users");

            var user = (from u in users.AsQueryable()
                        where u.ApplicationKey == appKey
                        select u).FirstOrDefault();

            if (user == null)
            {
                user = new AnonymousUser()
                {
                    ApplicationKey = appKey,
                    InstallationDate = DateTime.Now.ToUniversalTime(),
                    Version = this.Version.ToString(),
                    UpdateVersion = DateTime.Now.ToUniversalTime(),
                };
                users.Insert(user);
            }
            else if (user.Version != this.Version)
            {
                user.Version = this.Version;
                user.UpdateVersion = DateTime.Now.ToUniversalTime();
            }

            return user;
        }

        private void Update(IEnumerable<StatisticEntry> history, AnonymousUser user)
        {
            var statistics = this.Database.GetCollection<StatisticEntry>("statistics");
            var durations = this.Database.GetCollection<SessionDuration>("durations");
            var users = this.Database.GetCollection<AnonymousUser>("users");

            user.LastUpdate = DateTime.Now.ToUniversalTime();
            user.Version = this.Version;
            users.Save(user);

            durations.Insert(new SessionDuration()
            {
                Duration = this.Duration,
                TimeStamp = DateTime.Today.ToUniversalTime(),
                Version = this.Version,
            });
            statistics.InsertBatch(history);
        }

        #endregion Methods
    }
}