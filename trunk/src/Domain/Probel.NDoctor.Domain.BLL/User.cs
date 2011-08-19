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
namespace Probel.NDoctor.Domain.BLL
{
    /// <summary>
    /// This is a nDoctor user. Usually is the doctor, not the patient
    /// </summary>
    public class User : Person
    {
        /// <summary>
        /// Gets or sets the header that will be displayed in the prescriptions or
        /// other places where a header is needed.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public string Header { get; set; }
        /// <summary>
        /// Gets or sets the role the current user is assigned to.
        /// </summary>
        /// <value>
        /// The assigned role.
        /// </value>
        public Role AssignedRole { get; set; }
        /// <summary>
        /// Gets or sets the practice whereon the current user work.
        /// </summary>
        /// <value>
        /// The practice.
        /// </value>
        public Practice Practice { get; set; }
        /// <summary>
        /// Gets or sets the login of the user.
        /// </summary>
        /// <value>
        /// The login.
        /// </value>
        public string Login { get; set; }
        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets a string representing how the name of the user should
        /// be displayed.
        /// </summary>
        /// <value>
        /// The name of the displayed.
        /// </value>
        public string DisplayedName { get; set; }
    }
}