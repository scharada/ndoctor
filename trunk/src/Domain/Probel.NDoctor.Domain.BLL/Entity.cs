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
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
*/

#endregion Header

namespace Probel.NDoctor.Domain.BLL
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using Probel.Helpers.Assertion;

    /// <summary>
    /// Root for a Plain Old CLR Object
    /// </summary>
    public class Entity : INotifyPropertyChanged
    {
        #region Fields

        protected long id = -1;

        #endregion Fields

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the ID of this tuple.
        /// </summary>
        /// <value>The ID.</value>
        public virtual long Id
        {
            get
            {
                return this.id;
            }
            protected set
            {
                this.OnPropertyChanged("Id");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public virtual bool IsEmpty
        {
            get
            {
                return (this.id < 0);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Warns the developer if this object does not have a public property with
        /// the specified name. This method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // verify that the property name matches a real,
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                Assert.Fail("Invalid property name: {0}", propertyName);
            }
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Methods
    }
}