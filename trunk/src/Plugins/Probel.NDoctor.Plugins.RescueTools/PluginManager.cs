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
namespace Probel.NDoctor.Plugins.RescueTools
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Plugins.RescueTools.Properties;
    using Probel.NDoctor.Plugins.RescueTools.View;
    using Probel.NDoctor.Plugins.RescueTools.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;
    using Probel.NDoctor.Domain.DTO.Components;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Media;

    [Export(typeof(IPlugin))]
    [PartMetadata(Keys.Constraint, ">3.0.0.0")]
    [PartMetadata(Keys.PluginId, "{D70C84A1-AF3C-4FFF-AEAE-7331BE0BC4ED}")]
    public class PluginManager : StaticViewPlugin<Workbench>
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.RescueTools;component/Images\{0}.png";

        private ICommand navigateCommand;

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
            this.BuildButtons();
            this.BuildContextMenu();
        }

        /// <summary>
        /// Builds the buttons for the main menu.
        /// </summary>
        private void BuildButtons()
        {
            this.navigateCommand = new RelayCommand(() => this.Navigate());

            var navigateButton = new RibbonButtonData(Messages.Title_FirstAidManager
                    , imgUri.FormatWith("FirstAidKit")
                    , navigateCommand) { Order = 999 };
            PluginContext.Host.AddToApplicationMenu(navigateButton);

            //PluginContext.Host.AddInHome(navigateButton, Groups.GlobalTools);
        }

        /// <summary>
        /// Builds the context menu the ribbon for this plugin.
        /// </summary>
        private void BuildContextMenu()
        {
            var vacuumButton = new RibbonButtonData(Messages.Btn_Vacuum
                , imgUri.FormatWith("Vacuum")
                , new RelayCommand(() => VacuumDatabase()));
            var cgroup = new RibbonGroupData(Messages.Menu_Actions);
            cgroup.ButtonDataCollection.Add(vacuumButton);

            var tab = new RibbonTabData(Messages.Menu_File) { ContextualTabGroupHeader = Messages.Title_FirstAidManager };
            tab.GroupDataCollection.Add(cgroup);

            PluginContext.Host.AddTab(tab);

            this.ContextualMenu = new RibbonContextualTabGroupData(Messages.Title_FirstAidManager, tab) { Background = Brushes.OrangeRed, IsVisible = false };
            PluginContext.Host.AddContextualMenu(this.ContextualMenu);
        }

        private void VacuumDatabase()
        {
            PluginContext.Host.SetWaitCursor();
            var token = new CancellationTokenSource().Token;
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            var thread = Task.Factory
                .StartNew(() =>
                {
                    PluginContext.ComponentFactory
                       .GetInstance<ISqlComponent>()
                       .VacuumDatabase();
                });
            thread.ContinueWith(t => PluginContext.Host.SetArrowCursor(), token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler); ;
            thread.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
        }

        /// <summary>
        /// Configures the mapping for AutoMapper.
        /// </summary>
        private void ConfigureAutoMapper()
        {
            //Add the mapping here...
        }

        private void Navigate()
        {
            PluginContext.Host.Navigate(this.View);
            this.ShowContextMenu();
        }

        #endregion Methods
    }
}