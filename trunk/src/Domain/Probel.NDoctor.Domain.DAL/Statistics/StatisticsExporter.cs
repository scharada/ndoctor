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
    using Probel.NDoctor.Domain.DAL.Entities;

    /// <summary>
    /// Export the statistics about the usage of the current session of nDoctor
    /// </summary>
    public class StatisticsExporter
    {
        #region Fields

        private static readonly string ConnectionString = "mongodb://statistics:ndoctor@ds049157.mongolab.com:49157/ndoctor-statistics";
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StatisticsExporter));

        private readonly AppKey AppKey;
        private readonly MongoDatabase Database;
        private readonly string Version;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsExporter"/> class.
        /// </summary>
        /// <param name="executions">The executions.</param>
        public StatisticsExporter(string vendor, string applicationName, Version version, TimeSpan sessionDuration)
        {
            this.AppKey = AppKey.GetFromAppData(vendor, applicationName);
            this.Version = version.ToString();
            this.SessionDuration = sessionDuration;

            this.Database = new MongoClient(ConnectionString)
                .GetServer()
                .GetDatabase("ndoctor-statistics");
        }
        private readonly TimeSpan SessionDuration;
        #endregion Constructors

        #region Methods

        /// <summary>
        /// Exports the specified history into the global database.
        /// If this action fails, it'll be silently. That's, the thrown
        /// exception is swallowed and logged.
        /// </summary>
        /// <param name="executions">The executions.</param>
        public void Export(IEnumerable<ApplicationStatistics> executions)
        {
            try
            {
                var user = this.GetUser(this.AppKey.GetKey());

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

        private AnonymousUser GetUser(Guid guid)
        {
            var users = this.Database.GetCollection<AnonymousUser>("users");

            var user = (from u in users.AsQueryable()
                        where u.ApplicationKey == guid
                        select u).FirstOrDefault();

            if (user == null)
            {
                user = new AnonymousUser()
                {
                    ApplicationKey = this.AppKey.GetKey(),
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
            var users = this.Database.GetCollection<AnonymousUser>("users");

            if (this.SessionDuration.TotalSeconds > 0) { user.SessionDurations.Add(this.SessionDuration); }
            user.LastUpdate = DateTime.Now.ToUniversalTime();
            user.Version = this.Version;
            users.Save(user);

            statistics.InsertBatch(history);
        }

        #endregion Methods
    }
}