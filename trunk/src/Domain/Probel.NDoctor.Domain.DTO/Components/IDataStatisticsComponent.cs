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
    using System.Linq;
    using System.Text;

    using Probel.Helpers.Data;

    /// <summary>
    /// Provides statistics on the application data
    /// </summary>
    public interface IDataStatisticsComponent
    {
        #region Methods

        /// <summary>
        /// Gets the age repartion.
        /// </summary>
        /// <returns></returns>
        Chart<int, int> GetAgeRepartion();

        /// <summary>
        /// Gets the bmi repartition though time.
        /// </summary>
        /// <returns></returns>
        Chart<DateTime, double> GetBmiRepartition();

        /// <summary>
        /// Gets the gender repartition.
        /// </summary>
        /// <returns></returns>
        Chart<string, int> GetGenderRepartition();

        /// <summary>
        /// Gets the patient growth.
        /// </summary>
        /// <returns></returns>
        Chart<DateTime, int> GetPatientGrowth();

        #endregion Methods
    }
}