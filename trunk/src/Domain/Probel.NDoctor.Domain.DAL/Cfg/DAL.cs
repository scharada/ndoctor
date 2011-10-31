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
namespace Probel.NDoctor.Domain.DAL.Cfg
{
    using System;
    using System.IO;

    using FluentNHibernate.Automapping;
    using FluentNHibernate.Cfg.Db;
    using FluentNHibernate.Conventions.Helpers;

    using NHibernate;
    using NHibernate.Tool.hbm2ddl;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Exceptions;
    using Probel.NDoctor.Domain.DAL.Mappings;
    using Probel.NDoctor.Domain.DAL.Properties;
    using Probel.NDoctor.Domain.DTO.Components;

    using StructureMap;

    using NHConfiguration = NHibernate.Cfg.Configuration;
    using log4net;

    public class DAL
    {
        #region Fields

        private static ISessionFactory sessionFactory;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DAL));
        private IPersistenceConfigurer persistenceConfigurer;
        private Action<NHConfiguration> setupConfiguration;

        #endregion Fields

        #region Properties

        public bool IsConfigured
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the nHibernate's session factory.
        /// </summary>
        internal static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null) throw new NullSessionFactoryException();
                else return sessionFactory;
            }
        }

        private NHibernate.Cfg.Configuration Configuration
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Configures the DAL.
        /// </summary>
        public void ConfigureInMemory()
        {
            this.setupConfiguration = (configuration) => this.Configuration = configuration;
            this.persistenceConfigurer
                = SQLiteConfiguration
                    .Standard
                    .InMemory()
                    .ShowSql();

            this.Configure();
        }

        public void ConfigureUsingFile(string path, bool create)
        {
            if (!create && !File.Exists(path)) throw new FileNotFoundException(Messages.Msg_ErrorDbInvalidPath);

            if (create)
            {
                this.setupConfiguration = (configuration) =>
                {
                    this.Configuration = configuration;

                    // delete the existing db on each run
                    if (File.Exists(path)) File.Delete(path);

                    // this NHibernate tool takes a configuration (with mapping info in)
                    // and exports a database schema from it
                    new SchemaExport(configuration)
                      .Create(false, true);
                };
            }
            else this.setupConfiguration = (configuration) => this.Configuration = configuration;

            this.persistenceConfigurer
                = SQLiteConfiguration
                .Standard
                .UsingFile(path);

            this.Configure();
        }

        private void Configure()
        {
            sessionFactory = this.CreateSessionFactory();

            Mapping.Configure();
            this.IsConfigured = true;
        }

        static DAL()
        {
            Logger.Debug("Configuring StructureMap for the plugins...");
            ObjectFactory.Configure(x =>
            {
                x.For<IAdministrationComponent>().Add<AdministrationComponent>();
                x.SelectConstructor<AdministrationComponent>(() => new AdministrationComponent());

                x.For<IBmiComponent>().Add<BmiComponent>();
                x.SelectConstructor<BmiComponent>(() => new BmiComponent());

                x.For<ICalendarComponent>().Add<CalendarComponent>();
                x.SelectConstructor<CalendarComponent>(() => new CalendarComponent());

                x.For<IFamilyComponent>().Add<FamilyComponent>();
                x.SelectConstructor<FamilyComponent>(() => new FamilyComponent());

                x.For<IMedicalRecordComponent>().Add<MedicalRecordComponent>();
                x.SelectConstructor<MedicalRecordComponent>(() => new MedicalRecordComponent());

                x.For<IPathologyComponent>().Add<PathologyComponent>();
                x.SelectConstructor<PathologyComponent>(() => new PathologyComponent());

                x.For<IPatientDataComponent>().Add<PatientDataComponent>();
                x.SelectConstructor<PatientDataComponent>(() => new PatientDataComponent());

                x.For<IPatientSessionComponent>().Add<PatientSessionComponent>();
                x.SelectConstructor<PatientSessionComponent>(() => new PatientSessionComponent());

                x.For<IPictureComponent>().Add<PictureComponent>();
                x.SelectConstructor<PictureComponent>(() => new PictureComponent());

                x.For<IPrescriptionComponent>().Add<PrescriptionComponent>();
                x.SelectConstructor<PrescriptionComponent>(() => new PrescriptionComponent());

                x.For<IUserSessionComponent>().Add<UserSessionComponent>();
                x.SelectConstructor<UserSessionComponent>(() => new UserSessionComponent());

                x.For<IDebugComponent>().Add<DebugComponent>();
                x.SelectConstructor<DebugComponent>(() => new DebugComponent());
            });
        }

        private AutoPersistenceModel CreateModel()
        {
            return AutoMap.AssemblyOf<Entity>(new CustomAutomappingConfiguration())
                    .IgnoreBase<Entity>()
                    .Override<User>(m => m.IgnoreProperty(x => x.DisplayedName))

                    .Override<Appointment>(m => m.IgnoreProperty(x => x.DateRange))

                    .Override<Patient>(patient =>
                    {
                        patient.HasMany<Bmi>(x => x.BmiHistory).KeyColumn("Patient_Id");
                        patient.HasMany<MedicalRecord>(x => x.MedicalRecords).KeyColumn("Patient_Id");
                        patient.HasMany<IllnessPeriod>(x => x.IllnessHistory).KeyColumn("Patient_Id");
                        patient.HasMany<Appointment>(x => x.Appointments).KeyColumn("Patient_Id");
                    })

                    .Override<IllnessPeriod>(x => x.IgnoreProperty(p => p.Duration))
                    .Conventions.Add(DefaultCascade.SaveUpdate());
        }

        private ISessionFactory CreateSessionFactory()
        {
            return FluentNHibernate.Cfg.Fluently.Configure()
                .Database(this.persistenceConfigurer)
                .Mappings(m =>
                {
                    m.AutoMappings
                     .Add(this.CreateModel());

                    m.FluentMappings
                     .Conventions.Add(DefaultCascade.None());
                })
            .ExposeConfiguration(setupConfiguration)
            .BuildSessionFactory();
        }

        #endregion Methods
    }
}