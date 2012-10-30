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
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.FamilyManager.Helpers;
    using Probel.NDoctor.Plugins.FamilyManager.Properties;
    using Probel.NDoctor.Plugins.FamilyManager.View;
    using Probel.NDoctor.Plugins.FamilyManager.ViewModel;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;
    using Probel.NDoctor.View.Toolbox.Navigation;

    [Export(typeof(IPlugin))]
    public class FamilyManager : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.FamilyManager;component/Images\{0}.png";

        private readonly ViewService ViewService = new ViewService();

        private ICommand navAddRelationCommand;
        private ICommand navigateCommand;
        private ICommand navRemoveRelationCommand;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public FamilyManager([Import("version")] Version version)
            : base(version)
        {
            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);

            this.ConfigureAutoMapper();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialises this plugin. Basicaly it should configure the menus into the PluginHost
        /// Every task that could throw exception should be in this method and not in the ctor.
        /// </summary>
        public override void Initialise()
        {
            Assert.IsNotNull(PluginContext.Host, "PluginContext.Host");

            PluginContext.Host.Invoke(() =>
            {
                this.BuildButtons();
                this.BuildContextMenu();
            });
        }

        /// <summary>
        /// Builds the buttons for the main menu.
        /// </summary>
        private void BuildButtons()
        {
            #region Navigate
            this.navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());

            var navigateButton = this.BuildMainNavigationButton();
            PluginContext.Host.AddInHome(navigateButton, Groups.Managers);
            #endregion

            #region Relation add

            this.navAddRelationCommand = new RelayCommand(() =>
            {
                try
                {
                    InnerWindow.Show(Messages.Btn_Add, new AddFamilyView());
                }
                catch (Exception ex) { this.Handle.Error(ex, Messages.Msg_FailToLoadFamilyManager); }
            }, () => PluginContext.DoorKeeper.IsUserGranted(To.Write));
            #endregion

            #region Relation remove
            this.navRemoveRelationCommand = new RelayCommand(() =>
            {
                try
                {
                    InnerWindow.Show(Messages.Btn_Remove, this.ViewService.NewRemoveFamilyView());
                }
                catch (Exception ex) { this.Handle.Error(ex, Messages.Msg_FailToLoadFamilyManager); }
            }, () => PluginContext.DoorKeeper.IsUserGranted(To.Write));
            #endregion
        }

        /// <summary>
        /// Builds the context menu of this plugin.
        /// </summary>
        private void BuildContextMenu()
        {
            var cgroup = new RibbonGroupData(Messages.Menu_Actions);

            cgroup.ButtonDataCollection.Add(new RibbonButtonData(Messages.Title_AddFamilyManager, imgUri.FormatWith("Add"), navAddRelationCommand));
            cgroup.ButtonDataCollection.Add(new RibbonButtonData(Messages.Title_RemoveFamilyManager, imgUri.FormatWith("Delete"), navRemoveRelationCommand));

            var tab = new RibbonTabData(Messages.Menu_File, cgroup) { ContextualTabGroupHeader = Messages.Title_FamilyManager };
            PluginContext.Host.AddTab(tab);

            this.contextualMenu = new RibbonContextualTabGroupData(Messages.Title_FamilyManager, tab) { Background = Brushes.OrangeRed, IsVisible = false };
            PluginContext.Host.AddContextualMenu(this.contextualMenu);
        }

        private RibbonButtonData BuildMainNavigationButton()
        {
            var navigateButton = new RibbonButtonData(Messages.Title_FamilyManager
                , imgUri.FormatWith("Users")
                , navigateCommand) { Order = 7 };
            return navigateButton;
        }

        private bool CanNavigate()
        {
            return PluginContext.Host.SelectedPatient != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Read);
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<LightPatientDto, LightPatientViewModel>();
        }

        private void Navigate()
        {
            try
            {
                var view = new WorkbenchView();
                this.ViewService.GetViewModel(view).Refresh();
                PluginContext.Host.Navigate(view);

                this.ShowContextMenu();
            }
            catch (Exception ex)
            {
                this.Handle.Error(ex, Messages.Msg_FailToLoadFamilyManager);
            }
        }

        #endregion Methods
    }
}