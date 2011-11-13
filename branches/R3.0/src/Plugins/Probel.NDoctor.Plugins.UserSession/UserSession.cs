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
        private RibbonContextualTabGroupData contextualMenu;
        private ICommand printCommand;
        private ICommand showUpdateUserCommand;
        private ICommand updateCommand;
        private UpdateUserView updateUserPage;

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

            this.Host.Invoke(() =>
            {
                this.connectionPage = new ConnectionView();
                this.updateUserPage = new UpdateUserView();
            });

            var addButton = new RibbonButtonData(Messages.Title_ButtonAddUser, this.addCommand)
            {
                SmallImage = new Uri(uri.StringFormat("Add"), UriKind.Relative),
                Order = 3,
            };
            this.Host.AddInHome(addButton, Groups.Tools);

            this.InitialiseConnectionPage();
            this.InitialiseUpdateUserPage();
        }

        private void BuildCommands()
        {
            this.updateCommand = new RelayCommand(() => this.UpdateUser());
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
            #region Application Menu
            var menu = new RibbonControlData(Messages.Menu_ManagePersonalData, uri.StringFormat("Users"), showUpdateUserCommand) { Order = 3 };
            this.Host.AddToApplicationMenu(menu);
            #endregion

            #region Context Menu

            var saveButton = new RibbonButtonData(Messages.Menu_Save, uri.StringFormat("Save"), this.updateCommand);
            var printButton = new RibbonButtonData(Messages.Menu_Print, this.printCommand) { SmallImage = new Uri(uri.StringFormat("Printer"), UriKind.RelativeOrAbsolute) };
            var cgroup = new RibbonGroupData(Messages.Menu_Actions);

            cgroup.ButtonDataCollection.Add(saveButton);
            cgroup.ButtonDataCollection.Add(printButton);

            var tab = new RibbonTabData(Messages.Menu_File, cgroup) { ContextualTabGroupHeader = Messages.Title_ContextMenu };
            this.Host.Add(tab);

            this.contextualMenu = new RibbonContextualTabGroupData(Messages.Title_ContextMenu, tab) { Background = Brushes.OrangeRed, IsVisible = false };
            this.Host.Add(this.contextualMenu);

            #endregion
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
            var viewModel = this.updateUserPage.DataContext as UpdateUserViewModel;
            if (viewModel != null) viewModel.Refresh();

            this.Host.Navigate(this.updateUserPage);

            this.contextualMenu.IsVisible = true;
            this.contextualMenu.TabDataCollection[0].IsSelected = true;
        }

        private void PrintBusinessCard()
        {
            //MessageBox.Show(Messages.Msg_NotYetImplemented, Messages.Title_Info, MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void UpdateUser()
        {
            try
            {
                var component = ObjectFactory.GetInstance<IUserSessionComponent>();
                var viewmodel = this.updateUserPage.DataContext as UpdateUserViewModel;

                using (component.UnitOfWork)
                {
                    component.Update(viewmodel.User);
                }
                this.Host.WriteStatus(StatusType.Info, Messages.Msg_UserUpdated);
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_ErrorUpdateUser);
            }
        }

        #endregion Methods
    }
}