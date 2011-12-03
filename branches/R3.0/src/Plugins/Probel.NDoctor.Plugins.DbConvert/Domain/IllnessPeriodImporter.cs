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
    using System.Data.SQLite;
    using System.Linq;

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.DbConvert.Properties;

    public class IllnessPeriodImporter : MultipleImporter<IllnessPeriodDto>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorImporter"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="component">The component.</param>
        public IllnessPeriodImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component)
        {
        }

        #endregion Constructors

        #region Properties

        public static IllnessPeriodDto[] Chache
        {
            get { return InternalCache.Values.ToArray(); }
        }

        #endregion Properties

        #region Methods

        protected override IllnessPeriodDto Map(SQLiteDataReader reader)
        {
            var item = new IllnessPeriodDto();

            item.Start = (reader["StartDate"] as DateTime? ?? DateTime.Today).Date;
            item.End = (reader["EndDate"] as DateTime? ?? DateTime.Today).Date;
            item.Notes = reader["Remark"] as string;
            item.Pathology = this.MapPathology(reader["fk_Pathology"] as long?);
            item.Notes = Messages.Msg_DoneByConverter;

            return item;
        }

        protected override string Sql(long id)
        {
            return string.Format(
                      @"SELECT *
                        FROM PathologicEvent
                        INNER JOIN Patient_PathologicEvent ON Patient_PathologicEvent.fk_PathologicEvents = PathologicEvent.ID
                        WHERE Patient_PathologicEvent.fk_Patient = {0}", id);
        }

        private PathologyDto MapPathology(long? id)
        {
            var importer = new PathologyImporter(this.Connection, this.Component);
            this.Relay(importer);

            return importer.Import(id);
        }

        #endregion Methods
    }
}