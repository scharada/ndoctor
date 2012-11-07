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
    using System.Collections.Generic;
    using System.Windows.Input;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.Properties;
    using Probel.NDoctor.View.Core.View;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;
    using Probel.NDoctor.View.Toolbox;
    using Probel.NDoctor.View.Toolbox.Navigation;

    /// <summary>
    /// This ViewModel should contain all the information about the
    /// </summary>
    internal class MainWindowViewModel : BaseViewModel
    {
        #region Fields

        private const string uriImage = @"\Probel.NDoctor.View.Core;component/Images\{0}.png";

        private readonly ICommand aboutCommand;
        private readonly ICommand homeCommand;
        private readonly ICommand settingCommand;

        private LightUserDto connectedUser;
        private string message;
        private LightPatientDto selectedPatient;
        private StatusType type;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
            : base()
        {
            this.settingCommand = new RelayCommand(() => this.NavigateToSetting(), () => this.CanNavigateToSetting());
            this.aboutCommand = new RelayCommand(() => this.About());
            this.homeCommand = new RelayCommand(() => PluginContext.Host.NavigateToStartPage());

            var buttons = new List<RibbonControlData>();
            buttons.Add(new RibbonControlData(Messages.Menu_Home, "/Images/Home.png", homeCommand) { Order = 1 });
            buttons.Add(new RibbonSeparatorData(2));
            buttons.Add(new RibbonControlData(Messages.Title_Settings, uriImage.FormatWith("Settings"), settingCommand) { Order = int.MaxValue - 21 });
            buttons.Add(new RibbonControlData(Messages.Title_About, uriImage.FormatWith("Info"), aboutCommand) { Order = int.MaxValue - 20 });
            buttons.Add(new RibbonSeparatorData(int.MaxValue - 10));
            buttons.Add(new RibbonControlData(Messages.Menu_Exit, "", Commands.Shutdown) { Order = int.MaxValue });

            foreach (var button in buttons) { PluginContext.Host.AddToApplicationMenu(button); }

            App.RibbonData.ApplicationMenuData.LargeImage = new Uri(uriImage.FormatWith("Home"), UriKind.Relative);
            App.RibbonData.ApplicationMenuData.SmallImage = new Uri(uriImage.FormatWith("Home"), UriKind.Relative);

            this.ChildWindow = new ChildWindowViewModel();
        }

        #endregion Constructors

        #region Properties

        public ChildWindowViewModel ChildWindow
        {
            get;
            private set;
        }

        public string ConnectedPatientText
        {
            get
            {
                return (this.SelectedPatient != null)
                    ? Messages.Msg_ConnectedPatient.FormatWith(PluginContext.Host.SelectedPatient.DisplayedName)
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
                PluginContext.ComponentFactory.ConnectUser(value);

                this.OnPropertyChanged(() => ConnectedUser);
                this.OnPropertyChanged(() => WindowTitle);
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
                this.OnPropertyChanged(() => Message);
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
                this.OnPropertyChanged(() => SelectedPatient);
                this.OnPropertyChanged(() => ConnectedPatientText);
            }
        }

        public ICommand TriggerNewCommand
        {
            get;
            private set;
        }

        public ICommand TriggerRefreshCommand
        {
            get;
            private set;
        }

        public ICommand TriggerSaveCommand
        {
            get;
            private set;
        }

        public ICommand TriggerSearchCommand
        {
            get;
            private set;
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
                this.OnPropertyChanged(() => Type);
            }
        }

        public string WindowTitle
        {
            get
            {
                return (this.ConnectedUser != null)
                    ? Messages.Title_SelectedUser.FormatWith(this.ConnectedUser.DisplayedName)
                    : Messages.Title_NoUserConnected;
            }
        }

        #endregion Properties

        #region Methods

        private void About()
        {
            ViewService.Manager.ShowDialog<AboutBoxViewModel>();
        }

        private bool CanNavigateToSetting()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Administer);
        }

        private void NavigateToSetting()
        {
            //InnerWindow.Show(Messages.Title_Settings, new SettingsView());
            ViewService.Manager.ShowDialog<SettingsViewModel>();
        }

        #endregion Methods
    }
}