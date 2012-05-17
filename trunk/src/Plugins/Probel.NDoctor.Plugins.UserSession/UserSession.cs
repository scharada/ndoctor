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
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DAL.Components;
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

        private ICommand addCommand;
        private ICommand changePwdCommand;
        private IUserSessionComponent component;
        private ConnectionView connectionPage;
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
            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);
            this.BuildCommands();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialises this plugin. Basicaly it should configure the menus into the PluginHost
        /// Every task that could throw exception should be in this method and not in the ctor.
        /// </summary>
        public override void Initialise()
        {
            this.ConfigureAutoMapper();

            this.component = new ComponentFactory(PluginContext.Host.ConnectedUser, PluginContext.ComponentLogginEnabled).GetInstance<IUserSessionComponent>();

            TranslateExtension.ResourceManager = Messages.ResourceManager;

            PluginContext.Host.Invoke(() => { this.connectionPage = new ConnectionView(); });

            var splitter = PluginContext.Host.FindInHome("add", Groups.Tools);
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

            var addButton = new RibbonMenuItemData(Messages.Title_ButtonAddUser, uri.FormatWith("Add"), this.addCommand) { Order = 3, };
            (splitter as RibbonMenuButtonData).ControlDataCollection.Add(addButton);
            if (!splitterExist) PluginContext.Host.AddInHome((splitter as RibbonMenuButtonData), Groups.Tools);

            this.InitialiseConnectionPage();
            this.InitialiseUpdateUserPage();
        }

        private void BuildCommands()
        {
            this.printCommand = new RelayCommand(() => this.PrintBusinessCard());
            this.addCommand = new RelayCommand(() => this.NavigateAddUser());
            this.changePwdCommand = new RelayCommand(() => InnerWindow.Show(Messages.Title_ChangePwd, new ChangePasswordView()));
            this.showUpdateUserCommand = new RelayCommand(() => this.NavigateToUpdateUser());
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<UserDto, BusinessCardViewModel>();
            Mapper.CreateMap<BusinessCardViewModel, UserDto>();
        }

        private void InitialiseConnectionPage()
        {
            LightUserDto defaultUser = null;
            using (this.component.UnitOfWork)
            {
                defaultUser = this.component.FindDefaultUser();
            }

            if (defaultUser == null)
            {
                PluginContext.Host.HideMainMenu();
                PluginContext.Host.Navigate(this.connectionPage);
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

        private void NavigateAddUser()
        {
            InnerWindow.Show(Messages.Title_ButtonAddUser, new AddUserControl());
        }

        private void NavigateToUpdateUser()
        {
            InnerWindow.Show(Messages.Menu_ManagePersonalData, new UpdateUserControl());
        }

        private void PrintBusinessCard()
        {
            BusinessCard card;
            UserDto user;
            using (this.component.UnitOfWork)
            {
                card = new BusinessCard();
                user = this.component.GetUserById(PluginContext.Host.ConnectedUser.Id);
            }
            card.DataContext = BusinessCardViewModel.CreateFrom(user);

            card.Print();
        }

        #endregion Methods
    }
}