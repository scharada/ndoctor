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

    using Probel.Mvvm;

    /// <summary>
    /// Doctor
    /// </summary>
    [Serializable]
    public class DoctorDto : BaseDto
    {
        #region Fields

        private AddressDto address;
        private string firstName;
        private Gender gender;
        private string lastName;
        private string proMail;
        private string proMobile;
        private string proPhone;
        private TagDto specialisation;

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
                this.address = value;
                this.OnPropertyChanged(() => this.Address);
            }
        }

        /// <summary>
        /// Gets or sets a string representing how the name of the user should
        /// be displayed.
        /// </summary>
        /// <value>
        /// The name of the displayed.
        /// </value>
        public string DisplayedName
        {
            get { return string.Format("{0} {1}", this.FirstName, this.LastName); }
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName
        {
            get { return this.firstName; }
            set
            {
                this.firstName = value;
                this.OnPropertyChanged(() => this.FirstName);
            }
        }

        /// <summary>
        /// Gets or sets the sex.
        /// </summary>
        /// <value>The sex.</value>
        public Gender Gender
        {
            get { return this.gender; }
            set
            {
                this.gender = value;
                this.OnPropertyChanged(() => this.Gender);
            }
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName
        {
            get { return this.lastName; }
            set
            {
                this.lastName = value;
                this.OnPropertyChanged(() => this.LastName);
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

        public TagDto Specialisation
        {
            get { return this.specialisation; }
            set
            {
                this.specialisation = value;
                this.OnPropertyChanged(() => this.Specialisation);
            }
        }

        #endregion Properties
    }
}