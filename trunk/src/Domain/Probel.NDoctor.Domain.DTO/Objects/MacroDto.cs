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

namespace Probel.NDoctor.Domain.DTO.Objects
{
    using Probel.NDoctor.Domain.DTO.Validators;

    /// <summary>
    /// A macro is a text with markups that will be replaced with values from the database. 
    /// For instance, it allows to display the birthdate of the connected patient.
    /// </summary>
    public class MacroDto : BaseDto
    {
        #region Fields

        private string expression = string.Empty;
        private string notes = string.Empty;
        private string title = string.Empty;

        #endregion Fields

        #region Constructors

        public MacroDto()
            : base(new MacroValidator())
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the expression. That's the raw text with the markups
        /// </summary>
        /// <value>
        /// The expression.
        /// </value>
        public string Expression
        {
            get { return this.expression; }
            set
            {
                this.expression = value;
                this.OnPropertyChanged(() => Expression);
            }
        }

        /// <summary>
        /// Gets or sets the notes that describes the macro.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes
        {
            get { return this.notes; }
            set
            {
                this.notes = value;
                this.OnPropertyChanged(() => Notes);
            }
        }

        /// <summary>
        /// Gets or sets the title of the macro.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                this.OnPropertyChanged(() => Title);
            }
        }

        #endregion Properties
    }
}