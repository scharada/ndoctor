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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Probel.Helpers.Conversions;

namespace Probel.Helpers
{
    public static class ImageExtensions
    {
        private const int HEIGHT = 170;
        public static byte[] GetThumbnail(this Image img)
        {
            var ratio = img.Height / HEIGHT;
            var width = img.Width / ratio;

            var thumb = img.GetThumbnailImage(width, HEIGHT, () => false, IntPtr.Zero);
            return Converter.ImageToByteArray(thumb);
        }
    }
}
