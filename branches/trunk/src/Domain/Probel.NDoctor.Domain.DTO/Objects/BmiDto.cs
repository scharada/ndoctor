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
    /// Represents an item of the Bmi history of a Patient
    /// </summary>
    public class BmiDto : BaseDto, ICloneable
    {
        #region Fields

        private DateTime date;
        private int height;
        private float weight;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BmiDto"/> class.
        /// </summary>
        public BmiDto()
        {
            this.date = DateTime.Today;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the date of the item.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date
        {
            get { return this.date; }
            set
            {
                this.date = value;
                this.OnPropertyChanged("Date");
            }
        }

        /// <summary>
        /// Gets or sets the height of the patient in centimeters.
        /// </summary>
        /// <value>
        /// The height of the patient.
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
        /// Gets the Bmi index regarding the Height and the Weight.
        /// The formula is: Height / Weight²
        /// </summary>
        public float Index
        {
            get
            {
                var centimeters = (float)this.Height / 100;
                return this.Weight / (float)Math.Pow(centimeters, 2);
            }
        }

        /// <summary>
        /// Gets or sets the weight of the patient in kilograms.
        /// </summary>
        /// <value>
        /// The weight of the patient.
        /// </value>
        public float Weight
        {
            get { return this.weight; }
            set
            {
                this.weight = value;
                this.OnPropertyChanged("Weight");
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return new BmiDto()
            {
                Date = this.Date,
                Height = this.Height,
                Id = this.Id,
                Weight = this.Weight
            };
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("PatientBmiDto [H:{0} - W:{1} - BMI:{2}]", this.Height, this.Weight, this.Index);
        }

        #endregion Methods
    }
}