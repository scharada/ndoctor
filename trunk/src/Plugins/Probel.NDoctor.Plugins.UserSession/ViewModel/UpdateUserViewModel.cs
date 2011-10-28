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

    using Probel.Helpers.Conversions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.UserSession.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    public class UpdateUserViewModel : BaseViewModel
    {
        #region Fields

        private IUserSessionComponent component = ObjectFactory.GetInstance<IUserSessionComponent>();
        private ObservableCollection<PracticeDto> practices;
        private ObservableCollection<LightRoleDto> roles;
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
        }

        #endregion Constructors

        #region Properties

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
                this.OnPropertyChanged("IsDefaultUser");
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
                this.OnPropertyChanged("Practices");
            }
        }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public ObservableCollection<LightRoleDto> Roles
        {
            get { return this.roles; }
            set
            {
                this.roles = value;
                this.OnPropertyChanged("Roles");
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
                this.OnPropertyChanged("SelectedPractice");
            }
        }

        public LightRoleDto SelectedRole
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
                this.OnPropertyChanged("SelectedRole");
            }
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
                this.OnPropertyChanged("User", "IsDefaultUser");
            }
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            try
            {
                if (this.Host.ConnectedUser == null) return;

                using (this.component.UnitOfWork)
                {
                    this.Practices = this.component.GetAllPractices().ToObservableCollection();
                    this.Roles = this.component.GetAllRolesLight().ToObservableCollection();

                    var user = this.component.LoadUser(Host.ConnectedUser);
                    this.User = user;

                    this.SelectedPractice = (from p in this.Practices
                                             where p.Id == this.User.Practice.Id
                                             select p).FirstOrDefault();

                    this.SelectedRole = (from r in this.Roles
                                         where r.Id == this.User.AssignedRole.Id
                                         select r).FirstOrDefault();

                    this.Host.WriteStatus(StatusType.Info, Messages.Msg_Ready);
                }

            }
            catch (Exception ex) { this.HandleError(ex, Messages.Msg_ErrorWhileLoadingUser); }
        }

        #endregion Methods
    }
}