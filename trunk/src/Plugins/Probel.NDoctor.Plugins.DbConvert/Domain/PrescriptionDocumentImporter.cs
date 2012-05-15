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
    using System.Collections.ObjectModel;
    using System.Data.SQLite;
    using System.Linq;
    using System.Text;

    using Probel.Helpers.Conversions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.DbConvert.Properties;
    using Probel.Mvvm.DataBinding;

    public class PrescriptionDocumentImporter : MultipleImporter<PrescriptionDocumentDto>
    {
        #region Fields

        private static readonly TagDto DefaultTag = new TagDto(TagCategory.PrescriptionDocument)
        {
            Name = Messages.Title_DefaultPrescription,
            Notes = Messages.Msg_DoneByConverter,
            IsImported = true,
        };

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorImporter"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="component">The component.</param>
        public PrescriptionDocumentImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component)
        {
        }

        #endregion Constructors

        #region Properties

        public static PrescriptionDocumentDto[] Chache
        {
            get { return InternalCache.Values.ToArray(); }
        }

        #endregion Properties

        #region Methods

        protected override PrescriptionDocumentDto Map(SQLiteDataReader reader)
        {
            var item = new PrescriptionDocumentDto() { IsImported = true };
            item.CreationDate = (reader["CreationDate"] as DateTime? ?? DateTime.Today).Date;
            item.Prescriptions.Refill(this.MapPrescriptions(reader["fk_Patient"] as long?));
            item.Tag = this.MapTag();
            item.Title = item.CreationDate.ToString();

            return item;
        }

        protected override string Sql(long id)
        {
            return string.Format(
                      @"SELECT *
                        FROM Prescription_Drug
                        INNER JOIN Prescription ON Prescription.ID = Prescription_Drug.fk_Prescription
                        INNER JOIN Patient_Prescription ON Patient_Prescription.fk_Prescription = Prescription.ID
                        WHERE fk_Patient = {0}", id);
        }

        private PathologyDto MapPathology(long? id)
        {
            var importer = new PathologyImporter(this.Connection, this.Component);
            this.Relay(importer);

            return importer.Import(id);
        }

        private IEnumerable<PrescriptionDto> MapPrescriptions(long? id)
        {
            var importer = new PrescriptionImporter(this.Connection, this.Component);
            this.Relay(importer);

            return importer.Import(id);
        }

        private TagDto MapTag()
        {
            var tagImporter = new TagImporter(this.Connection, this.Component, TagCategory.PrescriptionDocument, string.Empty);
            tagImporter.Default = DefaultTag;
            this.Relay(tagImporter);

            return tagImporter.Import(-1);
        }

        #endregion Methods
    }
}