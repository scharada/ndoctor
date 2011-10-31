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
namespace Probel.NDoctor.Plugins.PathologyManager
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PathologyManager.Properties;
    using Probel.NDoctor.Plugins.PathologyManager.View;
    using Probel.NDoctor.Plugins.PathologyManager.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    [Export(typeof(IPlugin))]
    public class PathologyManager : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.PathologyManager;component/Images\{0}.png";

        private ICommand navigateCommand = null;
        private Workbench workbench;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public PathologyManager([Import("version")] Version version, [Import("host")] IPluginHost host)
            : base(version, host)
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
                Assert.IsNotNull(this.Host, "The IPluginHost is not set. It is impossible to setup the data context of the workbench of the plugin medical record");
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
            Assert.IsNotNull(this.Host, "To initialise the plugin, IPluginHost should be set.");

            this.Host.Invoke(() => workbench = new Workbench());
            this.BuildButtons();
            this.BuildContextMenu();
        }

        /// <summary>
        /// Builds the buttons for the main menu.
        /// </summary>
        private void BuildButtons()
        {
            this.navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());

            var navigateButton = new RibbonButtonData(Messages.Title_PathologyManager
                , imgUri.StringFormat("PathologyManager")
                , navigateCommand) { Order = 3 };

            this.Host.AddInHome(navigateButton, Groups.Managers);
        }

        /// <summary>
        /// Builds the context menu of this plugin.
        /// </summary>
        private void BuildContextMenu()
        {
            //This plugin doesn't have contextual menu
        }

        private bool CanNavigate()
        {
            return this.Host.SelectedPatient != null;
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<IllnessPeriodViewModel, IllnessPeriodDto>();
            Mapper.CreateMap<IllnessPeriodDto, IllnessPeriodViewModel>();

            Mapper.CreateMap<PathologyDto, IllnessPeriodToAddViewModel>()
                .ForMember(src => src.Pathology, opt => opt.MapFrom(dest => dest));
        }

        private void Navigate()
        {
            try
            {
                this.ViewModel.Refresh();
                this.Host.WriteStatusReady();
                this.Host.Navigate(this.workbench);
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_FailToLoadPathologyManager);
            }
        }

        #endregion Methods
    }
}