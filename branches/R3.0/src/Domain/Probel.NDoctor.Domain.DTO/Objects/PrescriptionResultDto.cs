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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Contains a list of prescription and the first and last creation date
    /// </summary>
    public class PrescriptionResultDto : BaseDto
    {
        #region Fields

        private DateTime from;
        private DateTime to;

        #endregion Fields

        #region Constructors

        public PrescriptionResultDto(IEnumerable<PrescriptionDocumentDto> documents, DateTime from, DateTime to)
        {
            this.Prescriptions = new ObservableCollection<PrescriptionDocumentDto>(documents);
            this.From = from;
            this.To = to;
        }

        #endregion Constructors

        #region Properties

        public DateTime From
        {
            get { return this.from; }
            set
            {
                this.from = value;
                this.OnPropertyChanged("From");
            }
        }

        public ObservableCollection<PrescriptionDocumentDto> Prescriptions
        {
            get;
            private set;
        }

        public DateTime To
        {
            get { return this.to; }
            set
            {
                this.to = value;
                this.OnPropertyChanged("To");
            }
        }

        #endregion Properties
    }
}