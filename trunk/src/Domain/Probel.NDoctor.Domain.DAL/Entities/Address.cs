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
    public class Address : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the box number.
        /// </summary>
        /// <value>The box number.</value>
        public virtual string BoxNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public virtual string City
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>The postal code.</value>
        public virtual string PostalCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        /// <value>The street.</value>
        public virtual string Street
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the street number.
        /// </summary>
        /// <value>The street number.</value>
        public virtual string StreetNumber
        {
            get;
            set;
        }

        #endregion Properties
    }
}