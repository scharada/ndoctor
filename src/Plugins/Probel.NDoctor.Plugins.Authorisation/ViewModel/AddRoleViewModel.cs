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
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Authorisation.Helpers;
    using Probel.NDoctor.Plugins.Authorisation.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Toolbox;
    using Probel.NDoctor.View.Toolbox.Navigation;

    internal class AddRoleViewModel : BaseViewModel
    {
        #region Fields

        private IAuthorisationComponent component = PluginContext.ComponentFactory.GetInstance<IAuthorisationComponent>();
        private string description;
        private string name;

        #endregion Fields

        #region Constructors

        public AddRoleViewModel()
        {
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IAuthorisationComponent>();

            this.AddCommand = new RelayCommand(() => this.AddRole(), () => this.CanAddRole());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
        }

        public string Description
        {
            get { return this.description; }
            set
            {
                this.description = value;
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

        #endregion Properties

        #region Methods

        private void AddRole()
        {
            try
            {
                this.component.Create(new RoleDto()
                {
                    Name = this.Name,
                    Description = this.Description,
                });

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_RoleCreated);
                Notifyer.OnRoleRefreshing(this);
                InnerWindow.Close();
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private bool CanAddRole()
        {
            return !string.IsNullOrWhiteSpace(this.Name);
        }

        #endregion Methods
    }
}