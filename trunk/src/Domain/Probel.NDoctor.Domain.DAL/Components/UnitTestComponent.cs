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

        public IList<LightUserDto> GetAllUsersLight()
        {
            return new Selector(this.Session).GetAllUsersLight();
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

        public LightUserDto GetLightUserById(int id)
        {
            var user = (from u in this.Session.Query<User>()
                        where u.Id == id
                        select u).First();
            return Mapper.Map<User, LightUserDto>(user);
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

        public IList<LightUserDto> GetUserByLastName(string criteria)
        {
            return new Selector(this.Session).GetUserByLastName(criteria);
        }

        #endregion Methods
    }
}