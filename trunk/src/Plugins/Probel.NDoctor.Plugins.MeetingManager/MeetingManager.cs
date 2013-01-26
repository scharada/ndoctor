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
    using Probel.Mvvm;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO;
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
    [PartMetadata(Keys.Constraint, ">3.0.0.0")]
    [PartMetadata(Keys.PluginName, "Meeting manager")]
    public class MeetingManager : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.MeetingManager;component/Images\{0}.png";

        private readonly ICalendarComponent Component = PluginContext.ComponentFactory.GetInstance<ICalendarComponent>();

        private ICommand navigateCommand;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public MeetingManager()
            : base()
        {
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

        private WorkbenchView View
        {
            get { return LazyLoader.Get<WorkbenchView>(); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initialises this plugin. Basicaly it should configure the menus into the PluginHost
        /// Every task that could throw exception should be in this method and not in the ctor.
        /// </summary>
        public override void Initialise()
        {
            Assert.IsNotNull(PluginContext.Host, "PluginContext.Host");

            new SettingsConfigurator().Add(Messages.Title_MeetingsManager, () => new SettingsView());
            this.ConfigureViewService();
            this.BuildButtons();
            this.BuildContextMenu();

            if (new PluginSettings().IsGoogleCalendarActivated)
            {
                this.Component.SpinUpGoogle(new PluginSettings().GetGoogleConfiguration());
            }
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
            this.ContextualMenu = new RibbonContextualTabGroupData(Messages.Title_MeetingsManager, tab) { Background = Brushes.OrangeRed, IsVisible = false, };
            PluginContext.Host.AddContextualMenu(this.ContextualMenu);
            PluginContext.Host.AddTab(tab);

            ICommand addCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<AddMeetingViewModel>(), () => IsCalendatEditor);
            cgroup.ButtonDataCollection.Add(new RibbonButtonData(Messages.Title_AddMeeting, imgUri.FormatWith("Add"), addCommand) { Order = 1, });

            ICommand removeCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<RemoveMeetingViewModel>(), () => IsCalendatEditor);
            cgroup.ButtonDataCollection.Add(new RibbonButtonData(Messages.Title_RemoveMeeting, imgUri.FormatWith("Delete"), removeCommand) { Order = 2, });

            ICommand addCategoryCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<AddCategoryViewModel>(), () => IsCalendatEditor);
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

        private void ConfigureViewService()
        {
            LazyLoader.Set<WorkbenchView>(() => new WorkbenchView());
            ViewService.Configure(e =>
            {
                e.Bind<AddCategoryView, AddCategoryViewModel>()
                    .OnClosing(() => this.View.As<WorkbenchViewModel>().Refresh());

                e.Bind<AddMeetingView, AddMeetingViewModel>()
                    .OnClosing(() => this.View.As<WorkbenchViewModel>().Refresh());

                e.Bind<RemoveMeetingView, RemoveMeetingViewModel>()
                    .OnClosing(() => this.View.As<WorkbenchViewModel>().Refresh());
            });
        }

        private void Navigate()
        {
            try
            {
                if (PluginContext.Host.Navigate(this.View))
                {
                    this.View.As<WorkbenchViewModel>().Refresh();
                    this.ShowContextMenu();
                }
            }
            catch (Exception ex) { this.Handle.Error(ex, Messages.Msg_FailToLoadCalendar); }
        }

        #endregion Methods
    }
}