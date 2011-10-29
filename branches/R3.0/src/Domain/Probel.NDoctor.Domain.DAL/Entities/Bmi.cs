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
namespace Probel.NDoctor.Domain.DAL.Entities
{
    using System;

    /// <summary>
    /// Represents an item of the Bmi history of a Patient
    /// </summary>
    public class Bmi : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the date of the item.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public virtual DateTime Date
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height of the patient in centimeters.
        /// </summary>
        /// <value>
        /// The height of the patient.
        /// </value>
        public virtual int Height
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the weight of the patient in kilograms.
        /// </summary>
        /// <value>
        /// The weight of the patient.
        /// </value>
        public virtual float Weight
        {
            get;
            set;
        }

        #endregion Properties
    }
}