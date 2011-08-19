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
namespace Probel.NDoctor.Domain.BLL
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Represents a generic person in the domain
    /// </summary>
    public class Person
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        public Person()
        {
            this.LastUpdate = DateTime.Now;
            this.Counter = 0;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public virtual Address Address
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value counting how many times this patient was used.
        /// </summary>
        /// <value>The counter.</value>
        public virtual long Counter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public virtual string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sex.
        /// </summary>
        /// <value>The sex.</value>
        public virtual Gender Gender
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a unique id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public virtual int Id
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is complete. That's if the person was quickly
        /// inserted into the repository, he/she does not contains all the pieces of information and therefore
        /// need to be completed later.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is complete; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsComplete
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public virtual string LastName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last update.
        /// </summary>
        /// <value>The last update.</value>
        public virtual DateTime LastUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mail.
        /// </summary>
        /// <value>The mail.</value>
        public virtual string Mail
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mobile pro.
        /// </summary>
        /// <value>The mobile pro.</value>
        public virtual string MobilePro
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the phone pro.
        /// </summary>
        /// <value>The phone pro.</value>
        public virtual string PhonePro
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>The thumbnail.</value>
        public virtual byte[] Thumbnail
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <value>The image.</value>
        public virtual Image ThumbnailImage
        {
            get;
            set;
        }

        #endregion Properties
    }
}