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
    using System.Windows;
    using System.Windows.Input;

    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Plugins.UserSession.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox;

    internal class ChangePasswordViewModel : BaseViewModel
    {
        #region Fields

        private string checkNewPassword;
        private IUserSessionComponent component;
        private bool isPopupOpened;
        private string newPassword;
        private string oldPassword = string.Empty;

        #endregion Fields

        #region Constructors

        public ChangePasswordViewModel()
        {
            this.IsPopupOpened = false;
            if (!Designer.IsDesignMode)
            {
                this.component = PluginContext.ComponentFactory.GetInstance<IUserSessionComponent>();
                PluginContext.Host.UserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IUserSessionComponent>();
            }

            this.SaveCommand = new RelayCommand(() => this.Save(), () => this.CanSave());
            this.OpenPopupCommand = new RelayCommand(() => this.IsPopupOpened = true);
        }

        #endregion Constructors

        #region Properties

        public string CheckNewPassword
        {
            get { return this.checkNewPassword; }
            set
            {
                this.checkNewPassword = value;
                this.OnPropertyChanged(() => CheckNewPassword);
            }
        }

        public bool IsPopupOpened
        {
            get { return this.isPopupOpened; }
            set
            {
                this.isPopupOpened = value;
                this.OnPropertyChanged(() => IsPopupOpened);
            }
        }

        public string NewPassword
        {
            get { return this.newPassword; }
            set
            {
                this.newPassword = value;
                this.OnPropertyChanged(() => NewPassword);
            }
        }

        public string OldPassword
        {
            get { return this.oldPassword; }
            set
            {
                this.oldPassword = value;
                this.OnPropertyChanged(() => OldPassword);
            }
        }

        public ICommand OpenPopupCommand
        {
            get;
            private set;
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private bool CanSave()
        {
            if (PluginContext.Host.ConnectedUser == null) return false;

            return PluginContext.DoorKeeper.IsUserGranted(To.MetaWrite)
                && this.NewPassword == this.CheckNewPassword
                && !string.IsNullOrWhiteSpace(this.NewPassword);
        }

        private bool IsValidPassword()
        {
            return this.component.CanConnect(PluginContext.Host.ConnectedUser, this.OldPassword);
        }

        private void Save()
        {
            try
            {
                if (!this.IsValidPassword())
                {
                    ViewService.MessageBox.Warning(Messages.Msg_ErrorWrongPassword);
                }
                else
                {
                    this.component.UpdatePassword(PluginContext.Host.ConnectedUser, this.NewPassword);

                    this.IsPopupOpened = false;
                    PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_PwdChanged);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                this.Handle.Error(ex, Messages.Msg_ErrorChangingPwd);
            }
        }

        #endregion Methods
    }
}