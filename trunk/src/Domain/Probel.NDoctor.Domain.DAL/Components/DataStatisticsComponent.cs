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
    using Probel.NDoctor.Domain.DTO.Helpers;

    /// <summary>
    /// Provides statistics on the application usage
    /// </summary>
    [Granted(To.Everyone)]
    public class DataStatisticsComponent : BaseComponent, IDataStatisticsComponent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStatisticsComponent"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public DataStatisticsComponent(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStatisticsComponent"/> class.
        /// </summary>
        public DataStatisticsComponent()
            : base()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the age repartion.
        /// </summary>
        /// <returns></returns>
        public Chart<int, int> GetAgeRepartion()
        {
            var result = (from p in this.Session.Query<Patient>().ToList()
                          group p by p.Age into g
                          where g.Key < 120
                          select new ChartPoint<int, int>(g.Key, g.Count()));

            return new Chart<int, int>(result);
        }

        /// <summary>
        /// Gets the gender repartition.
        /// </summary>
        /// <returns></returns>
        public Chart<string, int> GetGenderRepartition()
        {
            var result = (from pat in this.Session.Query<Patient>()
                          group pat by pat.Gender into g
                          select new ChartPoint<string, int> { X = g.Key.Translate(), Y = g.Count() });
            return new Chart<string, int>(result);
        }

        /// <summary>
        /// Gets the patient growth.
        /// </summary>
        /// <returns></returns>
        [BenchmarkThreshold(3000, "This query is very heavy and takes untill 4 seconds to be executed")]
        public Chart<DateTime, int> GetPatientGrowth()
        {
            var firstInscription = this.Session.Query<Patient>().Min(e => e.InscriptionDate).Year;
            var chart = new Chart<DateTime, int>();

            for (int i = firstInscription; i < DateTime.Today.Year; i++)
            {
                var count = (from p in this.Session.Query<Patient>()
                             where p.InscriptionDate.Year <= i
                             select p).Count();
                chart.AddPoint(new DateTime(i, 1, 1), count);
            }

            return chart;
        }

        #endregion Methods
    }
}