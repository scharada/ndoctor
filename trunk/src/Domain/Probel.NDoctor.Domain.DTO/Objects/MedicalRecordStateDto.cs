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


    public class MedicalRecordStateDto : BaseDto
    {
        #region Fields

        private DateTime lastUpdate;
        private string rtf;
        private TagDto tag;

        #endregion Fields

        #region Properties

        public DateTime LastUpdate
        {
            get { return this.lastUpdate; }
            set
            {
                this.lastUpdate = value;
                this.OnPropertyChanged(() => LastUpdate);
            }
        }

        public string Rtf
        {
            get { return this.rtf; }
            set
            {
                this.rtf = value;
                this.OnPropertyChanged(() => Rtf);
            }
        }

        public TagDto Tag
        {
            get { return this.tag; }
            set
            {
                this.tag = value;
                this.OnPropertyChanged(() => Tag);
            }
        }

        #endregion Properties
    }
}