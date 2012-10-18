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
    using System.Windows;
    using System.Windows.Input;

    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Authorisation.Helpers;
    using Probel.NDoctor.Plugins.Authorisation.Properties;
    using Probel.NDoctor.Plugins.Authorisation.View;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    internal class ManageUserViewModel : BaseViewModel
    {
        #region Fields

        private IAuthorisationComponent component;
        private LightUserDto selectedUser;

        #endregion Fields

        #region Constructors

        public ManageUserViewModel()
        {
            if (!Designer.IsDesignMode)
            {
                this.component = PluginContext.ComponentFactory.GetInstance<IAuthorisationComponent>();
                PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IAuthorisationComponent>();
            }
            this.Users = new ObservableCollection<LightUserDto>();
            this.Roles = new ObservableCollection<RoleDto>();

            this.UpdateUserCommand = new RelayCommand(() => this.UpdateUser(), () => this.CanUpdateUser());

            Notifyer.UserRefreshing += (sender, e) => this.Refresh();

            this.RemoveUserCommand = new RelayCommand(() => this.RemoveUser(), () => this.CanRemoveUser());
        }

        #endregion Constructors

        #region Properties

        public ICommand RemoveUserCommand
        {
            get;
            private set;
        }

        public ObservableCollection<RoleDto> Roles
        {
            get;
            private set;
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

        public ICommand UpdateUserCommand
        {
            get;
            private set;
        }

        public ObservableCollection<LightUserDto> Users
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            try
            {

                var users = this.component.GetAllLightUsers();
                var roles = this.component.GetAllRoles();

                this.Users.Refill(users);
                this.Roles.Refill(roles);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private bool CanRemoveUser()
        {
            return this.SelectedUser != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private bool CanUpdateUser()
        {
            return this.SelectedUser != null;
        }

        private void RemoveUser()
        {
            var dr = MessageBox.Show(Messages.Msg_AskRemoveUser, BaseText.Question, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dr == MessageBoxResult.No) return;

            try
            {
                if (this.component.IsSuperAdmin(this.SelectedUser))
                {
                    MessageBox.Show(Messages.Msg_CantRemoveSuperadmin, BaseText.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    this.component.Remove(this.SelectedUser);
                }

                this.Refresh();
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void UpdateUser()
        {
            var view = new EditAssignedRoleView(this.SelectedUser);
            ((EditAssignedRoleViewModel)view.DataContext).Refresh();
            InnerWindow.Show(Messages.Msg_UpdateRole, view);
            this.Refresh();
        }

        #endregion Methods
    }
}