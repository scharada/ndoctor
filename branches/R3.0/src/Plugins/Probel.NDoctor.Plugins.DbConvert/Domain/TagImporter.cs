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

    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.DbConvert.Properties;

    public class TagImporter : SingleImporter<TagDto>
    {
        #region Fields

        private string table;

        #endregion Fields

        #region Constructors

        public TagImporter(SQLiteConnection connection, IImportComponent component, TagCategory category, string table)
            : base(connection, component)
        {
            if (Category != category)
            {
                Category = category;
                Cache = new Dictionary<long, TagDto>();
            }
            this.table = table;
        }

        #endregion Constructors

        #region Properties

        private static TagCategory Category
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        protected override void Create(TagDto item)
        {
            using (this.Component.UnitOfWork)
            {
                this.Component.Create(item);
            }
        }

        protected override TagDto Map(SQLiteDataReader reader)
        {
            var id = reader["Id"] as long?;

            if (!id.HasValue) throw new NullReferenceException(Messages.Error_ImpossibleToMap.StringFormat("DoctorSpecialisation"));
            else if (Cache.ContainsKey(id.Value)) return Cache[id.Value];
            else
            {
                var tag = new TagDto(Category);
                tag.Name = reader["Title"] as string;
                tag.Notes = Messages.Msg_DoneByConverter;

                this.AddInCache(id.Value, tag);

                return tag;
            }
        }

        protected override string Sql(long id)
        {
            return string.Format("SELECT * FROM {0} WHERE id = {1}"
                , this.table
                , id);
        }

        #endregion Methods
    }
}