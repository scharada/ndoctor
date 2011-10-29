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
    /// Represents a period if time when the patient was ill
    /// </summary>
    public class IllnessPeriod : Entity
    {
        #region Properties

        /// <summary>
        /// Gets the duration of the illness period.
        /// </summary>
        public virtual TimeSpan Duration
        {
            get { return this.End - this.Start; }
        }

        /// <summary>
        /// Gets or sets the end date of the illness period.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public virtual DateTime End
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the notes about the illness period.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public virtual string Notes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pathology the patient suffered.
        /// </summary>
        /// <value>
        /// The pathology.
        /// </value>
        public virtual Pathology Pathology
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the start date of the pathology.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public virtual DateTime Start
        {
            get;
            set;
        }

        #endregion Properties
    }
}