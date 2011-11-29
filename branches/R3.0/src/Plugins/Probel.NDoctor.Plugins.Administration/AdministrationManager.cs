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
namespace Probel.NDoctor.Plugins.Administration
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Administration.Properties;
    using Probel.NDoctor.Plugins.Administration.View;
    using Probel.NDoctor.Plugins.Administration.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    using StructureMap;

    [Export(typeof(IPlugin))]
    public class AdministrationManager : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.Administration;component/Images\{0}.png";

        //private RibbonContextualTabGroupData contextualMenu;
        private ICommand navigateCommand;
        private Workbench workbench;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public AdministrationManager([Import("version")] Version version)
            : base(version)
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

            PluginContext.Host.Invoke(() => workbench = new Workbench());
            this.BuildButtons();
            this.BuildContextMenu();
        }

        /// <summary>
        /// Builds the buttons for the main menu.
        /// </summary>
        private void BuildButtons()
        {
            this.navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());

            var navigateButton = new RibbonButtonData(Messages.Title_AdministratorManager
                    , imgUri.StringFormat("Administration")
                    , navigateCommand) { Order = 4 };

            PluginContext.Host.AddToApplicationMenu(navigateButton);
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
            Mapper.CreateMap<TagViewModel, TagDto>();
            Mapper.CreateMap<TagDto, TagViewModel>();

            Mapper.CreateMap<ProfessionViewModel, ProfessionDto>();
            Mapper.CreateMap<ProfessionDto, ProfessionViewModel>();

            Mapper.CreateMap<ReputationViewModel, ReputationDto>();
            Mapper.CreateMap<ReputationDto, ReputationViewModel>();

            Mapper.CreateMap<DrugViewModel, DrugDto>();
            Mapper.CreateMap<DrugDto, DrugViewModel>();

            Mapper.CreateMap<PathologyDto, PathologyViewModel>();
            Mapper.CreateMap<PathologyViewModel, PathologyDto>();

            Mapper.CreateMap<PracticeDto, PracticeViewModel>();
            Mapper.CreateMap<PracticeViewModel, PracticeDto>();

            Mapper.CreateMap<InsuranceDto, InsuranceViewModel>();
            Mapper.CreateMap<InsuranceViewModel, InsuranceDto>();
        }

        private void ConfigureStructureMap()
        {
            ObjectFactory.Configure(x =>
            {
                x.For<IAdministrationComponent>().Add<AdministrationComponent>();
                x.SelectConstructor<IAdministrationComponent>(() => new AdministrationComponent());
            });
        }

        private void Navigate()
        {
            PluginContext.Host.Navigate(this.workbench);
            var viewModel = new WorkbenchViewModel();
            this.workbench.DataContext = viewModel;
        }

        #endregion Methods
    }
}