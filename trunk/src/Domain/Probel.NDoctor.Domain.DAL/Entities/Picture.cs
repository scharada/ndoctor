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
    /// Represents a medical picture with information about it
    /// </summary>
    public class Picture : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the bitmap. That is the picture
        /// </summary>
        /// <value>
        /// The bitmap.
        /// </value>
        public virtual byte[] Bitmap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation.
        /// </value>
        public virtual DateTime Creation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last update for this picture.
        /// </summary>
        /// <value>
        /// The last update.
        /// </value>
        public virtual DateTime LastUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the notes about this picture.
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
        /// Gets or sets the tag to categorise this picture.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public virtual Tag Tag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the thumbnailed bitmap.
        /// </summary>
        /// <value>
        /// The thumbnailed bitmap.
        /// </value>
        public virtual byte[] ThumbnailBitmap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the related patient of this picture.
        /// </summary>
        public virtual Patient Patient { get;  set; }

        #endregion Properties
    }
}