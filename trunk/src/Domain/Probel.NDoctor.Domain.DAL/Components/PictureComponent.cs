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
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Exceptions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;

    public class PictureComponent : BaseComponent, IPictureComponent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureComponent"/> class.
        /// </summary>
        public PictureComponent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientSessionComponent"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public PictureComponent(ISession session)
            : base(session)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates the specified picture for the specified patient.
        /// </summary>
        /// <param name="picture">The picture.</param>
        /// <param name="patient">The patient.</param>
        public void Create(PictureDto picture, LightPatientDto forPatient)
        {
            Assert.IsNotNull(picture, "The item to create shouldn't be null");
            this.CheckSession();

            var foundPatient = (from p in this.Session.Query<Patient>()
                                where p.Id == forPatient.Id
                                select p).FirstOrDefault();
            if (foundPatient == null) throw new ExistingItemException();

            var newItem = Mapper.Map<PictureDto, Picture>(picture);

            foundPatient.Pictures.Add(newItem);

            this.Session.SaveOrUpdate(foundPatient);
        }

        /// <summary>
        /// Gets the pictures for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>
        /// A list of picture
        /// </returns>
        public IList<PictureDto> FindPictures(LightPatientDto patient)
        {
            return this.FindPictures(patient, null);
        }

        /// <summary>
        /// Gets the pictures for the specified patient and with the specified tag.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="tag">The criteria of the search. If null, it'll take all the picture for the specified patient</param>
        /// <returns>
        /// A list of pictures
        /// </returns>
        public IList<PictureDto> FindPictures(LightPatientDto patient, TagDto tag)
        {
            this.CheckSession();
            var entity = (from p in this.Session.Query<Patient>()
                          where p.Id == patient.Id
                          select p).FirstOrDefault();

            if (entity == null) return new List<PictureDto>();

            IList<Picture> pictures = new List<Picture>();

            // If there's a non null criterium execute a search;
            // otherwise, select all the pictures
            if (tag == null) pictures = entity.Pictures;
            else
            {
                pictures = (from p in entity.Pictures
                            where p.Tag.Id == tag.Id
                            select p).ToList();
            }

            return Mapper.Map<IList<Picture>, IList<PictureDto>>(pictures);
        }

        #endregion Methods
    }
}