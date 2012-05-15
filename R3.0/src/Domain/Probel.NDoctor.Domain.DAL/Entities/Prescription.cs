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
    /// <summary>
    /// A prescription is a drug with notes. That's some drug a patient has to take
    /// with notes about how to take it.
    /// </summary>
    public class Prescription : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the drug involved in this prescription.
        /// </summary>
        /// <value>
        /// The drug.
        /// </value>
        public virtual Drug Drug
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes are expected to be something like: "Two pills in the morning" or "One drop in a glass
        /// of water before going to sleep".
        /// </value>
        public virtual string Notes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tag for this prescription.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public virtual Tag Tag
        {
            get;
            set;
        }

        #endregion Properties
    }
}