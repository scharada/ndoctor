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
    public class StatExporter
    {
        #region Fields

        private static readonly string ConnectionString = "mongodb://statistics:ndoctor@ds049157.mongolab.com:49157/ndoctor-statistics";
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StatExporter));

        private readonly AppKey AppKey;
        private readonly MongoDatabase Database;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatExporter"/> class.
        /// </summary>
        /// <param name="executions">The executions.</param>
        public StatExporter(string vendor, string applicationName)
        {
            this.AppKey = AppKey.GetFromAppData(vendor, applicationName);

            this.Database = new MongoClient(ConnectionString)
                .GetServer()
                .GetDatabase("ndoctor-statistics");
        }

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
                var history = Mapper.Map<IEnumerable<ApplicationStatistics>, IEnumerable<FeatureExecution>>(executions);

                user.History.AddRange(history);

                this.Update(user);

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
                };
                users.Insert(user);
            }

            return user;
        }

        private void Update(AnonymousUser user)
        {
            user.LastUpdate = DateTime.Now.ToUniversalTime();
            var users = this.Database.GetCollection<AnonymousUser>("users");
            users.Save(user);
        }

        #endregion Methods
    }
}