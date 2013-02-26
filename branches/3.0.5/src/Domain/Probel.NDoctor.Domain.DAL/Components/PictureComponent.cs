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
    using Probel.Helpers.Benchmarking;
    using Probel.NDoctor.Domain.DAL.AopConfiguration;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Helpers;
    using Probel.NDoctor.Domain.DAL.Subcomponents;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
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
            new Creator(this.Session).Create(picture, forPatient);
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
        /// Check the database state and creates the thumbnails if needed
        /// </summary>
        /// <param name="pictures">The pictures.</param>
        [NotBenchmarked]
        [ExcludeFromTransaction]
        [Granted(To.Everyone)]
        public void CreateAllThumbnails()
        {
            //var db = this.GetDatabaseState();
            var settings = new DbSettingsComponent(this.Session);
            var areThumbnailsCreated = (settings.Exists("AreThumbnailsCreated"))
                ? bool.Parse(settings["AreThumbnailsCreated"])
                : false;

            if (!areThumbnailsCreated)
            {
                settings["AreThumbnailsCreated"] = false.ToString();
                this.Logger.Info("Creation of the thumbnails");
                using (var bench = new Benchmark(e => this.Logger.InfoFormat("Thumbnails created in {0,3}.{1:###} sec", e.Seconds, e.Milliseconds)))
                {
                    var pictures = (from p in this.Session.Query<Picture>()
                                    select p).ToList();
                    new ImageHelper().TryCreateThumbnail(pictures);
                    using (var tx = this.Session.BeginTransaction())
                    {
                        new Updator(this.Session).Update(pictures);
                        bench.CheckNow(e => this.Logger.DebugFormat("Thumbnails created in memory in {0,3}.{1:###} sec", e.Seconds, e.Milliseconds));
                        areThumbnailsCreated = true;
                        settings["AreThumbnailsCreated"] = areThumbnailsCreated.ToString();
                        tx.Commit();
                        bench.CheckNow(e => this.Logger.DebugFormat("Transaction commited {0,3}.{1:###} sec", e.Seconds, e.Milliseconds));
                    }
                }
            }
            else { this.Logger.Debug("Thumbnails creation already executed. Action aborded."); }
        }

        /// <summary>
        /// Gets the pictures (only the thumbnails) for the specified patient and with the specified tag.
        /// If the specified tag is null, it'll select all the picture of the specified
        /// patient
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="tag">The criteria of the search. If null, it'll take all the picture for the specified patient</param>
        /// <returns>
        /// A list of pictures
        /// </returns>
        [BenchmarkThreshold(1500, "The SQL query execution time for 169 pictures is about 1,5 sec")]
        public IList<LightPictureDto> GetLightPictures(LightPatientDto patient, TagDto tag)
        {
            var pictures = GetEntityPictures(patient, tag);
            var result = Mapper.Map<IList<Picture>, IList<LightPictureDto>>(pictures);
            return result;
        }

        /// <summary>
        /// Gets the pictures (only the thumbnails) for the specified patient and with the specified tag.
        /// If the specified tag is null, it'll select all the picture of the specified
        /// patient
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>
        /// A list of pictures
        /// </returns>
        [BenchmarkThreshold(1500, "The SQL query execution time for 169 pictures is about 1,5 sec")]
        public IList<LightPictureDto> GetLightPictures(LightPatientDto patient)
        {
            var pictures = GetEntityPictures(patient, null);
            var result = Mapper.Map<IList<Picture>, IList<LightPictureDto>>(pictures);
            return result;
        }

        /// <summary>
        /// Gets the full picture from the thumbnail.
        /// </summary>
        /// <param name="picture">The picture.</param>
        /// <returns></returns>
        public PictureDto GetPicture(LightPictureDto picture)
        {
            Assert.IsNotNull(picture, "picture");
            try
            {
                var result = (from p in this.Session.Query<Picture>()
                              where p.Id == picture.Id
                              select p).First();
                return Mapper.Map<Picture, PictureDto>(result);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(Picture), ex);
            }
        }

        /// <summary>
        /// Gets the pictures for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>
        /// A list of picture
        /// </returns>
        public IList<PictureDto> GetPictures(LightPatientDto patient)
        {
            return this.GetPictures(patient, null);
        }

        /// <summary>
        /// Gets the pictures for the specified patient and with the specified tag.
        /// If the specified tag is null, it'll select all the picture of the specified
        /// patient
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="tag">The criteria of the search. If null, it'll take all the picture for the specified patient</param>
        /// <returns>
        /// A list of pictures
        /// </returns>
        public IList<PictureDto> GetPictures(LightPatientDto patient, TagDto tag)
        {
            IList<Picture> pictures = this.GetEntityPictures(patient, tag);
            return Mapper.Map<IList<Picture>, IList<PictureDto>>(pictures);
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
        /// Updates the specified picture.
        /// </summary>
        /// <param name="picture">The picture.</param>
        public void Update(PictureDto item)
        {
            var entity = this.Session.Get<Picture>(item.Id);
            if (entity == null) throw new EntityNotFoundException(typeof(Picture));

            Mapper.Map<PictureDto, Picture>(item, entity);

            entity.Tag = this.Session.Get<Tag>(item.Tag.Id);

            this.Session.Merge(entity);
        }

        private IList<Picture> GetEntityPictures(LightPatientDto patient, TagDto tag)
        {
            IList<Picture> pictures = new List<Picture>();

            // If there's a null criterium, select all the pictures;
            // otherwise, execute a search
            if (tag == null)
            {
                pictures = (from p in this.Session.Query<Picture>()
                            where p.Patient.Id == patient.Id
                            select p).ToList();
            }
            else
            {
                pictures = (from p in this.Session.Query<Picture>()
                            where p.Patient.Id == patient.Id
                            && p.Tag.Id == tag.Id
                            select p).ToList();
            }

            return pictures;
        }

        #endregion Methods
    }
}