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
    using System.Collections.Generic;

    /// <summary>
    /// Represents tags that are used to make arbitraty search on patients
    /// </summary>
    public class SearchTag : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the notes about the search tag.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public virtual string Explanations
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the search tag.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the patients binded to this tag.
        /// </summary>
        /// <value>
        /// The patients.
        /// </value>
        public virtual IList<Patient> Patients
        {
            get;
            set;
        }

        #endregion Properties
    }
}