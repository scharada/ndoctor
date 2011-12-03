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
    using System.Collections.Generic;
    using System.Data.SQLite;

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Plugins.DbConvert.Properties;

    public abstract class MultipleImporter<T> : BaseImporter
    {
        #region Constructors

        /// Initializes the <see cref="ReputationImporter"/> class.
        /// </summary>
        static MultipleImporter()
        {
            InternalCache = new Dictionary<Identifier, T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleImporter&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="component">The component.</param>
        public MultipleImporter(SQLiteConnection connection, IImportComponent component)
            : base(connection, component)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the cache.
        /// </summary>
        /// <value>
        /// The cache.
        /// </value>
        protected static Dictionary<Identifier, T> InternalCache
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public IEnumerable<T> Import(long? id)
        {
            var result = new List<T>();
            if (!id.HasValue) return result;

            var query = this.Sql(id.Value);

            using (var command = new SQLiteCommand(query, this.Connection))
            using (var reader = command.ExecuteReader())
            {
                int count = 0;
                while (reader.Read())
                {
                    var current = this.Map(reader);
                    result.Add(current);
                    count++;
                }
                this.OnLogged(Messages.Log_RecordCount, count, id.Value);
            }

            return result;
        }

        protected abstract T Map(SQLiteDataReader reader);

        protected abstract string Sql(long id);

        #endregion Methods
    }
}