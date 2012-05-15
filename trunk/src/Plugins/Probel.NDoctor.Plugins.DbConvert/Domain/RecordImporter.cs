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
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Linq;

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.DbConvert.Properties;

    public class RecordImporter : MultipleImporter<MedicalRecordDto>
    {
        #region Constructors

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

        public static MedicalRecordDto[] Cache
        {
            get { return InternalCache.Values.ToArray(); }
        }

        #endregion Properties

        #region Methods

        protected override MedicalRecordDto Map(SQLiteDataReader reader)
        {
            var record = new MedicalRecordDto() { IsImported = true };
            record.Tag = this.MapTag(reader["fk_MedicalCardType"] as long?);

            if (record.Tag == null || record.Tag.Id <= 0)
            {
                record.Tag = DefaultTag();
            }

            record.CreationDate = (reader["CreationDate"] as DateTime? ?? DateTime.Today).Date;
            record.LastUpdate = (reader["UpdateDate"] as DateTime? ?? DateTime.Today).Date;
            record.Rtf = reader["Remark"] as string;
            record.Name = reader["Title"] as string;

            return record;
        }

        protected override string Sql(long id)
        {
            return string.Format(
                        @"SELECT *
                        FROM MedicalCard
                        INNER JOIN MedicaCardlType ON MedicalCard.fk_MedicalCardType = MedicaCardlType.ID
                        WHERE fk_Patient = {0}", id);
        }

        private TagDto DefaultTag()
        {
            var tag = new TagDto(TagCategory.MedicalRecord)
            {
                IsImported = true,
                Name = Messages.Default_RecordType,
                Notes = string.Empty,
            };
            return tag;
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