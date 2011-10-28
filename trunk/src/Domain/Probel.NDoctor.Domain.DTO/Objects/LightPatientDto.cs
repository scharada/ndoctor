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
    /// <summary>
    /// Represents a light weight data patient
    /// </summary>
    public class LightPatientDto : BaseDto
    {
        #region Fields

        private string firstName;
        private Gender gender;
        private int height;
        private string lastName;

        #endregion Fields

        #region Properties

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
        /// Gets or sets the gender of the patient.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
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
                this.OnPropertyChanged("Height");
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