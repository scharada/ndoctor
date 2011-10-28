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
    /// Light version of a doctor
    /// </summary>
    public class LightDoctorDto : BaseDto
    {
        #region Fields

        private string firstName;
        private Gender gender;
        private string lastName;
        private TagDto specialisation;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the name of the displayed.
        /// </summary>
        /// <value>
        /// The name of the displayed.
        /// </value>
        public string DisplayedName
        {
            get
            {
                return string.Format("{0} {1}"
                , this.FirstName
                , this.LastName);
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
        /// Gets or sets the specialisation.
        /// </summary>
        /// <value>
        /// The specialisation.
        /// </value>
        public TagDto Specialisation
        {
            get { return this.specialisation; }
            set
            {
                this.specialisation = value;
                this.OnPropertyChanged("Specialisation");
            }
        }

        #endregion Properties
    }
}