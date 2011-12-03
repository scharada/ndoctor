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

    public class DrugImporter : SingleImporter<DrugDto>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InsuranceImporter"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="component">The component.</param>
        public DrugImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component, "Insurance")
        {
        }

        #endregion Constructors

        #region Properties

        public static DrugDto[] Cache
        {
            get
            {
                return InternalCache.Values.ToArray();
            }
        }

        #endregion Properties

        #region Methods

        protected override DrugDto Map(SQLiteDataReader reader)
        {
            var drug = new DrugDto();

            drug.Name = reader["Title"] as string;
            drug.Notes = Messages.Msg_DoneByConverter;
            drug.Tag = this.MapTag(reader["fk_DrugType"] as long?);

            return drug;
        }

        protected override string Sql(long id)
        {
            return string.Format("SELECT * FROM Drug WHERE Id = {0}"
                , id);
        }

        private TagDto MapTag(long? id)
        {
            var importer = new TagImporter(this.Connection, this.Component, TagCategory.Drug, "DrugType");
            this.Relay(importer);

            return importer.Import(id);
        }

        #endregion Methods
    }
}