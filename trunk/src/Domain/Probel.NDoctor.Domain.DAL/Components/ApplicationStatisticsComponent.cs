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

namespace Probel.NDoctor.Domain.DAL.Components
{
    using System;
    using System.Linq;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Data;
    using Probel.NDoctor.Domain.DAL.AopConfiguration;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;

    /// <summary>
    /// Provides statistics on the application usage
    /// </summary>
    [Granted(To.Everyone)]
    public class ApplicationStatisticsComponent : BaseComponent, IApplicationStatisticsComponent
    {
        #region Constructors

        public ApplicationStatisticsComponent()
            : base()
        {
        }

        public ApplicationStatisticsComponent(ISession session)
            : base(session)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Counts the entries inserted in the application statistics.
        /// </summary>
        /// <returns>Count the number of entries the statistics has</returns>
        public int CountEntries()
        {
            return (from a in this.Session.Query<ApplicationStatistics>()
                    select a).Count();
        }

        /// <summary>
        /// Get a graph of the average execution time of the methods by day
        /// </summary>
        /// <returns>A chart to be dicplayed</returns>
        public Chart<DateTime, double> ExecutionTimeGraph()
        {
            var points = (from a in this.Session.Query<ApplicationStatistics>()
                          group a by a.TimeStamp.Date into g
                          select new ChartPoint<DateTime, double>(g.Key, g.Average(e => e.ExecutionTime)));
            return new Chart<DateTime, double>(points);
        }

        /// <summary>
        /// Gets the average execution time of a method of a component.
        /// </summary>
        /// <returns>The average execution time of the methods</returns>
        public double GetAvgExecutionTime()
        {
            var result = (from a in this.Session.Query<ApplicationStatistics>()
                          select a.ExecutionTime).ToList();

            return (result.Count != 0) ? result.Average() : 0;
        }

        /// <summary>
        /// Gets the average execution time by methods.
        /// </summary>
        /// <returns>A chart to be dicplayed</returns>
        public Chart<string, double> GetAvgExecutionTimeByMethods()
        {
            var points = (from a in this.Session.Query<ApplicationStatistics>()
                          group a by a.MethodName into g
                          select new ChartPoint<string, double>(g.Key, g.Average(e => e.ExecutionTime)))
                                .ToList();
            return new Chart<string, double>(points);
        }

        /// <summary>
        /// Gets the bottlenecks by methods.
        /// </summary>
        /// <returns>A chart to be dicplayed</returns>
        public Chart<string, double> GetBottlenecksByMethods()
        {
            var points = (from a in this.Session.Query<ApplicationStatistics>()
                          where a.IsPossibleBottleneck == true
                          group a by a.MethodName into g
                          select new ChartPoint<string, double>(g.Key, g.Count()))
                            .ToList();
            return new Chart<string, double>(points);
        }

        /// <summary>
        /// Gets the number of times the methods have been called.
        /// </summary>
        /// <returns>A chart to be dicplayed</returns>
        public Chart<string, double> GetUsageByMethods()
        {
            var points = (from a in this.Session.Query<ApplicationStatistics>()
                          group a by a.MethodName into g
                          select new ChartPoint<string, double>(g.Key, g.Count()))
                                .ToList();
            return new Chart<string, double>(points);
        }

        /// <summary>
        /// Gets the number of times the targets have been called.
        /// </summary>
        /// <returns>A chart to be dicplayed</returns>
        public Chart<string, double> GetUsageByTargets()
        {
            var points = (from a in this.Session.Query<ApplicationStatistics>()
                          group a by a.TargetTypeName into g
                          select new ChartPoint<string, double>(g.Key, g.Count()))
                                .ToList();
            return new Chart<string, double>(points);
        }

        #endregion Methods
    }
}