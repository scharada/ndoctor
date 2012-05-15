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
namespace Probel.NDoctor.View.Core.ViewModel
{
    using System;
    using System.Windows.Input;

    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.Properties;
    using Probel.NDoctor.View.Core.View;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    /// <summary>
    /// This ViewModel should contain all the information about the
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields

        private const string uriImage = @"\Probel.NDoctor.View.Core;component/Images\{0}.png";

        private LightUserDto connectedUser;
        private string message;
        private LightPatientDto selectedPatient;
        private ICommand settingCommand;
        private StatusType type;

        #endregion Fields

        #region Constructors

        public MainWindowViewModel()
            : base()
        {
            this.settingCommand = new RelayCommand(() => PluginContext.Host.Navigate(new SettingsView()));

            var menu = new RibbonControlData(Messages.Title_Settings, uriImage.StringFormat("Settings"), settingCommand) { Order = 5 };
            this.Host.AddToApplicationMenu(menu);

            App.RibbonData.ApplicationMenuData.LargeImage = new Uri(uriImage.StringFormat("Home"), UriKind.Relative);
            App.RibbonData.ApplicationMenuData.SmallImage = new Uri(uriImage.StringFormat("Home"), UriKind.Relative);
        }

        #endregion Constructors

        #region Properties

        public string ConnectedPatientText
        {
            get
            {
                return (this.SelectedPatient != null)
                    ? Messages.Msg_ConnectedPatient.StringFormat(PluginContext.Host.SelectedPatient.DisplayedName)
                    : Messages.Title_NoUserConnected;
            }
        }

        /// <summary>
        /// Gets or sets the selected user.
        /// </summary>
        /// <value>
        /// The selected user.
        /// </value>
        public LightUserDto ConnectedUser
        {
            get { return this.connectedUser; }
            set
            {
                this.connectedUser = value;
                this.OnPropertyChanged("ConnectedUser", "WindowTitle");
            }
        }

        /// <summary>
        /// Gets or sets the message to display in the status bar.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get { return this.message; }
            set
            {
                this.message = value;
                this.OnPropertyChanged("Message");
            }
        }

        /// <summary>
        /// Gets or sets the selected patient.
        /// </summary>
        /// <value>
        /// The selected patient.
        /// </value>
        public LightPatientDto SelectedPatient
        {
            get { return this.selectedPatient; }
            set
            {
                this.selectedPatient = value;
                this.OnPropertyChanged("SelectedPatient", "ConnectedPatientText");
            }
        }

        /// <summary>
        /// Gets or sets the type of the icon in the status bar.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public StatusType Type
        {
            get { return this.type; }
            set
            {
                this.type = value;
                this.OnPropertyChanged("Type");
            }
        }

        public string WindowTitle
        {
            get
            {
                return (this.SelectedPatient != null)
                    ? Messages.Title_SelectedUser.StringFormat(this.ConnectedUser.DisplayedName)
                    : Messages.Title_NoUserConnected;
            }
        }

        #endregion Properties
    }
}