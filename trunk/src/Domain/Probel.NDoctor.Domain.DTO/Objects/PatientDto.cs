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
    public class PatientDto : LightPatientDto
    {
        #region Fields

        private AddressDto address = new AddressDto();
        private DateTime birthdate = DateTime.Today;
        private int counter;
        private decimal fee;

        private DateTime inscriptionDate = DateTime.Today;
        private LightInsuranceDto insurance = new LightInsuranceDto();
        private bool isComplete;
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
        private TagDto tag = new TagDto(TagCategory.Patient);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientDto"/> class.
        /// </summary>
        public PatientDto()
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
                this.OnPropertyChanged(() => Address);
            }
        }

        /// <summary>
        /// Calculate the age of the patient from his/her birthdate.
        /// </summary>
        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - birthdate.Year;
                if (birthdate > today.AddYears(-age)) age--;

                return age;
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
                this.OnPropertyChanged(() => Counter);
            }
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
                this.OnPropertyChanged(() => Fee);
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
                this.OnPropertyChanged(() => InscriptionDate);
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
                this.OnPropertyChanged(() => Insurance);
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
                this.OnPropertyChanged(() => IsComplete);
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
                this.OnPropertyChanged(() => LastUpdate);
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
                this.OnPropertyChanged(() => PlaceOfBirth);
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
                this.OnPropertyChanged(() => Practice);
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
                this.OnPropertyChanged(() => PrivateMail);
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
                this.OnPropertyChanged(() => PrivateMobile);
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
                this.OnPropertyChanged(() => PrivatePhone);
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
                this.OnPropertyChanged(() => ProMail);
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
                this.OnPropertyChanged(() => ProMobile);
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
                this.OnPropertyChanged(() => ProPhone);
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
                this.OnPropertyChanged(() => Reason);
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
                this.OnPropertyChanged(() => Reputation);
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
                this.OnPropertyChanged(() => Tag);
            }
        }

        #endregion Properties
    }
}