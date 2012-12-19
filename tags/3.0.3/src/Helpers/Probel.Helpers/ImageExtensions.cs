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

namespace Probel.Helpers
{
    using System;
    using System.Drawing;

    using Probel.Helpers.Conversions;

    public static class ImageExtensions
    {
        #region Fields

        private const int THUMB_HEIGHT = 170;

        #endregion Fields

        #region Methods

        public static byte[] GetThumbnail(this Image img)
        {
            var height = img.Height;
            var width = img.Width;

            if (img.Height > 170)
            {
                var ratio = img.Height / THUMB_HEIGHT;
                width = img.Width / ratio;
                height = THUMB_HEIGHT;
            }

            var thumb = img.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
            return Converter.ImageToByteArray(thumb);
        }

        #endregion Methods
    }
}