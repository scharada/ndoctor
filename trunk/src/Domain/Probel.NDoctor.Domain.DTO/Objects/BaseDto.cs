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
    using System.Collections.Generic;

    using Probel.Helpers.Data;
    using Probel.Helpers.Events;
    using Probel.Mvvm;
    using Probel.Mvvm.Validation;

    [Serializable]
    public abstract class BaseDto : BaseDto<long>, ICloneable
    {
        #region Fields

        private bool isImported;

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

        #endregion Properties

        #region Methods

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="other">The other.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(BaseDto dto, BaseDto other)
        {
            return !(dto == other);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="other">The other.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(BaseDto dto, BaseDto other)
        {
            if (object.ReferenceEquals(dto, other)) { return true; }
            else if ((object)dto == null || (object)other == null) { return false; }
            else { return dto.Equals(other); }
        }

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

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            else if (obj.GetType() == this.GetType() && obj is BaseDto) { return this.Id == (obj as BaseDto).Id; }
            else { return false; }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode() ^ this.GetType().GetHashCode();
        }

        #endregion Methods
    }
}