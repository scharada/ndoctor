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
    using NHibernate;

    using Probel.NDoctor.Domain.DAL.Entities;

    /// <summary>
    /// This is a wrapper that gives the opportunity to manually add dummy data
    /// </summary>
    public class NDoctorStatisticsTester
    {
        #region Fields

        private readonly NDoctorStatistics Statistics;

        #endregion Fields

        #region Constructors

        public NDoctorStatisticsTester(ISession session)
        {
            this.Statistics = new NDoctorStatistics(session);
        }

        #endregion Constructors

        #region Methods

        public void Add(ApplicationStatistics item)
        {
            this.Statistics.Add(item);
        }

        public void Flush()
        {
            this.Statistics.Flush();
        }

        #endregion Methods
    }
}