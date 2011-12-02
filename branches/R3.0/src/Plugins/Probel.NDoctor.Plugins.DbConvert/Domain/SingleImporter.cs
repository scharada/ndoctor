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

    using Probel.Helpers.Events;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Plugins.DbConvert.Properties;

    public abstract class SingleImporter<T> : BaseImporter
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="ReputationImporter"/> class.
        /// </summary>
        static SingleImporter()
        {
            Cache = new Dictionary<long, T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleImporter&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="component">The component.</param>
        public SingleImporter(SQLiteConnection connection, IImportComponent component)
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
        protected static Dictionary<long, T> Cache
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public T Import(long? id)
        {
            try
            {
                if (!id.HasValue) return default(T);
                else if (Cache.ContainsKey(id.Value)) { return Cache[id.Value]; }
                else
                {

                    using (var command = new SQLiteCommand(this.Sql(id.Value), this.Connection))
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var result = Map(reader);
                            this.AddInCache(id.Value, result);
                            return result;
                        }
                        else
                        {
                            this.OnLogged(Messages.Log_NoItem, id.Value);
                            return default(T);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Messages.Error_ImpossibleToMap, typeof(T).Name)
                    , ex);
            }
        }

        /// <summary>
        /// Adds the stecified item in the cache.
        /// </summary>
        /// <param name="id">The id of the stored item in the cache.</param>
        /// <param name="item">The item to cache.</param>
        protected void AddInCache(long id, T item)
        {
            if (!Cache.ContainsKey(id))
            {
                this.Create(item);
                Cache.Add(id, item);
            }
        }

        protected abstract void Create(T item);

        /// <summary>
        /// Maps the specified reader into the type <c>T</c>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>An intance of T with the data contained into the reader</returns>
        protected abstract T Map(SQLiteDataReader reader);

        /// <summary>
        /// Represent the SQL query to select the <c>T</c> item into the database.
        /// </summary>
        /// <param name="id">The id of the <c>T</c>.</param>
        /// <returns>A SQL query</returns>
        protected abstract string Sql(long id);

        #endregion Methods
    }
}