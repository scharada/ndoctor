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

namespace Probel.NDoctor.Plugins.DbConvert.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Linq;
    using System.Text;

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.DbConvert.Properties;

    public class BmiImporter : MultipleImporter<BmiDto>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorImporter"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="component">The component.</param>
        public BmiImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component)
        {
        }

        #endregion Constructors

        #region Properties

        public static BmiDto[] Cache
        {
            get
            {
                return InternalCache.Values.ToArray();
            }
        }

        public static BmiDto[] Records
        {
            get { return InternalCache.Values.ToArray(); }
        }

        #endregion Properties

        #region Methods

        protected override BmiDto Map(SQLiteDataReader reader)
        {
            var record = new BmiDto() { IsImported = true };

            var date = reader["Date"] as DateTime? ?? DateTime.Today;

            record.Date = date.Date;
            record.Height = (int)(reader["Height"] as long? ?? 0);
            record.Weight = (float)(reader["Weight"] as decimal? ?? 0);

            return record;
        }

        protected override string Sql(long id)
        {
            return string.Format(
                      @"SELECT *
                        FROM Bmi
                        WHERE fk_Patient = {0}", id);
        }

        #endregion Methods
    }
}