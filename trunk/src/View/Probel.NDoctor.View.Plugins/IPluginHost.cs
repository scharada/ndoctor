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
namespace Probel.NDoctor.View.Plugins
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Probel.Helpers.Events;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Plugins.MenuData;
    using Probel.NDoctor.View.Toolbox.Navigation;

    /// <summary>
    /// 
    /// </summary>
    public interface IPluginHost : IStatusWriter
    {
        #region Events

        /// <summary>
        /// Occurs when use navigate to a new workbench.
        /// The EventArgs Contains the previous and the current destination
        /// </summary>
        event EventHandler<NavigationRouteEventArgs> Navigating;

        /// <summary>
        /// Occurs when a patient session is closed.
        /// </summary>
        event EventHandler PatientConnected;

        /// <summary>
        /// Occurs when before new patient is connected.
        /// </summary>
        event EventHandler PatientConnecting;

        /// <summary>
        /// Occurs when a new user has connected.
        /// </summary>
        event EventHandler UserConnected;

        /// <summary>
        /// Occurs when current user is disconnecting or the main window is closing.
        /// </summary>
        event EventHandler UserDisconnecting;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets the connected user.
        /// If set to null, it means no user are connected into nDoctor
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        LightUserDto ConnectedUser
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the version of the host. It is used for the plugin validation.
        /// </summary>
        Version HostVersion
        {
            get;
        }

        /// <summary>
        /// Gets the owner of all toolboxes. That's the main window of nDoctor
        /// </summary>
        Window RootWindow
        {
            get;
        }

        /// <summary>
        /// Gets or sets the selected patient.
        /// </summary>
        /// <value>
        /// The patient.
        /// </value>
        LightPatientDto SelectedPatient
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds the specified context tab.
        /// </summary>
        /// <param name="contextTab">The context tab.</param>
        void AddContextualMenu(RibbonContextualTabGroupData contextTab);

        /// <summary>
        /// Adds a new side menu window
        /// </summary>
        /// <param name="title">The title of the side menu.</param>
        /// <param name="control">The control that will be inserted into the side menu.</param>
        void AddDockablePane(string title, UserControl control);

        /// <summary>
        /// Adds the specified button in the specified group in home menu.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="group">The group.</param>
        void AddInHome(RibbonControlData button, Groups group);

        /// <summary>
        /// Adds the specified tab.
        /// </summary>
        /// <param name="tab">The tab.</param>
        void AddTab(RibbonTabData tab);

        /// <summary>
        /// Adds the specified control into the application menu.
        /// </summary>
        /// <param name="control">The control.</param>
        void AddToApplicationMenu(RibbonControlData control);

        /// <summary>
        /// Gets in the home menu the control with the specified name in the specified group.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="group">The group.</param>
        /// <returns>The searched control or null is not found</returns>
        RibbonBase GetInHome(string name, Groups group);

        /// <summary>
        /// Deactivates the menu.
        /// </summary>
        void HideMainMenu();

        /// <summary>
        /// Executes the specified delegate synchronously on the thread 
        /// the System.Windows.Threading.Dispatcher is associated with.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        void Invoke(Action action);

        /// <summary>
        /// Navigates to specified page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns><c>True</c> if the navigation was allowed; otherwise <c>False</c></returns>
        bool Navigate(object page);

        /// <summary>
        /// Navigates to start page.
        /// </summary>
        /// <returns><c>True</c> if the navigation was allowed; otherwise <c>False</c></returns>
        bool NavigateToStartPage();

        /// <summary>
        /// Sets the arrow cursor.
        /// </summary>
        void SetArrowCursor();

        /// <summary>
        /// Sets the wait cursor.
        /// </summary>
        void SetWaitCursor();

        /// <summary>
        /// Activates the menu.
        /// </summary>
        void ShowMainMenu();

        /// <summary>
        /// Write the status "Ready." in the status box
        /// </summary>
        void WriteStatusReady();

        #endregion Methods
    }
}