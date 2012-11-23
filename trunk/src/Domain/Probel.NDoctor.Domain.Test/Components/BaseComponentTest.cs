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

namespace Probel.NDoctor.Domain.Test.Components
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using NHibernate;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Cfg;
    using Probel.NDoctor.Domain.DAL.Components;


    public abstract class BaseComponentTest<T>
        where T : BaseComponent
    {
        #region Properties

        public UnitTestComponent HelperComponent
        {
            get; private set;
        }

        protected T ComponentUnderTest
        {
            get;
            private set;
        }

        protected string RandomString
        {
            get { return Guid.NewGuid().ToString(); }
        }

        protected ISession Session
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        [TearDown]
        public void Teardown()
        {
            if (this.Session.IsOpen)
            {
                this.Session.Close();
            }
        }

        protected void BuildComponent(Func<ISession, T> ctor)
        {
            ISession session;
            new NUnitConfigWrapper(new DalConfigurator())
                .ConfigureInMemory(out session)
                .InjectDefaultData(session);
            this.Session = session;

            this.ComponentUnderTest = ctor(this.Session);
            this.HelperComponent = new UnitTestComponent(this.Session);
            this.InjectTestData();
        }

        protected void WrapInTransaction(Action actionUnderTransaction)
        {
            using (var tx = this.Session.BeginTransaction())
            {
                actionUnderTransaction();
                tx.Commit();
            }
        }

        [SetUp]
        protected abstract void _Setup();

        private void InjectTestData()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Probel.NDoctor.Domain.Test.InsertUsers.sql");
            if (stream == null) throw new NullReferenceException("The embedded script to create the database can't be loaded or doesn't exist.");

            string sql;

            using (var reader = new StreamReader(stream, Encoding.UTF8)) { sql = reader.ReadToEnd(); }
            new SqlComponent(this.Session).ExecuteSql(sql);
        }

        #endregion Methods
    }
}