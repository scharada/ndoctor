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
namespace Probel.NDoctor.Domain.DTO.Components
{
    using System.Collections.Generic;

    using Probel.NDoctor.Domain.DTO.Objects;

    public interface IImportComponent : IBaseComponent
    {
        #region Methods

        /// <summary>
        /// Creates the specified patients.
        /// </summary>
        /// <param name="?">The patients.</param>
        void Create(PatientFullDto[] patients);

        ///// <summary>
        ///// Creates the specified reputation and update its id after creation.
        ///// </summary>
        ///// <param name="reputation">The reputation.</param>
        //void Create(ReputationDto[] reputations);
        ///// <summary>
        ///// Creates the specified reputation and update its id after creation.
        ///// </summary>
        ///// <param name="practice">The reputation.</param>
        //void Create(PracticeDto[] practices);
        ///// <summary>
        ///// Creates the doctors
        ///// </summary>
        ///// <param name="doctors">The doctors.</param>
        //void Create(DoctorFullDto[] doctors);
        ///// <summary>
        ///// Creates the records
        ///// </summary>
        ///// <param name="doctors">The records.</param>
        //void Create(MedicalRecordDto[] records);
        ///// <summary>
        ///// Creates the specified appointments.
        ///// </summary>
        ///// <param name="appointments">The appointments.</param>
        //void Create(AppointmentDto[] appointments);
        ///// <summary>
        ///// Creates the specified tags.
        ///// </summary>
        ///// <param name="tags">The tags.</param>
        //void Create(TagDto[] tags);
        ///// <summary>
        ///// Creates the specified insurance.
        ///// </summary>
        ///// <param name="insuranceDto">The insurance.</param>
        //void Create(InsuranceDto[] insurance);
        ///// <summary>
        ///// Creates the specified bmi.
        ///// </summary>
        ///// <param name="bmi">The bmi.</param>
        //void Create(BmiDto[] bmi);
        /// <summary>
        /// Gets the default user.
        /// </summary>
        /// <returns></returns>
        LightUserDto GetDefaultUser();

        #endregion Methods
    }
}