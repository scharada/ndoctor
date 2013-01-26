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
namespace Probel.NDoctor.Plugins.FamilyViewer
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using System.Windows.Media;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Plugins.FamilyViewer.Helpers;
    using Probel.NDoctor.Plugins.FamilyViewer.Properties;
    using Probel.NDoctor.Plugins.FamilyViewer.View;
    using Probel.NDoctor.Plugins.FamilyViewer.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    [Export(typeof(IPlugin))]
    [PartMetadata(Keys.Constraint, ">3.0.0.0")]
    [PartMetadata(Keys.PluginName, "Family manager")]
    public class PluginManager : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.FamilyViewer;component/Images\{0}.png";

        private ICommand addRelationCommand;
        private ICommand navigateCommand;
        private WorkbenchView workbench;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public PluginManager()
            : base()
        {
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
            Assert.IsNotNull(PluginContext.Host, "To initialise the plugin, IPluginHost should be set.");

            PluginContext.Host.Invoke(() => workbench = new WorkbenchView());
            this.MapWindows();
            this.BuildButtons();
            this.BuildContextMenu();
        }

        /// <summary>
        /// Builds the buttons for the main menu.
        /// </summary>
        private void BuildButtons()
        {
            this.navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());
            var navigateButton = new RibbonButtonData(Messages.Title_FamilyManager
                , imgUri.FormatWith("Users")
                , navigateCommand) { Order = 7 };

            PluginContext.Host.AddInHome(navigateButton, Groups.Managers);
        }

        /// <summary>
        /// Builds the context menu the ribbon for this plugin.
        /// </summary>
        private void BuildContextMenu()
        {
            this.addRelationCommand = new RelayCommand(() => this.NavigateRelation(), () => this.CanNavigateRelation());

            var cgroup = new RibbonGroupData(Messages.Menu_Actions);
            cgroup.ButtonDataCollection.Add(new RibbonButtonData(Messages.Title_AddRelation, imgUri.FormatWith("Add"), addRelationCommand));

            var tab = new RibbonTabData(Messages.Menu_File, cgroup) { ContextualTabGroupHeader = Messages.Title_FamilyManager };
            PluginContext.Host.AddTab(tab);

            this.ContextualMenu = new RibbonContextualTabGroupData(Messages.Title_FamilyManager, tab) { Background = Brushes.OrangeRed, IsVisible = false, };
            PluginContext.Host.AddContextualMenu(this.ContextualMenu);
        }

        private bool CanNavigate()
        {
            return PluginContext.Host.SelectedPatient != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Read);
        }

        private bool CanNavigateRelation()
        {
            return PluginContext.Host.SelectedPatient != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Read)
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        /// <summary>
        /// Configures the mapping for AutoMapper.
        /// </summary>
        private void ConfigureAutoMapper()
        {
            //Add the mapping here...
        }

        private void MapWindows()
        {
            ViewService.Configure(e =>
            {
                e.Bind<AddRelationView, AddRelationViewModel>()
                 .OnClosing(vm => MvvmHelper.GetViewModel(workbench).Refresh());
            });
        }

        private void Navigate()
        {
            try
            {
                if (PluginContext.Host.Navigate(this.workbench))
                {
                    (this.workbench.DataContext as WorkbenchViewModel).Refresh();
                    this.ShowContextMenu();
                }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void NavigateRelation()
        {
            ViewService.Manager.ShowDialog<AddRelationViewModel>();
        }

        #endregion Methods
    }
}