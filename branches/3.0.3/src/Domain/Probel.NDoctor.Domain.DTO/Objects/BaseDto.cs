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

    using Probel.Helpers.Data;
    using Probel.Mvvm;
    using Probel.Mvvm.Validation;

    [Serializable]
    public abstract class BaseDto : BaseDto<long>, ICloneable
    {
        #region Fields

        private bool isImported;
        private string segretator;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDto"/> class.
        /// </summary>
        public BaseDto(IValidator validator)
            : base(validator)
        {
            this.IsImported = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDto"/> class.
        /// </summary>
        public BaseDto()
            : base()
        {
            this.IsImported = false;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this dto is imported from somewhere else.
        /// By imported, understand the dto wasn't added manually in the database
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is imported; otherwise, <c>false</c>.
        /// </value>
        public bool IsImported
        {
            get { return this.isImported; }
            set
            {
                this.isImported = value;
                this.OnPropertyChanged(() => this.IsImported);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is new.
        /// A new instance means it doesn't exist in the database.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is new; otherwise, <c>false</c>.
        /// </value>
        public bool IsNew
        {
            get
            {
                return this.Id <= 0;
            }
        }

        /// <summary>
        /// Gets or sets the list grouper. That's, a text that can be used in ListView
        /// to group the DTO of a list.
        /// </summary>
        /// <value>
        /// The list grouper.
        /// </value>
        public string Segretator
        {
            get { return this.segretator; }
            set
            {
                this.segretator = value;
                this.OnPropertyChanged(() => Segretator);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return Cloner.Clone(this);
        }

        #endregion Methods
    }
}