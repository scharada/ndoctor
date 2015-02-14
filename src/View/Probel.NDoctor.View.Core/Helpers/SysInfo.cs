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
namespace Probel.NDoctor.View.Core.Helpers
{
    using System;
    using System.Management;

    public class SysInfo
    {
        #region Constructors

        private SysInfo(string serial, string version)
        {
            this.Serial = serial;
            this.Version = version;
        }

        #endregion Constructors

        #region Properties

        public string Serial
        {
            get;
            private set;
        }

        public string Version
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public static SysInfo Load()
        {
            ManagementObject os = new ManagementObject("Win32_OperatingSystem=@");

            return new SysInfo((string)os["SerialNumber"], Environment.OSVersion.ToString());
        }

        #endregion Methods
    }
}