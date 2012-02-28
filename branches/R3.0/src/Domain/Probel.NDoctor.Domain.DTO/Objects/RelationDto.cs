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

    using Probel.Helpers.Assertion;

    /// <summary>
    /// Represents a relation with a patient. It's part of a family
    /// The read direction is right to left therefore the patient
    /// is on the left side (which is the property LeftSide of
    /// the dto <see cref="FamilyDto"/>)
    /// </summary>
    [Serializable]
    public class RelationDto : BaseDto
    {
        #region Fields

        private LightPatientDto leftSide;
        private LightPatientDto rightSide;
        private RelationTypeDto type;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the other side of the relation.
        /// </summary>
        /// <value>
        /// The other side.
        /// </value>
        public LightPatientDto LeftSide
        {
            get { return this.leftSide; }
            set
            {
                this.leftSide = value;
                this.OnPropertyChanged("LeftSide");
            }
        }

        /// <summary>
        /// Gets or sets the name of the relation.
        /// </summary>
        /// <value>
        /// The name of the relation.
        /// </value>
        public string Name
        {
            get
            {
                Assert.IsNotNull(this.type, "The type is null");
                return this.type.RightSideName;
            }
        }

        /// <summary>
        /// Gets or sets the right side of the relation.
        /// </summary>
        /// <value>
        /// The other side.
        /// </value>
        public LightPatientDto RightSide
        {
            get { return this.rightSide; }
            set
            {
                this.rightSide = value;
                this.OnPropertyChanged("RightSide");
            }
        }

        /// <summary>
        /// Gets or sets the type of the relation.
        /// </summary>
        /// <value>
        /// The type of the relation.
        /// </value>
        public RelationTypeDto Type
        {
            get { return this.type; }
            set
            {
                this.type = value;
                this.OnPropertyChanged("Type");
            }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return string.Format("Relation: {0} [Name: {1}] - RightSide: {2} {3}"
                , this.Type
                , this.Name
                , this.RightSide.FirstName
                , this.RightSide.LastName);
        }

        #endregion Methods
    }
}