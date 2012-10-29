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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Automapper advanced mapping
    /// </summary>
    internal static class MedicalRecordMapping
    {
        #region Methods

        /// <summary>
        /// Configure the mapping.
        /// </summary>
        public static void Configure()
        {

            Mapper.CreateMap<Patient, MedicalRecordCabinetDto>()
                .ConvertUsing(src => CustomConversion(src));
        }

        private static MedicalRecordFolderDto[] BuildFolders(IList<MedicalRecord> records)
        {
            var folders = new List<MedicalRecordFolderDto>();
            var tags = (from r in records
                        group r by r.Tag into t
                        select t.Key).ToList();

            foreach (var tag in tags)
            {
                var folder = new MedicalRecordFolderDto()
                {
                    Name = tag.Name,
                    Notes = tag.Notes,
                };
                folder.Records = BuildRecords(records, tag);
                folders.Add(folder);
            }
            return folders.ToArray();
        }

        private static MedicalRecordDto[] BuildRecords(IList<MedicalRecord> records, Tag tag)
        {
            var result = (from record in records
                          where record.Tag.Id == tag.Id
                          select record).ToArray();

            return Mapper.Map<MedicalRecord[], MedicalRecordDto[]>(result);
        }

        private static MedicalRecordCabinetDto CustomConversion(Patient src)
        {
            var cabinet = new MedicalRecordCabinetDto();
            cabinet.Folders = BuildFolders(src.MedicalRecords);
            return cabinet;
        }

        #endregion Methods

        #region Other

        //private static List<MedicalRecordFolderDto> GetFolders(Patient patient)
        //{
        //    var folders = Mapper.Map<List<Tag>, List<MedicalRecordFolderDto>>(GetTags(patient));
        //    foreach (var folder in folders)
        //    {
        //        folder.Records = GetRecords(patient, folder);
        //        foreach (var item in folder.Records) { item.Clean(); }
        //    }
        //    return folders;
        //}
        //private static MedicalRecordDto[] GetRecords(Patient patient, MedicalRecordFolderDto folder)
        //{
        //    var result = (from r in patient.MedicalRecords
        //                  where r.Tag.Name == folder.Name
        //                  select r).ToList();
        //    var buffer = Mapper.Map<List<MedicalRecord>, List<MedicalRecordDto>>(result).ToArray();
        //    foreach (var item in buffer) { item.Clean(); }
        //    return buffer;
        //}
        //private static List<Tag> GetTags(Patient patient)
        //{
        //    return (from r in patient.MedicalRecords
        //            group r by r.Tag into tags
        //            select tags.Key).ToList();
        //}

        #endregion Other
    }
}