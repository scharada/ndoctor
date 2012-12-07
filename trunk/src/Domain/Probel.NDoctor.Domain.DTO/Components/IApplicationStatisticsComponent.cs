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

namespace Probel.NDoctor.Domain.DTO.Components
{
    using System;
    using System.Collections.Generic;

    using Probel.Helpers.Data;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Provides statistics on the application usage
    /// </summary>
    public interface IApplicationStatisticsComponent
    {
        #region Methods

        /// <summary>
        /// Counts the entries inserted in the application statistics.
        /// </summary>
        /// <returns>Count the number of entries the statistics has</returns>
        int CountEntries();

        /// <summary>
        /// Get a graph of the average execution time of the methods by day
        /// </summary>
        /// <returns>A chart to be dicplayed</returns>
        Chart<DateTime, double> ExecutionTimeGraph();

        /// <summary>
        /// Gets the average execution time of a method of a component.
        /// </summary>
        /// <returns>The average execution time of the methods</returns>
        double GetAvgExecutionTime();

        /// <summary>
        /// Gets the average execution time by methods.
        /// </summary>
        /// <returns>A chart to be dicplayed</returns>
        Chart<string, double> GetAvgExecutionTimeGraph();

        /// <summary>
        /// Gets the bottlenecks by methods.
        /// </summary>
        /// <returns>A chart to be dicplayed</returns>
        Chart<string, double> GetBottlenecks();

        /// <summary>
        /// Gets the average execution time by method. Provides more information than the <see cref="GetAvgExecutionTimeGraph"/>
        /// </summary>
        /// <returns>A list with all the information about execution time by method</returns>
        IEnumerable<BottleneckDto> GetBottlenecksArray();

        /// <summary>
        /// Gets the number of times the methods have been called.
        /// </summary>
        /// <returns>A chart to be dicplayed</returns>
        Chart<string, double> GetUsageByMethods();

        /// <summary>
        /// Gets the number of times the targets have been called.
        /// </summary>
        /// <returns>A chart to be dicplayed</returns>
        Chart<string, double> GetUsageByTargets();

        #endregion Methods
    }
}