namespace Probel.NDoctor.Domain.DAL.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Data;
    using Probel.NDoctor.Domain.DAL.AopConfiguration;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

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
                          select new ChartPoint<DateTime, double>(g.Key, g.Average(e => e.ExecutionTime)))
                            .ToList();
            return new Chart<DateTime, double>(points.OrderBy(e => e.X));
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
        public Chart<string, double> GetAvgExecutionTimeGraph()
        {
            var points = (from a in this.Session.Query<ApplicationStatistics>()
                          group a by a.MethodName into g
                          select new ChartPoint<string, double>(g.Key, g.Average(e => e.ExecutionTime)))
                                .ToList();

            return new Chart<string, double>(points.OrderBy(e => e.Y));
        }

        /// <summary>
        /// Gets the bottlenecks by methods.
        /// </summary>
        /// <returns>A chart to be dicplayed</returns>
        public Chart<string, double> GetBottlenecks()
        {
            var points = (from a in this.Session.Query<ApplicationStatistics>()
                          where a.IsPossibleBottleneck == true
                          group a by a.MethodName into g
                          select new ChartPoint<string, double>(g.Key, g.Count()))
                            .ToList();
            return new Chart<string, double>(points.OrderByDescending(e => e.Y));
        }

        /// <summary>
        /// Gets the average execution time by method. Provides more information than the <see cref="GetAvgExecutionTimeGraph"/>
        /// </summary>
        /// <returns>A list with all the information about execution time by method</returns>
        public IEnumerable<BottleneckDto> GetBottlenecksArray()
        {
            var list = (from a in this.Session.Query<ApplicationStatistics>()
                        where a.IsPossibleBottleneck == true
                        group a by new { a.MethodName, a.TargetTypeName } into g
                        select new BottleneckDto()
                        {
                            MethodName = g.Key.MethodName,
                            Count = g.Count(),
                            AvgExecutionTime = g.Average(e => e.ExecutionTime) / 1000,
                            AvgThreshold = g.Average(e => e.Threshold) / 1000,
                            TargetTypeName = g.Key.TargetTypeName,
                        }).ToList();

            return list.OrderBy(e => e.AvgExecutionTime);
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
            return new Chart<string, double>(points.OrderBy(e => e.Y));
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
            return new Chart<string, double>(points.OrderBy(e => e.Y));
        }

        #endregion Methods
    }
}