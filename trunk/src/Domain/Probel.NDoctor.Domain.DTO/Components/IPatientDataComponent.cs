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

    public interface IPatientDataComponent : IBaseComponent
    {
        #region Methods

        /// <summary>
        /// Adds the specified doctor to the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="doctor">The doctor.</param>
        void AddDoctorTo(LightPatientDto patient, LightDoctorDto doctor);

        /// <summary>
        /// Creates the specified profession.
        /// </summary>
        /// <param name="profession">The profession.</param>
        long Create(ProfessionDto profession);

        /// <summary>
        /// Create the specified item into the database.
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        long Create(ReputationDto item);

        /// <summary>
        /// Create the specified item into the database.
        /// </summary>
        /// <param name="insurance">The insurance.</param>
        /// <returns></returns>
        long Create(InsuranceDto insurance);

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="reputation">The item to add in the database</param>
        long Create(PracticeDto practice);

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="reputation">The item to add in the database</param>
        long Create(DoctorDto doctor);

        /// <summary>
        /// Gets the doctors linked to the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>A list of doctors</returns>
        IList<LightDoctorDto> GetDoctorOf(LightPatientDto patient);

        /// <summary>
        /// Gets the doctors that can be linked to the specified doctor.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="on">Indicate where the search should be executed.</param>
        /// <returns>
        /// A list of doctor
        /// </returns>
        IList<LightDoctorDto> GetNotLinkedDoctorsFor(LightPatientDto patient, string criteria, SearchOn on);

        /// <summary>
        /// Loads all the data of the patient represented by the specified id.
        /// </summary>
        /// <param name="patient">The id of the patient to load.</param>
        /// <returns>A DTO with the whole data</returns>
        /// <exception cref="Probel.NDoctor.Domain.DAL.Exceptions.ItemNotFoundException">If the id is not linked to a patient</exception>
        PatientDto GetPatient(long id);

        /// <summary>
        /// Loads all the data of the patient.
        /// </summary>
        /// <param name="patient">The patient to load.</param>
        /// <returns>A DTO with the whole data</returns>
        /// <exception cref="Probel.NDoctor.Domain.DAL.Exceptions.ItemNotFoundException">If the patient doesn't exist</exception>
        PatientDto GetPatient(LightPatientDto patient);

        /// <summary>
        /// Removes the link that existed between the specified patient and the specified doctor.
        /// </summary>
        /// <exception cref="EntityNotFoundException">If there's no link between the doctor and the patient</exception>
        /// <param name="patient">The patient.</param>
        /// <param name="doctor">The doctor.</param>
        void RemoveDoctorFor(LightPatientDto patient, LightDoctorDto doctor);

        #endregion Methods
    }
}