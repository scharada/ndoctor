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
namespace Probel.Helpers.WPF.Calendar.Model
{
    using System;

    using Probel.Helpers.Data;

    public class Appointment : BindableObject
    {
        #region Fields

        private string body;
        private DateTime endTime;
        private string location;
        private DateTime startTime;
        private string subject;

        #endregion Fields

        #region Properties

        public string Body
        {
            get { return body; }
            set
            {
                if (body != value)
                {
                    body = value;
                    RaisePropertyChanged("Body");
                }
            }
        }

        public DateRange DateRange
        {
            get;
            private set;
        }

        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    this.SetDateRange();
                    RaisePropertyChanged("EndTime");
                }
            }
        }

        /// <summary>
        /// Gets the indentation of the current appointement when it has overlapping other appointments.
        /// </summary>
        public uint Indentation
        {
            get;
            internal set;
        }

        public string Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                    RaisePropertyChanged("Location");
                }
            }
        }

        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    this.SetDateRange();
                    RaisePropertyChanged("StartTime");
                }
            }
        }

        public string Subject
        {
            get { return subject; }
            set
            {
                if (subject != value)
                {
                    subject = value;
                    RaisePropertyChanged("Subject");
                }
            }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return this.Subject;
        }

        private void SetDateRange()
        {
            if (StartTime != null && endTime != null
                && StartTime > DateTime.MinValue && EndTime > DateTime.MinValue)
            {
                this.DateRange = new DateRange(StartTime, endTime);
            }
        }

        #endregion Methods
    }
}