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

namespace Probel.NDoctor.Domain.DAL.Components
{
    using System.Collections.Generic;

    using AutoMapper;

    using NHibernate;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

    public class ImportComponent : BaseComponent, IImportComponent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportComponent"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public ImportComponent(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportComponent"/> class.
        /// </summary>
        public ImportComponent()
            : base()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates the specified patients.
        /// </summary>
        /// <param name="patients"></param>
        public void Create(IEnumerable<PatientFullDto> patients)
        {
            this.CheckSession();
            var entities = Mapper.Map<IEnumerable<PatientFullDto>, IEnumerable<Patient>>(patients);

            using (var tx = this.Session.BeginTransaction())
            {
                foreach (var entity in entities)
                {
                    this.MapReputation(entity);

                    this.Session.SaveOrUpdate(entity);
                }
                tx.Commit();
            }
        }

        /// <summary>
        /// Creates the specified reputation and update its id after creation.
        /// </summary>
        /// <param name="reputation">The reputation.</param>
        public void Create(ReputationDto reputation)
        {
            Assert.IsNotNull(reputation);
            this.CheckSession();

            var entity = Mapper.Map<ReputationDto, Reputation>(reputation);
            var id = this.Session.Save(entity);

            reputation.Id = (long)id;
        }

        /// <summary>
        /// Creates the specified reputation and update its id after creation.
        /// </summary>
        /// <param name="practice">The reputation.</param>
        public void Create(PracticeDto practice)
        {
            Assert.IsNotNull(practice);
            this.CheckSession();

            var entity = Mapper.Map<PracticeDto, Practice>(practice);
            var id = this.Session.Save(entity);

            practice.Id = (long)id;
        }

        /// <summary>
        /// Creates the specified insurance and update its id after creation.
        /// </summary>
        /// <param name="insurance"></param>
        public void Create(InsuranceDto insurance)
        {
            Assert.IsNotNull(insurance);
            this.CheckSession();

            var entity = Mapper.Map<InsuranceDto, Insurance>(insurance);
            var id = this.Session.Save(entity);

            insurance.Id = (long)id;
        }

        /// <summary>
        /// Creates the specified tag and update its id after creation.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public void Create(TagDto tag)
        {
            Assert.IsNotNull(tag);
            this.CheckSession();

            var entity = Mapper.Map<TagDto, Tag>(tag);
            var id = this.Session.Save(entity);

            tag.Id = (long)id;
        }

        /// <summary>
        /// Creates the doctors
        /// </summary>
        /// <param name="doctors">The doctor.</param>
        public void Create(IEnumerable<DoctorFullDto> doctors)
        {
            Assert.IsNotNull(doctors);
            this.CheckSession();

            var entities = Mapper.Map<IEnumerable<DoctorFullDto>, IEnumerable<Doctor>>(doctors);

            using (var tx = this.Session.BeginTransaction())
            {
                foreach (var entity in entities)
                {
                    this.MapSpecialisation(entity);
                    this.Session.SaveOrUpdate(entity);
                }
                tx.Commit();
            }
        }

        /// <summary>
        /// Creates the records
        /// </summary>
        /// <param name="records"></param>
        public void Create(IEnumerable<MedicalRecordDto> records)
        {
            Assert.IsNotNull(records);
            this.CheckSession();

            var entities = Mapper.Map<IEnumerable<MedicalRecordDto>, IEnumerable<MedicalRecord>>(records);

            using (var tx = this.Session.BeginTransaction())
            {
                foreach (var entity in entities)
                {
                    this.MapType(entity);
                    this.Session.SaveOrUpdate(entity);
                }
                tx.Commit();
            }
        }

        private void MapReputation(Patient entity)
        {
            if (entity.Reputation != null)
            {
                var reputation = this.Session.Get<Reputation>(entity.Reputation.Id);
                if (reputation != null) entity.Reputation = reputation;
            }
        }

        private void MapSpecialisation(Doctor entity)
        {
            if (entity.Specialisation != null)
            {
                var reputation = this.Session.Get<Tag>(entity.Specialisation.Id);
                if (reputation != null) entity.Specialisation = reputation;
            }
        }

        private void MapType(MedicalRecord entity)
        {
            if (entity.Tag != null)
            {
                var tag = this.Session.Get<Tag>(entity.Tag.Id);
                if (tag != null) entity.Tag = tag;
            }
        }

        #endregion Methods
    }
}