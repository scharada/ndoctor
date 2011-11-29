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
namespace Probel.NDoctor.Plugins.PrescriptionManager
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using System.Windows.Media;

    using AutoMapper;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PrescriptionManager.Properties;
    using Probel.NDoctor.Plugins.PrescriptionManager.View;
    using Probel.NDoctor.Plugins.PrescriptionManager.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    using StructureMap;

    [Export(typeof(IPlugin))]
    public class PrescriptionManager : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.PrescriptionManager;component/Images\{0}.png";

        private AddPrescriptionView addPrescriptionView;
        private RibbonContextualTabGroupData contextualMenu;
        private bool isSaveCommandActivated = false;
        private ICommand navAddPrescriptionCommand;
        private ICommand navigateCommand;
        private ICommand saveCommand;
        private Workbench workbench;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public PrescriptionManager([Import("version")] Version version)
            : base(version)
        {
            this.ConfigureStructureMap();
            this.ConfigureAutoMapper();

            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);
        }

        #endregion Constructors

        #region Properties

        private AddPrescriptionViewModel AddPrescriptionViewModel
        {
            get
            {
                Assert.IsNotNull(PluginContext.Host, string.Format(
                    "The IPluginHost is not set. It is impossible to setup the data context of the workbench of the plugin '{0}'", this.GetType().Name));
                if (this.addPrescriptionView.DataContext == null) this.workbench.DataContext = new AddPrescriptionViewModel();
                return this.addPrescriptionView.DataContext as AddPrescriptionViewModel;
            }
            set
            {
                Assert.IsNotNull(this.workbench.DataContext);
                this.workbench.DataContext = value;
            }
        }

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

                this.addPrescriptionView = new AddPrescriptionView();
                this.addPrescriptionView.DataContext = new AddPrescriptionViewModel();
            });

            this.BuildButtons();
            this.BuildContextMenu();
        }

        /// <summary>
        /// Builds the buttons for the main menu.
        /// </summary>
        private void BuildButtons()
        {
            #region Navigate
            this.navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());

            var navigateButton = new RibbonButtonData(Messages.Title_PrescriptionManager
                    , imgUri.StringFormat("Prescription")
                    , navigateCommand) { Order = 4 };
            PluginContext.Host.AddInHome(navigateButton, Groups.Managers);
            #endregion

            this.navAddPrescriptionCommand = new RelayCommand(() => this.NavigateAddPrescription());
            this.saveCommand = new RelayCommand(() => this.Save(), () => CanSave());
        }

        /// <summary>
        /// Builds the context menu of this plugin.
        /// </summary>
        private void BuildContextMenu()
        {
            var navAddPrescriptionButton = new RibbonButtonData(Messages.Title_AddPrescription
                , imgUri.StringFormat("Add")
                , navAddPrescriptionCommand);

            var navWorkbenchButton = new RibbonButtonData(Messages.Title_PrescriptionManager
                , imgUri.StringFormat("Prescription")
                , navigateCommand);

            var saveButton = new RibbonButtonData(Messages.Btn_Save
                , imgUri.StringFormat("Save")
                , saveCommand);

            var cgroup = new RibbonGroupData(Messages.Menu_Actions);

            cgroup.ButtonDataCollection.Add(saveButton);
            cgroup.ButtonDataCollection.Add(navWorkbenchButton);
            cgroup.ButtonDataCollection.Add(navAddPrescriptionButton);

            var tab = new RibbonTabData(Messages.Menu_File, cgroup) { ContextualTabGroupHeader = Messages.Title_PrescriptionManager };
            PluginContext.Host.AddTab(tab);

            this.contextualMenu = new RibbonContextualTabGroupData(Messages.Title_PrescriptionManager, tab) { Background = Brushes.OrangeRed, IsVisible = false };
            PluginContext.Host.AddContextualMenu(this.contextualMenu);
        }

        private bool CanNavigate()
        {
            return PluginContext.Host.SelectedPatient != null;
        }

        private bool CanSave()
        {
            return this.isSaveCommandActivated;
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<DrugDto, DrugViewModel>();
        }

        private void ConfigureStructureMap()
        {
            ObjectFactory.Configure(x =>
            {
                x.For<IPrescriptionComponent>().Add<PrescriptionComponent>();
                x.SelectConstructor<PrescriptionComponent>(() => new PrescriptionComponent());
            });
        }

        private void Navigate()
        {
            try
            {
                this.isSaveCommandActivated = false;
                PluginContext.Host.Navigate(this.workbench);
                this.workbench.DataContext = this.ViewModel;

                this.contextualMenu.IsVisible = true;
                this.contextualMenu.TabDataCollection[0].IsSelected = true;
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_FailToLoadPrescriptionManager.StringFormat(ex.Message));
            }
        }

        private void NavigateAddPrescription()
        {
            try
            {
                //var page = new AddPrescriptionView();
                this.isSaveCommandActivated = true;
                var datacontext = new AddPrescriptionViewModel();
                datacontext.Refresh();
                this.addPrescriptionView.DataContext = datacontext;
                PluginContext.Host.Navigate(this.addPrescriptionView);

                this.contextualMenu.IsVisible = true;
                this.contextualMenu.TabDataCollection[0].IsSelected = true;
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_FailToLoadPrescriptionManager.StringFormat(ex.Message));
            }
        }

        private void Save()
        {
            (this.addPrescriptionView.DataContext as AddPrescriptionViewModel).Save();
        }

        #endregion Methods
    }
}