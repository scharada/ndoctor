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
namespace Probel.NDoctor.Domain.DAL.Entities
{
    /// <summary>
    /// A macro is a text with tags that will be replaced with database values by the <see cref="MacroBuilder"/>
    /// </summary>
    public class Macro : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the expression. That's the raw text with the markups
        /// </summary>
        /// <value>
        /// The expression.
        /// </value>
        public virtual string Expression
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the notes that describes the macro.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public virtual string Notes
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the title of the macro.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Title
        {
            get; set;
        }

        #endregion Properties
    }
}