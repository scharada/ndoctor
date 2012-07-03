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

    public class DoctorImporter : MultipleImporter<DoctorFullDto>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorImporter"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="component">The component.</param>
        public DoctorImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component)
        {
        }

        #endregion Constructors

        #region Properties

        public static DoctorFullDto[] Cache
        {
            get { return InternalCache.Values.ToArray(); }
        }

        #endregion Properties

        #region Methods

        protected override DoctorFullDto Map(SQLiteDataReader reader)
        {
            var doctor = new DoctorFullDto() { IsImported = true };
            doctor.Specialisation = this.MapSpecialisation(reader["fk_DoctorType"] as long?);

            doctor.Address.BoxNumber = reader["BoxNumber"] as string;
            doctor.Address.City = reader["City"] as string;
            doctor.Address.PostalCode = reader["PostalCode"] as string;
            doctor.Address.Street = reader["Street"] as string;
            doctor.Address.StreetNumber = reader["StreetNumber"] as string;

            doctor.FirstName = reader["FirstName"] as string;
            doctor.Gender = (reader["Sex"] as string == "M") ? Gender.Male : Gender.Female;
            doctor.LastName = reader["LastName"] as string;
            doctor.ProMail = reader["Mail"] as string;
            doctor.ProMobile = reader["MobilePro"] as string;
            doctor.ProPhone = reader["PhonePro"] as string;

            doctor.Counter = reader["Counter"] as int? ?? 0;
            doctor.IsComplete = reader["IsComplete"] as bool? ?? false;
            doctor.LastUpdate = reader["LastUpdate"] as DateTime? ?? DateTime.Today;
            doctor.Thumbnail = reader["Thumbnail"] as byte[];

            return doctor;
        }

        protected override string Sql(long id)
        {
            return string.Format(@"SELECT *
                                   FROM Doctor
                                   INNER JOIN Person ON Doctor.ID = Person.ID
                                   INNER JOIN DoctorType ON Doctor.fk_DoctorType = DoctorType.ID
                                   INNER JOIN Address ON Person.fk_Address = Address.ID
                                   INNER JOIN Patient_Doctor ON Patient_Doctor.fk_Doctor = Doctor.ID
                                   WHERE Patient_Doctor.fk_Patient = {0}", id);
        }

        private TagDto MapSpecialisation(long? id)
        {
            var importer = new TagImporter(this.Connection, this.Component, TagCategory.Doctor, "DoctorType");
            this.Relay(importer);

            return importer.Import(id);
        }

        #endregion Methods
    }
}