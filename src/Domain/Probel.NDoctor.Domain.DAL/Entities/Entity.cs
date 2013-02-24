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
namespace Probel.NDoctor.Domain.DAL.Entities
{
    /// <summary>
    /// Base entity used in the business layer
    /// </summary>
    public class Entity
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity()
        {
            this.IsImported = false;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the id.
        /// </summary>
        public virtual long Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this entity is imported from somewhere else.
        /// By imported, understand the entity wasn't added manually in the database
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is imported; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsImported
        {
            get;
            set;
        }

        #endregion Properties
    }
}