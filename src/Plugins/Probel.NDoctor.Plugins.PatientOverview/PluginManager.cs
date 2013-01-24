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
namespace Probel.NDoctor.Plugins.PatientOverview
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using System.Windows.Media;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Plugins.PatientOverview.Properties;
    using Probel.NDoctor.Plugins.PatientOverview.View;
    using Probel.NDoctor.Plugins.PatientOverview.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    [Export(typeof(IPlugin))]
    public class PluginManager : StaticViewPlugin<WorkbenchView>
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.PatientOverview;component/Images\{0}.png";

        private readonly ICommand editCommand;
        private readonly ICommand revertCommand;
        private readonly ICommand saveCommand;

        private ICommand navigateCommand;
        private WorkbenchView workbench;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public PluginManager([Import("version")] Version version)
            : base(version)
        {
            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);

            this.saveCommand = new RelayCommand(() => this.Save(), () => this.CanSave());
            this.editCommand = new RelayCommand(() => this.Edit(), () => this.CanEdit());
            this.revertCommand = new RelayCommand(() => this.Revert(), () => this.CanRevert());

            this.ConfigureAutoMapper();
        }

        #endregion Constructors

        #region Properties

        public ICommand EditCommand
        {
            get { return this.editCommand; }
        }

        public ICommand RevertCommand
        {
            get { return this.revertCommand; }
        }

        public ICommand SaveCommand
        {
            get { return this.saveCommand; }
        }

        private bool IsEditionActivated
        {
            get;
            set;
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

            PluginContext.Host.Invoke(() => workbench = new WorkbenchView());
            this.BuildButtons();
            this.BuildContextMenu();
        }

        /// <summary>
        /// Builds the buttons for the main menu.
        /// </summary>
        private void BuildButtons()
        {
            this.navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());

            var navigateButton = new RibbonButtonData(Messages.Title_PluginName
                    , imgUri.FormatWith("Card")
                    , navigateCommand) { Order = 1 };

            PluginContext.Host.AddInHome(navigateButton, Groups.Managers);
        }

        /// <summary>
        /// Builds the context menu the ribbon for this plugin.
        /// </summary>
        private void BuildContextMenu()
        {
            var editBtn = new RibbonButtonData(Messages.Btn_Edit, imgUri.FormatWith("Edit"), this.EditCommand);
            var saveBtn = new RibbonButtonData(Messages.Btn_Save, imgUri.FormatWith("Save"), this.SaveCommand);
            var revertBtn = new RibbonButtonData(Messages.Btn_Revert, imgUri.FormatWith("Revert"), this.RevertCommand);

            var cgroup = new RibbonGroupData(BaseText.Group_Action);
            cgroup.ButtonDataCollection.Add(editBtn);
            cgroup.ButtonDataCollection.Add(saveBtn);
            cgroup.ButtonDataCollection.Add(revertBtn);

            var tab = new RibbonTabData(BaseText.Menu_File) { ContextualTabGroupHeader = Messages.Title_PluginName };
            tab.GroupDataCollection.Add(cgroup);
            PluginContext.Host.AddTab(tab);

            this.contextualMenu = new RibbonContextualTabGroupData(Messages.Title_PluginName, tab) { Background = Brushes.OrangeRed, IsVisible = false };
            PluginContext.Host.AddContextualMenu(this.contextualMenu);
        }

        private bool CanEdit()
        {
            return this.IsEditionActivated == false;
        }

        private bool CanNavigate()
        {
            return PluginContext.Host.SelectedPatient != null;
        }

        private bool CanRevert()
        {
            return this.IsEditionActivated == true;
        }

        private bool CanSave()
        {
            return this.IsEditionActivated == true;
        }

        /// <summary>
        /// Configures the mapping for AutoMapper.
        /// </summary>
        private void ConfigureAutoMapper()
        {
            //Add the mapping here...
        }

        private void Edit()
        {
            this.View.As<WorkbenchViewModel>().IsEditModeActivated
                = this.IsEditionActivated
                = true;
        }

        private void Navigate()
        {
            var vm = this.View.As<WorkbenchViewModel>();
            vm.Refresh();
            this.IsEditionActivated = vm.IsEditModeActivated;

            PluginContext.Host.Navigate(this.View);
            this.ShowContextMenu();
        }

        private void Revert()
        {
            var viewModel = this.View.As<WorkbenchViewModel>();

            viewModel.Refresh();
            viewModel.IsEditModeActivated
                = this.IsEditionActivated
                = false;
        }

        private void Save()
        {
            var viewModel = this.View.As<WorkbenchViewModel>();
            viewModel.SaveCommand.TryExecute();

            viewModel.IsEditModeActivated
                = this.IsEditionActivated
                = false;
        }

        #endregion Methods
    }
}