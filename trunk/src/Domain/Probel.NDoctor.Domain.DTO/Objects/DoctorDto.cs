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

    /// <summary>
    /// Doctor
    /// </summary>
    [Serializable]
    public class DoctorDto : LightDoctorDto
    {
        #region Fields

        private AddressDto address;
        private string proMail;
        private string proMobile;
        private string proPhone;

        #endregion Fields

        #region Constructors

        public DoctorDto()
        {
            this.Address = new AddressDto();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public AddressDto Address
        {
            get { return this.address; }
            set
            {
                this.address = value ?? new AddressDto(); ;
                this.OnPropertyChanged(() => this.Address);
            }
        }

        /// <summary>
        /// Gets or sets the mail pro.
        /// </summary>
        /// <value>
        /// The mail pro.
        /// </value>
        public string ProMail
        {
            get { return this.proMail; }
            set
            {
                this.proMail = value;
                this.OnPropertyChanged(() => this.ProMail);
            }
        }

        /// <summary>
        /// Gets or sets the mobile pro.
        /// </summary>
        /// <value>
        /// The mobile pro.
        /// </value>
        public string ProMobile
        {
            get { return this.proMobile; }
            set
            {
                this.proMobile = value;
                this.OnPropertyChanged(() => this.ProMobile);
            }
        }

        /// <summary>
        /// Gets or sets the phone pro.
        /// </summary>
        /// <value>
        /// The phone pro.
        /// </value>
        public string ProPhone
        {
            get { return this.proPhone; }
            set
            {
                this.proPhone = value;
                this.OnPropertyChanged(() => this.ProPhone);
            }
        }

        #endregion Properties
    }
}