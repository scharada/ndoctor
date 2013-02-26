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

namespace Probel.NDoctor.Domain.DTO.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Probel.NDoctor.Domain.DTO.Objects;

    public interface IRescueToolsComponent
    {
        #region Methods

        /// <summary>
        /// Activates the specified deactivated patients.
        /// </summary>
        /// <param name="patients">The patients.</param>
        void Activate(IEnumerable<LightPatientDto> patients);

        /// <summary>
        /// Deactivates the specified patients.
        /// </summary>
        /// <param name="patients">The patients.</param>
        void Deactivate(IEnumerable<LightPatientDto> patients);

        /// <summary>
        /// Finds the deactivated patients.
        /// </summary>
        /// <returns></returns>
        IEnumerable<LightPatientDto> GetDeactivated();

        /// <summary>
        /// Gets a list of the doctor doubloons.
        /// </summary>
        /// <returns>A list of doubloons</returns>
        IEnumerable<DoubloonDoctorDto> GetDoctorDoubloons();

        /// <summary>
        /// Gets the doubloons of the specified doctor.
        /// </summary>
        /// <param name="doctor">The doctor that will be useds to find doubloons.</param>
        /// <returns>An enumeration of doctor that are doubloons with the specified doctor</returns>
        IEnumerable<LightDoctorDto> GetDoubloonsOf(LightDoctorDto doctor);

        /// <summary>
        /// Gets the doubloons of the specified doctor.
        /// </summary>
        /// <param name="firstName">The first name of the doctor.</param>
        /// <param name="lastName">The last name of the doctor.</param>
        /// <param name="specialisation">The specialisation of the doctor.</param>
        /// <returns>
        /// An enumeration of doctor that are doubloons with the specified doctor
        /// </returns>
        IEnumerable<LightDoctorDto> GetDoubloonsOf(string firstName, string lastName, TagDto specialisation);

        /// <summary>
        /// Gets the full doctor from the specified light dto.
        /// </summary>
        /// <param name="lightDoctorDto">The light doctor dto.</param>
        /// <returns></returns>
        DoctorDto GetFullDoctor(LightDoctorDto lightDoctorDto);

        /// <summary>
        /// Finds the patients older than the specified age.
        /// </summary>
        /// <param name="age">The age of the patient in years.</param>
        /// <returns></returns>
        IEnumerable<LightPatientDto> GetOlderThan(int age);

        /// <summary>
        /// Replaces the specified doubloons with the specified doctor.
        /// </summary>
        /// <param name="doubloons">The doubloons.</param>
        /// <param name="withDoctor">The doctor that will replace the doubloons.</param>
        void Replace(IEnumerable<LightDoctorDto> doubloons, LightDoctorDto withDoctor);

        /// <summary>
        /// Replaces the specified doubloons with the first encountered doctor.
        /// </summary>
        /// <param name="observableCollection">The observable collection.</param>
        void ReplaceWithFirstDoubloon(IEnumerable<LightDoctorDto> observableCollection);

        /// <summary>
        /// Updates the deactivation value for the specified patients.
        /// </summary>
        /// <param name="patients">The patients.</param>
        void UpdateDeactivation(IEnumerable<LightPatientDto> patients);

        #endregion Methods
    }
}