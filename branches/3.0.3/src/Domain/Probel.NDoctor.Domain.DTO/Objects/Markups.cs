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
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// List of all markups the <see cref="MacroBuilder"/> replaces with database values
    /// </summary>
    public class Markups
    {
        #region Fields

        public const string Age = OPEN_MARKUP + "age" + CLOSE_MARKUP;
        public const string Birthdate = OPEN_MARKUP + "birthdate" + CLOSE_MARKUP;
        public const string FirstName = OPEN_MARKUP + "firstname" + CLOSE_MARKUP;
        public const string Height = OPEN_MARKUP + "height" + CLOSE_MARKUP;
        public const string LastName = OPEN_MARKUP + "lastname" + CLOSE_MARKUP;
        public const string Now = OPEN_MARKUP + "now" + CLOSE_MARKUP;
        public const string Today = OPEN_MARKUP + "today" + CLOSE_MARKUP;

        private const string CLOSE_MARKUP = "$";
        private const string OPEN_MARKUP = "$";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="Markups"/> class from being created.
        /// </summary>
        private Markups()
        {
        }

        #endregion Constructors

        #region Methods

        public static string[] All()
        {
            var markups = new Markups();
            var fields = typeof(Markups).GetFields(BindingFlags.Public | BindingFlags.Static);
            var result = new List<string>();
            foreach (var field in fields)
            {
                if (field.IsLiteral && !field.IsInitOnly)
                {
                    result.Add(field.GetValue(markups) as string);
                }
            }
            return result.ToArray();
        }

        #endregion Methods
    }
}