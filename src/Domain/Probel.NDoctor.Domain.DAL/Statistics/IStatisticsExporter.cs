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

    using Probel.NDoctor.Domain.DAL.Entities;

    /// <summary>
    /// Export the statistics about the usage of the current session of nDoctor
    /// </summary>
    public interface IStatisticsExporter
    {
        #region Methods

        /// <summary>
        /// Exports the specified history into the global database.
        /// If this action fails, it'll be silently. That's, the thrown
        /// exception is swallowed and logged.
        /// </summary>
        /// <param name="executions">The executions.</param>
        void Export(IEnumerable<ApplicationStatistics> executions, Guid appKey);

        #endregion Methods
    }
}