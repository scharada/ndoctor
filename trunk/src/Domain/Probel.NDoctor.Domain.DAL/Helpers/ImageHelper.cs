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

namespace Probel.NDoctor.Domain.DAL.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    using log4net;

    using Probel.Helpers.Conversions;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Properties;
    using Probel.NDoctor.Domain.DTO.Exceptions;

    public class ImageHelper
    {
        #region Fields

        private const int HEIGHT = 170;

        private static ILog Logger = LogManager.GetLogger(typeof(ImageHelper));

        #endregion Fields

        #region Methods

        /// <summary>
        /// If the specified picture doesn't have a thumbnail, it creates it. Otherwise, throws an exception
        /// </summary>
        /// <param name="picture">The picture.</param>
        /// <exception cref="BusinessLogicException">If the picture already has an exception</exception>
        public void CreateThumbnail(Picture picture)
        {
            if (!this.HasThumbnail(picture)) { throw new ThumbnailSetException(); }

            this.UpdateThumbnail(picture);
        }

        /// <summary>
        /// If the specified pictures don't have a thumbnail, it creates it. Otherwise, throws an exception
        /// </summary>
        /// <param name="picture">The pictures.</param>
        /// <exception cref="BusinessLogicException">At least one picture has a thumbnail</exception>
        public void CreateThumbnail(IEnumerable<Picture> pictures)
        {
            foreach (var picture in pictures)
            {
                this.CreateThumbnail(picture);
            }
        }

        /// <summary>
        /// Determines whether the specified picture has thumbnail.
        /// </summary>
        /// <param name="picture">The picture.</param>
        /// <returns>
        ///   <c>true</c> if the specified picture has thumbnail; otherwise, <c>false</c>.
        /// </returns>
        public bool HasThumbnail(Picture picture)
        {
            return (picture.ThumbnailBitmap == null || picture.ThumbnailBitmap.Length == 0);
        }

        /// <summary>
        /// Creates a thumbnail from the specified <see cref="Picture"/>.
        /// If a thumbnail already exist, it doesn't change it and returns <c>False</c>; otherwise
        /// it creates the thumbnail and returns <c>True</c>
        /// </summary>
        /// <returns><c>True</c> if the thumbnail is created; otherwise <c>False</c></returns>
        public bool TryCreateThumbnail(Picture picture)
        {
            if (!this.HasThumbnail(picture)) { return false; }
            else
            {
                this.UpdateThumbnail(picture);
                return true;
            }
        }

        /// <summary>
        /// Creates a thumbnail from the specified list of<see cref="Picture"/>.
        /// If a thumbnail already exist, it doesn't change it and returns <c>False</c>; otherwise
        /// it creates the thumbnail and returns <c>True</c>
        /// </summary>
        /// <returns><c>True</c> if at least one thumbnail is created; otherwise <c>False</c></returns>
        public bool TryCreateThumbnail(IEnumerable<Picture> pictures)
        {
            var result = true;
            var count = 0;
            foreach (var picture in pictures)
            {
                var temp = this.TryCreateThumbnail(picture);
                if (result == true)
                {
                    result = temp;
                    count++;
                }
            }
            return result;
        }

        /// <summary>
        /// Updates the thumbnail even if a thumbnail already exist.
        /// </summary>
        /// <param name="picture">The picture.</param>
        public void UpdateThumbnail(Picture picture)
        {
            var img = Converter.ByteArrayToImage(picture.Bitmap);
            var ratio = img.Height / HEIGHT;
            var width = img.Width / ratio;

            var thumb = img.GetThumbnailImage(width, HEIGHT, () => false, IntPtr.Zero);

            picture.ThumbnailBitmap = Converter.ImageToByteArray(thumb);
        }

        /// <summary>
        /// Updates the thumbnail even if a thumbnail already exist.
        /// </summary>
        /// <param name="picture">The pictures.</param>
        public void UpdateThumbnail(IEnumerable<Picture> pictures)
        {
            foreach (var picture in pictures)
            {
                this.UpdateThumbnail(picture);
            }
        }

        #endregion Methods
    }
}