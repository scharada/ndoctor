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
namespace Probel.Helpers.Events
{
    using System;

    public class EventArgs<T> : EventArgs
    {
        #region Constructors

        public EventArgs(T data)
        {
            this.Data = data;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the Data contained in an event
        /// </summary>
        public T Data
        {
            get; private set;
        }

        #endregion Properties
    }
}