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

namespace Probel.NDoctor.Domain.DTO.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public class DoctorFullDto : BaseDto
    {
        #region Fields

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        private AddressDto address;

        /// <summary>
        /// Gets or sets the value counting how many times this patient was used.
        /// </summary>
        /// <value>The counter.</value>
        private long counter;

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        private string firstName;

        /// <summary>
        /// Gets or sets the sex.
        /// </summary>
        /// <value>The sex.</value>
        private Gender gender;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is complete. That's if the person was quickly
        /// inserted into the repository, he/she does not contains all the pieces of information and therefore
        /// need to be completed later.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is complete; otherwise, <c>false</c>.
        /// </value>
        private bool isComplete;

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        private string lastName;

        /// <summary>
        /// Gets or sets the last update.
        /// </summary>
        /// <value>The last update.</value>
        private DateTime lastUpdate;

        /// <summary>
        /// Gets or sets the mail.
        /// </summary>
        /// <value>The mail.</value>
        private string mailPro;

        /// <summary>
        /// Gets or sets the mobile pro.
        /// </summary>
        /// <value>The mobile pro.</value>
        private string mobilePro;

        /// <summary>
        /// Gets or sets the phone pro.
        /// </summary>
        /// <value>The phone pro.</value>
        private string phonePro;

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        private TagDto specialisation;

        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>The thumbnail.</value>
        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>The thumbnail.</value>
        private byte[] thumbnail;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorFullDto"/> class.
        /// </summary>
        public DoctorFullDto()
        {
            this.Address = new AddressDto();
        }

        #endregion Constructors

        #region Properties

        public AddressDto Address
        {
            get { return this.address; }
            set
            {
                this.address = value;
                this.OnPropertyChanged("Address");
            }
        }

        public long Counter
        {
            get { return this.counter; }
            set
            {
                this.counter = value;
                this.OnPropertyChanged("Counter");
            }
        }

        public string FirstName
        {
            get { return this.firstName; }
            set
            {
                this.firstName = value;
                this.OnPropertyChanged("FirstName");
            }
        }

        public Gender Gender
        {
            get { return this.gender; }
            set
            {
                this.gender = value;
                this.OnPropertyChanged("Gender");
            }
        }

        public bool IsComplete
        {
            get { return this.isComplete; }
            set
            {
                this.isComplete = value;
                this.OnPropertyChanged("IsComplete");
            }
        }

        public string LastName
        {
            get { return this.lastName; }
            set
            {
                this.lastName = value;
                this.OnPropertyChanged("LastName");
            }
        }

        public DateTime LastUpdate
        {
            get { return this.lastUpdate; }
            set
            {
                this.lastUpdate = value;
                this.OnPropertyChanged("LastUpdate");
            }
        }

        public string MailPro
        {
            get { return this.mailPro; }
            set
            {
                this.mailPro = value;
                this.OnPropertyChanged("MailPro");
            }
        }

        public string MobilePro
        {
            get { return this.mobilePro; }
            set
            {
                this.mobilePro = value;
                this.OnPropertyChanged("MobilePro");
            }
        }

        public string PhonePro
        {
            get { return this.phonePro; }
            set
            {
                this.phonePro = value;
                this.OnPropertyChanged("PhonePro");
            }
        }

        public TagDto Specialisation
        {
            get { return this.specialisation; }
            set
            {
                this.specialisation = value;
                this.OnPropertyChanged("Specialisation");
            }
        }

        public byte[] Thumbnail
        {
            get { return this.thumbnail; }
            set
            {
                this.thumbnail = value;
                this.OnPropertyChanged("Thumbnail");
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
            return string.Format("Person: {0} {1}", this.FirstName, this.LastName);
        }

        #endregion Methods
    }
}