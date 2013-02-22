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
    using System.Collections.Generic;

    using AutoMapper;

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Statistics;
    using Probel.NDoctor.Statistics.Domain;

    public class RemoteService
    {
        #region Fields

        private const string ConnectionString = "mongodb://statistics:ndoctor@ds049157.mongolab.com:49157/ndoctor-statistics";
        private const string DatabaseName = "ndoctor-statistics";

        private static readonly IFactory Get;

        #endregion Fields

        #region Constructors

        static RemoteService()
        {
            #if debug
            Get = Queries.ForMongoDb(DatabaseName, ConnectionString);
            #else
            Get = Queries.ForMySql("10.0.0.2", "ndoctor", "ndoctor", "ndoctor");
            #endif
        }

        #endregion Constructors

        #region Methods

        public void Export(IEnumerable<ApplicationStatistics> statistics, Version version, TimeSpan sessionDuration, Guid appKey)
        {
            var statToExport = Mapper.Map<IEnumerable<ApplicationStatistics>, IEnumerable<StatisticEntry>>(statistics);
            var app = Get.Scalars().GetApplicationByAppKey(appKey);
            if (app == null)
            {
                Get.Exporter().Insert(new Application()
                {
                    ApplicationKey = Guid.NewGuid(),
                    InstallationDate = DateTime.Today,
                    LastUpdate = DateTime.Today,
                    UpdateVersion = DateTime.Today,
                    Version = version.ToString(),
                });
            }
            else if (app.Version != version.ToString())
            {
                Get.Exporter().Update(app);
            }

            Get.Exporter().Insert(statToExport);
            Get.Exporter().Insert(new SessionDuration()
            {
                Duration = sessionDuration,
                TimeStamp = DateTime.Today,
                Version = version.ToString(),
            });
        }

        public VersionNotifyer NewVersionNotifyer()
        {
            return new VersionNotifyer(ConnectionString, DatabaseName);
        }

        #endregion Methods
    }
}