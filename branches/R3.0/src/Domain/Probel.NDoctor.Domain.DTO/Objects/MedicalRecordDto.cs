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
    /// Represents a medical record item
    /// </summary>
    [Serializable]
    public class MedicalRecordDto : BaseDto
    {
        #region Fields

        private DateTime creationDate;
        private DateTime lastUpdate;
        private string name;
        private string rtf;
        private TagDto tag = new TagDto(TagCategory.MedicalRecord);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicalRecord"/> class.
        /// Set the creation date to now
        /// </summary>
        public MedicalRecordDto()
        {
            this.creationDate
                = this.lastUpdate
                = DateTime.Now;
            this.name = DateTime.Today.ToShortDateString();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the date of creation creation.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public DateTime CreationDate
        {
            get { return this.creationDate; }
            set
            {
                if (this.creationDate != value)
                {
                    this.creationDate
                        = this.lastUpdate
                        = value;
                    this.OnPropertyChanged("CreationDate");
                }
            }
        }

        /// <summary>
        /// Gets or sets the date of the last update.
        /// </summary>
        /// <value>
        /// The last update.
        /// </value>
        public DateTime LastUpdate
        {
            get { return this.lastUpdate; }
            set
            {
                if (this.lastUpdate != value)
                {
                    this.lastUpdate = value;
                    this.OnPropertyChanged("LastUpdate");
                }
            }
        }

        /// <summary>
        /// Gets or sets the title of the medical records.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Name
        {
            get { return this.name; }
            set
            {
                if (value != null && this.name != value)
                {
                    this.name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Gets or sets the HTML representing the medical record.
        /// </summary>
        /// <value>
        /// The HTML.
        /// </value>
        public string Rtf
        {
            get { return this.rtf; }
            set
            {
                if (value != null && this.rtf != value)
                {
                    this.State = State.Updated;
                    this.rtf = value;
                    this.OnPropertyChanged("Rtf");
                }
            }
        }

        /// <summary>
        /// Gets or sets the tag to group the medical record.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public TagDto Tag
        {
            get { return this.tag; }
            set
            {
                if (value != null && tag.Id != value.Id)
                {
                    this.tag = value;
                    this.OnPropertyChanged("Tag");
                }
            }
        }

        #endregion Properties
    }
}