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

namespace Probel.NDoctor.Domain.Components.Statistics
{
    using System.Collections.Generic;

    using Probel.NDoctor.Domain.DAL.Cfg;
    using Probel.NDoctor.Domain.DAL.Entities;

    /// <summary>
    /// Contains in its internal state all the statistics about the application
    /// </summary>
    public class NDoctorStatistics
    {
        #region Fields

        private static readonly List<ApplicationStatistics> Statistics = new List<ApplicationStatistics>();

        #endregion Fields

        #region Methods

        /// <summary>
        /// Flushes the internal history into the database.
        /// </summary>
        public void Flush()
        {
            using (var session = DalConfigurator.SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                foreach (var item in Statistics) { session.Save(item); }
                tx.Commit();
                Statistics.Clear();
            }
        }

        /// <summary>
        /// Adds the specified stat into an internal buffer that will be presisted into the database when flushed.
        /// </summary>
        /// <param name="stat">The stat.</param>
        internal void Add(ApplicationStatistics stat)
        {
            Statistics.Add(stat);
        }

        #endregion Methods
    }
}