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
namespace Probel.NDoctor.Plugins.DbConvert.Domain
{
    using System.Collections.Generic;
    using System.Data.SQLite;

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
            InternalCache = new Dictionary<Identifier, T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleImporter&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="component">The component.</param>
        public SingleImporter(SQLiteConnection connection, IImportComponent component, string segragator)
            : base(connection, component)
        {
            this.Default = default(T);
            this.Segragator = segragator;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the default value of the type T.
        /// </summary>
        public virtual T Default
        {
            get;
            set;
        }

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

        protected string Segragator
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public T Import(long? rid)
        {
            Identifier id;
            if (!rid.HasValue || string.IsNullOrWhiteSpace(this.Sql(-1))) return this.Default;
            else { id = new Identifier(rid.Value, this.Segragator); }

            if (InternalCache.ContainsKey(id)) { return InternalCache[id]; }
            else
            {

                using (var command = new SQLiteCommand(this.Sql(id.Value), this.Connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var result = Map(reader);
                        this.AddInCache(new Identifier(id.Value, this.Segragator), result);
                        return result;
                    }
                    else
                    {
                        this.OnLogged(Messages.Log_NoItem, id.Value);
                        this.AddInCache(new Identifier(id.Value, this.Segragator), this.Default);
                        return this.Default;
                    }
                }
            }
        }

        /// <summary>
        /// Adds the stecified item in the cache.
        /// </summary>
        /// <param name="id">The id of the stored item in the cache.</param>
        /// <param name="item">The item to cache.</param>
        protected void AddInCache(Identifier id, T item)
        {
            if (!InternalCache.ContainsKey(id))
            {
                InternalCache.Add(id, item);
            }
        }

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