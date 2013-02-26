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

namespace Probel.NDoctor.Domain.DAL.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DAL.AopConfiguration;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Properties;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Provides an API to get and set database settings.
    /// See this class as a mechanism to store configuration binded to the database
    /// and not to the application. Values such as Application key are stored into
    /// this table
    /// </summary>
    [Granted(To.Everyone)]
    [NotBenchmarked]
    public class DbSettingsComponent : BaseComponent, IDbSettingsComponent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbSettingsComponent"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public DbSettingsComponent(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbSettingsComponent"/> class.
        /// </summary>
        public DbSettingsComponent()
            : base()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets all the settings.
        /// </summary>
        public IEnumerable<DbSettingDto> Settings
        {
            get
            {
                var entities = (from s in this.Session.Query<DbSetting>()
                                select s);
                return Mapper.Map<IEnumerable<DbSetting>, IEnumerable<DbSettingDto>>(entities);
            }
        }

        #endregion Properties

        #region Indexers

        /// <summary>
        /// Gets or sets the <see cref="System.String"/> at the specified index.
        /// If you're setting a new value to an existing key, the new value will replace
        /// the old one
        /// </summary>
        public string this[string index]
        {
            get { return this.GetKey(index); }
            set { this.SetKey(index, value); }
        }

        #endregion Indexers

        #region Methods

        /// <summary>
        /// Check whether the key already exists in the database
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>True</c> if the key already exists in the database; otherwise <c>False</c></returns>
        public bool Exists(string key)
        {
            return (from s in this.Session.Query<DbSetting>()
                    where s.Key.ToLower() == key.ToLower()
                    select s).Count() > 0;
        }

        private string GetKey(string index)
        {
            try
            {
                return (from s in this.Session.Query<DbSetting>()
                        where s.Key.ToLower() == index.ToLower()
                        select s).Single().Value;
            }
            catch (InvalidOperationException ex)
            {
                throw new EntityNotFoundException(typeof(DbSetting), ex);
            }
        }

        private void SetKey(string key, string value)
        {
            if (!this.Exists(key))
            {
                this.Session.Save(new DbSetting()
                {
                    Key = key,
                    Value = value,
                });
            }
            else
            {
                var setting = (from s in this.Session.Query<DbSetting>()
                               where s.Key == key
                               select s).Single();
                setting.Value = value;
                this.Session.Update(setting);
            }
        }

        #endregion Methods
    }
}