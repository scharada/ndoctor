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
    /// Represent a DTO of a patient
    /// </summary>
    [Serializable]
    public class PatientDto : BaseDto
    {
        #region Fields

        private AddressDto address = new AddressDto();
        private DateTime birthdate = DateTime.Today;
        private int counter;
        private decimal fee;
        private string firstName;
        private Gender gender;
        private string header;
        private long height;
        private DateTime inscriptionDate = DateTime.Today;
        private LightInsuranceDto insurance = new LightInsuranceDto();
        private bool isComplete;
        private string lastName;
        private DateTime lastUpdate = DateTime.Today;
        private string placeOfBirth;
        private LightPracticeDto practice = new LightPracticeDto();
        private string privateMail;
        private string privateMobile;
        private string privatePhone;
        private ProfessionDto profession = new ProfessionDto();
        private string proMail;
        private string proMobile;
        private string proPhone;
        private string reason;
        private ReputationDto reputation = new ReputationDto();
        private TagDto tag = new TagDto();
        private byte[] thumbnail;

        #endregion Fields

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
                this.OnPropertyChanged("Address");
            }
        }

        /// <summary>
        /// Gets or sets the birthdate.
        /// </summary>
        /// <value>
        /// The birthdate.
        /// </value>
        public DateTime Birthdate
        {
            get { return this.birthdate; }
            set
            {
                this.birthdate = value;
                this.OnPropertyChanged("Birthdate");
            }
        }

        /// <summary>
        /// Gets or sets the counter. This counter increments each time this patient is loaded
        /// </summary>
        /// <value>
        /// The counter.
        /// </value>
        public int Counter
        {
            get { return this.counter; }
            set
            {
                this.counter = value;
                this.OnPropertyChanged("Counter");
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
        /// Gets or sets the fee.
        /// </summary>
        /// <value>
        /// The fee.
        /// </value>
        public decimal Fee
        {
            get { return this.fee; }
            set
            {
                this.fee = value;
                this.OnPropertyChanged("Fee");
            }
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
                this.OnPropertyChanged("FirstName");
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
                this.OnPropertyChanged("Gender");
            }
        }

        /// <summary>
        /// Gets or sets the header that will be displayed in the prescriptions or
        /// other places where a header is needed.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public string Header
        {
            get { return this.header; }
            set
            {
                this.header = value;
                this.OnPropertyChanged("Header");
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public long Height
        {
            get { return this.height; }
            set
            {
                this.height = value;
                this.OnPropertyChanged("Height");
            }
        }

        /// <summary>
        /// Gets or sets the inscription date.
        /// </summary>
        /// <value>
        /// The inscription date.
        /// </value>
        public DateTime InscriptionDate
        {
            get
            {
                return this.inscriptionDate;
            }
            set
            {
                this.inscriptionDate = value;
                this.OnPropertyChanged("InscriptionDate");
            }
        }

        /// <summary>
        /// Gets or sets the insurance.
        /// </summary>
        /// <value>
        /// The insurance.
        /// </value>
        public LightInsuranceDto Insurance
        {
            get { return this.insurance; }
            set
            {
                this.insurance = value;
                this.OnPropertyChanged("Insurance");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is complete. That's if the person was quickly
        /// inserted into the repository, he/she does not contains all the pieces of information and therefore
        /// need to be completed later.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is complete; otherwise, <c>false</c>.
        /// </value>
        public bool IsComplete
        {
            get { return this.isComplete; }
            set
            {
                this.isComplete = value;
                this.OnPropertyChanged("IsComplete");
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
                this.OnPropertyChanged("LastName");
            }
        }

        /// <summary>
        /// Gets or sets the last update.
        /// </summary>
        /// <value>The last update.</value>
        public DateTime LastUpdate
        {
            get { return this.lastUpdate; }
            set
            {
                this.lastUpdate = value;
                this.OnPropertyChanged("LastUpdate");
            }
        }

        /// <summary>
        /// Gets or sets the place of birth.
        /// </summary>
        /// <value>
        /// The place of birth.
        /// </value>
        public string PlaceOfBirth
        {
            get { return this.placeOfBirth; }
            set
            {
                this.placeOfBirth = value;
                this.OnPropertyChanged("PlaceOfBirth");
            }
        }

        /// <summary>
        /// Gets or sets the practice.
        /// </summary>
        /// <value>
        /// The practice.
        /// </value>
        public LightPracticeDto Practice
        {
            get { return this.practice; }
            set
            {
                this.practice = value;
                this.OnPropertyChanged("Practice");
            }
        }

        /// <summary>
        /// Gets or sets the private mail.
        /// </summary>
        /// <value>
        /// The private mail.
        /// </value>
        public string PrivateMail
        {
            get { return this.privateMail; }
            set
            {
                this.privateMail = value;
                this.OnPropertyChanged("PrivateMail");
            }
        }

        /// <summary>
        /// Gets or sets the private mobile.
        /// </summary>
        /// <value>
        /// The private mobile.
        /// </value>
        public string PrivateMobile
        {
            get { return this.privateMobile; }
            set
            {
                this.privateMobile = value;
                this.OnPropertyChanged("PrivateMobile");
            }
        }

        /// <summary>
        /// Gets or sets the private phone.
        /// </summary>
        /// <value>
        /// The private phone.
        /// </value>
        public string PrivatePhone
        {
            get { return this.privatePhone; }
            set
            {
                this.privatePhone = value;
                this.OnPropertyChanged("PrivatePhone");
            }
        }

        /// <summary>
        /// Gets or sets the profession.
        /// </summary>
        /// <value>
        /// The profession.
        /// </value>
        public ProfessionDto Profession
        {
            get { return this.profession; }
            set
            {
                this.profession = value;
                this.OnPropertyChanged("Profession");
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
                this.OnPropertyChanged("ProMail");
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
                this.OnPropertyChanged("ProMobile");
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
                this.OnPropertyChanged("ProPhone");
            }
        }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        public string Reason
        {
            get { return this.reason; }
            set
            {
                this.reason = value;
                this.OnPropertyChanged("Reason");
            }
        }

        /// <summary>
        /// Gets or sets the reputation.
        /// </summary>
        /// <value>
        /// The reputation.
        /// </value>
        public ReputationDto Reputation
        {
            get { return this.reputation; }
            set
            {
                this.reputation = value;
                this.OnPropertyChanged("Reputation");
            }
        }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public TagDto Tag
        {
            get { return this.tag; }
            set
            {
                this.tag = value;
                this.OnPropertyChanged("Tag");
            }
        }

        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>The thumbnail.</value>
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
    }
}