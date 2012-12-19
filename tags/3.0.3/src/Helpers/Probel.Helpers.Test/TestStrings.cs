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
namespace Probel.Helpers.Test
{
    using System;

    using NUnit.Framework;

    using Probel.Helpers.Strings;

    public class TestStrings : TestBase
    {
        #region Methods

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckFormatWith_SpecifyNullArgs_ThrowArgumentNullException()
        {
            object[] args = null;
            "Hello".FormatWith(args);
        }

        [Test]
        public void ManagePassword_DecryptPassword_PasswordIsDecrypted()
        {
            for (int i = 0; i < 10; i++)
            {
                var password = "password";
                var encrypted = password.Encrypt();
                var decrypted = encrypted.Decrypt();

                Assert.AreEqual(password, decrypted);
            }
        }

        [Test]
        public void ManagePassword_EncryptPassword_PasswordIsEncrypted()
        {
            var password = "password";
            var encrypted = password.Encrypt();

            Assert.AreNotEqual(password, encrypted);
        }

        #endregion Methods
    }
}