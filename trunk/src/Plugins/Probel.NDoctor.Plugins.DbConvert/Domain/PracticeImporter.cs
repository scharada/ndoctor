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

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.DbConvert.Properties;

    public class PracticeImporter : SingleImporter<PracticeDto>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReputationImporter"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public PracticeImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component, "Practice")
        {
        }

        #endregion Constructors

        #region Methods

        protected override PracticeDto Map(SQLiteDataReader reader)
        {
            var practice = new PracticeDto() { IsImported = true };
            practice.Name = reader["Title"] as string;
            practice.Phone = reader["Phone"] as string;
            practice.Notes = Messages.Msg_DoneByConverter;

            practice.Address.BoxNumber = reader["BoxNumber"] as string;
            practice.Address.City = reader["City"] as string;
            practice.Address.PostalCode = reader["PostalCode"] as string;
            practice.Address.Street = reader["Street"] as string;
            practice.Address.StreetNumber = reader["StreetNumber"] as string;
            return practice;
        }

        protected override string Sql(long id)
        {
            return string.Format("SELECT * FROM Practice INNER JOIN Address ON Practice.fk_Address = Address.Id WHERE Practice.Id = {0}"
                , id);
        }

        #endregion Methods
    }
}