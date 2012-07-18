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

    [Serializable]
    public class RelationTypeDto : BaseDto
    {
        #region Fields

        private string leftSideName;

        /// <summary>
        /// Represent a relation in the genealogic tree. For instance the Father-Son is a relation type
        /// where the right side is father/mother and the left side if son/daughter
        /// </summary>
        private string rightSideName;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the left side of the relation.
        /// </summary>
        /// <value>
        /// The left side.
        /// </value>
        public string LeftSideName
        {
            get { return this.leftSideName; }
            set
            {
                this.leftSideName = value;
                this.OnPropertyChanged(() => LeftSideName);
            }
        }

        /// <summary>
        /// Gets or sets the right side of the relation.
        /// </summary>
        /// <value>
        /// The right side.
        /// </value>
        public string RightSideName
        {
            get { return this.rightSideName; }
            set
            {
                this.rightSideName = value;
                this.OnPropertyChanged(() => RightSideName);
            }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return string.Format("Relation type [left: {0} - right: {1}]", this.LeftSideName, this.RightSideName);
        }

        #endregion Methods
    }
}