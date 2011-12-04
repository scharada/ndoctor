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
    using System;
    using System.Linq;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

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
        public void Create(PatientFullDto[] patients)
        {
            this.CheckSession();
            var entities = Mapper.Map<PatientFullDto[], Patient[]>(patients);
            this.Save(entities, e =>
            {
                this.MapReputation(e as Patient);
                this.MapInsurance(e as Patient);
                this.MapMeetings(e as Patient);
            });
        }

        /// <summary>
        /// Gets the default user.
        /// </summary>
        /// <returns></returns>
        public LightUserDto GetDefaultUser()
        {
            this.CheckSession();

            var entity = (from u in this.Session.Query<User>()
                          where u.IsDefault
                          select u).FirstOrDefault();
            return Mapper.Map<User, LightUserDto>(entity);
        }

        private void MapInsurance(Patient entity)
        {
            if (entity.Insurance != null)
            {
                var insurance = this.Session.Get<Insurance>(entity.Insurance.Id);
                if (insurance != null) entity.Insurance = insurance;
            }
        }

        private void MapMeetings(Patient patient)
        {
            foreach (var appointment in patient.Appointments)
            {
                if (appointment != null)
                {
                    var user = this.Session.Get<User>(appointment.User.Id);
                    if (user != null) appointment.User = user;
                }
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

        private void Save(Entity[] entities, Action<object> action)
        {
            try
            {
                using (var tx = this.Session.BeginTransaction())
                {
                    for (int i = 0; i < entities.Length; i++)
                    {
                        if (action != null) action(entities[i]);

                        this.Session.SaveOrUpdate(entities[i]);
                    }
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new QueryException(ex);
            }
        }

        #endregion Methods
    }
}