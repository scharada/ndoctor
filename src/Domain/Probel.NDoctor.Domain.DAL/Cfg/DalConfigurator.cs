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
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentNHibernate.Conventions.Helpers;

    using log4net;

    using NHibernate;
    using NHibernate.Tool.hbm2ddl;

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Mappings;
    using Probel.NDoctor.Domain.DTO.Exceptions;

    using NHConfiguration = NHibernate.Cfg.Configuration;

    public class DalConfigurator
    {
        #region Fields

        public static bool isConfigured;

        private static readonly ILog Logger = LogManager.GetLogger(typeof(DalConfigurator));

        private static ISessionFactory sessionFactory;

        private bool executeScript = false;

        #endregion Fields

        #region Constructors

        public DalConfigurator()
        {
            AutoMapperMapping.Configure();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the nHibernate's session factory.
        /// </summary>
        public static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null) throw new NullSessionFactoryException();
                else return sessionFactory;
            }
        }

        private static NHConfiguration Configuration
        {
            get;
            set;
        }

        private AutoPersistenceModel AutoPersistenceModel
        {
            get
            {
                return AutoMap.AssemblyOf<Entity>(new CustomAutomappingConfiguration())
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
        }

        private Action<NHConfiguration> ConfigurationSetup
        {
            get;
            set;
        }

        private IPersistenceConfigurer MySQLiteConfiguration
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Configures the database using the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file (path to the SQLite db).</param>
        /// <param name="createFreshDb">if set to <c>true</c> creates a fresh db. Otherwise uses the already existing database</param>
        /// <returns>A way to have a small fluent interface</returns>
        public DalConfigurator ConfigureUsingFile(string fileName, bool createFreshDb)
        {
            if (isConfigured) { throw new ConfigurationException(); }
            if (!File.Exists(fileName)) { createFreshDb = true; }

            if (createFreshDb)
            {
                this.ConfigurationSetup = config =>
                {
                    executeScript = true; //Because this is a fresh db creation, a script should inject default data in it

                    Configuration = config;

                    // delete the existing db on each run
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                        Logger.WarnFormat("Deleting existing database '{0}'", fileName);
                    }
                    else { Logger.DebugFormat("The database '{0}' doesn't exist, creating a new file", fileName); }

                    // this NHibernate tool takes a configuration (with mapping info in)
                    // and exports a database schema from it
                    new SchemaExport(config)
                        .Create(false, true);
                };
            }
            else
            {
                this.ConfigurationSetup = config =>
                {
                    Configuration = config;
                    new SchemaUpdate(config)
                        .Execute(false, true);
                };
            }

            this.MySQLiteConfiguration
                = SQLiteConfiguration
                .Standard
                .UsingFile(fileName);

            this.BuildSessionFactory();
            return this;
        }

        /// <summary>
        /// Injects the default data into the database.
        /// </summary>
        public void InjectDefaultData()
        {
            using (var session = SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                this.InjectDefaultData(session);
                tx.Commit();
            }
        }

        /// <summary>
        /// Manually configure the SessionFactory. This method is meant to receive a ISessionFactory mock        
        /// </summary>
        internal void ConfigureForUnitTest(ISessionFactory factory)
        {
            sessionFactory = factory;
        }

        /// <summary>
        /// Configures the DAL.
        /// </summary>
        internal DalConfigurator ConfigureInMemory(out ISession session)
        {
            if (isConfigured) { throw new ConfigurationException(); }
            this.ConfigurationSetup = config =>
            {
                // this NHibernate tool takes a configuration (with mapping info in)
                // and exports a database schema from it
                new SchemaExport(config)
                  .Create(false, true);

                Configuration = config;
            };
            this.MySQLiteConfiguration
                = SQLiteConfiguration
                    .Standard
                    .InMemory();

            this.BuildSessionFactory();
            session = DalConfigurator.SessionFactory.OpenSession();
            this.executeScript = true;

            new SchemaExport(Configuration)
               .Execute(true, true, false, session.Connection, null);

            return this;
        }

        /// <summary>
        /// Injects the default data into the database.
        /// This method should be used only for unit tests
        /// </summary>
        /// <param name="session">The session.</param>
        internal void InjectDefaultData(ISession session)
        {
            if (this.executeScript)
            {
                Logger.Info("Inject default data [SQL]");
                if (session != null) { new Script().Execute(session); }
                else { new Script().Execute(); }
            }
            else { Logger.Info("Data injection script aborded. Data already in the database."); }

            if (session != null) { new Script().InjectForgottenData(session); }
            else { new Script().InjectForgottenData(); }
        }

        /// <summary>
        /// Reset the configuration flag to allow another configuration.
        /// This should be used only during unit testing.
        /// </summary>
        internal void ResetConfiguration()
        {
            isConfigured = false;
        }

        private void BuildSessionFactory()
        {
            sessionFactory = Fluently
                .Configure()
                .Database(this.MySQLiteConfiguration)
                .Mappings(m =>
                {
                    m.AutoMappings
                        .Add(this.AutoPersistenceModel);
                })
                .ExposeConfiguration(ConfigurationSetup)
                .BuildSessionFactory();

            isConfigured = true;
        }

        #endregion Methods
    }
}