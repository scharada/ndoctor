///*

#region Header

//    This file is part of NDoctor.
//    NDoctor is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//    NDoctor is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with NDoctor.  If not, see <http://www.gnu.org/licenses/>.
//*/
//namespace Probel.NDoctor.Domain.Test.Helpers
//{
//    using System;
//    using FluentNHibernate.Automapping;
//    using FluentNHibernate.Conventions.Helpers;
//    using NHibernate;
//    using NHibernate.Cfg;
//    using Probel.NDoctor.Domain.DAL.Cfg;
//    using Probel.NDoctor.Domain.DAL.Entities;
//    /// <summary>
//    /// Represents a memory only database that does not persist beyond the immediate
//    /// testing usage, using <see cref="System.Data.SQLite"/>.
//    /// </summary>
//    public class InMemoryDatabase : IDisposable
//    {
//        #region Constructors
//        public InMemoryDatabase()
//        {
//            SessionFactory = CreateSessionFactory();
//            Session = SessionFactory.OpenSession();
//            BuildSchema(Session);
//        }
//        #endregion Constructors
//        #region Properties
//        /// <summary>
//        /// The current session being used.
//        /// </summary>
//        public ISession Session
//        {
//            get; set;
//        }
//        /// <summary>
//        /// The configuration of the memorized database.
//        /// </summary>
//        private Configuration Configuration
//        {
//            get; set;
//        }
//        /// <summary>
//        /// The singleton session factory.
//        /// </summary>
//        private ISessionFactory SessionFactory
//        {
//            get; set;
//        }
//        #endregion Properties
//        #region Methods
//        /// <summary>
//        /// Dispose of the session and released resources.
//        /// </summary>
//        public void Dispose()
//        {
//            Session.Dispose();
//        }
//        protected AutoPersistenceModel CreateModel()
//        {
//            return AutoMap.AssemblyOf<User>(new CustomAutomappingConfiguration())
//                    .IgnoreBase<Probel.NDoctor.Domain.DAL.Entities.Entity>()
//                    .Override<User>(m => m.IgnoreProperty(x => x.DisplayedName))
//                    .OverrideAll(m => m.IgnoreProperties("IsEmpty", "Error"))
//                    .Conventions.Add(DefaultCascade.All());
//        }
//        /// <summary>
//        /// Builds the NHibernate Schema so that it can be mapped to the SessionFactory.
//        /// </summary>
//        /// <param name="Session">
//        /// The <see cref="NHibernate.ISession"/> to build a schema into.
//        /// </param>
//        private void BuildSchema(ISession Session)
//        {
//            var export = new NHibernate.Tool.hbm2ddl.SchemaExport(Configuration);
//            export.Execute(true, true, false, Session.Connection, null);
//        }
//        /// <summary>
//        /// Construct a memory based session factory.
//        /// </summary>
//        /// <returns>
//        /// The session factory in an SQLite Memory Database.
//        /// </returns>
//        private ISessionFactory CreateSessionFactory()
//        {
//            return FluentNHibernate.Cfg.Fluently.Configure()
//            .Database(FluentNHibernate.Cfg.Db.SQLiteConfiguration
//                .Standard
//                .InMemory()
//                .ShowSql())
//                .Mappings(m =>
//                {
//                    var mapping = m.AutoMappings
//                         .Add(this.CreateModel());
//                })
//            .ExposeConfiguration(configuration => Configuration = configuration)
//            .BuildSessionFactory();
//        }
//        #endregion Methods
//    }
//}

#endregion Header