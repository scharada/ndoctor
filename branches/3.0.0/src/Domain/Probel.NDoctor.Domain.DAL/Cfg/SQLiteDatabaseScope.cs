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
    using System.Data.SQLite;

    using log4net;

    using NHibernate;
    using NHibernate.Cfg;

    public class SQLiteDatabaseScope : IDisposable
    {
        #region Fields

        public static readonly string InitialDataFilename = SQLiteDatabaseCreator.InitialDataFilename;

        private const string CONNECTION_STRING = "Data Source=:memory:;Version=3;New=True;";

        private static readonly ILog Log = LogManager.GetLogger(typeof(SQLiteDatabaseScope));

        private Configuration config;
        private SQLiteConnection connection;
        private bool disposedValue = false;
        private ISessionFactory sessionFactory;
        private object sync = new object();

        #endregion Fields

        #region Constructors

        public SQLiteDatabaseScope(Configuration Configuration, ISessionFactory SessionFactory)
        {
            Log.Info("Creating database scope");
            config = Configuration;
            sessionFactory = SessionFactory;
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ISession OpenSession()
        {
            return sessionFactory.OpenSession(GetConnection());
        }

        public ISession OpenSession(IInterceptor Interceptor)
        {
            return sessionFactory.OpenSession(GetConnection(), Interceptor);
        }

        public IStatelessSession OpenStatelessSession()
        {
            return sessionFactory.OpenStatelessSession(GetConnection());
        }

        protected void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    Log.Info("Disposing database scope.");
                    if (null != connection)
                    {
                        connection.Dispose();
                    }
                }
            }
            this.disposedValue = true;
        }

        private void BuildConnection()
        {
            Log.Info("Building SQLite database connection");
            connection = new SQLiteConnection(CONNECTION_STRING);
            connection.Open();
            BuildSchema();
            if (!string.IsNullOrEmpty(InitialDataFilename))
                new SQLiteDataLoader(connection).ImportData();
        }

        private void BuildSchema()
        {
            Log.Debug("Creating schema");
            NHibernate.Tool.hbm2ddl.SchemaExport se;
            se = new NHibernate.Tool.hbm2ddl.SchemaExport(config);
            se.Execute(false, true, false, connection, null);
        }

        private SQLiteConnection GetConnection()
        {
            if (null == connection)
                BuildConnection();
            return connection;
        }

        #endregion Methods

        #region Other

        //public SQLiteDatabaseScope(Configuration Configuration,
        //    ISessionFactory SessionFactory,
        //    string InitialDataFilename)
        //    : this(Configuration, SessionFactory)
        //{
        //    initialDataFilename = InitialDataFilename;
        //}

        #endregion Other
    }
}