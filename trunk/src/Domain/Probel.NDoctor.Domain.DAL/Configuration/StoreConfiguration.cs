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
namespace Probel.NDoctor.Domain.DAL.Configuration
{
    using System;

    using FluentNHibernate.Automapping;

    using Probel.NDoctor.Domain.BLL;

    /// <summary>
    /// Configured store configuration for nDoctor
    /// </summary>
    public class StoreConfiguration : DefaultAutomappingConfiguration
    {
        #region Methods

        /// <summary>
        /// Determines whether the specified type is component.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is component; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsComponent(Type type)
        {
            return type == typeof(Address);
        }

        /// <summary>
        /// Shoulds the map.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public override bool ShouldMap(Type type)
        {
            if (type == typeof(Gender)) return false;

            return type.Namespace == "Probel.NDoctor.Domain.BLL";
        }

        #endregion Methods
    }
}