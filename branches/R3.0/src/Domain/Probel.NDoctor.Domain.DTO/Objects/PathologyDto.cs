#region Header

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

#endregion Header

namespace Probel.NDoctor.Domain.DTO.Objects
{
    using System;

    /// <summary>
    /// Represents a pathology
    /// </summary>
    [Serializable]
    public class PathologyDto : BaseDto
    {
        #region Fields

        private string name;
        private string notes;
        private TagDto tag;

        #endregion Fields

        #region Constructors

        public PathologyDto()
        {
            this.Tag = new TagDto();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the name of the pathology.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets the notes about the pathology.
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
        /// Gets or sets the tag to categorise this pathology. 
        /// It's better to have a TagCategory set to "Pathology"
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

        #endregion Properties
    }
}