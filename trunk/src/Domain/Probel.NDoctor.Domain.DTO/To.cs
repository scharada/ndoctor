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

namespace Probel.NDoctor.Domain.DTO
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represents the built in tasks nDoctor knows. This is a static list of tasks.
    /// That's, this list can't be modified by configuration.
    /// All the role should be in lower case
    /// </summary>
    public static class To
    {
        #region Fields

        /// <summary>
        /// User should be administrator to execute features decorated with this role.
        /// That's, user with high privileges
        /// </summary>
        public const string Administer = "administer";

        /// <summary>
        /// Users granted to edit the calendar can add/remove and update appointments in the calendars
        /// </summary>
        public const string EditCalendar = "editcalendar";

        /// <summary>
        /// All users can execute features decorated with this role.
        /// </summary>
        public const string Everyone = "everyone";

        /// <summary>
        /// User granted to modify their own data (name, surname, password) can execute features decorated with this role.
        /// </summary>
        public const string MetaWrite = "metawrite";

        /// <summary>
        /// Standard user can execute features decorated with this role.        
        /// </summary>
        public const string Read = "read";

        /// <summary>
        /// Users granted to execute database modification can execute features decorated with this role.
        /// </summary>
        public const string Write = "write";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Using reflection, this methods returns a list of roles defined as const in this static objects
        /// </summary>
        /// <returns></returns>
        public static string[] ToStringArray()
        {
            List<string> roles = new List<string>();
            foreach (FieldInfo field in typeof(To).GetFields())
            {
                roles.Add(field.GetRawConstantValue().ToString());
            }

            return roles.ToArray();
        }

        #endregion Methods
    }
}