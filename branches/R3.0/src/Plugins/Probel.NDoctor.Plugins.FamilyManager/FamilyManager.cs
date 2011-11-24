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
namespace Probel.NDoctor.Plugins.FamilyManager
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
    using Probel.NDoctor.Plugins.FamilyManager.Properties;
    using Probel.NDoctor.Plugins.FamilyManager.View;
    using Probel.NDoctor.Plugins.FamilyManager.ViewModel;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    using StructureMap;

    [Export(typeof(IPlugin))]
    public class FamilyManager : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.FamilyManager;component/Images\{0}.png";

        private RibbonContextualTabGroupData contextualMenu;
        private ICommand navAddRelationCommand;
        private ICommand navigateCommand;
        private ICommand navRemoveRelationCommand;
        private Workbench workbench;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public FamilyManager([Import("version")] Version version, [Import("host")] IPluginHost host)
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
            #region Navigate
            this.navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());

            var navigateButton = this.BuildMainNavigationButton();
            this.Host.AddInHome(navigateButton, Groups.Managers);
            #endregion

            #region Relation add

            this.navAddRelationCommand = new RelayCommand(() =>
            {
                try
                {
                    if (this.ViewModel != null) this.ViewModel.Reset();

                    InnerWindow.Show(Messages.Btn_Add, new AddFamilyWorkbench());
                }
                catch (Exception ex)
                {
                    this.HandleError(ex, Messages.Msg_FailToLoadFamilyManager);
                }
            });
            #endregion

            #region Relation remove
            this.navRemoveRelationCommand = new RelayCommand(() =>
            {
                try
                {
                    if (this.ViewModel != null) this.ViewModel.Reset();

                    InnerWindow.Show(Messages.Btn_Remove, new RemoveFamilyWorkbench());
                }
                catch (Exception ex)
                {
                    this.HandleError(ex, Messages.Msg_FailToLoadFamilyManager);
                }
            });
            #endregion
        }

        /// <summary>
        /// Builds the context menu of this plugin.
        /// </summary>
        private void BuildContextMenu()
        {
            var navAddRelationButton = new RibbonButtonData(Messages.Title_AddFamilyManager, imgUri.StringFormat("Add"), navAddRelationCommand);
            var navRemoveRelationButton = new RibbonButtonData(Messages.Title_RemoveFamilyManager, imgUri.StringFormat("Delete"), navRemoveRelationCommand);

            var cgroup = new RibbonGroupData(Messages.Menu_Actions);

            cgroup.ButtonDataCollection.Add(navAddRelationButton);
            cgroup.ButtonDataCollection.Add(navRemoveRelationButton);

            var tab = new RibbonTabData(Messages.Menu_File, cgroup) { ContextualTabGroupHeader = Messages.Title_FamilyManager };
            this.Host.AddTab(tab);

            this.contextualMenu = new RibbonContextualTabGroupData(Messages.Title_FamilyManager, tab) { Background = Brushes.OrangeRed, IsVisible = false };
            this.Host.AddContextualMenu(this.contextualMenu);
        }

        private RibbonButtonData BuildMainNavigationButton()
        {
            var navigateButton = new RibbonButtonData(Messages.Title_FamilyManager
                , imgUri.StringFormat("Users")
                , navigateCommand) { Order = 7 };
            return navigateButton;
        }

        private bool CanNavigate()
        {
            return this.Host.SelectedPatient != null;
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<LightPatientDto, LightPatientViewModel>();
        }

        private void ConfigureStructureMap()
        {
            ObjectFactory.Configure(x =>
            {
                x.For<IFamilyComponent>().Add<FamilyComponent>();
                x.SelectConstructor<FamilyComponent>(() => new FamilyComponent());

                x.For<IMedicalRecordComponent>().Add<MedicalRecordComponent>();
                x.SelectConstructor<MedicalRecordComponent>(() => new MedicalRecordComponent());
            });
        }

        private void Navigate()
        {
            try
            {
                this.ViewModel.Refresh();
                this.Host.Navigate(this.workbench);

                this.contextualMenu.IsVisible = true;
                this.contextualMenu.TabDataCollection[0].IsSelected = true;
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_FailToLoadFamilyManager);
            }
        }

        #endregion Methods
    }
}