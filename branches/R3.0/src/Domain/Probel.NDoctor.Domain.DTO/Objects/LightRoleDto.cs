﻿#region Header

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

namespace Probel.NDoctor.Domain.DTO.Objects
{
    using System;

    [Serializable]
    public class LightRoleDto : BaseDto
    {
        #region Fields

        string description;
        string name;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return this.description; }
            set
            {
                this.description = value;
                this.OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged("Name");
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, name: {1}", base.ToString(), this.name);
        }

        #endregion Methods
    }
}