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

    using Probel.Helpers.Data;

    /// <summary>
    /// Represents a meeting for a patient and linked to a user
    /// </summary>
    public class Appointment : Entity
    {
        #region Properties

        /// <summary>
        /// Gets the date range for this appointment.
        /// </summary>
        public virtual DateRange DateRange
        {
            get
            {
                return new DateRange(this.StartTime, this.EndTime);
            }
        }

        /// <summary>
        /// Gets or sets the end date of the meeting.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public virtual DateTime EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the notes about the meeting.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public virtual string Notes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the start date of the meeting.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public virtual DateTime StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the subject of the meeting.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public virtual string Subject
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
        /// Gets or sets the user linked to this meeting.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public virtual User User
        {
            get;
            set;
        }

        #endregion Properties
    }
}