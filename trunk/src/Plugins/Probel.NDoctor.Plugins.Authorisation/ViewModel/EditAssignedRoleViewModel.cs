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

namespace Probel.NDoctor.Plugins.Authorisation.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using Probel.Helpers.Strings;
    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Authorisation.Properties;
    using Probel.NDoctor.View;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox;

    internal class EditAssignedRoleViewModel : BaseViewModel
    {
        #region Fields

        private IAuthorisationComponent component;
        private RoleDto selectedRole;
        private LightUserDto selectedUser;

        #endregion Fields

        #region Constructors

        public EditAssignedRoleViewModel()
        {
            if (!Designer.IsDesignMode)
            {
                this.component = PluginContext.ComponentFactory.GetInstance<IAuthorisationComponent>();
                PluginContext.Host.UserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IAuthorisationComponent>();
            }
            this.Roles = new ObservableCollection<RoleDto>();
            this.UpdateCommand = new RelayCommand(() => this.Update(), () => this.CanUpdate());
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<RoleDto> Roles
        {
            get;
            private set;
        }

        public RoleDto SelectedRole
        {
            get { return this.selectedRole; }
            set
            {
                this.selectedRole = value;
                this.OnPropertyChanged(() => SelectedRole);
            }
        }

        public LightUserDto SelectedUser
        {
            get { return this.selectedUser; }
            set
            {
                this.selectedUser = value;
                this.OnPropertyChanged(() => SelectedUser);
            }
        }

        public ICommand UpdateCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            var roles = component.GetAllRoles();

            this.Roles.Refill(roles);
            if (this.SelectedUser != null && this.SelectedUser.AssignedRole != null)
            {
                this.SelectedRole = (from r in this.Roles
                                     where r.Name == this.SelectedUser.AssignedRole.Name
                                     select r).FirstOrDefault();
            }
        }

        private bool CanUpdate()
        {
            return this.SelectedUser != null
                && this.SelectedRole != null;
        }

        private void Update()
        {
            try
            {
                this.SelectedUser.AssignedRole = this.SelectedRole;

                component.Update(this.SelectedUser);

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_RoleUpdatedFor.FormatWith(this.SelectedUser.DisplayedName));
                this.Close();
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        #endregion Methods
    }
}