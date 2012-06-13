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
namespace Probel.NDoctor.Plugins.MeetingManager
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using System.Windows.Media;

    using AutoMapper;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Data;
    using Probel.Helpers.Strings;
    using Probel.Helpers.WPF.Calendar.Model;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MeetingManager.Helpers;
    using Probel.NDoctor.Plugins.MeetingManager.Properties;
    using Probel.NDoctor.Plugins.MeetingManager.View;
    using Probel.NDoctor.Plugins.MeetingManager.ViewModel;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    [Export(typeof(IPlugin))]
    public class MeetingManager : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.MeetingManager;component/Images\{0}.png";

        private RibbonContextualTabGroupData contextualMenu = null;
        private ICommand navigateCommand;
        private Workbench workbench;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public MeetingManager([Import("version")] Version version)
            : base(version)
        {
            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);

            this.ConfigureAutoMapper();
        }

        #endregion Constructors

        #region Properties

        private WorkbenchViewModel ViewModel
        {
            get
            {
                Assert.IsNotNull(PluginContext.Host, string.Format(
                    "The IPluginHost is not set. It is impossible to setup the data context of the workbench of the plugin '{0}'", this.GetType().Name));
                if (this.workbench.DataContext == null) this.workbench.DataContext = new WorkbenchViewModel();
                return this.workbench.DataContext as WorkbenchViewModel;
            }
            set
            {
                Assert.IsNotNull(this.workbench.DataContext);
                this.workbench.DataContext = value;
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
            Assert.IsNotNull(PluginContext.Host, "To initialise the plugin, IPluginHost should be set.");

            PluginContext.Host.Invoke(() =>
            {
                this.workbench = new Workbench();
                this.workbench.DataContext = this.ViewModel;
            });
            this.BuildButtons();
            this.BuildContextMenu();
        }

        /// <summary>
        /// Builds the buttons for the main menu.
        /// </summary>
        private void BuildButtons()
        {
            this.navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());

            var navigateButton = new RibbonButtonData(Messages.Title_Calendar
                , imgUri.FormatWith("Calendar")
                , navigateCommand);

            PluginContext.Host.AddInHome(navigateButton, Groups.GlobalTools);
        }

        /// <summary>
        /// Builds the context menu of this plugin.
        /// </summary>
        private void BuildContextMenu()
        {
            var cgroup = new RibbonGroupData(Messages.Menu_Actions, 1);
            var tab = new RibbonTabData() { Header = Messages.Menu_File, ContextualTabGroupHeader = Messages.Title_MeetingsManager };

            tab.GroupDataCollection.Add(cgroup);
            this.contextualMenu = new RibbonContextualTabGroupData(Messages.Title_MeetingsManager, tab) { Background = Brushes.OrangeRed, IsVisible = false, };
            PluginContext.Host.AddContextualMenu(this.contextualMenu);
            PluginContext.Host.AddTab(tab);

            ICommand addCommand = new RelayCommand(() => InnerWindow.Show(Messages.Title_AddMeeting, new AddMeetingView()));
            cgroup.ButtonDataCollection.Add(new RibbonButtonData(Messages.Title_AddMeeting, imgUri.FormatWith("Add"), addCommand) { Order = 1, });

            ICommand removeCommand = new RelayCommand(() => InnerWindow.Show(Messages.Title_RemoveMeeting, new RemoveMeetingView()));
            cgroup.ButtonDataCollection.Add(new RibbonButtonData(Messages.Title_RemoveMeeting, imgUri.FormatWith("Delete"), removeCommand) { Order = 2, });
        }

        private bool CanNavigate()
        {
            return true;
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<AppointmentDto, Appointment>();
        }

        private void Navigate()
        {
            try
            {
                //this.ViewModel.Refresh();
                PluginContext.Host.Navigate(this.workbench);

                if (this.contextualMenu != null)
                {
                    this.contextualMenu.IsVisible = true;
                    this.contextualMenu.TabDataCollection[0].IsSelected = true;
                }

                Notifyer.OnRefreshed(this);
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_FailToLoadCalendar);
            }
        }

        #endregion Methods
    }
}