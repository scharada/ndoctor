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
    using Probel.Helpers.Strings;
    using Probel.Helpers.WPF.Calendar.Model;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MeetingManager.Helpers;
    using Probel.NDoctor.Plugins.MeetingManager.Properties;
    using Probel.NDoctor.Plugins.MeetingManager.View;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;
    using Probel.NDoctor.View.Toolbox.Navigation;

    [Export(typeof(IPlugin))]
    public class MeetingManager : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.MeetingManager;component/Images\{0}.png";

        private readonly ViewService ViewService = new ViewService();

        private ICommand navigateCommand;

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

        private static bool IsCalendatEditor
        {
            get
            {
                return PluginContext.DoorKeeper.IsUserGranted(To.EditCalendar);
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

            ICommand addCommand = new RelayCommand(() => InnerWindow.Show(Messages.Title_AddMeeting, this.ViewService.NewAddMeetingView()), () => IsCalendatEditor);
            cgroup.ButtonDataCollection.Add(new RibbonButtonData(Messages.Title_AddMeeting, imgUri.FormatWith("Add"), addCommand) { Order = 1, });

            ICommand removeCommand = new RelayCommand(() => InnerWindow.Show(Messages.Title_RemoveMeeting, new RemoveMeetingView()), () => IsCalendatEditor);
            cgroup.ButtonDataCollection.Add(new RibbonButtonData(Messages.Title_RemoveMeeting, imgUri.FormatWith("Delete"), removeCommand) { Order = 2, });

            ICommand addCategoryCommand = new RelayCommand(() => InnerWindow.Show(Messages.Msg_AddCategory, new AddCategoryView()), () => IsCalendatEditor);
            cgroup.ButtonDataCollection.Add(new RibbonButtonData(Messages.Msg_AddCategory, imgUri.FormatWith("AddCategory"), addCategoryCommand) { Order = 3, });
        }

        private bool CanNavigate()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.EditCalendar);
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<AppointmentDto, Appointment>();
        }

        private void Navigate()
        {
            try
            {
                var view = new WorkbenchView();
                //this.ViewService.GetViewModel(view).Refresh();
                PluginContext.Host.Navigate(view);
                this.ShowContextMenu();

                Notifyer.OnRefreshed(this);
            }
            catch (Exception ex)
            {
                this.Handle.Error(ex, Messages.Msg_FailToLoadCalendar);
            }
        }

        #endregion Methods
    }
}