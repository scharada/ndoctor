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
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace Probel.NDoctor.PluginHost.Host.View
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Microsoft.Windows.Controls.Ribbon;

    using Probel.NDoctor.PluginHost.Core;
    using Probel.NDoctor.PluginHost.Host.ViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow, IPluginHost
    {
        #region Fields

        private StatusMessageVM statusMessage = new StatusMessageVM();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this.statusMessage;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Sets the specified menu in the contextual menu.
        /// </summary>
        /// <param name="menus"></param>
        public void SetContextualMenu(string tabName, MenuInfo[] menus)
        {
            if (menus == null || menus.Length == 0) return;

            this.ContextGroup.Items.Clear();
            this.ContextGroup.Header = menus[0].Group;
            foreach (var menu in menus)
            {
                this.ContextGroup.Items.Add(new RibbonButton()
                {
                    Label = menu.Name,
                    SmallImageSource = menu.ImageSource,
                    Command = menu.Command,
                });
            }
            this.ContextTab.Header = tabName;
            this.ContextTab.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Sets the specified menu in the global menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        public void SetGlobalMenu(MenuInfo menu)
        {
            this.RibbonApplicationMenu.Items.Insert(0, new RibbonApplicationMenuItem()
            {
                Header = menu.Name,
                ImageSource = menu.ImageSource,
                Command = menu.Command,
            });
        }

        /// <summary>
        /// Sets the specified menu in the ribbon menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        public void SetRibbonMenu(MenuInfo menu)
        {
            this.ManagersGroup.Items.Add(new RibbonButton()
            {
                Label = menu.Name,
                SmallImageSource = menu.ImageSource,
                Command = menu.Command,
            });
        }

        /// <summary>
        /// Sets the workbench, that's the main GUI item of the plugin, into the main window.
        /// </summary>
        /// <param name="workbench">The workbench.</param>
        public void SetWorkbench(UserControl workbench)
        {
            if (this.IsDisplayed(workbench)) return;

            if (this.LayoutRoot.Children.Count > 1) this.LayoutRoot.Children.RemoveAt(1);

            this.LayoutRoot.Children.Add(workbench);
            Grid.SetRow(workbench, 1);
            Grid.SetColumn(workbench, 0);

            this.ContextTab.IsSelected = true;
        }

        /// <summary>
        /// Writes the specified status into the status bar.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void WriteStatus(StatusType type, string format, params object[] args)
        {
            this.statusMessage.Message = string.Format(format, args);
            this.statusMessage.Type = type;
        }

        private bool IsDisplayed(UserControl workbench)
        {
            return (this.LayoutRoot.Children.Count > 1 && this.LayoutRoot.Children[1].GetType() == workbench.GetType());
        }

        /// <summary>
        /// Handles the Initialized event of the RibbonWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RibbonWindow_Initialized(object sender, EventArgs e)
        {
            var container = new PluginContainer(this, new MefLoader(), new Version("0.0.0.0"));
            container.SetupPlugins();
            this.WriteStatus(StatusType.Info, "Application started");
        }

        #endregion Methods
    }
}