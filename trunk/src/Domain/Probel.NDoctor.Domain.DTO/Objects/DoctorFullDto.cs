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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Probel.Mvvm;
    using Probel.NDoctor.Domain.DTO.Validators;

    [Serializable]
    public class DoctorFullDto : DoctorDto
    {
        #region Fields

        /// <summary>
        /// Gets or sets the value counting how many times this patient was used.
        /// </summary>
        /// <value>The counter.</value>
        private long counter;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is complete. That's if the person was quickly
        /// inserted into the repository, he/she does not contains all the pieces of information and therefore
        /// need to be completed later.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is complete; otherwise, <c>false</c>.
        /// </value>
        private bool isComplete;

        /// <summary>
        /// Gets or sets the last update.
        /// </summary>
        /// <value>The last update.</value>
        private DateTime lastUpdate;

        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>The thumbnail.</value>
        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>The thumbnail.</value>
        private byte[] thumbnail;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorFullDto"/> class.
        /// </summary>
        public DoctorFullDto()
        {
            this.Address = new AddressDto();
        }

        #endregion Constructors

        #region Properties

        public long Counter
        {
            get { return this.counter; }
            set
            {
                this.counter = value;
                this.OnPropertyChanged(() => this.Counter);
            }
        }

        public bool IsComplete
        {
            get { return this.isComplete; }
            set
            {
                this.isComplete = value;
                this.OnPropertyChanged(() => this.IsComplete);
            }
        }

        public DateTime LastUpdate
        {
            get { return this.lastUpdate; }
            set
            {
                this.lastUpdate = value;
                this.OnPropertyChanged(() => this.LastUpdate);
            }
        }

        public byte[] Thumbnail
        {
            get { return this.thumbnail; }
            set
            {
                this.thumbnail = value;
                this.OnPropertyChanged(() => this.Thumbnail);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Person: {0} {1}", this.FirstName, this.LastName);
        }

        #endregion Methods
    }
}