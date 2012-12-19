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
namespace Probel.Helpers.Conversions
{
    using System;
    using System.ComponentModel;
    using System.Data.Common;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media.Imaging;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Exceptions;
    using Probel.Helpers.Properties;

    public static class Converter
    {
        #region Methods

        /// <summary>
        /// Convert the specified Byte array into image?
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>The converted byte array into an image</returns>
        public static Image ByteArrayToImage(byte[] bytes)
        {
            Assert.IsNotNull(bytes, "bytes");
            if (bytes.Length == 0) { return null; }
            try
            {
                using (var stream = new MemoryStream(bytes))
                {
                    return Image.FromStream(stream);
                }
            }
            catch (Exception ex) { throw new ConversionException(ex); }
        }

        /// <summary>
        /// Convert the file specified by the fileName into a byte array.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The converted file into a byte array</returns>
        public static byte[] FileToByteArray(string fileName)
        {
            Assert.IsFalse(string.IsNullOrEmpty(fileName), Messages.Ex_ConversionException_WrongPath);
            if (!File.Exists(fileName)) throw new FileNotFoundException();

            try
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                using (var reader = new BinaryReader(stream))
                {
                    var length = new FileInfo(fileName).Length;

                    return reader.ReadBytes((int)length);
                }
            }
            catch (Exception ex) { throw new ConversionException(ex); }
        }

        /// <summary>
        /// Gets the bytes contained in the column of the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static byte[] GetBytes(DbDataReader reader, string columnName)
        {
            const int CHUNK_SIZE = 2 * 1024;
            byte[] buffer = new byte[CHUNK_SIZE];
            long bytesRead;
            long fieldOffset = 0;
            int col = reader.GetOrdinal(columnName);

            using (MemoryStream stream = new MemoryStream())
            {
                while ((bytesRead = reader.GetBytes(col, fieldOffset, buffer, 0, buffer.Length)) > 0)
                {
                    byte[] actualRead = new byte[bytesRead];
                    Buffer.BlockCopy(buffer, 0, actualRead, 0, (int)bytesRead);
                    stream.Write(actualRead, 0, actualRead.Length);
                    fieldOffset += bytesRead;
                }
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Converts the specified image into an array of bytes
        /// </summary>
        /// <param name="image">The image to convert.</param>
        /// <returns>The converted image into a byte array</returns>
        public static byte[] ImageToByteArray(Image image)
        {
            var stream = new MemoryStream();
            image.Save(stream, (ImageFormat.Jpeg));
            return stream.ToArray();
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.Image"/> into a WPF <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="source">The source image.</param>
        /// <returns>A BitmapSource</returns>
        public static BitmapSource ToBitmapSource(this Image source)
        {
            if (source == null) return null;

            using (Bitmap bitmap = new Bitmap(source))
            {
                return bitmap.ToBitmapSource();
            }
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.Bitmap"/> into a WPF <see cref="BitmapSource"/>.
        /// </summary>
        /// <remarks>Uses GDI to do the conversion. Hence the call to the marshalled DeleteObject.
        /// </remarks>
        /// <param name="source">The source bitmap.</param>
        /// <returns>A BitmapSource</returns>
        public static BitmapSource ToBitmapSource(this Bitmap source)
        {
            BitmapSource bitmapSource = null;

            var hBitmap = source.GetHbitmap();

            try
            {
                bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitmapSource = null;
            }
            finally
            {
                NativeMethods.DeleteObject(hBitmap);
            }

            return bitmapSource;
        }

        #endregion Methods
    }

    /// <summary>
    /// FxCop requires all Marshalled functions to be in a class called NativeMethods.
    /// </summary>
    internal static class NativeMethods
    {
        #region Methods

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr hObject);

        #endregion Methods
    }
}