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
namespace Probel.NDoctor.Domain.DAL.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A prescription is a drug and notes about how to take it.
    /// </summary>
    public class PrescriptionDocument : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the creation date of this prescripiton.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public virtual DateTime CreationDate
        {
            get;
            set;
        }

        public virtual IList<Prescription> Prescriptions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public virtual Tag Tag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a facultative title for this prescription document.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Title
        {
            get;
            set;
        }

        #endregion Properties
    }
}