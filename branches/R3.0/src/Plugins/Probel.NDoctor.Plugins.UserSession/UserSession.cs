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
    using System.Windows.Media;

    using AutoMapper;

    using Microsoft.Windows.Controls;

    using Probel.Helpers.Strings;
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

    using StructureMap;

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

        private ICommand addCommand = null;
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
        public UserSession([Import("version")] Version version, [Import("host")] IPluginHost host)
            : base(version, host)
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
            this.ConfigureStructureMap();

            this.component = ObjectFactory.GetInstance<IUserSessionComponent>();

            TranslateExtension.ResourceManager = Messages.ResourceManager;

            this.Host.Invoke(() => { this.connectionPage = new ConnectionView(); });

            var splitter = this.Host.FindInHome("add", Groups.Tools);
            var splitterExist = true;
            if (splitter == null || splitter.GetType() != typeof(RibbonSplitButtonData))
            {
                splitterExist = false;
                splitter = new RibbonSplitButtonData(Messages.Btn_Add, uri.StringFormat("Add"), null)
                {
                    Order = 1,
                    Name = "add",
                };
            }

            var addButton = new RibbonButtonData(Messages.Title_ButtonAddUser, uri.StringFormat("Add"), this.addCommand)
            {
                Order = 3,
            };
            (splitter as RibbonSplitButtonData).ControlDataCollection.Add(addButton);
            if (!splitterExist) this.Host.AddInHome((splitter as RibbonSplitButtonData), Groups.Tools);

            this.InitialiseConnectionPage();
            this.InitialiseUpdateUserPage();
        }

        private void BuildCommands()
        {
            this.printCommand = new RelayCommand(() => this.PrintBusinessCard());
            this.addCommand = new RelayCommand(() => this.NavigateAddUser());
            this.showUpdateUserCommand = new RelayCommand(() => this.NavigateToUpdateUser());
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<UserDto, BusinessCardViewModel>();
            Mapper.CreateMap<BusinessCardViewModel, UserDto>();
        }

        private void ConfigureStructureMap()
        {
            ObjectFactory.Configure(x =>
            {
                x.For<IUserSessionComponent>().Add<UserSessionComponent>();
                x.SelectConstructor<UserSessionComponent>(() => new UserSessionComponent());
            });
        }

        private void InitialiseConnectionPage()
        {
            LightUserDto defaultUser = null;
            using (this.component.UnitOfWork)
            {
                defaultUser = this.component.GetDefaultUser();
            }

            if (defaultUser == null)
            {
                this.Host.HideMainMenu();
                this.Host.Navigate(this.connectionPage);
            }
            else
            {
                this.Host.ConnectedUser = defaultUser;
                this.Host.NavigateToStartPage();
            }
        }

        private void InitialiseUpdateUserPage()
        {
            var menu = new RibbonControlData(Messages.Menu_ManagePersonalData, uri.StringFormat("Users"), showUpdateUserCommand) { Order = 3 };
            this.Host.AddToApplicationMenu(menu);
        }

        private void NavigateAddUser()
        {
            ChildWindowContext.Content = new AddUserControl();
            ChildWindowContext.WindowState = WindowState.Open;
            ChildWindowContext.IsModal = false;
            ChildWindowContext.Caption = Messages.Title_ButtonAddUser;
        }

        private void NavigateToUpdateUser()
        {
            ChildWindowContext.Content = new UpdateUserControl();
            ChildWindowContext.WindowState = WindowState.Open;
            ChildWindowContext.IsModal = false;
            ChildWindowContext.Caption = Messages.Menu_ManagePersonalData;
        }

        private void PrintBusinessCard()
        {
            BusinessCard card;
            UserDto user;
            using (this.component.UnitOfWork)
            {
                card = new BusinessCard();
                user = this.component.GetUserById(this.Host.ConnectedUser.Id);
            }
            card.DataContext = BusinessCardViewModel.CreateFrom(user);

            card.Print();
        }

        #endregion Methods
    }
}