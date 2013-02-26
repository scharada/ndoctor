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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Probel.NDoctor.Domain.DTO.Validators;

    [Serializable]
    public class LightUserDto : PersonDto
    {
        #region Fields

        private string assignedRoleName;
        private bool isDefaultUser;

        #endregion Fields

        #region Constructors

        public LightUserDto()
            : this(false)
        {
        }

        public LightUserDto(bool isSuperAdmin)
            : base()
        {
            this.IsSuperAdmin = isSuperAdmin;
        }

        #endregion Constructors

        #region Properties

        public string AssignedRoleName
        {
            get { return this.assignedRoleName; }
            set
            {
                this.assignedRoleName = value;
                this.OnPropertyChanged(() => AssignedRoleName);
            }
        }

        public string DisplayedName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        public bool IsDefault
        {
            get { return this.isDefaultUser; }
            set
            {
                this.isDefaultUser = value;
                this.OnPropertyChanged(() => IsDefault);
            }
        }

        public bool IsSuperAdmin
        {
            get;
            protected set;
        }

        #endregion Properties
    }
}