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

namespace Probel.NDoctor.Domain.DTO.Components
{
    using System.Collections.Generic;

    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Provides an API to get and set database settings.
    /// See this class as a mechanism to store configuration binded to the database
    /// and not to the application. Values such as Application key are stored into
    /// this table
    /// </summary>
    public interface IDbSettingsComponent
    {
        #region Properties

        /// <summary>
        /// Gets all the settings.
        /// </summary>
        IEnumerable<DbSettingDto> Settings
        {
            get;
        }

        /// <summary>
        /// Gets or sets the <see cref="System.String"/> at the specified index.
        /// If you're setting a new value to an existing key, the new value will replace
        /// the old one
        /// </summary>
        string this[string index]
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Check whether the key already exists in the database
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>True</c> if the key already exists in the database; otherwise <c>False</c></returns>
        bool Exists(string key);

        #endregion Methods
    }
}