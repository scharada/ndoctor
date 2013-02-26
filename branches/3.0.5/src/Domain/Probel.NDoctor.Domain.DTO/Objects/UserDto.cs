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
namespace Probel.NDoctor.Domain.DTO.Objects
{
    using System;
    using System.Drawing;

    [Serializable]
    public class UserDto : LightUserDto
    {
        #region Fields

        private decimal fee;
        private string header;
        private PracticeDto practice;
        private string proMail;
        private string proMobile;
        private string proPhone;
        private Image thumbnailImage;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDto"/> class.
        /// </summary>
        /// <param name="isSuperAdmin">if set to <c>true</c> [is super admin].</param>
        public UserDto(bool isSuperAdmin)
            : base(isSuperAdmin)
        {
            this.IsSuperAdmin = isSuperAdmin;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDto"/> class.
        /// </summary>
        public UserDto()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the fee the user ask.
        /// </summary>
        /// <value>
        /// The fee.
        /// </value>
        public decimal Fee
        {
            get { return this.fee; }
            set
            {
                this.fee = value;
                this.OnPropertyChanged(() => Fee);
            }
        }

        /// <summary>
        /// Gets or sets the header that will be displayed in the prescriptions or
        /// other places where a header is needed.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public string Header
        {
            get { return this.header; }
            set
            {
                this.header = value;
                this.OnPropertyChanged(() => Header);
            }
        }

        /// <summary>
        /// Gets or sets the practice whereon the current user work.
        /// </summary>
        /// <value>
        /// The practice.
        /// </value>
        public PracticeDto Practice
        {
            get { return this.practice; }
            set
            {
                this.practice = value;
                this.OnPropertyChanged(() => Practice);
            }
        }

        /// <summary>
        /// Gets or sets the mail pro.
        /// </summary>
        /// <value>
        /// The mail pro.
        /// </value>
        public string ProMail
        {
            get { return this.proMail; }
            set
            {
                this.proMail = value;
                this.OnPropertyChanged(() => ProMail);
            }
        }

        /// <summary>
        /// Gets or sets the mobile pro.
        /// </summary>
        /// <value>
        /// The mobile pro.
        /// </value>
        public string ProMobile
        {
            get { return this.proMobile; }
            set
            {
                this.proMobile = value;
                this.OnPropertyChanged(() => ProMobile);
            }
        }

        /// <summary>
        /// Gets or sets the phone pro.
        /// </summary>
        /// <value>
        /// The phone pro.
        /// </value>
        public string ProPhone
        {
            get { return this.proPhone; }
            set
            {
                this.proPhone = value;
                this.OnPropertyChanged(() => ProPhone);
            }
        }

        /// <summary>
        /// Gets or sets the thumbnail image.
        /// </summary>
        /// <value>
        /// The thumbnail image.
        /// </value>
        public Image ThumbnailImage
        {
            get { return this.thumbnailImage; }
            set
            {
                this.thumbnailImage = value;
                this.OnPropertyChanged(() => ThumbnailImage);
            }
        }

        #endregion Properties
    }
}