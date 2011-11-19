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
    /// Dto of a tag
    /// </summary>
    public class TagDto : BaseDto
    {
        #region Fields

        private string name;
        private string notes;
        private TagCategory tagType;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TagDto"/> class.
        /// </summary>
        public TagDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagDto"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        public TagDto(TagCategory category)
        {
            this.Category = category;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public TagCategory Category
        {
            get { return this.tagType; }
            set
            {
                this.tagType = value;
                this.OnPropertyChanged("Category");
            }
        }

        /// <summary>
        /// Gets or sets the name.
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
        /// Gets or sets the notes.
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

        #endregion Properties
    }
}