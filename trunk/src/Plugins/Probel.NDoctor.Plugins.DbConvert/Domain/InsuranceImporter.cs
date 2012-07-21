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

    public class InsuranceImporter : SingleImporter<InsuranceDto>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InsuranceImporter"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="component">The component.</param>
        public InsuranceImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component, "Insurance")
        {
        }

        #endregion Constructors

        #region Properties

        public static InsuranceDto[] Cache
        {
            get
            {
                return InternalCache.Values.ToArray();
            }
        }

        #endregion Properties

        #region Methods

        protected override InsuranceDto Map(SQLiteDataReader reader)
        {
            var insurance = new InsuranceDto() { IsImported = true };

            insurance.Name = reader["Title"] as string;
            insurance.Notes = Messages.Msg_DoneByConverter;
            insurance.Phone = reader["Phone"] as string;

            insurance.Address.BoxNumber = reader["BoxNumber"] as string;
            insurance.Address.City = reader["City"] as string;
            insurance.Address.PostalCode = reader["PostalCode"] as string;
            insurance.Address.Street = reader["Street"] as string;
            insurance.Address.StreetNumber = reader["StreetNumber"] as string;
            return insurance;
        }

        protected override string Sql(long id)
        {
            return string.Format("SELECT * FROM Insurance INNER JOIN Address ON Insurance.fk_Address = Address.Id WHERE Insurance.Id = {0}"
                , id);
        }

        #endregion Methods
    }
}