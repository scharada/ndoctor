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

namespace Probel.Helpers.Strings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class PasswordExtension
    {
        #region Fields

        private static readonly string IV = "/rVRErnUre4=";
        private static readonly string Key = "/UMeHc7ZBmBDT87E1F5YLA==";

        #endregion Fields

        #region Methods

        public static string Decrypt(this string encrypted)
        {
            if (string.IsNullOrEmpty(encrypted)) { return string.Empty; }

            RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
            ICryptoTransform decryptor = rc2CSP.CreateDecryptor(Convert.FromBase64String(Key), Convert.FromBase64String(IV));
            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encrypted)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    List<Byte> bytes = new List<byte>();
                    int b;
                    do
                    {
                        b = csDecrypt.ReadByte();
                        if (b != -1)
                        {
                            bytes.Add(Convert.ToByte(b));
                        }

                    }
                    while (b != -1);

                    return Encoding.Unicode.GetString(bytes.ToArray());
                }
            }
        }

        public static string Encrypt(this string password)
        {
            RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();

            ICryptoTransform encryptor = rc2CSP.CreateEncryptor(Convert.FromBase64String(Key), Convert.FromBase64String(IV));
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    byte[] toEncrypt = Encoding.Unicode.GetBytes(password);

                    csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
                    csEncrypt.FlushFinalBlock();

                    byte[] encrypted = msEncrypt.ToArray();

                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        #endregion Methods
    }
}