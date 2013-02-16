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

namespace Probel.NDoctor.Domain.DAL.Cfg
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using FluentNHibernate.Automapping;
    using FluentNHibernate.Conventions.Helpers;

    using Probel.NDoctor.Domain.DAL.Entities;

    internal class MyAutoPersistenceModel
    {
        #region Fields

        private readonly AutoPersistenceModel AutoPersistenceModel;

        #endregion Fields

        #region Constructors

        public MyAutoPersistenceModel()
        {
            this.AutoPersistenceModel = AutoMap.AssemblyOf<Entity>(new CustomAutomappingConfiguration())
                    .IgnoreBase<Entity>()
                    .Override<User>(map => map.IgnoreProperty(x => x.DisplayedName))
                    .Override<Appointment>(map => map.IgnoreProperty(x => x.DateRange))
                    .Override<IllnessPeriod>(map => map.IgnoreProperty(p => p.Duration))
                    .Override<Role>(map => map.HasManyToMany(x => x.Tasks).Cascade.All())
                    .Override<DbSetting>(map => map.Map(p => p.Key).Unique())
                    .Override<Patient>(map =>
                    {
                        map.DynamicUpdate();
                        map.IgnoreProperty(x => x.Age);
                        map.Map(x => x.IsDeactivated).Default("0").Not.Nullable();
                        map.HasMany<Bmi>(x => x.BmiHistory).KeyColumn("Patient_Id");
                        map.HasMany<MedicalRecord>(x => x.MedicalRecords).KeyColumn("Patient_Id");
                        map.HasMany<IllnessPeriod>(x => x.IllnessHistory).KeyColumn("Patient_Id");
                        map.HasMany<Appointment>(x => x.Appointments).KeyColumn("Patient_Id");
                    })
                    .Override<Person>(map =>
                    {
                        map.Map(p => p.FirstName).Index("idx_person_FirstName");
                        map.Map(p => p.LastName).Index("idx_person_LastName");
                    })
                    .Override<ApplicationStatistics>(map =>
                    {
                        map.Map(e => e.IsExported).Default("0").Not.Nullable();
                        map.Map(e => e.Version).Default("\"3.0.3\"").Not.Nullable();
                    })
                    .Conventions.Add(DefaultCascade.SaveUpdate()
                                   , DynamicUpdate.AlwaysTrue()
                                   , DynamicInsert.AlwaysTrue()
                                   , LazyLoad.Always());
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="Probel.NDoctor.Domain.DAL.Cfg.MyAutoPersistenceModel"/> to <see cref="?"/>.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator AutoPersistenceModel(MyAutoPersistenceModel model)
        {
            return model.AutoPersistenceModel;
        }

        #endregion Methods
    }
}