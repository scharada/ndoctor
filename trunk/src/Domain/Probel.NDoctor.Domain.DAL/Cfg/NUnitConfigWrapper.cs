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
    using NHibernate;

    /// <summary>
    /// Wraps a <see cref="DalConfigurator"/> and opens its interface to 
    /// allow unit test with a in memory SQLite database
    /// </summary>
    public class NUnitConfigWrapper
    {
        #region Fields

        private readonly DalConfigurator Configurator;

        #endregion Fields

        #region Constructors

        public NUnitConfigWrapper(DalConfigurator configurator)
        {
            this.Configurator = configurator;
            configurator.ResetConfiguration();
        }

        #endregion Constructors

        #region Methods

        public void ConfigureForUnitTest(ISessionFactory factory)
        {
            this.Configurator.ConfigureForUnitTest(factory);
        }

        public NUnitConfigWrapper ConfigureInMemory(out ISession session)
        {
            return new NUnitConfigWrapper(Configurator.ConfigureInMemory(out session));
        }

        public void InjectDefaultData(ISession session)
        {
            this.Configurator.InjectDefaultData(session);
        }

        #endregion Methods
    }
}