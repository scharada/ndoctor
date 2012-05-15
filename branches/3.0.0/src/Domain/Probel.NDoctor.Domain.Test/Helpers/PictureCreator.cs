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
namespace Probel.NDoctor.Domain.Test.Helpers
{
    using System;
    using System.IO;
    using System.Reflection;

    public static class Build
    {
        #region Methods

        public static byte[] Picture(int index)
        {
            if (index <= 0 || index > 8) throw new IndexOutOfRangeException("The index should be above zero and below or equal to 8");
            var path = string.Format("Probel.NDoctor.Domain.Test.Images.picture{0}.jpg", index);

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
            using (var reader = new BinaryReader(stream))
            {
                var length = stream.Length;
                return reader.ReadBytes((int)length);
            }
        }

        #endregion Methods
    }
}