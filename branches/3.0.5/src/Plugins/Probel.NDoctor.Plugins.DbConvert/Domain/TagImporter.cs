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
    using System.Data.SQLite;
    using System.Linq;

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.DbConvert.Properties;

    public class TagImporter : SingleImporter<TagDto>
    {
        #region Constructors

        public TagImporter(SQLiteConnection connection, IImportComponent component, TagCategory category, string table)
            : base(connection, component, category.ToString())
        {
            this.Table = table;
            this.Category = category;
        }

        #endregion Constructors

        #region Properties

        public static TagDto[] Cache
        {
            get
            {
                return InternalCache.Values.ToArray();
            }
        }

        private TagCategory Category
        {
            get;
            set;
        }

        private string Table
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        protected override TagDto Map(SQLiteDataReader reader)
        {
            var tag = new TagDto(Category) { IsImported = true };
            tag.Name = reader["Title"] as string;
            tag.Notes = Messages.Msg_DoneByConverter;

            return tag;
        }

        protected override string Sql(long id)
        {
            if (string.IsNullOrWhiteSpace(this.Table)) return string.Empty;
            else
            {
                return string.Format("SELECT * FROM {0} WHERE id = {1}"
                    , this.Table
                    , id);
            }
        }

        #endregion Methods
    }
}