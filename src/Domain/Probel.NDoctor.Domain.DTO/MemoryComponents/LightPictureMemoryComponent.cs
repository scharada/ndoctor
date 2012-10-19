#region Header

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

#endregion Header

namespace Probel.NDoctor.Domain.DTO.MemoryComponents
{
    using System.Collections.Generic;
    using System.Linq;

    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Provides in memory selection methods for a list of <see cref="LightPictureDto"/>
    /// </summary>
    public class LightPictureMemoryComponent
    {
        #region Fields

        private IList<LightPictureDto> pictures;

        #endregion Fields

        #region Constructors

        public LightPictureMemoryComponent(IList<LightPictureDto> pictures)
        {
            this.pictures = pictures;
        }

        #endregion Constructors

        #region Properties

        public static LightPictureMemoryComponent Empty
        {
            get
            {
                return new LightPictureMemoryComponent(new List<LightPictureDto>());
            }
        }

        #endregion Properties

        #region Methods

        public LightPictureDto[] Get(TagDto criteria)
        {
            return (from picture in this.pictures
                    where picture.Tag.Id == criteria.Id
                    select picture).ToArray();
        }

        public LightPictureDto[] GetAll()
        {
            return this.pictures.ToArray();
        }

        #endregion Methods
    }
}