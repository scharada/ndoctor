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
namespace Probel.NDoctor.Domain.BLL
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a role in nDoctor. A role has several tasks and determines what a user can do
    /// </summary>
    public class Role
    {
        #region Properties

        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public virtual string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a unique id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public virtual int Id
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tasks the role has.
        /// </summary>
        /// <value>
        /// The tasks.
        /// </value>
        public virtual List<Task> Tasks
        {
            get;
            set;
        }

        #endregion Properties
    }
}