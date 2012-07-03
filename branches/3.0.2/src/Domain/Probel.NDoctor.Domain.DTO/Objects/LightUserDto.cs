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

    using Probel.Mvvm;

    [Serializable]
    public class LightUserDto : BaseDto
    {
        #region Fields

        private RoleDto assignedRole;
        private string firstName;
        private bool isDefaultUser;
        private string lastName;

        #endregion Fields

        #region Constructors

        public LightUserDto()
        {
        }

        public LightUserDto(bool isSuperAdmin)
        {
            this.IsSuperAdmin = isSuperAdmin;
        }

        #endregion Constructors

        #region Properties

        public RoleDto AssignedRole
        {
            get { return this.assignedRole; }
            set
            {
                this.assignedRole = value;
                this.OnPropertyChanged(() => AssignedRole);
            }
        }

        public string DisplayedName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        public string FirstName
        {
            get { return this.firstName; }
            set
            {
                this.firstName = value;
                this.OnPropertyChanged(() => FirstName);
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
            get; private set;
        }

        public string LastName
        {
            get { return this.lastName; }
            set
            {
                this.lastName = value;
                this.OnPropertyChanged(() => LastName);
            }
        }

        #endregion Properties
    }
}