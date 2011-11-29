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

    using AutoMapper;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Data;
    using Probel.Helpers.Strings;
    using Probel.Helpers.WPF.Calendar.Model;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MeetingManager.Properties;
    using Probel.NDoctor.Plugins.MeetingManager.View;
    using Probel.NDoctor.Plugins.MeetingManager.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    using StructureMap;

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
        public MeetingManager([Import("version")] Version version, [Import("host")] IPluginHost host)
            : base(version, host)
        {
            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);

            this.ConfigureStructureMap();
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
                , imgUri.StringFormat("Calendar")
                , navigateCommand);

            PluginContext.Host.AddInHome(navigateButton, Groups.GlobalTools);
        }

        /// <summary>
        /// Builds the context menu of this plugin.
        /// </summary>
        private void BuildContextMenu()
        {
            //No context menu
        }

        private bool CanNavigate()
        {
            return true;
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<LightPatientDto, PatientViewModel>()
                .ForMember(src => src.Patient, opt => opt.MapFrom(dest => dest));

            Mapper.CreateMap<DateRange, DateRangeViewModel>()
                  .ConstructUsing(obj => new DateRangeViewModel(obj.StartTime, obj.EndTime, PluginContext.Host));

            Mapper.CreateMap<DateRangeViewModel, AppointmentDto>()
                .ForMember(src => src.User, opt => opt.MapFrom(dest => PluginContext.Host.ConnectedUser))
                .ForMember(src => src.Tag, opt => opt.MapFrom(dest => dest.SelectedTag));

            Mapper.CreateMap<AppointmentDto, DateRangeViewModel>()
                .ForMember(src => src.SelectedTag, opt => opt.MapFrom(dest => dest.Tag))
                .ConstructUsing(obj => new DateRangeViewModel(obj.StartTime, obj.EndTime, PluginContext.Host));

            Mapper.CreateMap<DateRangeViewModel, DateRange>();

            Mapper.CreateMap<AppointmentDto, Appointment>();
        }

        private void ConfigureStructureMap()
        {
            ObjectFactory.Configure(x =>
            {
                x.For<ICalendarComponent>().Add<CalendarComponent>();
                x.SelectConstructor<CalendarComponent>(() => new CalendarComponent());
            });
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
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_FailToLoadCalendar);
            }
        }

        #endregion Methods
    }
}