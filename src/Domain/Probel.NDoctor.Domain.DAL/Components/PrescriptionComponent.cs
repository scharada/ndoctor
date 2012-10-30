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
    using Probel.NDoctor.Domain.DAL.Subcomponents;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using NHibernate;

    public class PrescriptionComponent : BaseComponent, IPrescriptionComponent
    {
        public PrescriptionComponent()
            : base()
        {

        }
        public PrescriptionComponent(ISession session)
            : base(session)
        {

        }
        #region Methods

        /// <summary>
        /// Creates the specified prescription document for the specified patient.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="patient">The patient.</param>
        public void Create(PrescriptionDocumentDto document, LightPatientDto patient)
        {
            new Creator(this.Session).Create(document, patient);
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
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        /// <returns></returns>
        public long Create(DrugDto item)
        {
            return new Creator(this.Session).Create(item);
        }

        /// <summary>
        /// Gets the drugs which has in their name the specified criteria.
        /// </summary>
        /// <param name="name">The criteria.</param>
        /// <returns>
        /// A list of drugs
        /// </returns>
        public IList<DrugDto> GetDrugsByName(string name)
        {
            return new Selector(this.Session).GetDrugsByName(name);
        }

        /// <summary>
        /// Gets the drugs by tags.
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns>
        /// A list of drugs
        /// </returns>
        public IList<DrugDto> GetDrugsByTags(string tagName)
        {
            return new Selector(this.Session).GetDrugsByTags(tagName);
        }

        /// <summary>
        /// Gets the prescriptions between the specified dates for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>A list of prescriptions</returns>
        public IList<PrescriptionDocumentDto> GetPrescriptionsByDates(LightPatientDto patient, DateTime start, DateTime end)
        {
            var entity = this.Session.Get<Patient>(patient.Id);
            var prescriptions = (from p in entity.PrescriptionDocuments
                                 where p.CreationDate >= start
                                    && p.CreationDate <= end
                                 select p).ToList();

            return Mapper.Map<IList<PrescriptionDocument>, IList<PrescriptionDocumentDto>>(prescriptions);
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
        /// Removes the specified item but doesn't touch the drugs liked to it.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(PrescriptionDocumentDto item)
        {
            new Remover(this.Session).Remove(item);
        }

        /// <summary>
        /// Removes the specified prescription.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(PrescriptionDto item)
        {
            new Remover(this.Session).Remove(item);
        }

        /// <summary>
        /// Updates the specified prescription.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Update(PrescriptionDto item)
        {
            new Updator(this.Session).Update(item);
        }

        #endregion Methods
    }
}