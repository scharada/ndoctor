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

    using AutoMapper;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.Test.Helpers;

    public class TestBaseComponent : TestBase<BaseComponent>
    {
        #region Methods

        [Test]
        public void CanCreateAndDeleteDoctor()
        {
            var countBefore = this.Component.GetAllDoctorsLight().Count;
            var uniqueName = Guid.NewGuid().ToString();

            var item = Mapper.Map<Doctor, LightDoctorDto>(Create.ADoctor(Create.Address(), Create.ASpecialisation(), uniqueName));

            this.Transaction(() => this.Component.Create(item));

            var countAfter = this.Component.GetAllDoctorsLight().Count;
            Assert.AreEqual(countBefore + 1, countAfter, "The practice wasn't added");

            var result = this.Component.FindDoctorsByNameLight(uniqueName, SearchOn.LastName);
            Assert.AreEqual(1, result.Count, "The practice wasn't found");

            this.Transaction(() => this.Component.Remove(new LightDoctorDto() { Id = result[0].Id }));

            countAfter = this.Component.GetAllDoctorsLight().Count;
            Assert.AreEqual(countAfter, countBefore, "The insurance wasn't deleted");
        }

        [Test]
        public void CanCreateAndDeletePatient()
        {
            int countBefore, countAfter = 0;

            countBefore = this.Component.GetAllPatientsLight().Count;

            var item = Mapper.Map<Patient, PatientDto>(Create.Patient());

            var uniqueName = item.FirstName;

            this.Transaction(() => this.Component.Create(item));

            countAfter = this.Component.GetAllPatientsLight().Count;
            Assert.AreEqual(countBefore + 1, countAfter, "The patient wasn't added");

            var result = this.Component.FindPatientsByNameLight(uniqueName, SearchOn.FirstAndLastName);
            Assert.AreEqual(1, result.Count, "The patient wasn't found");

            this.Transaction(() => this.Component.Remove(new PatientDto() { Id = result[0].Id }));

            countAfter = this.Component.GetAllPatientsLight().Count;
            Assert.AreEqual(countAfter, countBefore, "The patient wasn't deleted");
        }

        [Test]
        public void CanCreateAndDeleteTag()
        {
            var tagType = TagCategory.Doctor;

            int countBefore, countAfter = 0;
            var uniqueName = Guid.NewGuid().ToString();

            countBefore = this.Component.GetAllTags().Count;

            var item = Mapper.Map<Tag, TagDto>(Create.ATag(tagType, uniqueName));

            this.Transaction(() => this.Component.Create(item));

            countAfter = this.Component.GetAllTags().Count;
            Assert.AreEqual(countBefore + 1, countAfter, "The tag wasn't added");

            var result = this.Component.FindTags(uniqueName, tagType);
            Assert.AreEqual(1, result.Count, "The tag wasn't found");

            this.Transaction(() => this.Component.Remove(new TagDto(TagCategory.MedicalRecord) { Id = result[0].Id }));

            countAfter = this.Component.GetAllTags().Count;
            Assert.AreEqual(countAfter, countBefore, "The tag wasn't deleted");
        }

        [Test]
        public void CanGetAllDoctors()
        {
            var result = this.Component.GetAllDoctorsLight();
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void CanGetAllInsurances()
        {
            var result = this.Component.GetAllInsurances();
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public void CanGetAllInsurancesLight()
        {
            var result = this.Component.GetAllInsurancesLight();
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public void CanGetAllInsurancesWithName()
        {
            var result = this.Component.FindInsurances("Insurance 1");
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void CanGetAllPractices()
        {
            var result = this.Component.GetAllPractices();
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void CanGetAllPracticesLight()
        {
            var result = this.Component.GetAllPracticesLight();
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void CanGetAllPracticesWithName()
        {
            var result = this.Component.FindPractices("Cabinet de Liege");
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void CanGetAllProfessions()
        {
            var result = this.Component.GetAllProfessions();
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void CanGetAllProfessionsWithName()
        {
            var result = this.Component.FindProfessions("some profession");
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void CanGetAllReputations()
        {
            var result = this.Component.GetAllReputations();

            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void CanGetAllReputationsWithName()
        {
            var result = this.Component.FindReputations("some reputation");

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void CanGetAllRoleLight()
        {
            var result = this.Component.GetAllRolesLight();
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void CanGetAllTagsForPatient()
        {
            var result = this.Component.FindTags(TagCategory.Patient);

            foreach (var item in result) Assert.AreEqual(item.Category, TagCategory.Patient);

            Assert.AreEqual(result.Count, 2);
        }

        [Test]
        public void CanGetAllTagsForPatientWithName()
        {
            var result = this.Component.FindTags("Some tag", TagCategory.Patient);

            foreach (var item in result) Assert.AreEqual(item.Category, TagCategory.Patient);

            Assert.AreEqual(result.Count, 1);
        }

        [Test]
        public void CanGetAllUsers()
        {
            var result = this.Component.GetAllUsers();
            Assert.AreEqual(2, result.Count, "3 users are expected");
        }

        [Test]
        public void CanGetDoctorOnFirstAndLastName()
        {
            var result = this.Component.FindDoctorsByNameLight("ert", SearchOn.FirstAndLastName);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void CanGetDoctorOnFirstName()
        {
            var result = this.Component.FindDoctorsByNameLight("ert", SearchOn.FirstName);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void CanGetDoctorOnLastName()
        {
            var result = this.Component.FindDoctorsByNameLight("pont", SearchOn.LastName);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void CanGetDoctorOnSpecialisation()
        {
            var specialisations = this.Component.FindTags("Dentist", TagCategory.Doctor);
            Assert.AreEqual(1, specialisations.Count, "Only one specialisation 'Dentist' should exist");

            var result = this.Component.FindDoctorsBySpecialisationLight(specialisations[0]);
            Assert.AreEqual(2, result.Count, "2 doctors with this specialisation are expected");
        }

        [Test]
        public void CanGetPatientOnFirstAndLastName()
        {
            var result = this.Component.FindPatientsByNameLight("patient", SearchOn.FirstAndLastName);
            Assert.NotNull(result);
            Assert.AreEqual(9, result.Count);
        }

        [Test]
        public void CanGetPatientOnFirstName()
        {
            var result = this.Component.FindPatientsByNameLight("patient", SearchOn.FirstName);
            Assert.NotNull(result);
            Assert.AreEqual(9, result.Count);
        }

        [Test]
        public void CanGetPatientOnLastName()
        {
            var result = this.Component.FindPatientsByNameLight("pont", SearchOn.LastName);
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void CanGetUserByItsId()
        {
            var user = this.Component.GetUserById(1);

            Assert.NotNull(user, "The user 'Robert Dupont' should be in the database");
            Assert.AreEqual("User Robert", user.FirstName);
            Assert.AreEqual("Dupont", user.LastName);
        }

        [Test]
        [ExpectedException(typeof(DetachedEntityException))]
        public void FailToGetDoctorOnSpecialisationWhenDetached()
        {
            var specialisation = new TagDto(TagCategory.Doctor)
            {
                Name = "SpecialisationToFind",
            };

            var result = this.Component.FindDoctorsBySpecialisationLight(specialisation);
            Assert.AreEqual(2, result.Count, "2 doctors with this specialisation are expected");
        }

        /// <summary>
        /// Gets the component instance.
        /// </summary>
        /// <returns></returns>
        protected override BaseComponent GetComponentInstance()
        {
            return new MockBaseComponent(this.Database.Session);
        }

        private PracticeDto CreatePractice(string uniqueName)
        {
            var practice = new PracticeDto()
            {
                Address = new AddressDto()
                {
                    BoxNumber = Guid.NewGuid().ToString(),
                    City = Guid.NewGuid().ToString(),
                    PostalCode = Guid.NewGuid().ToString(),
                    Street = Guid.NewGuid().ToString(),
                    StreetNumber = Guid.NewGuid().ToString(),
                },
                Name = uniqueName,
                Notes = Guid.NewGuid().ToString(),
                Phone = Guid.NewGuid().ToString(),
            };
            return practice;
        }

        private ProfessionDto CreateProfession(string uniqueName)
        {
            return new ProfessionDto()
            {
                Name = uniqueName,
                Notes = Guid.NewGuid().ToString(),
            };
        }

        #endregion Methods
    }
}