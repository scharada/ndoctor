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
namespace Probel.NDoctor.Domain.DAL.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Macro;
    using Probel.NDoctor.Domain.DAL.Subcomponents;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Contains all the query to manage the medical records of a patient
    /// </summary>
    public class MedicalRecordComponent : BaseComponent, IMedicalRecordComponent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicalRecordComponent"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public MedicalRecordComponent(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicalRecordComponent"/> class.
        /// </summary>
        public MedicalRecordComponent()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates the specified record and link it to the specidied patient.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="forPatient">For patient.</param>
        public void Create(MedicalRecordDto record, LightPatientDto forPatient)
        {
            new Creator(this.Session).Create(record, forPatient);
        }

        /// <summary>
        /// Creates the specified macro.
        /// </summary>
        /// <param name="macro">The macro.</param>
        /// <returns>The id of the created macro</returns>
        public long Create(MacroDto macro)
        {
            return new Creator(this.Session).Create(macro);
        }

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        /// <returns>
        /// The id of the just created item
        /// </returns>
        public long Create(TagDto item)
        {
            return new Creator(this.Session).Create(item);
        }

        /// <summary>
        /// Gets all the macros.
        /// </summary>
        /// <returns></returns>
        public MacroDto[] GetAllMacros()
        {
            var dto = (from macro in this.Session.Query<Macro>()
                       select macro);
            return Mapper.Map<IEnumerable<Macro>, MacroDto[]>(dto);
        }

        /// <summary>
        /// Gets the history of the specified medical record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns>The items contained in the history</returns>
        public IEnumerable<MedicalRecordStateDto> GetHistory(MedicalRecordDto record)
        {
            var history = this.Session.Get<MedicalRecord>(record.Id);
            return Mapper.Map<IEnumerable<MedicalRecordState>, IEnumerable<MedicalRecordStateDto>>(history.PreviousStates);
        }

        /// <summary>
        /// Gets the medical record by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The medical record or <c>Null</c> if not found
        /// </returns>
        public MedicalRecordDto GetMedicalRecordById(long id)
        {
            var result = this.Session.Get<MedicalRecord>(id);

            return (result != null)
                ? Mapper.Map<MedicalRecord, MedicalRecordDto>(result)
                : null;
        }

        /// <summary>
        /// Gets all the medical records of the specified patient. The records are packed into a 
        /// medical record cabinet which contains medical records folders. Each folder contains a list 
        /// of medical records.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public MedicalRecordCabinetDto GetMedicalRecordCabinet(LightPatientDto patient)
        {
            return new Selector(this.Session).GetMedicalRecordCabinet(patient);
        }

        /// <summary>
        /// Gets all the tags with the specified catagory.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public IList<TagDto> GetTags(TagCategory category)
        {
            return new Selector(this.Session).GetTags(category);
        }

        /// <summary>
        /// Determines whether the specified macro is valid.
        /// </summary>
        /// <param name="macro"></param>
        /// <returns>
        ///   <c>true</c> if macro is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid(MacroDto macro)
        {
            return (macro != null)
                ? MacroBuilder.IsValidExpression(macro.Expression)
                : false;
        }

        /// <summary>
        /// Determines whether the specified macros are valid.
        /// </summary>
        /// <param name="macros">The macros.</param>
        /// <returns>
        ///   <c>true</c> if the specified macros are valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid(IEnumerable<MacroDto> macros)
        {
            foreach (var macro in macros)
            {
                if (!MacroBuilder.IsValidExpression(macro.Expression)) { return false; }
            }
            return true;
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(MacroDto item)
        {
            new Remover(this.Session).Remove(item);
        }

        /// <summary>
        /// Resolves the specified macro with the data of the specified patient.
        /// </summary>
        /// <param name="macro">The macro.</param>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public string Resolve(MacroDto macro, LightPatientDto patient)
        {
            if (macro == null || patient == null) return string.Empty;

            var p = this.Session.Get<Patient>(patient.Id);
            if (p == null) throw new EntityNotFoundException(typeof(Patient));

            var builder = new MacroBuilder(p);
            return builder.Resolve(macro.Expression);
        }

        /// <summary>
        /// Revert the specified medical record into the specified state
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="toState">The state.</param>
        public void Revert(MedicalRecordDto record, MedicalRecordStateDto toState)
        {
            Assert.IsNotNull(toState, "toState");

            record.Rtf = toState.Rtf;
            record.Tag = toState.Tag;
            record.LastUpdate = DateTime.Now;

            new Updator(this.Session).Update(record);
        }

        /// <summary>
        /// Updates the specified macro.
        /// </summary>
        /// <param name="macro">The macro.</param>
        public void Update(MacroDto macro)
        {
            new Updator(this.Session).Update(macro);
        }

        /// <summary>
        /// Commits the changes on medical record cabinet.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="cabinet">The cabinet.</param>
        public void Update(MedicalRecordCabinetDto cabinet)
        {
            new Updator(this.Session).Update(cabinet);
        }

        /// <summary>
        /// Updates the specified macros.
        /// </summary>
        /// <param name="macros">The macros.</param>
        public void Update(IEnumerable<MacroDto> macros)
        {
            var updator = new Updator(this.Session);

            foreach (var macro in macros)
            {
                updator.Update(macro);
            }
        }

        #endregion Methods
    }
}