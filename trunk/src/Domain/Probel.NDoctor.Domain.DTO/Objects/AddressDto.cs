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
namespace Probel.NDoctor.Domain.DTO.Objects
{
    using System;

    [Serializable]
    public class AddressDto : BaseDto
    {
        #region Fields

        string boxNumber;
        string city;
        string postalCode;
        string street;
        string streetNumber;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the box number.
        /// </summary>
        /// <value>The box number.</value>
        public string BoxNumber
        {
            get { return this.boxNumber; }
            set
            {
                this.boxNumber = value;
                this.OnPropertyChanged(()=>this.BoxNumber);
            }
        }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City
        {
            get { return this.city; }
            set
            {
                this.city = value;
                this.OnPropertyChanged(()=>this.City);
            }
        }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>The postal code.</value>
        public string PostalCode
        {
            get { return this.postalCode; }
            set
            {
                this.postalCode = value;
                this.OnPropertyChanged(()=>this.PostalCode);
            }
        }

        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        /// <value>The street.</value>
        public string Street
        {
            get { return this.street; }
            set
            {
                this.street = value;
                this.OnPropertyChanged(()=>this.Street);
            }
        }

        /// <summary>
        /// Gets or sets the street number.
        /// </summary>
        /// <value>The street number.</value>
        public string StreetNumber
        {
            get { return this.streetNumber; }
            set
            {
                this.streetNumber = value;
                this.OnPropertyChanged(()=>this.StreetNumber);
            }
        }

        #endregion Properties
    }
}