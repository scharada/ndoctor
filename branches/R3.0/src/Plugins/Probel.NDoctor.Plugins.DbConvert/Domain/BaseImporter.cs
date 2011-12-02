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

namespace Probel.NDoctor.Plugins.DbConvert.Domain
{
    using System;
    using System.Data.SQLite;

    using Probel.Helpers.Events;
    using Probel.NDoctor.Domain.DTO.Components;

    public abstract class BaseImporter
    {
        #region Constructors

        public BaseImporter(SQLiteConnection connection, IImportComponent component)
        {
            this.Connection = connection;
            this.Component = component;
        }

        #endregion Constructors

        #region Events

        public event EventHandler<EventArgs<string>> Logged;

        #endregion Events

        #region Properties

        public SQLiteConnection Connection
        {
            get;
            private set;
        }

        protected IImportComponent Component
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        protected void OnLogged(string log)
        {
            if (this.Logged != null)
                this.Logged(this, new EventArgs<string>(log));
        }

        protected void OnLogged(string log, params object[] args)
        {
            this.OnLogged(string.Format(log, args));
        }

        protected BaseImporter Relay(BaseImporter item)
        {
            item.Logged += (sender, e) => this.OnLogged(e.Data);
            return item;
        }

        #endregion Methods
    }
}