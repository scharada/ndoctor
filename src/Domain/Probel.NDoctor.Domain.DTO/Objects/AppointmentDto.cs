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

    using Probel.NDoctor.Domain.DTO.Properties;
    using Probel.NDoctor.Domain.DTO.Validators;

    [Serializable]
    public class AppointmentDto : BaseDto
    {
        #region Fields

        private DateTime endTime;
        private string notes;
        private DateTime startTime;
        private string subject;
        private TagDto tag;
        private SecurityUserDto user;

        #endregion Fields

        #region Constructors

        public AppointmentDto()
            : base(new AppointmentValidator())
        {
        }

        #endregion Constructors

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
                this.OnPropertyChanged(() => this.EndTime);
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
                this.OnPropertyChanged(() => this.Notes);
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
                this.OnPropertyChanged(() => this.StartTime);
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
                this.OnPropertyChanged(() => this.Subject);
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
                this.OnPropertyChanged(() => this.Tag);
            }
        }

        public string TimeToDisplay
        {
            get
            {
                var dateFrom = this.StartTime.ToShortDateString();
                var dateTo = (this.StartTime.Date == this.EndTime.Date)
                    ? string.Empty
                    : this.EndTime.ToShortDateString();

                var timeFrom = this.StartTime.ToString("HH:mm");
                var timeTo = this.EndTime.ToString("HH:mm");

                return string.Format(Messages.Title_FromTo, dateFrom, timeFrom, dateTo, timeTo);
            }
        }

        /// <summary>
        /// Gets or sets the user linked to this meeting.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public SecurityUserDto User
        {
            get { return this.user; }
            set
            {
                this.user = value;
                this.OnPropertyChanged(() => this.User);
            }
        }

        #endregion Properties
    }
}