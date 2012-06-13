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

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

    public class PrescriptionComponent : BaseComponent, IPrescriptionComponent
    {
        #region Methods

        /// <summary>
        /// Creates the specified prescription document for the specified patient.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="patient">The patient.</param>
        public void Create(PrescriptionDocumentDto document, LightPatientDto patient)
        {
            var entity = this.Session.Get<Patient>(patient.Id);
            var prescriptionEntity = Mapper.Map<PrescriptionDocumentDto, PrescriptionDocument>(document);

            entity.PrescriptionDocuments.Add(prescriptionEntity);
            this.Session.SaveOrUpdate(entity);
        }

        /// <summary>
        /// Finds the prescriptions between the specified dates for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>A list of prescriptions</returns>
        public IList<PrescriptionDocumentDto> FindPrescriptionsByDates(LightPatientDto patient, DateTime start, DateTime end)
        {
            var entity = this.Session.Get<Patient>(patient.Id);
            var prescriptions = (from p in entity.PrescriptionDocuments
                                 where p.CreationDate >= start
                                    && p.CreationDate <= end
                                 select p).ToList();

            return Mapper.Map<IList<PrescriptionDocument>, IList<PrescriptionDocumentDto>>(prescriptions);
        }

        /// <summary>
        /// Removes the specified item but doesn't touch the drugs liked to it.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(PrescriptionDocumentDto item)
        {
            foreach (var prescription in item.Prescriptions)
            {
                this.Remove<Prescription>(item);
            }

            this.Remove<PrescriptionDocument>(item);
        }

        /// <summary>
        /// Removes the specified prescription.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(PrescriptionDto item)
        {
            this.Remove<Prescription>(item);
        }

        #endregion Methods


        /// <summary>
        /// Updates the specified prescription.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Update(PrescriptionDto item)
        {
            var entity = this.Session.Get<Prescription>(item.Id);
            Mapper.Map<PrescriptionDto, Prescription>(item, entity);

            this.Session.Update(entity);
        }
    }
}