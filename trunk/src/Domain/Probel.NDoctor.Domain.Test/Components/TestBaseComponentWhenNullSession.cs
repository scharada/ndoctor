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
    using Probel.NDoctor.Domain.DAL.Helpers;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.Test.Helpers;

    public class TestBaseComponentWhenNullSession : TestBase<BaseComponent>
    {
        #region Methods

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void CanGetAllPracticesLight()
        {
            var result = this.Component.GetAllPracticesLight();
            Assert.AreEqual(4, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void CanGetUserByItsId()
        {
            var user = this.Component.GetUserById(1);

            Assert.NotNull(user, "The user 'Robert Dupont' should be in the database");
            Assert.AreEqual("Robert", user.FirstName);
            Assert.AreEqual("Dupont", user.LastName);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToCreateAndDeleteDoctor()
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
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToCreateAndDeletePatient()
        {
            int countBefore, countAfter = 0;

            countBefore = this.Component.GetAllPatientsLight().Count;

            var item = Mapper.Map<Patient, PatientDto>(Create.Patient(Create.Address()
                , Create.Tag(TagCategory.Doctor), Create.Reputation(), Create.Insurance()
                , Create.Practice(Create.Address()), Create.Profession()));

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
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToCreateAndDeleteTag()
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

            this.Transaction(() => this.Component.Remove(new TagDto() { Id = result[0].Id }));

            countAfter = this.Component.GetAllTags().Count;
            Assert.AreEqual(countAfter, countBefore, "The tag wasn't deleted");
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetAllDoctors()
        {
            var result = this.Component.GetAllDoctorsLight();
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetAllInsurances()
        {
            var result = this.Component.GetAllInsurances();
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetAllInsurancesWithName()
        {
            var result = this.Component.FindInsurances("Create");
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetAllPractices()
        {
            var result = this.Component.GetAllPractices();
            Assert.AreEqual(4, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetAllPracticesWithName()
        {
            var result = this.Component.FindPractices("PracticeToFind");
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetAllProfessions()
        {
            var result = this.Component.GetAllProfessions();
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetAllProfessionsWithName()
        {
            var result = this.Component.FindProfessions("ProfessionToFind");
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetAllReputations()
        {
            var result = this.Component.FindReputations("ReputationToFind");

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetAllReputationsWithName()
        {
            var result = this.Component.GetAllReputations();

            Assert.AreEqual(3, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetAllTagsForPatient()
        {
            var result = this.Component.FindTags(TagCategory.Patient);

            foreach (var item in result) Assert.AreEqual(item.Category, TagCategory.Patient);

            Assert.AreEqual(result.Count, 3);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetAllTagsForPatientWithName()
        {
            var result = this.Component.FindTags("TagToFind", TagCategory.Patient);

            foreach (var item in result) Assert.AreEqual(item.Category, TagCategory.Patient);

            Assert.AreEqual(result.Count, 1);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetAllUsers()
        {
            var result = this.Component.GetAllUsers();
            Assert.AreEqual(3, result.Count, "3 users are expected");
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetDoctorOnFirstAndLastName()
        {
            var result = this.Component.FindDoctorsByNameLight("DoctorLastNameToFind", SearchOn.FirstAndLastName);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetDoctorOnFirstName()
        {
            var result = this.Component.FindDoctorsByNameLight("DoctorLastNameToFind", SearchOn.FirstName);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetDoctorOnLastName()
        {
            var result = this.Component.FindDoctorsByNameLight("DoctorFirstNameToFind", SearchOn.LastName);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetDoctorOnSpecialisation()
        {
            var specialisations = this.Component.FindTags("SpecialisationToFind", TagCategory.Doctor);
            Assert.AreEqual(1, specialisations.Count, "Only one specialisation '{0}' should exist", "SpecialisationToFind");

            var result = this.Component.FindDoctorsBySpecialisationLight(specialisations[0]);
            Assert.AreEqual(2, result.Count, "2 doctors with this specialisation are expected");
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetDoctorOnSpecialisationWhenDetached()
        {
            var specialisation = new TagDto()
            {
                Name = "SpecialisationToFind",
                Category = TagCategory.Doctor
            };

            var result = this.Component.FindDoctorsBySpecialisationLight(specialisation);
            Assert.AreEqual(2, result.Count, "2 doctors with this specialisation are expected");
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetPatientOnFirstAndLastName()
        {
            var result = this.Component.FindPatientsByNameLight("wa", SearchOn.FirstAndLastName);
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetPatientOnFirstName()
        {
            var result = this.Component.FindPatientsByNameLight("Luc", SearchOn.FirstName);
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        [ExpectedException(typeof(Probel.Helpers.Assertion.AssertionException))]
        public void FailToGetPatientOnLastName()
        {
            var result = this.Component.FindPatientsByNameLight("ke", SearchOn.LastName);
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        /// <summary>
        /// Gets the component instance.
        /// </summary>
        /// <returns></returns>
        protected override BaseComponent GetComponentInstance()
        {
            return new MockBaseComponent(this.Database.Session);
        }

        protected override void PostFixtureSetup()
        {
            TestTools.NullifySession(this.Component);
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