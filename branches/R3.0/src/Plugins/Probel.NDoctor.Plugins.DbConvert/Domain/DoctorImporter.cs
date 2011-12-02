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

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.DbConvert.Properties;

    public class DoctorImporter : BaseImporter
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="DoctorImporter"/> class.
        /// </summary>
        static DoctorImporter()
        {
            Cache = new Dictionary<long, DoctorFullDto>();
        }

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

        public static DoctorFullDto[] Doctors
        {
            get { return Cache.Values.ToArray(); }
        }

        private static Dictionary<long, DoctorFullDto> Cache
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public IEnumerable<DoctorFullDto> Import(long? id)
        {
            var result = new List<DoctorFullDto>();
            if (!id.HasValue) return result;

            var sql = @"SELECT *
                        FROM Doctor
                        INNER JOIN Person ON Doctor.ID = Person.ID
                        INNER JOIN DoctorType ON Doctor.fk_DoctorType = DoctorType.ID
                        INNER JOIN Address ON Person.fk_Address = Address.ID";

            using (var command = new SQLiteCommand(sql, this.Connection))
            using (var reader = command.ExecuteReader())
            {
                int count = 0;
                while (reader.Read())
                {
                    var current = this.Map(reader);
                    result.Add(current);
                    count++;
                }
                this.OnLogged(Messages.Log_DoctorCount, count, id.Value);
            }

            return result;
        }

        private DoctorFullDto Map(SQLiteDataReader reader)
        {
            var id = reader["Id"] as long?;

            if (!id.HasValue) { throw new NullReferenceException(); }
            else if (Cache.ContainsKey(id.Value)) { return Cache[id.Value]; }
            else
            {
                var doctor = new DoctorFullDto();
                doctor.Specialisation = this.MapSpecialisation(reader["fk_DoctorType"] as long?);

                doctor.Address.BoxNumber = reader["BoxNumber"] as string;
                doctor.Address.City = reader["City"] as string;
                doctor.Address.PostalCode = reader["PostalCode"] as string;
                doctor.Address.Street = reader["Street"] as string;
                doctor.Address.StreetNumber = reader["StreetNumber"] as string;

                doctor.FirstName = reader["FirstName"] as string;
                doctor.Gender = (reader["Sex"] as string == "M") ? Gender.Male : Gender.Female;
                doctor.LastName = reader["LastName"] as string;
                doctor.MailPro = reader["Mail"] as string;
                doctor.MobilePro = reader["MobilePro"] as string;
                doctor.PhonePro = reader["PhonePro"] as string;

                doctor.Counter = reader["Counter"] as int? ?? 0;
                doctor.IsComplete = reader["IsComplete"] as bool? ?? false;
                doctor.LastUpdate = reader["LastUpdate"] as DateTime? ?? DateTime.Today;
                doctor.Thumbnail = reader["Thumbnail"] as byte[];

                Cache.Add(id.Value, doctor);

                return doctor;
            }
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