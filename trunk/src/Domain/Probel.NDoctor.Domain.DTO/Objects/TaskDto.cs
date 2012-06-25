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

    public class TaskDto : BaseDto
    {
        #region Fields

        private string name;
        private string notes;
        private string refName;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDto"/> class.
        /// </summary>
        /// <param name="refName">Name of the ref.</param>
        public TaskDto(string refName)
            : base(new TaskValidator())
        {
            this.refName = refName;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the name of the task.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged(() => Name);
            }
        }

        /// <summary>
        /// Gets or sets the notes to explain the database.
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
        /// Gets the name of the ref.
        /// </summary>
        /// <value>
        /// The name of the ref.
        /// </value>
        public string RefName
        {
            get { return this.refName; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, name: {1}", base.ToString(), this.name);
        }

        #endregion Methods
    }
}