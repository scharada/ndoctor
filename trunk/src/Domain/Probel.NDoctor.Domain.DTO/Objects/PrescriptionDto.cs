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

    using Probel.NDoctor.Domain.DTO.Validators;

    /// <summary>
    /// Represents a prescription. That's a drug with notes about how to take it.    
    /// </summary>
    [Serializable]
    public class PrescriptionDto : BaseDto
    {
        #region Fields

        private DrugDto drug;
        private string notes;
        private TagDto tag;

        #endregion Fields

        #region Constructors

        public PrescriptionDto()
            : base(new PrescriptionValidator())
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the drug.
        /// </summary>
        /// <value>
        /// The drug.
        /// </value>
        public DrugDto Drug
        {
            get { return this.drug; }
            set
            {
                this.drug = value;
                this.OnPropertyChanged(() => Drug);
            }
        }

        /// <summary>
        /// Gets or sets the notes. The notes are excpected to be something like: "Two pills before breakfast" or 
        /// "Two drops in a glass of water before going to sleep".
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
                this.OnPropertyChanged(() => Notes);
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