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

    using Probel.Helpers.Events;

    public abstract class BaseDto : ObservableObject, IEquatable<BaseDto>
    {
        #region Fields

        private static IEqualityComparer<BaseDto> equalityComparer = new BaseDtoEqualityComparer();

        private long id;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDto"/> class.
        /// </summary>
        public BaseDto()
        {
            this.State = State.Clean;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the equality comparer used to compare two <see cref="LightPatientDto"/>
        /// </summary>
        public static IEqualityComparer<BaseDto> EqualityComparer
        {
            get { return equalityComparer; }
        }

        /// <summary>
        /// Gets or sets the id of the entity.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public long Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
                this.OnPropertyChanged("Id");
            }
        }

        /// <summary>
        /// Gets or sets the state of the DTO.
        /// This state will be used to determine whether
        /// this DTO is new/updated/deleted.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public State State
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// The equality is checked on the id
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(BaseDto other)
        {
            return this.id == other.id;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("[{0}] id: {1}", this.GetType().Name, this.id);
        }

        #endregion Methods

        #region Nested Types

        private class BaseDtoEqualityComparer : IEqualityComparer<BaseDto>
        {
            #region Methods

            public bool Equals(BaseDto x, BaseDto y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(BaseDto obj)
            {
                return obj.GetHashCode();
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}