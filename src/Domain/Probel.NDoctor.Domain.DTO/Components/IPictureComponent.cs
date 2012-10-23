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
namespace Probel.NDoctor.Domain.DTO.Components
{
    using System.Collections.Generic;

    using Probel.NDoctor.Domain.DTO.Objects;

    public interface IPictureComponent : IBaseComponent
    {
        #region Methods

        /// <summary>
        /// Creates the specified picture for the specified patient.
        /// </summary>
        /// <param name="picture">The picture.</param>
        /// <param name="patient">The patient.</param>
        void Create(PictureDto picture, LightPatientDto forPatient);

        /// <summary>
        /// Create the specified item into the database
        /// </summary>
        /// <param name="item">The item to add in the database</param>
        /// <returns>The id of the just created item</returns>
        long Create(TagDto item);

        /// <summary>
        /// Check the database state and creates the thumbnails if needed
        /// </summary>
        /// <param name="pictures">The pictures.</param>
        void CreateAllThumbnails();

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
        IList<LightPictureDto> GetLightPictures(LightPatientDto patient, TagDto tag);

        /// <summary>
        /// Gets the pictures (only the thumbnails) for the specified patient and with the specified tag.
        /// If the specified tag is null, it'll select all the picture of the specified
        /// patient
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>
        /// A list of pictures
        /// </returns>
        IList<LightPictureDto> GetLightPictures(LightPatientDto patient);

        /// <summary>
        /// Gets the full picture from the thumbnail.
        /// </summary>
        /// <param name="picture">The picture.</param>
        /// <returns></returns>
        PictureDto GetPicture(LightPictureDto picture);

        /// <summary>
        /// Gets the pictures for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>A list of picture</returns>
        IList<PictureDto> GetPictures(LightPatientDto patient);

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
        IList<PictureDto> GetPictures(LightPatientDto patient, TagDto tag);

        /// <summary>
        /// Gets all the tags with the specified catagory.
        /// </summary>
        /// <returns></returns>
        IList<TagDto> GetTags(TagCategory category);

        /// <summary>
        /// Updates the specified picture.
        /// </summary>
        /// <param name="picture">The picture.</param>
        void Update(PictureDto item);

        #endregion Methods
    }
}