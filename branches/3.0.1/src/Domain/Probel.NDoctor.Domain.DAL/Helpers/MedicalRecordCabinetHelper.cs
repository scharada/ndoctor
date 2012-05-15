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
namespace Probel.NDoctor.Domain.DAL.Helpers
{
    using System;

    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Helpers methods for the medical records cabinet
    /// </summary>
    public static class MedicalRecordCabinetHelper
    {
        #region Methods

        /// <summary>
        /// Executes the specified action on all the medical records of the specified cabinet
        /// </summary>
        /// <param name="cabinet">The cabinet.</param>
        /// <param name="action">The action.</param>
        public static void ForeachRecords(this MedicalRecordCabinetDto cabinet, Action<MedicalRecordDto> action)
        {
            foreach (var folder in cabinet.Folders)
                foreach (var record in folder.Records)
                {
                    action(record);
                }
        }

        #endregion Methods
    }
}