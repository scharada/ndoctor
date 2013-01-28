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
namespace Probel.NDoctor.Plugins.BmiRecord
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using System.Windows.Media;

    using Probel.Helpers.Strings;
    using Probel.Mvvm;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.BmiRecord.Properties;
    using Probel.NDoctor.Plugins.BmiRecord.View;
    using Probel.NDoctor.Plugins.BmiRecord.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    [Export(typeof(IPlugin))]
    [PartMetadata(Keys.Constraint, ">3.0.0.0")]
    [PartMetadata(Keys.PluginName, "Bmi Manager")]
    public class BmiRecord : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.BmiRecord;component/Images\{0}.png";

        private IBmiComponent component;
        private ICommand navigateCommand;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BmiRecord"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="host">The host.</param>
        [ImportingConstructor]
        public BmiRecord()
            : base()
        {
            this.component = PluginContext.ComponentFactory.GetInstance<IBmiComponent>();
            PluginContext.Host.UserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IBmiComponent>();

            this.ConfigureAutoMapper();
        }

        #endregion Constructors

        #region Properties

        private WorkbenchView Workbench
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
            this.component = PluginContext.ComponentFactory.GetInstance<IBmiComponent>();
            PluginContext.Host.Invoke(() =>
            {
                this.ConfigureViewService();
                this.BuildButtons();
                this.BuildContextMenu();
            });
        }

        private void BuildButtons()
        {
            this.navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());

            var navigateButton = new RibbonButtonData(Messages.Title_BmiRecordManager
                    , imgUri.FormatWith("History")
                    , navigateCommand) { Order = 5 };

            PluginContext.Host.AddInHome(navigateButton, Groups.Managers);
        }

        /// <summary>
        /// Builds the context menu of this plugin.
        /// </summary>
        private void BuildContextMenu()
        {
            var cgroup = new RibbonGroupData(Messages.Menu_Actions, 1);
            var tab = new RibbonTabData() { Header = Messages.Menu_File, ContextualTabGroupHeader = Messages.Title_Bmi };

            tab.GroupDataCollection.Add(cgroup);
            this.ContextualMenu = new RibbonContextualTabGroupData(Messages.Title_Bmi, tab) { Background = Brushes.OrangeRed, IsVisible = false, };
            PluginContext.Host.AddContextualMenu(this.ContextualMenu);
            PluginContext.Host.AddTab(tab);

            ICommand addPeriodCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<AddBmiViewModel>(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write));
            cgroup.ButtonDataCollection.Add(new RibbonButtonData(Messages.Title_AddBmi, imgUri.FormatWith("Add"), addPeriodCommand) { Order = 1, });
        }

        private bool CanNavigate()
        {
            return PluginContext.Host.SelectedPatient != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Read);
        }

        private void ConfigureAutoMapper()
        {
            //Nothing to do
        }

        private void ConfigureViewService()
        {
            LazyLoader.Set<WorkbenchView>(() => new WorkbenchView());
            ViewService.Configure(e =>
            {
                e.Bind<AddBmiView, AddBmiViewModel>()
                    .OnShow(vm => vm.CurrentBmi = new BmiDto() { Height = PluginContext.Host.SelectedPatient.Height })
                    .OnClosing(() => this.Workbench.As<WorkbenchViewModel>().Refresh());
            });
        }

        private void Navigate()
        {
            if (PluginContext.Host.Navigate(this.Workbench))
            {
                this.Workbench.As<WorkbenchViewModel>().Refresh();
                this.ShowContextMenu();
            }
        }

        #endregion Methods
    }
}