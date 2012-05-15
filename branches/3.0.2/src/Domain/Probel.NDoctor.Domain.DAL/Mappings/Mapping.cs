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
namespace Probel.NDoctor.Domain.DAL.Mappings
{
    using AutoMapper;

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Objects;

    public static class Mapping
    {
        #region Methods

        public static void Configure()
        {
            MapEntityToDto();
            MapDtoToEntity();
            MapDtoToDto();
            MedicalRecordMapping.Configure();
        }

        private static void MapDtoToDto()
        {
            Mapper.CreateMap<LightPatientDto, PatientBmiDto>();
            Mapper.CreateMap<PatientBmiDto, LightPatientDto>();
            Mapper.CreateMap<LightPracticeDto, PracticeDto>();
            Mapper.CreateMap<PracticeDto, LightPracticeDto>();
        }

        private static void MapDtoToEntity()
        {
            Mapper.CreateMap<UserDto, User>();
            Mapper.CreateMap<AddressDto, Address>();
            Mapper.CreateMap<PracticeDto, Practice>();
            Mapper.CreateMap<LightRoleDto, Role>();
            Mapper.CreateMap<LightPatientDto, Patient>();
            Mapper.CreateMap<InsuranceDto, Insurance>();
            Mapper.CreateMap<LightInsuranceDto, Insurance>();
            Mapper.CreateMap<ProfessionDto, Profession>();
            Mapper.CreateMap<ReputationDto, Reputation>();
            Mapper.CreateMap<TagDto, Tag>();
            Mapper.CreateMap<PatientDto, Patient>();
            Mapper.CreateMap<LightUserDto, User>();
            Mapper.CreateMap<LightPracticeDto, Practice>();
            Mapper.CreateMap<BmiDto, Bmi>();
            Mapper.CreateMap<MedicalRecordDto, MedicalRecord>();
            Mapper.CreateMap<PictureDto, Picture>();
            Mapper.CreateMap<IllnessPeriodDto, IllnessPeriod>();
            Mapper.CreateMap<PathologyDto, Pathology>();
            Mapper.CreateMap<DrugDto, Drug>();
            Mapper.CreateMap<PrescriptionDto, Prescription>();
            Mapper.CreateMap<PrescriptionDocumentDto, PrescriptionDocument>();
            Mapper.CreateMap<AppointmentDto, Appointment>();
            Mapper.CreateMap<DoctorDto, Doctor>();
            Mapper.CreateMap<PatientFullDto, Patient>();
            Mapper.CreateMap<DoctorFullDto, Doctor>();
        }

        private static void MapEntityToDto()
        {
            Mapper.CreateMap<Doctor, LightDoctorDto>()
                .ForMember(dest => dest.DisplayedName, opt => opt.Ignore());

            Mapper.CreateMap<User, LightUserDto>();
            Mapper.CreateMap<User, UserDto>();
            Mapper.CreateMap<Address, AddressDto>();
            Mapper.CreateMap<Role, LightRoleDto>();
            Mapper.CreateMap<Practice, PracticeDto>();
            Mapper.CreateMap<Patient, LightPatientDto>();
            Mapper.CreateMap<Patient, PatientDto>();
            Mapper.CreateMap<Insurance, InsuranceDto>();
            Mapper.CreateMap<Insurance, LightInsuranceDto>();
            Mapper.CreateMap<Reputation, ReputationDto>();
            Mapper.CreateMap<Profession, ProfessionDto>();
            Mapper.CreateMap<Tag, TagDto>();
            Mapper.CreateMap<User, LightUserDto>();
            Mapper.CreateMap<Practice, LightPracticeDto>();
            Mapper.CreateMap<Bmi, BmiDto>();
            Mapper.CreateMap<MedicalRecord, MedicalRecordDto>();
            Mapper.CreateMap<Picture, PictureDto>();
            Mapper.CreateMap<IllnessPeriod, IllnessPeriodDto>();
            Mapper.CreateMap<Pathology, PathologyDto>();
            Mapper.CreateMap<Drug, DrugDto>();
            Mapper.CreateMap<Prescription, PrescriptionDto>();
            Mapper.CreateMap<PrescriptionDocument, PrescriptionDocumentDto>();
            Mapper.CreateMap<Appointment, AppointmentDto>();
            Mapper.CreateMap<Doctor, DoctorDto>();
            Mapper.CreateMap<Patient, PatientFullDto>();
            Mapper.CreateMap<Doctor, DoctorFullDto>();
        }

        #endregion Methods
    }
}