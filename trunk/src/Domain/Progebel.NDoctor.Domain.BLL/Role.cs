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
using System.Collections.Generic;
namespace Probel.NDoctor.Domain.BLL
{
    /// <summary>
    /// Represents a role in nDoctor. A role has several tasks and determines what a user can do
    /// </summary>
    public class Role 
    {
        /// <summary>
        /// Gets or sets the tasks the role has.
        /// </summary>
        /// <value>
        /// The tasks.
        /// </value>
        public List<Task> Tasks { get; set; }
        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}