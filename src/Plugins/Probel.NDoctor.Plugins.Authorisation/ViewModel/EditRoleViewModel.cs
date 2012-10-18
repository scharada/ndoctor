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
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Authorisation.Helpers;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Toolbox.Navigation;

    internal class EditRoleViewModel : BaseViewModel
    {
        #region Fields

        private IAuthorisationComponent component = PluginContext.ComponentFactory.GetInstance<IAuthorisationComponent>();
        private string name;
        private string notes;
        private RoleDto selectedRole;

        #endregion Fields

        #region Constructors

        public EditRoleViewModel(RoleDto role)
        {
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IAuthorisationComponent>();

            this.SelectedRole = role;
            this.Name = role.Name;
            this.Description = role.Description;

            this.UpdateCommand = new RelayCommand(() => this.Update(), () => this.CanUpdate());
        }

        #endregion Constructors

        #region Properties

        public string Description
        {
            get { return this.notes; }
            set
            {
                this.notes = value;
                this.OnPropertyChanged(() => Description);
            }
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged(() => Name);
            }
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

        public ICommand UpdateCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private bool CanUpdate()
        {
            return !string.IsNullOrWhiteSpace(this.Name);
        }

        private void Update()
        {
            this.SelectedRole.Name = this.Name;
            this.SelectedRole.Description = this.Description;

            component.Update(this.SelectedRole);

            InnerWindow.Close();
            Notifyer.OnRoleRefreshing(this);
        }

        #endregion Methods
    }
}