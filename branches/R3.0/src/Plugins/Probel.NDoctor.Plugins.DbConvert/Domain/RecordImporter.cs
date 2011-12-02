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

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.DbConvert.Properties;

    public class RecordImporter : BaseImporter
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="DoctorImporter"/> class.
        /// </summary>
        static RecordImporter()
        {
            Cache = new Dictionary<long, MedicalRecordDto>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorImporter"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="component">The component.</param>
        public RecordImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component)
        {
        }

        #endregion Constructors

        #region Properties

        public static MedicalRecordDto[] Records
        {
            get { return Cache.Values.ToArray(); }
        }

        private static Dictionary<long, MedicalRecordDto> Cache
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public IEnumerable<MedicalRecordDto> Import(long? id)
        {
            var result = new List<MedicalRecordDto>();
            if (!id.HasValue) return result;

            var sql = string.Format(
                        @"SELECT *
                        FROM MedicalCard
                        INNER JOIN MedicaCardlType ON MedicalCard.fk_MedicalCardType = MedicaCardlType.ID
                        WHERE fk_Patient = {0}", id.Value);

            using (var command = new SQLiteCommand(sql, this.Connection))
            using (var reader = command.ExecuteReader())
            {
                int count = 0;
                while (reader.Read())
                {
                    var current = this.Map(reader);
                    result.Add(current);
                    count++;
                }
                this.OnLogged(Messages.Log_RecordCount, count, id.Value);
            }

            return result;
        }

        private MedicalRecordDto Map(SQLiteDataReader reader)
        {
            var id = reader["Id"] as long?;

            if (!id.HasValue) { throw new NullReferenceException(); }
            else if (Cache.ContainsKey(id.Value)) { return Cache[id.Value]; }
            else
            {
                var record = new MedicalRecordDto();
                record.Tag = this.MapTag(reader["fk_MedicalCardType"] as long?);

                record.CreationDate = reader["CreationDate"] as DateTime? ?? DateTime.Today;
                record.Rtf = reader["Remark"] as string;
                record.LastUpdate = reader["UpdateDate"] as DateTime? ?? DateTime.Today;
                record.Name = reader["Title"] as string;

                Cache.Add(id.Value, record);

                return record;
            }
        }

        private TagDto MapTag(long? id)
        {
            /* There's a typo in the table name ;) */
            var importer = new TagImporter(this.Connection, this.Component, TagCategory.MedicalRecord, "MedicaCardlType");
            this.Relay(importer);

            return importer.Import(id);
        }

        #endregion Methods
    }
}