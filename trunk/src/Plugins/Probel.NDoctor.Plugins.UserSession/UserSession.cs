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
namespace Probel.NDoctor.Plugins.UserSession
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.UserSession.Controls;
    using Probel.NDoctor.Plugins.UserSession.Properties;
    using Probel.NDoctor.Plugins.UserSession.View;
    using Probel.NDoctor.Plugins.UserSession.ViewModel;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;
    using Probel.NDoctor.View.Toolbox.Navigation;
    using Probel.Mvvm.Gui;
    using Probel.Mvvm;

    /// <summary>
    /// When the application user has logged into the application, it opens a User session that contains the modules the logged user can use. 
    /// </summary>
    /// <remarks>
    /// It contains:
    /// a menu to change the user's information. This information is:
    ///    User's name
    ///    User' surname
    ///    The header that will be used in the printable documents (Not printable)
    ///    The default fee the patient should pay for a meeting (Not printable)
    ///    The address of the user's practice
    ///    The phone number
    ///    The mobile phone number
    ///    The email 
    /// a menu to print a card with all this information.
    /// a system to load plugins.
    /// a system to manage the rights of the user.
    /// a list of modules. 
    /// </remarks>
    [Export(typeof(IPlugin))]
    public class UserSession : Plugin
    {
        #region Fields

        private readonly string uri = @"\Probel.NDoctor.Plugins.UserSession;component/Images\{0}.png";

        private ICommand addUserCommand;
        private ICommand changePwdCommand;
        private IUserSessionComponent component;
        private ICommand printCommand;
        private ICommand showUpdateUserCommand;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSession"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="host">The host.</param>
        [ImportingConstructor]
        public UserSession([Import("version")] Version version)
            : base(version)
        {
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IUserSessionComponent>();
            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);
            this.BuildCommands();
        }

        #endregion Constructors

        #region Properties

        public ICommand DisconnectCommand
        {
            get;
            private set;
        }

        private ConnectionView ConnectionPage
        {
            get
            {
                ConnectionView connectionView = null;
                PluginContext.Host.Invoke(() => connectionView = new ConnectionView());
                return connectionView;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initialises this plugin. Basicaly it should configure the menus into the PluginHost
        /// Every task that could throw exception should be in this method and not in the ctor.
        /// </summary>
        public override void Initialise()
        {
            this.ConfigureAutoMapper();
            this.ConfigureViewService();

            this.component = PluginContext.ComponentFactory.GetInstance<IUserSessionComponent>();

            TranslateExtension.ResourceManager = Messages.ResourceManager;

            var splitter = PluginContext.Host.GetInHome("add", Groups.Tools);
            var splitterExist = true;
            if (splitter == null || splitter.GetType() != typeof(RibbonMenuButtonData))
            {
                splitterExist = false;
                splitter = new RibbonMenuButtonData(Messages.Btn_Add, uri.FormatWith("Add"), null)
                {
                    Order = 1,
                    Name = "add",
                };
            }

            var addButton = new RibbonMenuItemData(Messages.Title_ButtonAddUser, uri.FormatWith("Add"), this.addUserCommand) { Order = 3, };
            (splitter as RibbonMenuButtonData).ControlDataCollection.Add(addButton);
            if (!splitterExist) PluginContext.Host.AddInHome((splitter as RibbonMenuButtonData), Groups.Tools);

            var navigateButton = new RibbonButtonData(Messages.Title_Deconnection
                , uri.FormatWith("Administration")
                , this.DisconnectCommand) { Order = int.MaxValue - 1 };

            PluginContext.Host.AddToApplicationMenu(navigateButton);

            this.InitialiseConnectionPage();
            this.InitialiseUpdateUserPage();
        }

        private void BuildCommands()
        {
            this.DisconnectCommand = new RelayCommand(() => this.Disconnect(), () => this.CanDisconnect());

            this.printCommand = new RelayCommand(() => this.PrintBusinessCard(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write));

            this.addUserCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<AddUserViewModel>(), () => this.CanNavigateAddUser());

            this.changePwdCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<ChangePasswordViewModel>()
                , () => PluginContext.DoorKeeper.IsUserGranted(To.MetaWrite));

            this.showUpdateUserCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<UpdateUserViewModel>()
                , () => PluginContext.DoorKeeper.IsUserGranted(To.MetaWrite));
        }

        private void ConfigureViewService()
        {
            LazyLoader.Set<ConnectionView>(() => new ConnectionView());

            ViewService.Configure(e =>
            {
                e.Bind<AddUserControl, AddUserViewModel>()
                    .OnClosing(() => LazyLoader.Get<ConnectionView>().As<ConnectionViewModel>().Refresh());
                e.Bind<ChangePasswordView, ChangePasswordViewModel>();
                e.Bind<UpdateUserControl, UpdateUserViewModel>()
                    .OnShow(vm => vm.Refresh());
            });
        }
        private bool CanDisconnect()
        {
            return true;
        }

        private bool CanNavigateAddUser()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Administer);
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<UserDto, BusinessCardViewModel>();
            Mapper.CreateMap<BusinessCardViewModel, UserDto>();
        }

        private void Disconnect()
        {
            PluginContext.Host.ConnectedUser = null;
            PluginContext.Host.HideMainMenu();
            PluginContext.Host.Navigate(this.ConnectionPage);
        }

        private void InitialiseConnectionPage()
        {
            var defaultUser = this.component.GetDefaultUser();

            if (defaultUser == null)
            {
                PluginContext.Host.HideMainMenu();
                PluginContext.Host.Navigate(this.ConnectionPage);
            }
            else
            {
                PluginContext.Host.ConnectedUser = defaultUser;
                PluginContext.Host.NavigateToStartPage();
            }
        }

        private void InitialiseUpdateUserPage()
        {
            var pwd = new RibbonControlData(Messages.Title_ChangePwd, uri.FormatWith("Keys"), changePwdCommand) { Order = 3 };
            var menu = new RibbonControlData(Messages.Menu_ManagePersonalData, uri.FormatWith("Users"), showUpdateUserCommand) { Order = 3 };

            PluginContext.Host.AddToApplicationMenu(pwd);
            PluginContext.Host.AddToApplicationMenu(menu);
        }

        private void PrintBusinessCard()
        {
            var card = new BusinessCard();
            var user = this.component.GetUserById(PluginContext.Host.ConnectedUser.Id);

            card.DataContext = BusinessCardViewModel.CreateFrom(user);

            card.Print();
        }

        #endregion Methods
    }
}