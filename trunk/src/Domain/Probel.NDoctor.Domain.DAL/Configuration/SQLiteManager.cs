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
namespace Probel.NDoctor.Domain.DAL.Confirugration
{
    using System;
    using System.IO;

    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;

    using NHibernate;
    using NHibernate.Tool.hbm2ddl;

    using Probel.NDoctor.Domain.DAL.Configuration;
    using Probel.NDoctor.Domain.DAL.Properties;

    public class SQLiteManager : DbManager, IRepositoryManager
    {
        #region Fields

        private FluentConfiguration configuration;
        private string dbPath;

        #endregion Fields

        #region Methods

        public SQLiteManager ConfigureDatabaseAsFile(string path)
        {
            try
            {
                this.dbPath = path;
                if (File.Exists(this.dbPath)) File.Delete(this.dbPath);

                this.Configure(SQLiteConfiguration
                    .Standard
                    .UsingFile(this.dbPath));
                return this;
            }
            catch (Exception ex)
            {
                throw new DalConfigurationException(Messages.Msg_DbCreationImpossible, ex);
            }
        }

        public SQLiteManager ConfigureDatabaseAsInMemory()
        {
            this.Configure(SQLiteConfiguration
                .Standard
                .InMemory());
            return this;
        }

        public SQLiteManager CreateDb()
        {
            if (string.IsNullOrEmpty(this.dbPath)) throw new DalConfigurationException(Messages.Msg_PDbPathNotSet);

            this.configuration.ExposeConfiguration(BuildSchema);
            return this;
        }

        public ISessionFactory CreateSessionFactory()
        {
            if (this.configuration != null) return this.configuration.BuildSessionFactory();
            else throw new DalConfigurationException(Messages.Msg_SQLiteConfigurationNotSet);
        }

        private void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            // delete the existing db on each run
            if (File.Exists(this.dbPath)) File.Delete(this.dbPath);

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config)
              .Create(false, true);
        }

        private void Configure(SQLiteConfiguration config)
        {
            this.configuration = Fluently
                .Configure()
                .Database(config)
                .Mappings(m =>
                {
                    var mapping = m.AutoMappings
                         .Add(this.CreateModel());

                    //var mapping = m.FluentMappings.AddFromAssemblyOf<SQLiteManager>();
                    if (DalSettings.ExportHbm)
                    {
                        if (!Directory.Exists(DalSettings.ExportHbmPath))
                            throw new DalConfigurationException(Messages.Msg_InvalidHbmPath);
                        mapping.ExportTo(DalSettings.ExportHbmPath);
                    }
                });
        }

        #endregion Methods
    }
}