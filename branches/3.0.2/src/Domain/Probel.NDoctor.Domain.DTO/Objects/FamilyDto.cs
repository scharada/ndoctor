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
    using System.Collections.ObjectModel;

    using Probel.Mvvm;
    using Probel.NDoctor.Domain.DTO.Validators;

    /// <summary>
    /// Represents the family of a patient
    /// </summary>
    [Serializable]
    public class FamilyDto : BaseDto
    {
        #region Fields

        private LightPatientDto current;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyDto"/> class.
        /// </summary>
        public FamilyDto()
            : base(new FamilyValidator())
        {
            this.Fathers = new ObservableCollection<LightPatientDto>();
            this.Mothers = new ObservableCollection<LightPatientDto>();
            this.Children = new ObservableCollection<LightPatientDto>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyDto"/> class.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="father">The father.</param>
        /// <param name="mother">The mother.</param>
        /// <param name="children">The children.</param>
        public FamilyDto(LightPatientDto current, LightPatientDto father, LightPatientDto mother, LightPatientDto[] children)
        {
            this.Current = current;
            this.Fathers = new ObservableCollection<LightPatientDto>(new LightPatientDto[] { father });
            this.Mothers = new ObservableCollection<LightPatientDto>(new LightPatientDto[] { mother });
            this.Children = new ObservableCollection<LightPatientDto>(children);
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<LightPatientDto> Children
        {
            get;
            private set;
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
                this.OnPropertyChanged(() => this.Current);
            }
        }

        /// <summary>
        /// Gets or sets the father of the current patient
        /// </summary>
        /// <value>The father</value>
        public ObservableCollection<LightPatientDto> Fathers
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the mother of the current patient
        /// </summary>
        /// <value>The mother</value>
        public ObservableCollection<LightPatientDto> Mothers
        {
            get;
            private set;
        }

        #endregion Properties
    }
}