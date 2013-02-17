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
    /// Represents a light weight data patient
    /// </summary>
    [Serializable]
    public class LightPatientDto : PersonDto
    {
        #region Fields

        private AddressDto address;
        private DateTime birthdate;
        private int height;
        private DateTime inscriptionDate;
        private bool isDeactivated;
        private DateTime lastUpdate = DateTime.Today;
        private ProfessionDto profession;
        private string reason;

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
        /// Gets or sets the birth date of the patient.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        public DateTime Birthdate
        {
            get { return this.birthdate; }
            set
            {
                this.birthdate = value;
                this.OnPropertyChanged(() => Birthdate);
            }
        }

        /// <summary>
        /// Gets the name of the patient as a concatenation of the first name and
        /// the last name.
        /// </summary>
        /// <value>The full name of the patient</value>
        public string DisplayedName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        /// <summary>
        /// Gets or sets the height of the patient in centimeters.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height
        {
            get { return this.height; }
            set
            {
                this.height = value;
                this.OnPropertyChanged(() => Height);
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
        /// Gets or sets a value indicating whether this patient is deactivated.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is deactivated; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeactivated
        {
            get { return this.isDeactivated; }
            set
            {
                this.isDeactivated = value;
                this.OnPropertyChanged(() => IsDeactivated);
            }
        }

        /// <summary>
        /// Gets or sets the last update.
        /// </summary>
        /// <value>
        /// The last update.
        /// </value>
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
        /// Gets or sets the profession of the patient.
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
                this.OnPropertyChanged(() => Profession);
            }
        }

        /// <summary>
        /// Gets or sets the reason why the patient came the first time.
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
            return string.Format("LightPatientDto: {0}", this.DisplayedName);
        }

        #endregion Methods
    }
}