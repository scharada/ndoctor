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
namespace Probel.NDoctor.Domain.DAL.Confirugration
{
    using System.Configuration;

    /// <summary>
    /// Contains configuration for the DAL
    /// </summary>
    public static class DalSettings
    {
        #region Properties

        /// <summary>
        /// Gets the path where the database is created.
        /// </summary>
        public static string DbPath
        {
            get
            {
                if (ExportHbm == null) return string.Empty;
                else return ConfigurationManager.AppSettings["DBPath"];
            }
        }

        /// <summary>
        /// Gets a value indicating whether nHibernate should export the HBM files.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [export HBM]; otherwise, <c>false</c>.
        /// </value>
        public static bool ExportHbm
        {
            get
            {
                string value = ConfigurationManager.AppSettings["ExportHbm"];
                if (string.IsNullOrEmpty(value)) return false;
                else return value.ToLower() == "true";

            }
        }

        /// <summary>
        /// Gets the path where export the HBM files if ExportHbm is set to <c>True</c>.
        /// </summary>
        public static string ExportHbmPath
        {
            get
            {
                if (!ExportHbm) return string.Empty;
                else return ConfigurationManager.AppSettings["ExportHbmPath"];
            }
        }

        #endregion Properties
    }
}