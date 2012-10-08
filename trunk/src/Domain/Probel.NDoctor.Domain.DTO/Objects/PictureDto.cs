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

    [Serializable]
    public class PictureDto : LightPictureDto
    {
        #region Fields

        private byte[] bitmap;
        private DateTime creation;
        private DateTime lastUpdate;
        private string notes;
        private TagDto tag;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureDto"/> class.
        /// </summary>
        public PictureDto()
        {
            this.Creation = DateTime.Today;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the bitmap. That is the picture
        /// </summary>
        /// <value>
        /// The bitmap.
        /// </value>
        public byte[] Bitmap
        {
            get { return this.bitmap; }
            set
            {
                this.bitmap = value;
                this.OnPropertyChanged(() => this.Bitmap);
            }
        }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation.
        /// </value>
        public DateTime Creation
        {
            get { return this.creation; }
            set
            {
                this.creation = value;
                this.OnPropertyChanged(() => this.Creation);
            }
        }

        /// <summary>
        /// Gets or sets the last update for this picture.
        /// </summary>
        /// <value>
        /// The last update.
        /// </value>
        public DateTime LastUpdate
        {
            get { return this.lastUpdate; }
            set
            {
                this.lastUpdate = value;
                this.OnPropertyChanged(() => this.LastUpdate);
            }
        }

        /// <summary>
        /// Gets or sets the notes about this picture.
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
                this.OnPropertyChanged(() => this.Notes);
            }
        }

        /// <summary>
        /// Gets or sets the tag to categorise this picture.
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
                this.OnPropertyChanged(() => this.Tag);
            }
        }

        #endregion Properties
    }
}