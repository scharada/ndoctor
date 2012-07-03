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
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    using Probel.Mvvm;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Validators;

    [Serializable]
    public class RoleDto : BaseDto
    {
        #region Fields

        string description;
        string name;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDto"/> class.
        /// </summary>
        public RoleDto()
            : base(new RoleValidator())
        {
            this.Tasks = new ObservableCollection<TaskDto>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return this.description; }
            set
            {
                this.description = value;
                this.OnPropertyChanged(() => Description);
            }
        }

        /// <summary>
        /// Gets or sets the name of the role.
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
        /// Gets the task collection.
        /// </summary>
        public ObservableCollection<TaskDto> Tasks
        {
            get;
            private set;
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