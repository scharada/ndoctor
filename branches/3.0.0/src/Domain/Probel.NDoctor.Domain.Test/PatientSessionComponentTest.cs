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

namespace Probel.NDoctor.Domain.DAL.Test
{
    using System;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Cfg;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Test.Helpers;

    [TestFixture]
    public class PatientSessionComponentTest : IDisposable
    {
        #region Fields

        private PatientSessionComponent component;
        private InMemoryDatabase db = new InMemoryDatabase();

        #endregion Fields

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            db.Dispose();
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            new DalConfiguration().Configure();
            this.component = new PatientSessionComponent(this.db.Session);
            Build.Database(this.db.Session);
        }

        #endregion Methods
    }
}