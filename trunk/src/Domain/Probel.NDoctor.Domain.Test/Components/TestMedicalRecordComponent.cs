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
namespace Probel.NDoctor.Domain.Test.Component
{
    using System;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.Test.Helpers;

    public class TestMedicalRecordComponent : TestBase<MedicalRecordComponent>
    {
        #region Methods

        [Test]
        public void CanCreateNewRecordForPatient()
        {
            var uniqueId = Guid.NewGuid().ToString();

            var patients = this.Component.GetAllPatientsLight();
            var tags = this.Component.FindTags(TagCategory.MedicalRecord);

            var sql = "SELECT COUNT(*) FROM MedicalRecord";
            var cardCountBefore = (long)this.ExecuteNonQuery(sql);

            var record = new MedicalRecordDto() { Tag = new TagDto(TagCategory.MedicalRecord), Name = uniqueId };

            this.Transaction(() => this.Component.Create(record, patients[0]));

            var cardCountAfter = (long)this.ExecuteNonQuery(sql);
            Assert.AreEqual(cardCountBefore + 1, cardCountAfter, "The medical record wasn't added");
        }

        [Test]
        public void CanGetMedicalRecordCabinet()
        {
            var patients = this.Component.GetAllPatientsLight();

            Assert.Greater(patients.Count, 0, "No patient were found");

            var cabinet = this.Component.FindMedicalRecordCabinet(patients[0]);

            Assert.Greater(cabinet.Folders.Length, 0, "No folders in the cabinet");
            Assert.Greater(cabinet.Folders[0].Records.Length, 0, "No records in folders");
        }

        protected override MedicalRecordComponent GetComponentInstance()
        {
            return new MedicalRecordComponent(this.Database.Session);
        }

        private object ExecuteNonQuery(string sql)
        {
            using (var tx = this.Database.Session.Transaction)
            {
                var query = this.Database.Session.CreateQuery(sql);
                return query.UniqueResult();
            }
        }

        private void ExecuteSql(string sql)
        {
            using (var tx = this.Database.Session.Transaction)
            {
                var query = this.Database.Session.CreateQuery(sql);
                query.ExecuteUpdate();
                tx.Commit();
            }
        }

        #endregion Methods
    }
}