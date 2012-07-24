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

namespace Probel.NDoctor.Domain.DAL.Entities
{
    using System;

    public class MedicalRecordState : Entity
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicalRecord"/> class.
        /// Set the creation date to now
        /// </summary>
        public MedicalRecordState(DateTime creationDate)
        {
            this.CreationDate = creationDate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicalRecordState"/> class.
        /// </summary>
        public MedicalRecordState()
        {
            this.CreationDate = DateTime.Now;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the date of creation creation.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public virtual DateTime CreationDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date of the last update.
        /// </summary>
        /// <value>
        /// The last update.
        /// </value>
        public virtual DateTime LastUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title of the medical record.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HTML representing the medical record.
        /// </summary>
        /// <value>
        /// The HTML.
        /// </value>
        public virtual string Rtf
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tag to group the medical record.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public virtual Tag Tag
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("{0} Rtf: {1}", base.ToString(), this.Rtf);
        }

        #endregion Properties
    }
}