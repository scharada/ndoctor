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
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SQLite;
    using System.Threading;

    using Probel.Helpers.Conversions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.DbConvert.Properties;

    public class PatientImporter : BaseImporter
    {
        #region Fields

        private static readonly TagDto PatientTag = new TagDto(TagCategory.Patient)
        {
            Name = "Patient",
            Notes = Messages.Msg_DoneByConverter,
            IsImported = true,
        };

        #endregion Fields

        #region Constructors

        public PatientImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component)
        {
        }

        #endregion Constructors

        #region Methods

        public long Count()
        {
            var sql = "SELECT COUNT(*) FROM Patient";
            using (var command = new SQLiteCommand(sql, this.Connection))
            {
                return (long)command.ExecuteScalar();
            }
        }

        public void Import()
        {
            var newPatients = new List<PatientFullDto>();

            if (this.Connection.State != ConnectionState.Open) this.Connection.Open();

            long count = this.Count();
            long step = 1;

            var sql = @"SELECT *
                        FROM Patient
                        INNER JOIN Person ON Person.ID = Patient.ID
                        INNER JOIN Address ON Person.fk_Address = Address.ID";

            using (var command = new SQLiteCommand(sql, this.Connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var current = this.MapPatient(reader);
                    newPatients.Add(current);
                    this.OnProgressChanged((int)(Percentage(count, ++step)));
                }
                this.OnLogged(Messages.Log_PatientCount, count);
            }

            using (this.Component.UnitOfWork) { this.Component.Create(newPatients.ToArray()); }
        }

        private static int Percentage(long count, long step)
        {
            var divideBy = (count / step);
            if (divideBy == 0) divideBy = 1;

            return (int)(100 / divideBy);
        }

        private IEnumerable<BmiDto> MapBmiHistory(long? id)
        {
            var importer = new BmiImporter(this.Connection, this.Component);

            return importer.Import(id);
        }

        private IEnumerable<DoctorFullDto> MapDoctors(long? id)
        {
            var importer = new DoctorImporter(this.Connection, this.Component);

            return importer.Import(id);
        }

        private IEnumerable<IllnessPeriodDto> MapIllnessHistory(long? id)
        {
            var importer = new IllnessPeriodImporter(this.Connection, this.Component);
            this.Relay(importer);

            return importer.Import(id);
        }

        private InsuranceDto MapInsurance(SQLiteDataReader reader)
        {
            var importer = new InsuranceImporter(this.Connection, this.Component);
            this.Relay(importer);

            return importer.Import((reader[Columns.Practice] as long?));
        }

        private IEnumerable<AppointmentDto> MapMeetings(long? id)
        {
            var importer = new AppointmentImporter(this.Connection, this.Component);
            this.Relay(importer);

            return importer.Import(id);
        }

        private PatientFullDto MapPatient(SQLiteDataReader reader)
        {
            var current = new PatientFullDto() { IsImported = true };
            current.Address.BoxNumber = reader[Columns.BoxNumber] as string;
            current.Address.City = reader[Columns.City] as string;
            current.Address.PostalCode = reader[Columns.PostalCode] as string;
            current.Address.Street = reader[Columns.Street] as string;
            current.Address.StreetNumber = reader[Columns.StreetNumber] as string;

            current.Birthdate = reader[Columns.Birthdate] as DateTime? ?? DateTime.MinValue;
            current.Counter = reader[Columns.Counter] as int? ?? default(int);
            current.Fee = 0;
            current.FirstName = reader[Columns.FirstName] as string;
            current.Gender = ((reader[Columns.Gender] as string) == "M") ? Gender.Male : Gender.Female;
            current.Header = string.Empty;
            current.Height = reader[Columns.Height] as long? ?? default(long);
            current.InscriptionDate = reader[Columns.InscriptionDate] as DateTime? ?? DateTime.MinValue;
            current.IsComplete = reader[Columns.IsComplete] as bool? ?? default(bool);
            current.LastName = reader[Columns.LastName] as string;
            current.LastUpdate = reader[Columns.LastUpdate] as DateTime? ?? DateTime.MinValue;
            current.PlaceOfBirth = reader[Columns.PlaceOfBirth] as string;
            current.PrivateMail = reader[Columns.PrivateMail] as string;
            current.PrivateMobile = reader[Columns.PrivateMobile] as string;
            current.PrivatePhone = reader[Columns.PrivatePhone] as string;
            current.ProMail = string.Empty;

            current.ProMobile = reader[Columns.ProMobile] as string;
            current.ProPhone = reader[Columns.ProPhone] as string;
            current.Reason = reader[Columns.Reason] as string;
            current.Thumbnail = reader[Columns.Thumbnail] as byte[];

            current.Tag = PatientTag;

            current.Reputation = this.MapReputation(reader);
            current.Profession = this.MapProfession(reader);
            current.Insurance = this.MapInsurance(reader);
            current.Practice = this.MapPractice(reader);

            current.Doctors.Refill(this.MapDoctors(reader[Columns.Id] as long?));
            current.MedicalRecords.Refill(this.MapRecords(reader[Columns.Id] as long?));
            current.IllnessHistory.Refill(this.MapIllnessHistory(reader[Columns.Id] as long?));
            current.PrescriptionDocuments.Refill(this.MapPrescriptions(reader[Columns.Id] as long?));
            current.BmiHistory.Refill(this.MapBmiHistory(reader[Columns.Id] as long?));
            current.Pictures.Refill(this.MapPictures(reader[Columns.Id] as long?));
            current.Appointments.Refill(this.MapMeetings(reader[Columns.Id] as long?));

            return current;
        }

        private IEnumerable<PictureDto> MapPictures(long? id)
        {
            var importer = new PictureImporter(this.Connection, this.Component);
            this.Relay(importer);

            return importer.Import(id);
        }

        private PracticeDto MapPractice(SQLiteDataReader reader)
        {
            var importer = new PracticeImporter(this.Connection, this.Component);
            this.Relay(importer);

            return importer.Import((reader[Columns.Practice] as long?));
        }

        private IEnumerable<PrescriptionDocumentDto> MapPrescriptions(long? id)
        {
            var importer = new PrescriptionDocumentImporter(this.Connection, this.Component);
            this.Relay(importer);

            return importer.Import(id);
        }

        private ProfessionDto MapProfession(SQLiteDataReader reader)
        {
            // nDoctor version 2.0.9 doesn't have profession.
            return null;
        }

        private IEnumerable<MedicalRecordDto> MapRecords(long? id)
        {
            var importer = new RecordImporter(this.Connection, this.Component);
            this.Relay(importer);

            return importer.Import(id);
        }

        private ReputationDto MapReputation(SQLiteDataReader reader)
        {
            var importer = new ReputationImporter(this.Connection, this.Component);
            this.Relay(importer);

            return importer.Import((reader[Columns.Reputation] as long?));
        }

        #endregion Methods

        #region Nested Types

        private static class Columns
        {
            #region Fields

            public const string Birthdate = "DateOfBirth";
            public const string BoxNumber = "BoxNumber";
            public const string City = "City";
            public const string Counter = "Counter";
            public const string FirstName = "FirstName";
            public const string Gender = "Sex";
            public const string Header = "Header";
            public const string Height = "Height";
            public const string Id = "ID";
            public const string InscriptionDate = "InscriptionDate";
            public const string IsComplete = "IsComplete";
            public const string LastName = "LastName";
            public const string LastUpdate = "LastUpdate";
            public const string PlaceOfBirth = "PlaceOfBirth";
            public const string PostalCode = "PostalCode";
            public const string Practice = "fk_Practice";
            public const string PrivateMail = "Mail";
            public const string PrivateMobile = "MobilePrivate";
            public const string PrivatePhone = "PhonePrivate";
            public const string Profession = "Profession";
            public const string ProMail = "ProMail";
            public const string ProMobile = "MobilePro";
            public const string ProPhone = "PhonePro";
            public const string Reason = "Reason";
            public const string Reputation = "fk_Reputation";
            public const string Street = "Street";
            public const string StreetNumber = "StreetNumber";
            public const string Thumbnail = "Thumbnail";

            #endregion Fields
        }

        #endregion Nested Types
    }
}