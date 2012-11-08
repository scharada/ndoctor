﻿/*
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
    using FluentNHibernate.Conventions;
    using FluentNHibernate.Conventions.Helpers;

    using log4net;

    using NHibernate;
    using NHibernate.Tool.hbm2ddl;

    using Probel.Helpers.Assertion;
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
        private IPersistenceConfigurer persistenceConfigurer;
        private Action<NHConfiguration> setupConfiguration;

        #endregion Fields

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

        private static NHibernate.Cfg.Configuration Configuration
        {
            get;
            set;
        }


        #endregion Properties

        #region Methods

        public void ConfigureAutoMapper()
        {
            AutoMapperMapping.Configure();
        }

        public DalConfigurator ConfigureUsingFile(string path, bool create)
        {
            if (isConfigured) { throw new ConfigurationException(); }
            if (!File.Exists(path)) create = true;

            if (create)
            {
                this.setupConfiguration = (configuration) =>
                {
                    Configuration = configuration;

                    // delete the existing db on each run
                    if (File.Exists(path)) File.Delete(path);

                    // this NHibernate tool takes a configuration (with mapping info in)
                    // and exports a database schema from it
                    new SchemaExport(configuration)
                        .Create(false, true);

                    executeScript = true;
                };
            }
            else
            {
                this.setupConfiguration = (configuration) =>
                    {
                        Configuration = configuration;

                        new SchemaUpdate(configuration)
                            .Execute(false, true);
                    };
            }

            this.persistenceConfigurer
                = SQLiteConfiguration
                .Standard
                .UsingFile(path);

            this.Configure();
            return this;
        }

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
            this.setupConfiguration = (configuration) =>
            {
                // this NHibernate tool takes a configuration (with mapping info in)
                // and exports a database schema from it
                new SchemaExport(configuration)
                  .Create(false, true);

                Configuration = configuration;
            };
            this.persistenceConfigurer
                = SQLiteConfiguration
                    .Standard
                    .InMemory()
                    .ShowSql()
                    .Raw("hibernate.generate_statistics", "true");

            this.Configure();
            session = DalConfigurator.SessionFactory.OpenSession();
            this.executeScript = true;
            this.BuildSchema(session);
            return this;
        }

        internal void InjectDefaultData(ISession session)
        {
            if (this.executeScript)
            {
                Logger.Info("Inject default data [SQL]");
                if (session != null) { new Script().Execute(session); }
                else { new Script().Execute(); }
            }
            else { Logger.Info("Data injection script aborded. Data already in the database."); }
        }

        /// <summary>
        /// Reset the configuration flag to allow another configuration.
        /// This should be used only during unit testing.
        /// </summary>
        internal void ResetConfiguration()
        {
            isConfigured = false;
        }

        /// <summary>
        /// Builds the schema of the database. Should only be used for unit testing
        /// </summary>
        /// <param name="session">The session.</param>
        private void BuildSchema(ISession session)
        {
            var export = new SchemaExport(Configuration);
            export.Execute(true, true, false, session.Connection, null);
        }

        private void Configure()
        {
            sessionFactory = this.CreateSessionFactory();

            this.ConfigureAutoMapper();
            isConfigured = true;
        }

        private AutoPersistenceModel CreateModel()
        {
            return AutoMap.AssemblyOf<Entity>(new CustomAutomappingConfiguration())
                    .IgnoreBase<Entity>()
                    .Override<User>(map => map.IgnoreProperty(x => x.DisplayedName))
                    .Override<Appointment>(map => map.IgnoreProperty(x => x.DateRange))
                    .Override<IllnessPeriod>(map => map.IgnoreProperty(p => p.Duration))
                    .Override<Patient>(map =>
                    {
                        map.HasMany<Bmi>(x => x.BmiHistory).KeyColumn("Patient_Id");
                        map.HasMany<MedicalRecord>(x => x.MedicalRecords).KeyColumn("Patient_Id");
                        map.HasMany<IllnessPeriod>(x => x.IllnessHistory).KeyColumn("Patient_Id");
                        map.HasMany<Appointment>(x => x.Appointments).KeyColumn("Patient_Id");
                    })
                    .Override<Role>(map => map.HasManyToMany(x => x.Tasks).Cascade.All())
                    .Conventions.Add(DefaultCascade.SaveUpdate()
                                    , DynamicUpdate.AlwaysTrue()
                                    , DynamicInsert.AlwaysTrue());
        }

        private ISessionFactory CreateSessionFactory()
        {
            return FluentNHibernate.Cfg.Fluently.Configure()
                .Database(this.persistenceConfigurer)
                .Mappings(m =>
                {
                    m.AutoMappings
                     .Add(this.CreateModel());
                })
            .ExposeConfiguration(setupConfiguration)
            .BuildSessionFactory();
        }

        #endregion Methods
    }
}