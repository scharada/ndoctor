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
namespace Probel.NDoctor.Domain.DAL.Configuration
{
    using FluentNHibernate.Automapping;

    using Probel.NDoctor.Domain.BLL;

    public abstract class DbManager
    {
        #region Methods

        protected AutoPersistenceModel CreateModel()
        {
            return AutoMap.AssemblyOf<User>(new StoreConfiguration())
                    .Override<Person>(m =>
                    {
                        m.IgnoreProperty(x => x.ThumbnailImage);
                    });
        }

        #endregion Methods
    }
}