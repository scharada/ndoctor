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
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PrescriptionManager.Helpers;
    using Probel.NDoctor.Plugins.PrescriptionManager.Properties;
    using Probel.NDoctor.Plugins.PrescriptionManager.View;
    using Probel.NDoctor.Plugins.PrescriptionManager.ViewModel;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    [Export(typeof(IPlugin))]
    public class PrescriptionManager : Plugin
    {
        #region Fields

        private const string icoUri = @"\Probel.NDoctor.Plugins.PrescriptionManager;component/Images\{0}.ico";
        private const string imgUri = @"\Probel.NDoctor.Plugins.PrescriptionManager;component/Images\{0}.png";

        private readonly ViewService ViewService = new ViewService();

        private bool isSaveCommandActivated = false;
        private bool isSearching = false;
        private LastNavigation lastNavigation;
        private ICommand navAddPrescriptionCommand;
        private ICommand navPrescriptionCommand;
        private ICommand navSearchCommand;
        private ICommand navWorkbenchCommand;
        private ICommand saveCommand;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public PrescriptionManager([Import("version")] Version version)
            : base(version)
        {
            this.lastNavigation = LastNavigation.None;
            this.ConfigureAutoMapper();

            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);

            this.saveCommand = new RelayCommand(() => this.Save(), () => CanSave());
            this.navSearchCommand = new RelayCommand(() => this.NavigateSearch(), () => this.CanNavigateSearch());
            this.navAddPrescriptionCommand = new RelayCommand(() => this.NavigateAddPrescription(), () => CanNavigateToAddPrescription());
            this.navWorkbenchCommand = new RelayCommand(() => this.NavigateWorkbench(), () => this.CanNavigateWorkbench());
            this.navPrescriptionCommand = new RelayCommand(() => this.NavigateWorkbench(), () => this.CanNavigatePrescription());
        }

        #endregion Constructors

        #region Enumerations

        private enum LastNavigation
        {
            Workbench,
            AddPrescription,
            None,
        }

        #endregion Enumerations

        #region Methods

        public override void Close()
        {
            this.ViewService.CloseAll();
        }

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
            #region Navigate

            var navigateButton = new RibbonButtonData(Messages.Title_PrescriptionManager
                    , imgUri.FormatWith("Prescription")
                    , navWorkbenchCommand) { Order = 4 };
            PluginContext.Host.AddInHome(navigateButton, Groups.Managers);
            #endregion
        }

        /// <summary>
        /// Builds the context menu of this plugin.
        /// </summary>
        private void BuildContextMenu()
        {
            var navAddPrescriptionButton = new RibbonButtonData(Messages.Title_AddPrescription
                , imgUri.FormatWith("Add")
                , navAddPrescriptionCommand);

            var navWorkbenchButton = new RibbonButtonData(Messages.Title_PrescriptionManager
                , imgUri.FormatWith("Prescription")
                , navPrescriptionCommand);

            var saveButton = new RibbonButtonData(Messages.Btn_Save
                , imgUri.FormatWith("Save")
                , saveCommand);

            var navSearchButton = new RibbonButtonData(Messages.Btn_Search
                , icoUri.FormatWith("Search")
                , navSearchCommand);

            var addDrugButton = new RibbonButtonData(Messages.Btn_AddDrug
                , imgUri.FormatWith("Drug")
                , new RelayCommand(() =>
                {
                    var view = new AddDrugView();
                    var model = new AddDrugViewModel();
                    model.Refresh();
                    view.DataContext = model;

                    InnerWindow.Show(Messages.Btn_Add, view);
                }
                    , () => this.lastNavigation == LastNavigation.AddPrescription && PluginContext.DoorKeeper.IsUserGranted(To.Write)));

            var addDrugTypeButton = new RibbonButtonData(Messages.Btn_AddDrugType
                , imgUri.FormatWith("DrugType")
                , new RelayCommand(() => InnerWindow.Show(Messages.Btn_Add, new AddDrugTypeView())
                    , () => this.lastNavigation == LastNavigation.AddPrescription && PluginContext.DoorKeeper.IsUserGranted(To.Write)));

            var cgroup = new RibbonGroupData(Messages.Menu_Actions);
            var ngroup = new RibbonGroupData(Messages.Menu_Navigation);
            var mgroup = new RibbonGroupData(Messages.Menu_Manage);

            cgroup.ButtonDataCollection.Add(saveButton);
            cgroup.ButtonDataCollection.Add(navSearchButton);

            ngroup.ButtonDataCollection.Add(navWorkbenchButton);
            ngroup.ButtonDataCollection.Add(navAddPrescriptionButton);

            mgroup.ButtonDataCollection.Add(addDrugButton);
            mgroup.ButtonDataCollection.Add(addDrugTypeButton);

            var tab = new RibbonTabData(Messages.Menu_File) { ContextualTabGroupHeader = Messages.Title_PrescriptionManager };
            tab.GroupDataCollection.Add(cgroup);
            tab.GroupDataCollection.Add(ngroup);
            tab.GroupDataCollection.Add(mgroup);

            PluginContext.Host.AddTab(tab);

            this.contextualMenu = new RibbonContextualTabGroupData(Messages.Title_PrescriptionManager, tab) { Background = Brushes.OrangeRed, IsVisible = false };
            PluginContext.Host.AddContextualMenu(this.contextualMenu);
        }

        private bool CanNavigatePrescription()
        {
            return this.CanNavigateWorkbench()
                && this.lastNavigation != LastNavigation.Workbench;
        }

        private bool CanNavigateSearch()
        {
            return this.lastNavigation == LastNavigation.Workbench
                && !this.isSearching;
        }

        private bool CanNavigateToAddPrescription()
        {
            return this.lastNavigation != LastNavigation.AddPrescription && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private bool CanNavigateWorkbench()
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

        private void LoadDefaultPrescriptions()
        {
            var component = PluginContext.ComponentFactory.GetInstance<IPrescriptionComponent>();
            using (component.UnitOfWork)
            {
                var from = DateTime.Today.AddMonths(-1);
                var to = DateTime.Today;

                var found = component.FindPrescriptionsByDates(PluginContext.Host.SelectedPatient, from, to);

                Notifyer.OnPrescriptionFound(this, new PrescriptionResultDto(found, from, to));
            }
        }

        private void NavigateAddPrescription()
        {
            try
            {
                this.isSaveCommandActivated = true;
                this.ViewService.AddPrescriptionViewModel.Refresh();
                PluginContext.Host.Navigate(this.ViewService.AddPrescriptionView);

                this.ShowContextMenu();
                this.lastNavigation = LastNavigation.AddPrescription;
            }
            catch (Exception ex)
            {
                this.HandleError(ex);
            }
        }

        private void NavigateSearch()
        {
            this.isSearching = true;
            InnerWindow.Closed += (sender, e) => this.isSearching = false;
            InnerWindow.Show(Messages.Btn_Search, new SearchPrescriptionView());
        }

        private void NavigateWorkbench()
        {
            try
            {
                this.isSaveCommandActivated = false;
                PluginContext.Host.Navigate(this.ViewService.WorkbenchView);

                this.ShowContextMenu();
                this.lastNavigation = LastNavigation.Workbench;
                this.LoadDefaultPrescriptions();
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private void Save()
        {
            this.ViewService.AddPrescriptionViewModel.SaveCommand.TryExecute();
        }

        #endregion Methods
    }
}