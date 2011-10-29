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
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the family of a patient
    /// </summary>
    public class FamilyDto : BaseDto
    {
        #region Fields

        private LightPatientDto current;
        private LightPatientDto father;
        private LightPatientDto mother;

        #endregion Fields

        #region Constructors

        public FamilyDto()
        {
            this.Children = new ObservableCollection<LightPatientDto>();
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<LightPatientDto> Children
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current patient
        /// </summary>
        /// <value>The current patient</value>
        public LightPatientDto Current
        {
            get { return this.current; }
            set
            {
                this.current = value;
                this.OnPropertyChanged("Current");
            }
        }

        /// <summary>
        /// Gets or sets the father of the current patient
        /// </summary>
        /// <value>The father</value>
        public LightPatientDto Father
        {
            get { return this.father; }
            set
            {
                this.father = value;
                this.OnPropertyChanged("Father");
            }
        }

        /// <summary>
        /// Gets or sets the mother of the current patient
        /// </summary>
        /// <value>The mother</value>
        public LightPatientDto Mother
        {
            get { return this.mother; }
            set
            {
                this.mother = value;
                this.OnPropertyChanged("Mother");
            }
        }

        #endregion Properties
    }
}