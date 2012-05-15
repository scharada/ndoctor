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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class PatientBmiDto : LightPatientDto
    {
        #region Constructors

        public PatientBmiDto()
        {
            this.BmiHistory = new ObservableCollection<BmiDto>();
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<BmiDto> BmiHistory
        {
            get;
            private set;
        }

        public IList<BmiDto> Obesity
        {
            get
            {
                return this.CreateList(30F);
            }
        }

        public IList<BmiDto> Overweight
        {
            get
            {
                return this.CreateList(25);
            }
        }

        public IList<BmiDto> Underweight
        {
            get
            {
                return this.CreateList(18.5F);
            }
        }

        #endregion Properties

        #region Methods

        private IList<BmiDto> CreateList(float index)
        {
            var list = new List<BmiDto>();
            if (this.BmiHistory.Count > 0)
            {
                var start = this.BmiHistory.Max(e => e.Date);
                var end = this.BmiHistory.Min(e => e.Date);

                list.Add(new BmiDto() { Date = start, Height = 100, Weight = index });
                list.Add(new BmiDto() { Date = end, Height = 100, Weight = index });
            }
            return list;
        }

        #endregion Methods
    }
}