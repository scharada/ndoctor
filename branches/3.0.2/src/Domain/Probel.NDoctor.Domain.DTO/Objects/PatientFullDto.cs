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
namespace Probel.NDoctor.Domain.DTO.Objects
{
    using System;
    using System.Collections.ObjectModel;

    using Probel.Mvvm;

    [Serializable]
    public class PatientFullDto : PatientDto
    {
        #region Fields

        private AddressDto address = new AddressDto();
        private DateTime birthdate = DateTime.Today;
        private DateTime inscriptionDate = DateTime.Today;
        private InsuranceDto insurance = new InsuranceDto();
        private DateTime lastUpdate = DateTime.Today;
        private PracticeDto practice = new PracticeDto();
        private ProfessionDto profession = new ProfessionDto();
        private ReputationDto reputation = new ReputationDto();
        private TagDto tag = new TagDto(TagCategory.Patient);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientFullDto"/> class.
        /// </summary>
        public PatientFullDto()
        {
            this.Doctors = new ObservableCollection<DoctorFullDto>();
            this.MedicalRecords = new ObservableCollection<MedicalRecordDto>();
            this.BmiHistory = new ObservableCollection<BmiDto>();
            this.Pictures = new ObservableCollection<PictureDto>();
            this.Appointments = new ObservableCollection<AppointmentDto>();
            this.IllnessHistory = new ObservableCollection<IllnessPeriodDto>();
            this.PrescriptionDocuments = new ObservableCollection<PrescriptionDocumentDto>();
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<AppointmentDto> Appointments
        {
            get;
            private set;
        }

        public ObservableCollection<BmiDto> BmiHistory
        {
            get;
            private set;
        }

        public ObservableCollection<DoctorFullDto> Doctors
        {
            get;
            private set;
        }

        public ObservableCollection<IllnessPeriodDto> IllnessHistory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the medical records of the patient.
        /// </summary>
        /// <value>
        /// The medical records.
        /// </value>
        public ObservableCollection<MedicalRecordDto> MedicalRecords
        {
            get;
            private set;
        }

        public ObservableCollection<PictureDto> Pictures
        {
            get;
            private set;
        }

        public ObservableCollection<PrescriptionDocumentDto> PrescriptionDocuments
        {
            get;
            private set;
        }

        #endregion Properties
    }
}