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
namespace Probel.NDoctor.Plugins.UserSession.ViewModel
{
    using AutoMapper;

    using Probel.NDoctor.Domain.DTO.Objects;

    public class BusinessCardViewModel : UserDto
    {
        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="BusinessCardViewModel"/> class from being created.
        /// </summary>
        private BusinessCardViewModel()
        {
        }

        #endregion Constructors

        #region Properties

        public string AddressLine1
        {
            get
            {
                return string.Format("{0}, {1}"
                    , this.Practice.Address.Street
                    , this.Practice.Address.StreetNumber);
            }
        }

        public string AddressLine2
        {
            get
            {
                return string.Format("{0} {1}"
                    , this.Practice.Address.PostalCode
                    , this.Practice.Address.City);
            }
        }

        #endregion Properties

        #region Methods

        public static BusinessCardViewModel CreateFrom(UserDto user)
        {
            return Mapper.Map<UserDto, BusinessCardViewModel>(user);
        }

        #endregion Methods
    }
}