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
    using System.Data.SQLite;
    using System.Linq;

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.DbConvert.Properties;

    class PrescriptionImporter : MultipleImporter<PrescriptionDto>
    {
        #region Fields

        private static readonly TagDto DefaultTag = new TagDto()
        {
            Category = TagCategory.Prescription,
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
        public PrescriptionImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component)
        {
        }

        #endregion Constructors

        #region Properties

        public static PrescriptionDto[] Chache
        {
            get { return InternalCache.Values.ToArray(); }
        }

        #endregion Properties

        #region Methods

        protected override PrescriptionDto Map(SQLiteDataReader reader)
        {
            var item = new PrescriptionDto() { IsImported = true };

            item.Drug = this.MapDrug(reader["fk_Drug"] as long?);
            item.Tag = this.MapTag();
            item.Notes = reader["Remark"] as string;

            return item;
        }

        protected override string Sql(long id)
        {
            return string.Format(
                      @"SELECT *
                        FROM Prescription
                        INNER JOIN Prescription_Drug ON Prescription.ID = Prescription_Drug.fk_Prescription
                        INNER JOIN Patient_Prescription ON Prescription.ID = Patient_Prescription.fk_Prescription
                        WHERE Patient_Prescription.fk_Patient = {0}", id);
        }

        private DrugDto MapDrug(long? id)
        {
            var importer = new DrugImporter(this.Connection, this.Component);
            this.Relay(importer);

            return importer.Import(id);
        }

        private TagDto MapTag()
        {
            var tagImporter = new TagImporter(this.Connection, this.Component, TagCategory.Prescription, string.Empty);
            tagImporter.Default = DefaultTag;
            this.Relay(tagImporter);

            return tagImporter.Import(-1);
        }

        #endregion Methods
    }
}