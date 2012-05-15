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
    /// Represents a illness period while a patient was ill
    /// </summary>
    public class IllnessPeriodDto : BaseDto
    {
        #region Fields

        private DateTime end;
        private string notes;
        private PathologyDto pathology;
        private DateTime start;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the end date of the period.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public DateTime End
        {
            get { return this.end; }
            set
            {
                this.end = value;
                this.OnPropertyChanged("End");
            }
        }

        /// <summary>
        /// Gets or sets the notes about the illness period.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes
        {
            get { return this.notes; }
            set
            {
                this.notes = value;
                this.OnPropertyChanged("Notes");
            }
        }

        /// <summary>
        /// Gets or sets the pathology.
        /// </summary>
        /// <value>
        /// The pathology.
        /// </value>
        public PathologyDto Pathology
        {
            get { return this.pathology; }
            set
            {
                this.pathology = value;
                this.OnPropertyChanged("Pathology");
            }
        }

        /// <summary>
        /// Gets or sets the start date of the period.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public DateTime Start
        {
            get { return this.start; }
            set
            {
                this.start = value;
                this.OnPropertyChanged("Start");
            }
        }

        #endregion Properties
    }
}