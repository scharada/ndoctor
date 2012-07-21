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

namespace Probel.NDoctor.Domain.DAL.Helpers
{
    using System;

    /// <summary>
    /// When decorated with the attribute, the method won't be wrapped into a transaction.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property,
        AllowMultiple = false,
        Inherited = true)]
    public class ExcludeFromTransactionAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcludeFromTransactionAttribute"/> class.
        /// </summary>
        public ExcludeFromTransactionAttribute()
        {
            this.IsTransaction = true;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Returns always true.
        /// </summary>
        /// <value>
        ///   <c>true</c> this method will be encapsulated into a transaction; otherwise, <c>false</c>.
        /// </value>
        public bool IsTransaction
        {
            get;
            private set;
        }

        #endregion Properties
    }
}