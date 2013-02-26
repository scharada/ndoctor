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
namespace Probel.Helpers.Management
{
    using System;

    /// <summary>
    /// A unit of work is a disposable object that is initiated at start and disposed at the end.
    /// </summary>
    public class UnitOfWork : UnitOfWork<object>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="init">The init.</param>
        /// <param name="dispose">The dispose.</param>
        public UnitOfWork(object context, Func<object, object> init, Action<object> dispose)
            : base(context, init, dispose)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="init">The init.</param>
        /// <param name="dispose">The dispose.</param>
        public UnitOfWork(Func<object, object> init, Action<object> dispose)
            : this(null, init, dispose)
        {
        }

        #endregion Constructors
    }
}