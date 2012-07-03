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
namespace Probel.NDoctor.Plugins.DbConvert.Domain
{
    using System;
    using System.Data.SQLite;
    using System.Linq;

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.DbConvert.Properties;

    public class ReputationImporter : SingleImporter<ReputationDto>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReputationImporter"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public ReputationImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component, "Reputation")
        {
        }

        #endregion Constructors

        #region Properties

        public static ReputationDto[] Cache
        {
            get
            {
                return InternalCache.Values.ToArray();
            }
        }

        #endregion Properties

        #region Methods

        protected override ReputationDto Map(SQLiteDataReader reader)
        {
            var reputation = new ReputationDto() { IsImported = true };
            reputation.Name = reader["Title"] as string;
            reputation.Notes = reader["Remark"] as string;
            return reputation;
        }

        protected override string Sql(long id)
        {
            return string.Format("SELECT * FROM Reputation WHERE ID = {0}", id);
        }

        #endregion Methods
    }
}