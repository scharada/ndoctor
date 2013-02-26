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
            if (this.Session.Query<Patient>().Count() > 0)
            {
                var result = (from p in this.Session.Query<Patient>().ToList()
                              where p.IsDeactivated == false
                              group p by p.Age into g
                              where g.Key < 120
                              select new ChartPoint<int, int>(g.Key, g.Count()));

                return new Chart<int, int>(result);
            }
            else { return new Chart<int, int>(); }
        }

        /// <summary>
        /// Gets the bmi repartition though time.
        /// </summary>
        /// <returns>Graph of the average bmi index through time</returns>
        public Chart<DateTime, double> GetBmiRepartition()
        {
            if (this.Session.Query<Bmi>().Count() > 0)
            {
                var points = (from bmi in this.Session.Query<Bmi>().ToList()
                              group bmi by new { bmi.Date.Month, bmi.Date.Year } into g
                              select new ChartPoint<DateTime, double>
                              {
                                  X = new DateTime(g.Key.Year, g.Key.Month, 1),
                                  Y = (from item in g
                                       select (float)item.Weight / Math.Pow((double)item.Height / 100, 2d)).Average()
                              })
                         .OrderBy(e => e.X.Year)
                         .OrderBy(e => e.X.Month);

                return new Chart<DateTime, double>(points);
            }
            else { return new Chart<DateTime, double>(); }
        }

        /// <summary>
        /// Gets the gender repartition.
        /// </summary>
        /// <returns></returns>
        public Chart<string, int> GetGenderRepartition()
        {
            if (this.Session.Query<Patient>().Count() > 0)
            {
                var result = (from pat in this.Session.Query<Patient>()
                              where pat.IsDeactivated == false
                              group pat by pat.Gender into g
                              select new ChartPoint<string, int> { X = g.Key.Translate(), Y = g.Count() });
                return new Chart<string, int>(result);
            }
            else { return new Chart<string, int>(); }
        }

        /// <summary>
        /// Gets the patient growth.
        /// </summary>
        /// <returns></returns>
        public Chart<DateTime, int> GetPatientGrowth()
        {
            if (this.Session.Query<Patient>().Count() > 0)
            {
                var firstInscription = this.Session.Query<Patient>().Min(e => e.InscriptionDate).Year;
                var chart = new Chart<DateTime, int>();

                for (int i = firstInscription; i < DateTime.Today.Year; i++)
                {
                    var count = (from p in this.Session.Query<Patient>()
                                 where p.InscriptionDate.Year <= i
                                    && p.IsDeactivated == false
                                 select p).Count();
                    chart.AddPoint(new DateTime(i, 1, 1), count);
                }

                return chart;
            }
            else { return new Chart<DateTime, int>(); }
        }

        #endregion Methods
    }
}