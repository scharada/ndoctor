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

    using log4net;

    using Probel.Helpers.Events;
    using Probel.NDoctor.Domain.DTO.Components;

    public abstract class BaseImporter
    {
        #region Constructors

        public BaseImporter(SQLiteConnection connection, IImportComponent component)
        {
            this.Logger = LogManager.GetLogger(this.GetType());
            this.Connection = connection;
            this.Component = component;
        }

        #endregion Constructors

        #region Events

        public event EventHandler<EventArgs<string>> Logged;

        public event EventHandler<EventArgs<int>> ProgressChanged;

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

        protected ILog Logger
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public void OnProgressChanged(int percentage)
        {
            if (this.ProgressChanged != null)
                this.ProgressChanged(this, new EventArgs<int>(percentage));
        }

        protected void HandleError(Exception ex)
        {
            this.Logger.Error(ex);
            OnLogged(ex.Message);
        }

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

        #region Nested Types

        protected class Identifier
        {
            #region Fields

            private static readonly string defaultSegregator = Guid.NewGuid().ToString();

            #endregion Fields

            #region Constructors

            public Identifier(long id, string segregator)
            {
                this.Segregator = segregator;
                this.Value = id;
            }

            public Identifier(long id)
                : this(id, defaultSegregator)
            {
            }

            #endregion Constructors

            #region Properties

            public string Segregator
            {
                get;
                private set;
            }

            public long Value
            {
                get;
                private set;
            }

            #endregion Properties

            #region Methods

            public static bool operator !=(Identifier left, Identifier right)
            {
                return !(left == right);
            }

            public static bool operator ==(Identifier left, Identifier right)
            {
                return left.Equals(right);
            }

            public override bool Equals(object obj)
            {
                if (obj == null) { return false; }
                else if (obj is Identifier) { return this.GetHashCode() == obj.GetHashCode(); }
                else { return false; }
            }

            public override int GetHashCode()
            {
                return (string.Format("{0}-{1}", this.Value, this.Segregator)).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format("[{0}] {1}"
                    , this.Value
                    , this.Segregator);
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}