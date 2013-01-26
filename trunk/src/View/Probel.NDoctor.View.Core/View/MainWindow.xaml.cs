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
namespace Probel.NDoctor.View.Core.View
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using AvalonDock;

    using log4net;

    using Microsoft.Windows.Controls.Ribbon;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Exceptions;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;
    using Probel.NDoctor.View.Toolbox;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow, IPluginHost
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));

        private readonly Page Startpage;

        private LightUserDto connectedUser;
        public object LastDestination { get; private set; }

        #endregion Fields

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            PluginContext.Host = this;
            this.Startpage = new StartPage();
            this.DataContext = new MainWindowViewModel();
            this.WriteStatus(StatusType.Info, Messages.Msg_Ready);

            this.workbench.NavigationService.Navigating += (sender, e) => this.OnNavigating(LastDestination, e.Content);

            this.Closing += (sender, e) => this.OnDisconnecting();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when before new patient is connected.
        /// </summary>
        public event EventHandler BeforeNewPatientConnected;

        /// <summary>
        /// Occurs when current user is disconnecting or the main window is closing.
        /// </summary>
        public event EventHandler Disconnecting;

        /// <summary>
        /// Occurs when use navigate to a new workbench.
        /// The EventArgs Contains the previous and the current destination
        /// </summary>
        public event EventHandler<NavigationRouteEventArgs> Navigating;

        /// <summary>
        /// Occurs when a patient session is closed.
        /// </summary>
        public event EventHandler NewPatientConnected;

        /// <summary>
        /// Occurs when a new user has connected.
        /// </summary>
        public event EventHandler NewUserConnected;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in back navigation
        ///  history.
        /// </summary>
        /// <value>
        ///  <c>true</c> if there is at least one entry in back navigation history; otherwise, <c>false</c>.
        /// </value>
        public bool CanNavigateBack
        {
            get
            {
                return this.workbench.NavigationService.CanGoBack;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in forward
        /// navigation history.
        /// </summary>
        /// <value>
        /// <c>true</c> if there is at least one entry in forward navigation history; otherwise,<c>false</c>.
        /// </value>
        public bool CanNavigateForward
        {
            get
            {
                return this.workbench.NavigationService.CanGoForward;
            }
        }

        /// <summary>
        /// Gets or sets the connecyed user.
        /// If set to null, it means no user are connected into nDoctor
        /// </summary>
        /// <value>
        /// The user.
        /// </value>        
        public LightUserDto ConnectedUser
        {
            get { return this.connectedUser; }
            set
            {
                if (this.connectedUser != null) { this.OnDisconnecting(); }
                this.connectedUser = value;
                this.RefreshDataContext(this.connectedUser);
            }
        }

        /// <summary>
        /// Gets the version of the host. It is used for the plugin validation.
        /// </summary>
        public Version HostVersion
        {
            get { return Assembly.GetCallingAssembly().GetName().Version; }
        }

        /// <summary>
        /// Gets the owner of all toolboxes. That's the main window of nDoctor
        /// </summary>
        public Window Root
        {
            get { return this; }
        }

        /// <summary>
        /// Gets the search algorithm to apply on a search.
        /// </summary>
        /// <value>
        /// The type of the search.
        /// </value>
        public SearchOn SearchType
        {
            get
            {
                return Settings.Default.SearchType;
            }
        }

        public LightPatientDto SelectedPatient
        {
            get
            {
                if (this.DataContext != null && this.DataContext is MainWindowViewModel)
                {
                    return (this.DataContext as MainWindowViewModel).SelectedPatient;
                }
                else { throw new WrongDataContextException(); }
            }
            set
            {
                if (this.DataContext != null && this.DataContext is MainWindowViewModel)
                {
                    this.OnBeforeNewPatientConnected();
                    (this.DataContext as MainWindowViewModel).SelectedPatient = value;
                    this.OnNewPatientConnected();
                }
                else { throw new WrongDataContextException(); }
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds the specified context tab.
        /// </summary>
        /// <param name="contextTab">The context tab.</param>
        public void AddContextualMenu(RibbonContextualTabGroupData contextTab)
        {
            this.Dispatcher.Invoke((Action)delegate { App.RibbonData.ContextualTabGroupDataCollection.Add(contextTab); });
        }

        /// <summary>
        /// Adds a new side menu window
        /// </summary>
        /// <param name="title">The title of the side menu.</param>
        /// <param name="control">The control that will be inserted into the side menu.</param>
        public void AddDockablePane(string title, UserControl control)
        {
            var dockable = new DockableContent()
            {
                Title = title,
                Content = control,
                IsCloseable = false,
                DockableStyle = DockableStyle.AutoHide,
            };
            dockable.Show(this.dockManger);
        }

        /// <summary>
        /// Adds the specified button in the specified group in home menu.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="group">The group.</param>
        public void AddInHome(RibbonControlData button, Groups group)
        {
            string criteria = this.GetGroupName(group);

            var tab = (from menu in App.RibbonData.TabDataCollection
                       where menu.Header == Messages.Title_Home
                       select menu).FirstOrDefault();

            if (tab == null) throw new PluginException(Messages.PluginException_HomeMenuVoid);

            this.Dispatcher.Invoke((Action)delegate
            {
                switch (group)
                {
                    case Groups.Managers:
                        this.AddButton(tab, Messages.Title_SessionManagers, button);
                        break;
                    case Groups.Tools:
                        this.AddButton(tab, Messages.Title_Tools, button);
                        break;
                    case Groups.GlobalTools:
                        this.AddButton(tab, Messages.Title_GlobalTools, button);
                        break;
                    default:
                        Assert.FailOnEnumeration(group);
                        break;
                }
            });
        }

        /// <summary>
        /// Adds the specified tab.
        /// </summary>
        /// <param name="tab">The tab.</param>
        public void AddTab(RibbonTabData tab)
        {
            this.Dispatcher.Invoke((Action)delegate { App.RibbonData.TabDataCollection.Add(tab); });
        }

        /// <summary>
        /// Adds the specified control into the application menu.
        /// </summary>
        /// <param name="control">The control.</param>
        public void AddToApplicationMenu(RibbonControlData control)
        {
            this.Dispatcher.Invoke((Action)delegate
            {
                App.RibbonData.ApplicationMenuData.ControlDataCollection.Add(control);
                App.RibbonData.ApplicationMenuData.ControlDataCollection.Refill(
                    App.RibbonData.ApplicationMenuData.ControlDataCollection.OrderBy(e => e.Order).ToList());
            });
        }

        /// <summary>
        /// Gets in the home menu the control with the specified name in the specified group.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="group">The group.</param>
        /// <returns>
        /// The searched control or null is not found
        /// </returns>
        public RibbonBase GetInHome(string name, Groups group)
        {
            string criteria = this.GetGroupName(group);

            var tab = (from menu in App.RibbonData.TabDataCollection
                       where menu.Header == Messages.Title_Home
                       select menu).FirstOrDefault();
            if (tab == null) throw new PluginException(Messages.PluginException_HomeMenuVoid);

            foreach (var grp in tab.GroupDataCollection)
            {
                var result = (from control in grp.ButtonDataCollection
                              where control.Name == name
                              select control).FirstOrDefault();
                if (result != null) return result;
            }

            return null;
        }

        /// <summary>
        /// Deactivates the menu.
        /// </summary>
        public void HideMainMenu()
        {
            this.Dispatcher.Invoke((Action)delegate { this.ribbon.Visibility = System.Windows.Visibility.Collapsed; });
        }

        /// <summary>
        /// Executes the specified delegate synchronously on the thread
        /// the System.Windows.Threading.Dispatcher is associated with.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void Invoke(Action action)
        {
            this.Dispatcher.Invoke(action);
        }

        /// <summary>
        /// Navigates to specified page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>
        ///   <c>True</c> if the navigation was allowed; otherwise <c>False</c>
        /// </returns>
        public bool Navigate(object page)
        {
            Assert.IsNotNull(page, "page");
            var result = false;

            this.Dispatcher.Invoke((Action)delegate
            {
                if (this.LastDestination is IWorkbench)
                {
                    var wb = LastDestination as IWorkbench;
                    if (wb.CanLeave() || (!wb.CanLeave() && wb.AskToLeave()))
                    {
                        this.Goto(page);
                        result = true;
                    }
                }
                else
                {
                    this.Goto(page);
                    result = true;
                }
            });
            return result;
        }

        /// <summary>
        /// Navigates to start page.
        /// </summary>
        /// <returns>
        ///   <c>True</c> if the navigation was allowed; otherwise <c>False</c>
        /// </returns>
        public bool NavigateToStartPage()
        {
            var result = this.Navigate(Startpage);
            this.Dispatcher.Invoke((Action)delegate
            {
                if (this.Startpage.DataContext is StartPageViewModel)
                {
                    (this.Startpage.DataContext as StartPageViewModel).RefreshStatisticsCommand.TryExecute();
                }
            });
            return result;
        }

        /// <summary>
        /// Sets the arrow cursor.
        /// </summary>
        public void SetArrowCursor()
        {
            PluginContext.Host.Invoke(() => Mouse.OverrideCursor = Cursors.Arrow);
        }

        /// <summary>
        /// Sets the wait cursor.
        /// </summary>
        public void SetWaitCursor()
        {
            PluginContext.Host.Invoke(() => Mouse.OverrideCursor = Cursors.Wait);
        }

        /// <summary>
        /// Activates the menu.
        /// </summary>
        public void ShowMainMenu()
        {
            this.Dispatcher.Invoke((Action)delegate { this.ribbon.Visibility = System.Windows.Visibility.Visible; });
        }

        /// <summary>
        /// Writes a message in the StatusBar.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        public void WriteStatus(StatusType type, string message)
        {
            var context = this.DataContext as MainWindowViewModel;
            if (context != null)
            {
                context.Message = message;
                context.Type = type;
            }
        }

        /// <summary>
        /// Write the status "Ready." in the status box
        /// </summary>
        public void WriteStatusReady()
        {
            this.WriteStatus(StatusType.Info, Messages.Msg_Ready);
        }

        protected void OnDisconnecting()
        {
            if (this.Disconnecting != null)
            {
                this.Disconnecting(this, EventArgs.Empty);
            }
        }

        private void AddButton(RibbonTabData tab, string goupName, RibbonControlData button)
        {
            var group = (from g in tab.GroupDataCollection
                         where g.Header == goupName
                         select g).FirstOrDefault();

            if (group == null) throw new PluginException(Messages.PluginException_HomeMenuVoid);

            group.ButtonDataCollection.Add(button);
            // I use ToList() because it makes a copy of the list. Otherwise, clearing the ObservableCollection
            // clears the linq result.
            group.ButtonDataCollection.Refill(group.ButtonDataCollection.OrderBy(e => e.Order).ToList());
        }

        private string GetGroupName(Groups group)
        {
            string criteria = string.Empty;
            switch (group)
            {
                case Groups.Managers:
                    criteria = Messages.Title_SessionManagers;
                    break;
                case Groups.Tools:
                    criteria = Messages.Title_Tools;
                    break;
                case Groups.GlobalTools:
                    criteria = Messages.Title_GlobalTools;
                    break;
                default:
                    Assert.FailOnEnumeration(group);
                    break;
            }
            return criteria;
        }

        private void Goto(object page)
        {
            // Hide other context menus otherwise, the context menu of this page
            // will be added to the others.
            foreach (var context in App.RibbonData.ContextualTabGroupDataCollection)
            { context.IsVisible = false; }

            this.workbench.NavigationService.Navigate(page);
            this.WriteStatus(StatusType.Info, Messages.Msg_Ready);
            this.LastDestination = page;
        }

        private void OnBeforeNewPatientConnected()
        {
            if (this.BeforeNewPatientConnected != null)
            {
                this.BeforeNewPatientConnected(this, EventArgs.Empty);
            }
        }

        private void OnNavigating(object from, object to)
        {
            if (this.Navigating != null)
            {
                this.Navigating(this, new NavigationRouteEventArgs(from, to));
            }
        }

        private void OnNewPatientConnected()
        {
            if (this.NewPatientConnected != null)
            {
                this.NewPatientConnected(this, EventArgs.Empty);
            }
        }

        private void OnNewUserConnected()
        {
            if (this.NewUserConnected != null)
            {
                this.NewUserConnected(this, EventArgs.Empty);
            }
        }

        private void RefreshDataContext(LightUserDto lightUserDto)
        {
            this.Dispatcher.Invoke((Action)delegate
            {
                if (this.DataContext != null && this.DataContext is MainWindowViewModel)
                {
                    (this.DataContext as MainWindowViewModel).ConnectedUser = lightUserDto;
                }
                else { throw new WrongDataContextException(); }
            });
            this.OnNewUserConnected();
        }

        private void this_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
#if DEBUG
            this.WindowState = System.Windows.WindowState.Normal;
#else
            this.WindowState = System.Windows.WindowState.Maximized;
#endif
        }

        private void WriteStatus(LightPatientDto value)
        {
            var name = string.Format("{0} {1}", value.FirstName, value.LastName);
            this.WriteStatus(StatusType.Info, Messages.Msg_SelectPatient.FormatWith(name));
        }

        #endregion Methods

        private void this_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.LastDestination is IWorkbench)
            {
                var wb = LastDestination as IWorkbench;
                if (!(wb.CanLeave() || (!wb.CanLeave() && wb.AskToLeave())))
                {
                    e.Cancel = true;
                }
            }
        }
    }
}