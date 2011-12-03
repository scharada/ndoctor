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

    public class PictureImporter : MultipleImporter<PictureDto>
    {
        #region Fields

        private static TagDto DefaultTag = new TagDto()
        {
            Category = TagCategory.Picture,
            Name = Messages.Title_DefaultPictureType,
            Notes = Messages.Msg_DoneByConverter,
        };

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorImporter"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="component">The component.</param>
        public PictureImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component)
        {
        }

        #endregion Constructors

        #region Properties

        public static PictureDto[] Cache
        {
            get { return InternalCache.Values.ToArray(); }
        }

        #endregion Properties

        #region Methods

        protected override PictureDto Map(SQLiteDataReader reader)
        {
            var item = new PictureDto();

            item.Creation
                = item.LastUpdate
                = reader["InsertDate"] as DateTime? ?? DateTime.Now;

            item.Bitmap = reader["Bitmap"] as byte[];
            item.Notes = reader["Remark"] as string;
            item.Tag = this.MapTag();

            return item;
        }

        protected override string Sql(long id)
        {
            return string.Format(
                      @"SELECT *
                        FROM Picture
                        INNER JOIN Patient_Picture ON Picture.Id = Picture.Id
                        WHERE fk_Patient = {0}", id);
        }

        private TagDto MapTag()
        {
            var importer = new TagImporter(this.Connection, this.Component, TagCategory.Picture, string.Empty);
            importer.Default = DefaultTag;
            this.Relay(importer);

            return importer.Import(-1);
        }

        #endregion Methods
    }
}