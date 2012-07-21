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

namespace Probel.NDoctor.Domain.DAL.Subcomponents
{
    using System;

    using AutoMapper;

    using log4net;

    using NHibernate;

    using Probel.Helpers.Assertion;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Helpers;
    using Probel.NDoctor.Domain.DAL.Properties;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;

    internal class Updator
    {
        #region Fields

        private readonly ILog Logger = LogManager.GetLogger(typeof(Creator));
        private readonly ISession Session;

        #endregion Fields

        #region Constructors

        public Updator(ISession session)
        {
            this.Session = session;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Updates the specified macro.
        /// </summary>
        /// <param name="macro">The macro.</param>
        public void Update(MacroDto macro)
        {
            var entity = Mapper.Map<MacroDto, Macro>(macro);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public void Update(TagDto tag)
        {
            var entity = Mapper.Map<TagDto, Tag>(tag);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified profession.
        /// </summary>
        /// <param name="profession">The tag.</param>
        public void Update(ProfessionDto profession)
        {
            var entity = Mapper.Map<ProfessionDto, Profession>(profession);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified reputation.
        /// </summary>
        /// <param name="reputation">The tag.</param>
        public void Update(ReputationDto reputation)
        {
            var entity = Mapper.Map<ReputationDto, Reputation>(reputation);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified drug.
        /// </summary>
        /// <param name="drug">The drug.</param>
        public void Update(DrugDto drug)
        {
            var entity = Mapper.Map<DrugDto, Drug>(drug);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified pathology.
        /// </summary>
        /// <param name="pathology">The drug.</param>
        public void Update(PathologyDto pathology)
        {
            var entity = Mapper.Map<PathologyDto, Pathology>(pathology);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified practice.
        /// </summary>
        /// <param name="practice">The drug.</param>
        public void Update(PracticeDto practice)
        {
            var entity = Mapper.Map<PracticeDto, Practice>(practice);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified insurance.
        /// </summary>
        /// <param name="insurance">The drug.</param>
        public void Update(InsuranceDto insurance)
        {
            var entity = Mapper.Map<InsuranceDto, Insurance>(insurance);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified doctor.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Update(DoctorDto item)
        {
            var entity = Mapper.Map<DoctorDto, Doctor>(item);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        public void Update(RoleDto role)
        {
            var entity = Mapper.Map<RoleDto, Role>(role);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Update(LightUserDto user)
        {
            var entity = this.Session.Get<User>(user.Id);
            Mapper.Map<LightUserDto, User>(user, entity);

            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified medical record.
        /// </summary>
        /// <param name="item">The insurance.</param>
        public void Update(MedicalRecordDto item)
        {
            Assert.IsNotNull(item, "The item to create shouldn't be null");

            var entity = this.Session.Get<MedicalRecord>(item.Id);
            var tag = this.Session.Get<Tag>(entity.Tag.Id);

            //Reload the tag to avoid exception from nhibernate
            Mapper.Map<MedicalRecordDto, MedicalRecord>(item, entity);

            entity.Tag = tag;
            this.Session.Merge(entity);
        }

        /// <summary>
        /// Commits the changes on medical record cabinet.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="cabinet">The cabinet.</param>
        public void Update(LightPatientDto patient, MedicalRecordCabinetDto cabinet)
        {
            using (var tx = this.Session.BeginTransaction())
            {
                cabinet.ForeachRecords(record =>
                {
                    switch (record.State)
                    {
                        case State.Clean:
                            //Nothing to do
                            break;
                        case State.Updated:
                            record.LastUpdate = DateTime.Now;
                            this.Update(record);
                            break;
                        case State.Created:
                            record.LastUpdate
                                = record.CreationDate
                                = DateTime.Now;
                            new Creator(this.Session).Create(record);
                            break;
                        case State.Removed:
                            new Remover(this.Session).Remove(record);
                            break;
                        default:
                            Assert.FailOnEnumeration(record.State);
                            break;
                    }
                });
                tx.Commit();
            }
        }

        /// <summary>
        /// Updates the specified prescription.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Update(PrescriptionDto item)
        {
            var entity = this.Session.Get<Prescription>(item.Id);
            Mapper.Map<PrescriptionDto, Prescription>(item, entity);

            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="item">The user.</param>
        public void Update(UserDto item)
        {
            var entity = this.Session.Get<User>(item.Id);
            Mapper.Map<UserDto, User>(item, entity);

            if (string.IsNullOrWhiteSpace(entity.Password)) throw new BusinessLogicException(Messages.Validation_PasswordCantBeEmpty);
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Update(MacroDto item)
        {
            var entity = this.Session.Get<Macro>(item.Id);
            Mapper.Map<MacroDto, Macro>(item, entity);

            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the password of the connected user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        public void Update(LightUserDto user, string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new BusinessLogicException(Messages.Validation_PasswordCantBeEmpty);

            var entity = this.Session.Get<User>(user.Id);
            if (entity == null) throw new EntityNotFoundException(typeof(User));

            entity.Password = password;
            this.Session.Update(entity);
        }

        /// <summary>
        /// Updates the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public bool UpdateParent(LightPatientDto member, Patient entity)
        {
            var isUpdated = false;
            if (entity.Father != null && entity.Father.Id == member.Id)
            {
                entity.Father = null;
                isUpdated = true;
            }
            if (entity.Mother != null && entity.Mother.Id == member.Id)
            {
                entity.Mother = null;
                isUpdated = true;
            }

            if (isUpdated) this.Session.Update(entity);
            return isUpdated;
        }

        #endregion Methods
    }
}