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
    using System.Windows.Media;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.Mvvm;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Plugins.PrescriptionManager.Properties;
    using Probel.NDoctor.Plugins.PrescriptionManager.View;
    using Probel.NDoctor.Plugins.PrescriptionManager.ViewModel;
    using Probel.NDoctor.View;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.MenuData;

    [Export(typeof(IPlugin))]
    [PartMetadata(Keys.Constraint, ">3.0.0.0")]
    [PartMetadata(Keys.PluginId, "{283FDC8B-FA71-44D1-9750-3C0413B36008}")]
    public class PrescriptionManager : Plugin
    {
        #region Fields

        private const string icoUri = @"\Probel.NDoctor.Plugins.PrescriptionManager;component/Images\{0}.ico";
        private const string imgUri = @"\Probel.NDoctor.Plugins.PrescriptionManager;component/Images\{0}.png";

        private bool isSaveCommandActivated = false;
        private bool isSearching = false;
        private LastNavigation lastNavigation;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public PrescriptionManager()
            : base()
        {
            this.lastNavigation = LastNavigation.None;
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

        #region Properties

        private AddPrescriptionView AddPrescriptionView
        {
            get { return LazyLoader.Get<AddPrescriptionView>(); }
        }

        private WorkbenchView WorkbenchView
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

            this.ConfigureViewService();
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
                    , new RelayCommand(() => this.NavigateWorkbench(), () => this.CanNavigateWorkbench())) { Order = 4 };
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
                , new RelayCommand(() => this.NavigateAddPrescription(), () => CanNavigateToAddPrescription()));

            var navWorkbenchButton = new RibbonButtonData(Messages.Title_PrescriptionManager
                , imgUri.FormatWith("Prescription")
                , new RelayCommand(() => this.NavigateWorkbench(), () => this.CanNavigatePrescription()));

            var saveButton = new RibbonButtonData(Messages.Btn_Save
                , imgUri.FormatWith("Save")
                , new RelayCommand(() => this.Save(), () => CanSave()));

            var navSearchButton = new RibbonButtonData(Messages.Btn_Search
                , icoUri.FormatWith("Search")
                , new RelayCommand(() => ViewService.Manager.ShowDialog<SearchPrescriptionViewModel>(), () => this.CanNavigateSearch()));

            var addDrugButton = new RibbonButtonData(Messages.Btn_AddDrug
                , imgUri.FormatWith("Drug")
                , new RelayCommand(() => ViewService.Manager.ShowDialog<AddDrugViewModel>()
                    , () => this.lastNavigation == LastNavigation.AddPrescription && PluginContext.DoorKeeper.IsUserGranted(To.Write)));

            var addDrugTypeButton = new RibbonButtonData(Messages.Btn_AddDrugType
                , imgUri.FormatWith("DrugType")
                , new RelayCommand(() => ViewService.Manager.ShowDialog<AddDrugTypeViewModel>()
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

            this.ContextualMenu = new RibbonContextualTabGroupData(Messages.Title_PrescriptionManager, tab) { Background = Brushes.OrangeRed, IsVisible = false };
            PluginContext.Host.AddContextualMenu(this.ContextualMenu);
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
            return this.lastNavigation != LastNavigation.AddPrescription
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private bool CanNavigateWorkbench()
        {
            return PluginContext.Host.SelectedPatient != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private bool CanSave()
        {
            return this.isSaveCommandActivated
                && this.AddPrescriptionView.As<AddPrescriptionViewModel>().SaveCommand.CanExecute(null);
        }

        private void ConfigureViewService()
        {
            LazyLoader.Set<AddPrescriptionView>(() => new AddPrescriptionView());
            LazyLoader.Set<WorkbenchView>(() => new WorkbenchView());

            ViewService.Configure(e =>
            {
                e.Bind<AddDrugTypeView, AddDrugTypeViewModel>()
                    .OnClosing(() => this.AddPrescriptionView.As<AddPrescriptionViewModel>().Refresh());
                e.Bind<AddDrugView, AddDrugViewModel>()
                    .OnShow(vm => vm.Refresh());
                e.Bind<SearchDrugView, SearchDrugViewModel>()
                    .OnShow(vm =>
                    {
                        this.isSearching = true;
                        vm.Refresh();
                    })
                    .OnClosing(() => this.isSearching = false);
                e.Bind<SearchPrescriptionView, SearchPrescriptionViewModel>()
                    .OnClosing(vm => this.WorkbenchView.As<WorkbenchViewModel>().RefreshPrescriptions(vm.StartCriteria, vm.EndCriteria));
                e.Bind<EditionView, EditionViewModel>()
                    .OnClosing(() => this.WorkbenchView.As<WorkbenchViewModel>().Refresh());
            });
        }

        private void LoadDefaultPrescriptions()
        {
            var component = PluginContext.ComponentFactory.GetInstance<IPrescriptionComponent>();
            var from = DateTime.Today.AddMonths(-1);
            var to = DateTime.Today;

            var found = component.GetPrescriptionsByDates(PluginContext.Host.SelectedPatient, from, to);
        }

        private void NavigateAddPrescription()
        {
            try
            {
                if (PluginContext.Host.Navigate(this.AddPrescriptionView))
                {
                    this.isSaveCommandActivated = true;
                    this.AddPrescriptionView.As<AddPrescriptionViewModel>().Refresh();
                    this.ShowContextMenu();
                    this.lastNavigation = LastNavigation.AddPrescription;
                }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void NavigateWorkbench()
        {
            try
            {
                if (PluginContext.Host.Navigate(this.WorkbenchView))
                {
                    this.isSaveCommandActivated = false;
                    this.ShowContextMenu();
                    this.lastNavigation = LastNavigation.Workbench;
                    this.LoadDefaultPrescriptions();
                }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void Save()
        {
            this.AddPrescriptionView.As<AddPrescriptionViewModel>().SaveCommand.TryExecute();
        }

        #endregion Methods
    }
}