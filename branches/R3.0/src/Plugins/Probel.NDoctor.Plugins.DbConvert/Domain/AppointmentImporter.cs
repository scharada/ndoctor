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

    public class AppointmentImporter : MultipleImporter<AppointmentDto>
    {
        #region Fields

        private static TagDto DefaultTag = new TagDto(TagCategory.Appointment)
        {
            Name = Messages.Title_DefaultAppointementType,
            Notes = Messages.Msg_DoneByConverter,
            IsImported = true,
        };

        private LightUserDto defaultUser;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorImporter"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="component">The component.</param>
        public AppointmentImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component)
        {
        }

        #endregion Constructors

        #region Properties

        public static AppointmentDto[] Cache
        {
            get { return InternalCache.Values.ToArray(); }
        }

        private LightUserDto DefaultUser
        {
            get
            {
                if (this.defaultUser == null)
                {
                    using (this.Component.UnitOfWork) { this.defaultUser = this.Component.GetDefaultUser(); }
                }
                return this.defaultUser;
            }
        }

        #endregion Properties

        #region Methods

        protected override AppointmentDto Map(SQLiteDataReader reader)
        {
            var rid = reader["Id"] as long?;
            Identifier id;

            if (!rid.HasValue) { throw new NullReferenceException(); }
            else { id = new Identifier(rid.Value); }

            if (InternalCache.ContainsKey(id)) { return InternalCache[id]; }
            else
            {
                var item = new AppointmentDto() { IsImported = true };

                item.StartTime = reader["StartDate"] as DateTime? ?? DateTime.Now;
                item.EndTime = reader["EndDate"] as DateTime? ?? DateTime.Now;

                item.Subject = reader["Title"] as string;
                item.Notes = Messages.Msg_DoneByConverter;
                item.User = this.DefaultUser;
                item.Tag = this.MapTag(reader["fk_MeetingType"] as long?);

                InternalCache.Add(id, item);

                return item;
            }
        }

        protected override string Sql(long id)
        {
            return string.Format(
                      @"SELECT *
                        FROM Meeting
                        INNER JOIN MeetingType on Meeting.fk_MeetingType = MeetingType.ID
                        INNER JOIN Patient_Meeting ON Meeting.ID = Patient_Meeting.fk_Meeting
                        WHERE Patient_Meeting.fk_Patient = {0}", id);
        }

        private TagDto MapTag(long? id)
        {
            if (!id.HasValue) return DefaultTag;

            var importer = new TagImporter(this.Connection, this.Component, TagCategory.Appointment, "MeetingType");
            importer.Default = DefaultTag;
            this.Relay(importer);

            return importer.Import(id.Value);
        }

        #endregion Methods
    }
}