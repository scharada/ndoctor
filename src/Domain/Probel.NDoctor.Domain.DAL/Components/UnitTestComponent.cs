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
    using System.Linq;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Subcomponents;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;

    public class UnitTestComponent : BaseComponent
    {
        #region Constructors

        public UnitTestComponent(ISession session)
            : base(session)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Clears the table with entities of <typeparamref name="T"/>.
        /// This only works if this table is not referenced!
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        public void ClearTable<T>()
        {
            using (var tx = this.Session.BeginTransaction())
            {
                var query = string.Format("DELETE FROM {0}", typeof(T).Name);
                this.Session
                    .CreateQuery(query)
                    .ExecuteUpdate();
                tx.Commit();
            }
        }

        public long Create(LightPatientDto patient)
        {
            return new Creator(this.Session).Create(patient);
        }

        public IList<DoctorDto> GetAllDoctors()
        {
            var result = (from d in this.Session.Query<Doctor>()
                          select d).ToList();
            return Mapper.Map<IList<Doctor>, IList<DoctorDto>>(result);
        }

        public IList<PatientDto> GetAllPatients()
        {
            return new Selector(this.Session).GetAllPatients();
        }

        public IList<LightPatientDto> GetAllPatientsLight()
        {
            return new Selector(this.Session).GetAllPatientsLight();
        }

        public IList<UserDto> GetAllUsers()
        {
            var result = (from u in this.Session.Query<User>()
                          select u).ToList();
            return Mapper.Map<IList<User>, IList<UserDto>>(result);
        }

        public IList<SecurityUserDto> GetAllUsersLight()
        {
            return new Selector(this.Session).GetAllUsersLight();
        }

        /// <summary>
        /// Gets the doctor by id.
        /// </summary>
        /// <param name="id">The id of the doctor.</param>
        /// <returns></returns>
        public DoctorDto GetDoctorById(long id)
        {
            var entity = (from doc in this.Session.Query<Doctor>()
                          where doc.Id == id
                          select doc).Single();
            return Mapper.Map<Doctor, DoctorDto>(entity);
        }

        public InsuranceDto GetInsurance(long id)
        {
            var found = (from i in this.Session.Query<Insurance>()
                         where i.Id == id
                         select i).FirstOrDefault();
            return Mapper.Map<Insurance, InsuranceDto>(found);
        }

        public LightPatientDto GetLightPatient(int id)
        {
            var result = (from p in this.Session.Query<Patient>()
                          where p.Id == id
                          select p).FirstOrDefault();
            return Mapper.Map<Patient, LightPatientDto>(result);
        }

        /// <summary>
        /// Gets the light user by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public UserDto GetUserById(long id)
        {
            var entity = this.Session.Get<User>(id);
            return Mapper.Map<User, UserDto>(entity);
        }

        public PathologyDto GetPathology(long id)
        {
            var found = (from i in this.Session.Query<Pathology>()
                         where i.Id == id
                         select i).FirstOrDefault();
            return Mapper.Map<Pathology, PathologyDto>(found);
        }

        public IList<PatientDto> GetPatientsByName(string criteria, SearchOn searchOn)
        {
            return new Selector(this.Session).GetPatientByName(criteria, searchOn);
        }

        public IList<LightPatientDto> GetPatientsByNameLight(string criteria, SearchOn searchOn)
        {
            return new Selector(this.Session).GetPatientByNameLight(criteria, searchOn);
        }

        public IList<RoleDto> GetRoleByName(string description)
        {
            return new Selector(this.Session).GetRoleByName(description);
        }

        public SecurityUserDto GetSecurityUserById(long id)
        {
            var user = (from u in this.Session.Query<User>()
                        where u.Id == id
                        select u).First();
            return Mapper.Map<User, SecurityUserDto>(user);
        }

        public IList<SecurityUserDto> GetUserByLastName(string criteria)
        {
            return new Selector(this.Session).GetUserByLastName(criteria);
        }

        #endregion Methods
    }
}