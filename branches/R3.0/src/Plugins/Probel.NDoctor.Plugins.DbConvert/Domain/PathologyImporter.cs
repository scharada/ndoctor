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

    public class PathologyImporter : SingleImporter<PathologyDto>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReputationImporter"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public PathologyImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component, "Pathology")
        {
        }

        #endregion Constructors

        #region Properties

        public static PathologyDto[] Cache
        {
            get
            {
                return InternalCache.Values.ToArray();
            }
        }

        #endregion Properties

        #region Methods

        protected override PathologyDto Map(SQLiteDataReader reader)
        {
            var tagImporter = new TagImporter(this.Connection, this.Component, TagCategory.Pathology, "PathologyType");

            var pathology = new PathologyDto();
            pathology.Name = reader["Title"] as string;
            pathology.Notes = Messages.Msg_DoneByConverter;
            pathology.Tag = tagImporter.Import(reader["fk_PathologyType"] as long?);
            return pathology;
        }

        protected override string Sql(long id)
        {
            return string.Format("SELECT * FROM Pathology WHERE ID = {0}", id);
        }

        #endregion Methods
    }
}