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

namespace Probel.NDoctor.Domain.DAL.Entities
{
    using System;

    /// <summary>
    /// Represents a settings stored in the database
    /// </summary>
    public class DbSetting : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the key of the setting. It should be unique
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public virtual string Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value binded to the key.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public virtual string Value
        {
            get;
            set;
        }

        #endregion Properties
    }
}