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

    [Serializable]
    public class MedicalRecordFolderDto : BaseDto
    {
        #region Fields

        private string name;
        private string notes;
        private MedicalRecordDto[] records;

        #endregion Fields

        #region Constructors

        public MedicalRecordFolderDto()
        {
            this.Records = new MedicalRecordDto[] { };
        }

        #endregion Constructors

        #region Properties

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged("Name");
            }
        }

        public string Notes
        {
            get { return this.notes; }
            set
            {
                this.notes = value;
                this.OnPropertyChanged("Notes");
            }
        }

        public MedicalRecordDto[] Records
        {
            get { return this.records; }
            set
            {
                this.records = value;
                this.OnPropertyChanged("Records");
            }
        }

        #endregion Properties
    }
}