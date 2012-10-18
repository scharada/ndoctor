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
namespace Probel.NDoctor.Plugins.UserSession.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Helpers.Conversions;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.UserSession.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.Services.Messaging;

    internal class UpdateUserViewModel : BaseViewModel
    {
        #region Fields

        private IUserSessionComponent component = PluginContext.ComponentFactory.GetInstance<IUserSessionComponent>();
        private ObservableCollection<PracticeDto> practices;
        private ObservableCollection<RoleDto> roles;
        private UserDto user;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserViewModel"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public UpdateUserViewModel()
            : base()
        {
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IUserSessionComponent>();
            this.UpdateCommand = new RelayCommand(() => this.UpdateUser(), () => this.CanUpdateUser());
            this.CancelCommand = new RelayCommand(() => InnerWindow.Close());
            InnerWindow.Loaded += (sender, e) => this.Refresh();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="UpdateUserViewModel"/> is reclaimed by garbage collection.
        /// </summary>
        ~UpdateUserViewModel()
        {
            InnerWindow.Loaded -= (sender, e) => this.Refresh();
        }

        #endregion Constructors

        #region Properties

        public ICommand CancelCommand
        {
            get;
            private set;
        }

        public bool IsDefaultUser
        {
            get
            {
                if (this.User == null) { this.User = new UserDto(); }

                return this.User.IsDefault;
            }
            set
            {
                if (this.User == null) { this.User = new UserDto(); }

                if (value == true)
                {
                    var dr = MessageBox.Show(Messages.Msg_WarnDefaultUser
                        , Messages.Title_Warning
                        , MessageBoxButton.YesNo
                        , MessageBoxImage.Asterisk);
                    if (dr == MessageBoxResult.No) return;
                }
                this.User.IsDefault = value;
                this.OnPropertyChanged(() => IsDefaultUser);
            }
        }

        /// <summary>
        /// Gets or sets all the practices defined in the application.
        /// </summary>
        /// <value>
        /// The practices.
        /// </value>
        public ObservableCollection<PracticeDto> Practices
        {
            get { return this.practices; }
            set
            {
                this.practices = value;
                this.OnPropertyChanged(() => Practices);
            }
        }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public ObservableCollection<RoleDto> Roles
        {
            get { return this.roles; }
            set
            {
                this.roles = value;
                this.OnPropertyChanged(() => Roles);
            }
        }

        public PracticeDto SelectedPractice
        {
            get
            {
                return (this.User == null)
                    ? null
                    : this.User.Practice;
            }
            set
            {
                if (this.User == null) return;
                this.User.Practice = value;
                this.OnPropertyChanged(() => SelectedPractice);
            }
        }

        public RoleDto SelectedRole
        {
            get
            {
                return (this.User == null)
                    ? null
                    : this.User.AssignedRole;
            }
            set
            {
                if (this.User == null) return;
                this.User.AssignedRole = value;
                this.OnPropertyChanged(() => SelectedRole);
            }
        }

        public ICommand UpdateCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the user to manage.
        /// </summary>
        /// <value>
        /// The user or null if an error occured.
        /// </value>
        public UserDto User
        {
            get { return this.user; }
            set
            {
                this.user = value;
                this.OnPropertyChanged(() => User);
                this.OnPropertyChanged(() => IsDefaultUser);
            }
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            try
            {
                if (PluginContext.Host.ConnectedUser == null) return;

                this.Practices = this.component.GetAllPractices().ToObservableCollection();
                this.Roles = this.component.GetAllRolesLight().ToObservableCollection();

                var user = this.component.LoadUser(PluginContext.Host.ConnectedUser);
                this.User = user;

                if (this.User.Practice != null)
                {
                    this.SelectedPractice = (from p in this.Practices
                                             where p.Id == this.User.Practice.Id
                                             select p).FirstOrDefault();
                }

                if (this.User.AssignedRole != null)
                {
                    this.SelectedRole = (from r in this.Roles
                                         where r.Id == this.User.AssignedRole.Id
                                         select r).FirstOrDefault();
                }

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_Ready);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private bool CanUpdateUser()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.MetaWrite)
                && (!string.IsNullOrWhiteSpace(this.User.FirstName)
                && !string.IsNullOrWhiteSpace(this.User.LastName));
        }

        private void UpdateUser()
        {
            try
            {
                var component = PluginContext.ComponentFactory.GetInstance<IUserSessionComponent>();

                component.Update(this.User);
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_UserUpdated);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
            finally { InnerWindow.Close(); }
        }

        #endregion Methods
    }
}