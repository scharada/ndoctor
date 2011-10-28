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

namespace Probel.NDoctor.Domain.DTO.Objects
{
    using System;

    public class AppointmentDto : BaseDto
    {
        #region Fields

        private DateTime endTime;
        private string notes;
        private DateTime startTime;
        private string subject;
        private TagDto tag;
        private LightUserDto user;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the end date of the meeting.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndTime
        {
            get { return this.endTime; }
            set
            {
                this.endTime = value;
                this.OnPropertyChanged("EndTime");
            }
        }

        /// <summary>
        /// Gets or sets the notes about the meeting.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes
        {
            get { return this.notes; }
            set
            {
                this.notes = value;
                this.OnPropertyChanged("Notes");
            }
        }

        /// <summary>
        /// Gets or sets the start date of the meeting.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartTime
        {
            get { return this.startTime; }
            set
            {
                this.startTime = value;
                this.OnPropertyChanged("StartTime");
            }
        }

        /// <summary>
        /// Gets or sets the subject of the meeting.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject
        {
            get { return this.subject; }
            set
            {
                this.subject = value;
                this.OnPropertyChanged("Subject");
            }
        }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public TagDto Tag
        {
            get { return this.tag; }
            set
            {
                this.tag = value;
                this.OnPropertyChanged("Tag");
            }
        }

        /// <summary>
        /// Gets or sets the user linked to this meeting.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public LightUserDto User
        {
            get { return this.user; }
            set
            {
                this.user = value;
                this.OnPropertyChanged("User");
            }
        }

        #endregion Properties
    }
}