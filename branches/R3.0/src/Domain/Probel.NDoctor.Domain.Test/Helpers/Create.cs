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
namespace Probel.NDoctor.Domain.Test.Helpers
{
    using System;
    using System.Collections.Generic;

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Objects;

    public static class Create
    {
        #region Methods

        public static Address Address()
        {
            var address = new Address()
            {
                BoxNumber = "box number",
                City = "city",
                PostalCode = "postal code",
                Street = "street",
                StreetNumber = "street number",
            };
            return address;
        }

        public static Doctor ADoctor(Address address, Tag specialisation)
        {
            return ADoctor(address, specialisation, Guid.NewGuid().ToString());
        }

        public static Doctor ADoctor(Address address, Tag specialisation, string lastName)
        {
            return new Doctor()
            {
                Address = address,
                Counter = 0,
                FirstName = Guid.NewGuid().ToString(),
                Gender = Gender.Male,
                LastName = lastName,
                LastUpdate = DateTime.Now,
                ProMail = Guid.NewGuid().ToString(),
                ProMobile = Guid.NewGuid().ToString(),
                ProPhone = Guid.NewGuid().ToString(),
                Specialisation = specialisation,
                Tag = specialisation,
            };
        }

        public static Insurance AnInsurance(string name)
        {
            return new Insurance()
            {
                Address = Address(),
                Name = name,
                Notes = Guid.NewGuid().ToString(),
                Phone = Guid.NewGuid().ToString(),
            };
        }

        public static Patient APatientBillGates(Address address, Tag tag, Reputation reputation, Insurance insurance, Practice practice, Profession profession)
        {
            return new Patient()
            {
                FirstName = "Bill",
                LastName = "Gates",
                ProMail = "Some mail pro",
                ProMobile = "Some mobile pro",
                ProPhone = "Some phone pro",
                Address = address,
                BirthDate = new DateTime(2000, 1, 1),
                Counter = 10,
                Fee = 150,
                Gender = Gender.Male,
                Height = 190,
                InscriptionDate = new DateTime(2001, 10, 10),
                Insurance = insurance,
                LastUpdate = DateTime.Now,
                PlaceOfBirth = Guid.NewGuid().ToString(),
                Practice = practice,
                PrivateMail = Guid.NewGuid().ToString(),
                PrivateMobile = Guid.NewGuid().ToString(),
                PrivatePhone = Guid.NewGuid().ToString(),
                Profession = profession,
                Reason = Guid.NewGuid().ToString(),
                Reputation = reputation,
                Tag = tag,
            };
        }

        public static Practice APractice(Address address, string name)
        {
            return new Practice()
            {
                Address = address,
                Name = name,
                Notes = Guid.NewGuid().ToString(),
                Phone = Guid.NewGuid().ToString(),

            };
        }

        public static Profession AProfession(string name)
        {
            return new Profession()
                {
                    Name = name,
                    Notes = Guid.NewGuid().ToString(),
                };
        }

        public static Reputation AReputation(string name)
        {
            return new Reputation()
            {
                Name = name,
                Notes = Guid.NewGuid().ToString(),
            };
        }

        public static Tag ASpecialisation(string name)
        {
            return new Tag()
            {
                Category = TagCategory.Doctor,
                Name = name,
                Notes = Guid.NewGuid().ToString(),
            };
        }

        public static Tag ASpecialisation()
        {
            return ASpecialisation(Guid.NewGuid().ToString());
        }

        public static Tag ATag(TagCategory type, string name)
        {
            return new Tag()
            {
                Name = name,
                Notes = Guid.NewGuid().ToString(),
                Category = type
            };
        }

        public static Task ATask()
        {
            return new Task()
            {
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
            };
        }

        public static User AUser(Address address, Role role, Practice practice, bool isDefaultUser)
        {
            var robertDupont = new User()
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString(),
                Header = Guid.NewGuid().ToString(),
                ProMail = Guid.NewGuid().ToString(),
                ProMobile = Guid.NewGuid().ToString(),
                ProPhone = Guid.NewGuid().ToString(),
                Practice = practice,
                AssignedRole = role,
                IsDefault = isDefaultUser,
            };
            return robertDupont;
        }

        public static Role GetRole(Task task)
        {
            var role = new Role()
            {
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Tasks = new List<Task>() { task },
            };
            return role;
        }

        public static Insurance Insurance()
        {
            return AnInsurance(Guid.NewGuid().ToString());
        }

        public static Patient Patient(Address address, Tag tag, Reputation reputation, Insurance insurance, Practice practice, Profession profession)
        {
            return new Patient()
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                ProMail = Guid.NewGuid().ToString(),
                ProMobile = Guid.NewGuid().ToString(),
                ProPhone = Guid.NewGuid().ToString(),
                Address = address,
                BirthDate = new DateTime(2000, 1, 1),
                Counter = 10,
                Fee = 150,
                Gender = Gender.Male,
                Height = 190,
                InscriptionDate = new DateTime(2001, 10, 10),
                Insurance = insurance,
                LastUpdate = DateTime.Now,
                PlaceOfBirth = Guid.NewGuid().ToString(),
                Practice = practice,
                PrivateMail = Guid.NewGuid().ToString(),
                PrivateMobile = Guid.NewGuid().ToString(),
                PrivatePhone = Guid.NewGuid().ToString(),
                Profession = profession,
                Reason = Guid.NewGuid().ToString(),
                Reputation = reputation,
                Tag = tag,
            };
        }

        public static Patient Patient()
        {
            return Create.Patient(Create.Address()
                , Create.Tag(TagCategory.Patient), Create.Reputation(), Create.Insurance()
                , Create.Practice(Create.Address()), Create.Profession());
        }

        public static Patient PatientWithFamily()
        {
            var father = new Patient { Id = 100, FirstName = "father", LastName = "father" };
            var mother = new Patient { Id = 100, FirstName = "mother", LastName = "mother" };

            var current = new Patient { Id = 100, FirstName = "current", LastName = "current", Father = father, Mother = mother };

            var child1 = new Patient { Id = 100, FirstName = "child1", LastName = "child1", Father = current };
            var child2 = new Patient { Id = 100, FirstName = "child2", LastName = "child2", Father = current };
            var child3 = new Patient { Id = 100, FirstName = "child3", LastName = "child3", Father = current };

            return current;
        }

        public static Patient PatientWithMedicalRecord()
        {
            var patient = new Patient() { Id = 100, FirstName = "First1", LastName = "Last1" };

            var tag1 = new Tag() { Category = TagCategory.MedicalRecord, Name = "tag1" };
            var tag2 = new Tag() { Category = TagCategory.MedicalRecord, Name = "tag2" };
            var tag3 = new Tag() { Category = TagCategory.MedicalRecord, Name = "tag3" };

            List<MedicalRecord> records = new List<MedicalRecord>();
            records.Add(new MedicalRecord() { Tag = tag1 });
            records.Add(new MedicalRecord() { Tag = tag1 });
            records.Add(new MedicalRecord() { Tag = tag2 });
            records.Add(new MedicalRecord() { Tag = tag2 });
            records.Add(new MedicalRecord() { Tag = tag3 });
            records.Add(new MedicalRecord() { Tag = tag3 });

            foreach (var record in records) patient.MedicalRecords.Add(record);

            return patient;
        }

        public static Practice Practice(Address address)
        {
            return APractice(address, Guid.NewGuid().ToString());
        }

        public static Profession Profession()
        {
            return AProfession(Guid.NewGuid().ToString());
        }

        public static Reputation Reputation()
        {
            return AReputation(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Gets the tag.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Tag Tag(TagCategory type)
        {
            return ATag(type, Guid.NewGuid().ToString());
        }

        #endregion Methods
    }
}