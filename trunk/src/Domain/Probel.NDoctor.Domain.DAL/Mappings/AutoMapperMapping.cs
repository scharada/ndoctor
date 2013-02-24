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
    using Probel.NDoctor.Statistics.Domain;

    public static class AutoMapperMapping
    {
        #region Methods

        public static void Configure()
        {
            MapEntityToDto();
            MapEntityToDto_Optimsised();

            MapDtoToEntity();
            MapDtoToDto();
            MapEntityToEntity();
            MedicalRecordMapping.Configure();
        }

        private static void Clean(BaseDto dto)
        {
            if (dto != null) { dto.Clean(); }
        }

        private static void MapDtoToDto()
        {
            Mapper.CreateMap<PatientDto, LightPatientDto>();
            Mapper.CreateMap<PatientBmiDto, LightPatientDto>();
            Mapper.CreateMap<LightPatientDto, PatientBmiDto>();
            Mapper.CreateMap<LightPracticeDto, PracticeDto>();
            Mapper.CreateMap<PracticeDto, LightPracticeDto>();
            Mapper.CreateMap<LightPatientDto, PatientDto>();

            Mapper.CreateMap<ApplicationStatistics, StatisticEntry>();
        }

        private static void MapDtoToEntity()
        {
            Mapper.CreateMap<PatientDto, Patient>();
            Mapper.CreateMap<LightPatientDto, Patient>();
            Mapper.CreateMap<PatientFullDto, Patient>();

            Mapper.CreateMap<DoctorDto, Doctor>();
            Mapper.CreateMap<DoctorFullDto, Doctor>();
            Mapper.CreateMap<LightDoctorDto, Doctor>();

            Mapper.CreateMap<UserDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            Mapper.CreateMap<SecurityUserDto, User>()
                .ForMember(dest => dest.Header, opt => opt.Ignore())
                .ForMember(dest => dest.Login, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Practice, opt => opt.Ignore());

            Mapper.CreateMap<AddressDto, Address>();
            Mapper.CreateMap<PracticeDto, Practice>();
            Mapper.CreateMap<InsuranceDto, Insurance>();
            Mapper.CreateMap<LightInsuranceDto, Insurance>();
            Mapper.CreateMap<ProfessionDto, Profession>();
            Mapper.CreateMap<ReputationDto, Reputation>();
            Mapper.CreateMap<TagDto, Tag>();
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
            Mapper.CreateMap<RoleDto, Role>();
            Mapper.CreateMap<TaskDto, Task>();
            Mapper.CreateMap<MacroDto, Macro>();
            Mapper.CreateMap<MedicalRecordState, MedicalRecordStateDto>();
            Mapper.CreateMap<LightPictureDto, Picture>();
        }

        private static void MapEntityToDto()
        {
            Mapper.CreateMap<Doctor, LightDoctorDto>()
                .ForMember(dest => dest.DisplayedName, opt => opt.Ignore())
                .AfterMap((entity, dto) => Clean(dto));

            Mapper.CreateMap<User, SecurityUserDto>()
                .AfterMap((entity, dto) => Clean(dto))
                .ConstructUsing(e => new SecurityUserDto(e.IsSuperAdmin));

            Mapper.CreateMap<User, UserDto>()
                .AfterMap((entity, dto) => Clean(dto))
                .ConstructUsing(e => new UserDto(e.IsSuperAdmin));

            Mapper.CreateMap<Task, TaskDto>()
                .AfterMap((entity, dto) => Clean(dto))
                .ConstructUsing(e => new TaskDto(e.RefName));

            Mapper.CreateMap<Address, AddressDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Practice, PracticeDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Insurance, InsuranceDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Insurance, LightInsuranceDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Reputation, ReputationDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Profession, ProfessionDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<User, SecurityUserDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Practice, LightPracticeDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Bmi, BmiDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<MedicalRecord, MedicalRecordDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<IllnessPeriod, IllnessPeriodDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Pathology, PathologyDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Drug, DrugDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Prescription, PrescriptionDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<PrescriptionDocument, PrescriptionDocumentDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Appointment, AppointmentDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Doctor, DoctorDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Doctor, DoctorFullDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Role, RoleDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Macro, MacroDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<DbSetting, DbSettingDto>().AfterMap((entity, dto) => Clean(dto));

            Mapper.CreateMap<Patient, PatientDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Patient, PatientFullDto>().AfterMap((entity, dto) => Clean(dto));

            Mapper.CreateMap<MedicalRecord, MedicalRecordDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Tag, MedicalRecordFolderDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Tag, TagDto>().AfterMap((entity, dto) => Clean(dto));
            Mapper.CreateMap<Doctor, DoubloonDoctorDto>();
        }

        /// <summary>
        /// Optimisation of mapping when mapping is slow
        /// </summary>
        private static void MapEntityToDto_Optimsised()
        {
            Mapper.CreateMap<Picture, PictureDto>().ConvertUsing(src =>
                {
                    var dto = new PictureDto()
                    {
                        Id = src.Id,
                        Creation = src.Creation,
                        IsImported = src.IsImported,
                        LastUpdate = src.LastUpdate,
                        Notes = src.Notes,
                        Tag = Mapper.Map<Tag, TagDto>(src.Tag),
                        ThumbnailBitmap = src.ThumbnailBitmap,
                        Bitmap = src.Bitmap,
                    };
                    Clean(dto);
                    return dto;
                });

            Mapper.CreateMap<Picture, LightPictureDto>().ConvertUsing(src =>
            {
                var dto = new LightPictureDto()
                {
                    Id = src.Id,
                    IsImported = src.IsImported,
                    Tag = Mapper.Map<Tag, TagDto>(src.Tag),
                    ThumbnailBitmap = src.ThumbnailBitmap,
                };
                Clean(dto);
                return dto;
            });

            Mapper.CreateMap<Patient, LightPatientDto>().ConvertUsing(src =>
            {
                var dto = new LightPatientDto()
                {
                    Birthdate = src.BirthDate,
                    FirstName = src.FirstName,
                    Gender = src.Gender,
                    Height = (int)src.Height,
                    Id = src.Id,
                    IsImported = src.IsImported,
                    LastName = src.LastName,
                    IsDeactivated = src.IsDeactivated,
                    Profession = Mapper.Map<ProfessionDto>(src.Profession),
                    InscriptionDate = src.InscriptionDate,
                    LastUpdate = src.LastUpdate,
                    Address = Mapper.Map<Address, AddressDto>(src.Address),
                    Reason = src.Reason,
                };
                Clean(dto);
                return dto;
            });
        }

        private static void MapEntityToEntity()
        {
            Mapper.CreateMap<MedicalRecord, MedicalRecordState>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            Mapper.CreateMap<MedicalRecordState, MedicalRecord>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            Mapper.CreateMap<ApplicationStatistics, StatisticEntry>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }

        #endregion Methods
    }
}