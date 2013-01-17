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

namespace Probel.Helpers.Test
{
    using System;
    using System.IO;

    using NUnit.Framework;

    using Probel.Helpers.Data;

    [TestFixture]
    public class TestAppKey
    {
        #region Fields

        private readonly string TempFile = Path.GetTempPath() + Guid.NewGuid().ToString() + ".key";

        #endregion Fields

        #region Methods

        [Test]
        public void ReadKey_KeyExists_ReadKey()
        {
            var guid = this.CreateKeyFile();

            var key = new AppKey(this.TempFile);

            Assert.AreEqual(guid, key.GetKey());
        }

        [Test]
        public void ReadKey_NoKeyExists_CreateAndReadKey()
        {
            Assert.IsFalse(File.Exists(this.TempFile));

            var key = new AppKey(this.TempFile);

            Assert.AreNotEqual(key.GetKey(), new Guid());
            Assert.IsTrue(File.Exists(this.TempFile));
        }

        [SetUp]
        public void _Setup()
        {
            if (File.Exists(this.TempFile))
            {
                File.Delete(this.TempFile);
            }
        }

        private Guid CreateKeyFile()
        {
            var guid = Guid.NewGuid();

            using (var stream = File.OpenWrite(this.TempFile))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(guid.ToString());
            }
            return guid;
        }

        #endregion Methods
    }
}