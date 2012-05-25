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
namespace Probel.NDoctor.Domain.DAL.Components
{
    using System.Text.RegularExpressions;

    using Probel.NDoctor.Domain.DAL.Helpers;
    using Probel.NDoctor.Domain.DTO.Components;

    public class SqlComponent : BaseComponent, ISqlComponent
    {
        #region Methods

        /// <summary>
        /// Executes a SQL query that resurns only one result
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns>The result of the query</returns>
        [InspectionIgnored]
        public object ExecuteNonQuery(string sql)
        {
            var query = this.Session.CreateSQLQuery(sql);
            return query.UniqueResult();
        }

        /// <summary>
        /// Executes the specified SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        [InspectionIgnored]
        public void ExecuteSql(string sql)
        {
            var regex = new Regex(";", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string[] lines = regex.Split(sql);

            using (var tx = this.Session.BeginTransaction())
            {
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;

                    var query = this.Session.CreateSQLQuery(line);
                    query.ExecuteUpdate();
                }
                tx.Commit();
            }
        }

        /// <summary>
        /// Determines whether the database is empty.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the database is empty; otherwise, <c>false</c>.
        /// </returns>
        [InspectionIgnored]
        public bool IsDatabaseEmpty()
        {
            var sql = "SELECT count(*) FROM sqlite_sequence WHERE seq > 0";
            var empty = (long)this.ExecuteNonQuery(sql);
            return (empty <= 2);
        }

        #endregion Methods
    }
}