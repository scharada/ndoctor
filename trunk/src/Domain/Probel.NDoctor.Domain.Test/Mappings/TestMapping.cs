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
namespace Probel.NDoctor.Domain.Test.Mappings
{
    using System.Collections.Generic;

    using AutoMapper;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.Test.Helpers;

    [Category(Categories.UnitTest)]
    [TestFixture]
    public class TestMapping
    {
        #region Methods

        [Test]
        public void CanMapMedicalCabinet_EntityToDto()
        {
            Patient patient = Create.PatientWithMedicalRecord();
            var cabinet = Mapper.Map<Patient, MedicalRecordCabinetDto>(patient);

            var recordCount = CountRecords(cabinet);
            var tagCount = cabinet.Folders.Length;

            Assert.AreEqual(3, tagCount, "Not the expected tag count ");
            Assert.AreEqual(6, recordCount, "Not the expected record count");
        }

        [TestFixtureSetUp]
        public void SetUpFixtureAttribute()
        {
            GlobalConfigurator.Configure();
        }

        private int CountRecords(MedicalRecordCabinetDto cabinet)
        {
            var list = new HashSet<MedicalRecordDto>();

            foreach (var folder in cabinet.Folders)
                foreach (var record in folder.Records)
                {
                    if (!list.Contains(record))
                    {
                        list.Add(record);
                    }
                }

            return list.Count;
        }

        #endregion Methods
    }
}