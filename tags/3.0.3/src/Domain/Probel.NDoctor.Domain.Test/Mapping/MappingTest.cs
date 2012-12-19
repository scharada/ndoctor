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

namespace Probel.NDoctor.Domain.Test.Mapping
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Mappings;
    using Probel.NDoctor.Domain.DTO.Objects;

    [TestFixture]
    public class MappingTest
    {
        #region Properties

        public Tag RandomTag
        {
            get
            {
                return new Tag()
                {
                    Category = TagCategory.Appointment,
                    Name = this.RandomString,
                    Notes = this.RandomString,
                };
            }
        }

        private MedicalRecord RandomMedicalRecord
        {
            get
            {
                return new MedicalRecord()
                {
                    CreationDate = DateTime.Now.AddDays(-10),
                    LastUpdate = DateTime.Now,
                    Name = this.RandomString,
                    Rtf = this.RandomString,
                    Tag = this.RandomTag,
                };
            }
        }

        private string RandomString
        {
            get { return Guid.NewGuid().ToString(); }
        }

        #endregion Properties

        #region Methods

        [Test]
        public void ListMapEntityToDto_MapDrugToDrugDto_MappingOccured()
        {
            var temp = new List<Drug>();
            var tag = this.RandomTag;
            var length = 10;

            for (int i = 0; i < length; i++)
            {
                temp.Add(new Drug()
                {
                    Name = this.RandomString,
                    Notes = this.RandomString,
                    Tag = tag,
                });
            }

            var record = temp.ToArray();
            var mapped = Mapper.Map<Drug[], DrugDto[]>(record);

            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(record[i].Id, mapped[i].Id);
                Assert.AreEqual(record[i].Notes, mapped[i].Notes);
                Assert.AreEqual(record[i].Name, mapped[i].Name);
                Assert.AreEqual(record[i].Tag.Category, mapped[i].Tag.Category);
                Assert.AreEqual(record[i].Tag.Name, mapped[i].Tag.Name);
                Assert.AreEqual(record[i].Tag.Notes, mapped[i].Tag.Notes);
            }
        }

        [Test]
        public void ListMapEntityToDto_MapMedicalRecordToMedicalRecordDto_MappingOccured()
        {
            var temp = new List<MedicalRecord>();
            var tag1 = this.RandomTag;
            var length = 10;

            for (int i = 0; i < length; i++)
            {
                temp.Add(new MedicalRecord()
                {
                    CreationDate = DateTime.Now.AddDays(-10),
                    LastUpdate = DateTime.Now,
                    Name = this.RandomString,
                    Rtf = this.RandomString,
                    Tag = tag1,
                });
            }
            var record = temp.ToArray();

            var mapped = Mapper.Map<MedicalRecord[], MedicalRecordDto[]>(record);

            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(record[i].CreationDate, mapped[i].CreationDate);
                Assert.AreEqual(record[i].Id, mapped[i].Id);
                Assert.AreEqual(record[i].LastUpdate, mapped[i].LastUpdate);
                Assert.AreEqual(record[i].Name, mapped[i].Name);
                Assert.AreEqual(record[i].Rtf, mapped[i].Rtf);
                Assert.AreEqual(record[i].Tag.Category, mapped[i].Tag.Category);
                Assert.AreEqual(record[i].Tag.Name, mapped[i].Tag.Name);
                Assert.AreEqual(record[i].Tag.Notes, mapped[i].Tag.Notes);
            }
        }

        [Test]
        public void ListMapEntityToDto_MapTagToTagDto_MappingOccured()
        {
            var entityList = new List<Tag>();
            entityList.Add(new Tag()
            {
                Category = TagCategory.Appointment,
                Notes = null,
                Name = "Hello world 1",
            });
            entityList.Add(new Tag()
            {
                Category = TagCategory.Appointment,
                Notes = null,
                Name = "Hello world 2",
            });
            entityList.Add(new Tag()
            {
                Category = TagCategory.Appointment,
                Notes = null,
                Name = "Hello world 3",
            });
            var dtoList = Mapper.Map<IList<Tag>, IList<TagDto>>(entityList);

            Assert.AreEqual(entityList.Count, entityList.Count);

            Assert.AreEqual(entityList[0].Category, dtoList[0].Category);
            Assert.Null(dtoList[0].Notes);
            Assert.AreEqual(entityList[0].Notes, dtoList[0].Notes);

            Assert.AreEqual(entityList[1].Category, dtoList[1].Category);
            Assert.Null(dtoList[1].Notes);
            Assert.AreEqual(entityList[1].Notes, dtoList[1].Notes);

            Assert.AreEqual(entityList[2].Category, dtoList[2].Category);
            Assert.Null(dtoList[2].Notes);
            Assert.AreEqual(entityList[2].Notes, dtoList[2].Notes);
        }

        [Test]
        public void MapEntityToDto_MapMedicalRecordToMedicalRecordDto_MappingOccured()
        {
            var record = new MedicalRecord()
            {
                CreationDate = DateTime.Now.AddDays(-10),
                LastUpdate = DateTime.Now,
                Name = this.RandomString,
                Rtf = this.RandomString,
                Tag = this.RandomTag,
            };

            var mapped = Mapper.Map<MedicalRecord, MedicalRecordDto>(record);

            Assert.AreEqual(record.CreationDate, mapped.CreationDate);
            Assert.AreEqual(record.Id, mapped.Id);
            Assert.AreEqual(record.LastUpdate, mapped.LastUpdate);
            Assert.AreEqual(record.Name, mapped.Name);
            Assert.AreEqual(record.Rtf, mapped.Rtf);
            Assert.AreEqual(record.Tag.Category, mapped.Tag.Category);
            Assert.AreEqual(record.Tag.Name, mapped.Tag.Name);
            Assert.AreEqual(record.Tag.Notes, mapped.Tag.Notes);
        }

        [Test]
        public void MapEntityToDto_MapPatientToRecord_MappingOccured()
        {
            var patient = new Patient();
            patient.MedicalRecords.Add(this.RandomMedicalRecord);

            var cabinet = Mapper.Map<Patient, MedicalRecordCabinetDto>(patient);

            Assert.AreEqual(1, cabinet.Folders.Count);
            Assert.AreEqual(1, cabinet.Folders[0].Records.Count);

            Assert.AreEqual(patient.MedicalRecords[0].CreationDate, cabinet.Folders[0].Records[0].CreationDate);
            Assert.AreEqual(patient.MedicalRecords[0].LastUpdate, cabinet.Folders[0].Records[0].LastUpdate);
            Assert.AreEqual(patient.MedicalRecords[0].Name, cabinet.Folders[0].Records[0].Name);
            Assert.AreEqual(patient.MedicalRecords[0].Rtf, cabinet.Folders[0].Records[0].Rtf);
            Assert.AreEqual(patient.MedicalRecords[0].Tag.Category, cabinet.Folders[0].Records[0].Tag.Category);
            Assert.AreEqual(patient.MedicalRecords[0].Tag.Name, cabinet.Folders[0].Records[0].Tag.Name);
            Assert.AreEqual(patient.MedicalRecords[0].Tag.Notes, cabinet.Folders[0].Records[0].Tag.Notes);
        }

        [Test]
        public void MapEntityToDto_MapTagToMedicalRecord_MappingOccured()
        {
            var tag = new Tag()
            {
                Name = this.RandomString,
                Notes = this.RandomString,
                Category = TagCategory.Appointment,
            };

            var folder = Mapper.Map<Tag, MedicalRecordFolderDto>(tag);

            Assert.AreEqual(tag.Name, folder.Name);
            Assert.AreEqual(tag.Notes, folder.Notes);
        }

        //[Test]
        //public void Configuration_CheckIfMappingIsValid_MappingIsValid()
        //{
        //    Mapper.AssertConfigurationIsValid();
        //}
        [Test]
        public void MapEntityToDto_MapTagToTagDto_MappingOccured()
        {
            var tag = new Tag()
            {
                Category = TagCategory.Appointment,
                Name = "Hello world",
                Notes = null,
            };
            var dto = Mapper.Map<Tag, TagDto>(tag);

            Assert.AreEqual(tag.Category, dto.Category);
            Assert.Null(dto.Notes);
            Assert.AreEqual(tag.Notes, dto.Notes);
        }

        [SetUp]
        public void Setup()
        {
            Mapper.Reset();
            AutoMapperMapping.Configure();
        }

        #endregion Methods
    }
}