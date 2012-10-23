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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NHibernate;

    using Probel.NDoctor.Domain.DAL.Cfg;
    using Probel.NDoctor.Domain.DAL.Components;

    public class BaseComponentTest<T>
        where T : BaseComponent
    {
        #region Properties

        protected T Component
        {
            get; private set;
        }

        #endregion Properties

        #region Methods

        protected void BuildComponent(Func<ISession, T> ctor)
        {
            new DalConfigurator().ConfigureInMemory();
            var component = ctor(DalConfigurator.SessionFactory.OpenSession());
            DalConfigurator.BuildSchema(component.Session);
            this.Component = component;
        }

        #endregion Methods
    }
}