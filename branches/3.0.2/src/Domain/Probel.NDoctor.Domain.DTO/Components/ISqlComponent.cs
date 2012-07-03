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
namespace Probel.NDoctor.Domain.DTO.Components
{
    public interface ISqlComponent : IBaseComponent
    {
        #region Methods

        /// <summary>
        /// Executes a SQL query that resurns only one result
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        object ExecuteNonQuery(string sql);

        /// <summary>
        /// Executes the specified SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        void ExecuteSql(string sql);

        /// <summary>
        /// Determines whether the database is empty.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the database is empty; otherwise, <c>false</c>.
        /// </returns>
        bool IsDatabaseEmpty();

        #endregion Methods
    }
}