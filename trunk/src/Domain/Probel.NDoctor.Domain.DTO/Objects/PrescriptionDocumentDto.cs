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
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents a set of drug prescriptions.
    /// </summary>
    public class PrescriptionDocumentDto : BaseDto
    {
        #region Fields

        private DateTime creationDate;
        private TagDto tag;
        private string title;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PrescriptionDocumentDto"/> class.
        /// </summary>
        public PrescriptionDocumentDto()
        {
            this.Prescriptions = new ObservableCollection<PrescriptionDto>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the creation date of this document.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public DateTime CreationDate
        {
            get { return this.creationDate; }
            set
            {
                this.creationDate = value;
                this.OnPropertyChanged("CreationDate");
            }
        }

        public string CreationDateToDisplay
        {
            get { return this.CreationDate.ToShortDateString(); }
        }

        /// <summary>
        /// Gets the prescriptions that compose this document.
        /// </summary>
        public ObservableCollection<PrescriptionDto> Prescriptions
        {
            get;
            private set;
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
        /// Gets or sets the title of this document.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                this.OnPropertyChanged("Title");
            }
        }

        #endregion Properties
    }
}