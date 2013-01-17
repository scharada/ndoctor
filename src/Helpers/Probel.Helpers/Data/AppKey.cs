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

namespace Probel.Helpers.Data
{
    using System;
    using System.IO;

    public class AppKey
    {
        #region Fields

        private readonly string KeyPath;

        #endregion Fields

        #region Constructors

        public AppKey(string keyPath)
        {
            this.KeyPath = keyPath;
        }

        #endregion Constructors

        #region Methods

        public static AppKey GetFromAppData(string vendor, string applicationName)
        {
            var fileName = string.Format("{0}\\{1}\\{2}\\Application.Key"
                     , Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                     , vendor
                     , applicationName);
            return new AppKey(fileName);
        }

        public Guid GetKey()
        {
            Guid guid = Guid.NewGuid();

            if (!File.Exists(this.KeyPath)) { this.CreateKey(guid); }
            else { guid = this.ReadKey(); }

            return guid;
        }

        private void CreateKey(Guid guid)
        {
            using (var writer = File.CreateText(this.KeyPath))
            {
                writer.Write(guid);
            }
        }

        private Guid ReadKey()
        {
            Guid guid;

            using (var stream = File.OpenRead(this.KeyPath))
            using (var reader = new StreamReader(stream))
            {
                var text = reader.ReadToEnd();
                if (!Guid.TryParse(text, out guid))
                {
                    guid = Guid.NewGuid();
                    CreateKey(guid);
                    return guid;
                }
            }
            return guid;
        }

        #endregion Methods
    }
}