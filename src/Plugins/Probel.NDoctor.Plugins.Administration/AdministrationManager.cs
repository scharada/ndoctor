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
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using System.Windows.Media;

    using AutoMapper;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Administration.Helpers;
    using Probel.NDoctor.Plugins.Administration.Properties;
    using Probel.NDoctor.Plugins.Administration.View;
    using Probel.NDoctor.Plugins.Administration.ViewModel;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    [Export(typeof(IPlugin))]
    public class AdministrationManager : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.Administration;component/Images\{0}.png";

        //DOTO: replace with the window manager
        private static WorkbenchView __workbenchview; //Dont use the variable, use the property instead

        private ICommand navigateCommand;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public AdministrationManager([Import("version")] Version version)
            : base(version)
        {
            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);

            this.ConfigureAutoMapper();
        }

        #endregion Constructors

        #region Properties

        public WorkbenchView WorkbenchView
        {
            get
            {
                if (__workbenchview == null) { __workbenchview = new WorkbenchView(); }
                return __workbenchview;
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

            var navigateButton = new RibbonButtonData(Messages.Title_AdministratorManager
                    , imgUri.FormatWith("Administration")
                    , navigateCommand) { Order = 4 };

            PluginContext.Host.AddToApplicationMenu(navigateButton);
        }

        /// <summary>
        /// Builds the context menu of this plugin.
        /// </summary>
        private void BuildContextMenu()
        {
            var tab = new RibbonTabData() { Header = BaseText.Menu_File, ContextualTabGroupHeader = Messages.Title_AdministratorManager };
            this.contextualMenu = new RibbonContextualTabGroupData(Messages.Title_AdministratorManager, tab) { Background = Brushes.OrangeRed, IsVisible = false, };
            var cgroup = new RibbonGroupData(BaseText.Group_Action, 1);

            tab.GroupDataCollection.Add(cgroup);
            PluginContext.Host.AddContextualMenu(this.contextualMenu);
            PluginContext.Host.AddTab(tab);

            int i = 0;
            var buttons = new List<RibbonButtonData>();

            buttons.Add(new RibbonButtonData(Messages.Title_AddInsurance, imgUri.FormatWith("New")
                , new RelayCommand(() => InnerWindow.Show(Messages.Title_AddInsurance, new AddInsuranceView()), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddPractice, imgUri.FormatWith("New")
                , new RelayCommand(() => InnerWindow.Show(Messages.Title_AddPractice, new AddPracticeView()), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddDrug, imgUri.FormatWith("New")
                , new RelayCommand(() => InnerWindow.Show(Messages.Title_AddDrug, new AddDrugView()), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddDrugType, imgUri.FormatWith("New")
                , new RelayCommand(() => InnerWindow.Show(Messages.Title_AddDrugType, new AddDrugTypeView()), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddPathologyType, imgUri.FormatWith("New")
                , new RelayCommand(() => InnerWindow.Show(Messages.Title_AddPathologyType, new AddPathologyTypeView()), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddDrug, imgUri.FormatWith("New")
                , new RelayCommand(() => InnerWindow.Show(Messages.Title_AddPathology, new AddPathologyView()), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddProfession, imgUri.FormatWith("New")
                , new RelayCommand(() => InnerWindow.Show(Messages.Title_AddProfession, new AddProfessionView()), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddReputation, imgUri.FormatWith("New")
                , new RelayCommand(() => InnerWindow.Show(Messages.Title_AddReputation, new AddReputationView()), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddPictureType, imgUri.FormatWith("New")
                , new RelayCommand(() => InnerWindow.Show(Messages.Title_AddPictureType, new AddPictureTypeView()), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddSpecialisation, imgUri.FormatWith("New")
                , new RelayCommand(() => InnerWindow.Show(Messages.Title_AddSpecialisation, new AddSpecialisationView()), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddDoctor, imgUri.FormatWith("New")
                , new RelayCommand(() => InnerWindow.Show(Messages.Title_AddDoctor, new AddDoctorView()), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            foreach (var button in buttons) { cgroup.ButtonDataCollection.Add(button); }
        }

        private bool CanNavigate()
        {
            return true && PluginContext.DoorKeeper.IsUserGranted(To.Read);
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<TagViewModel, TagDto>();
            Mapper.CreateMap<TagDto, TagViewModel>();
        }

        private void Navigate()
        {
            PluginContext.Host.Navigate(this.WorkbenchView);
            Notifyer.OnRefreshing(this);

            this.contextualMenu.IsVisible = true;
            this.contextualMenu.TabDataCollection[0].IsSelected = PluginContext.Configuration.AutomaticContextMenu;
        }

        #endregion Methods
    }
}