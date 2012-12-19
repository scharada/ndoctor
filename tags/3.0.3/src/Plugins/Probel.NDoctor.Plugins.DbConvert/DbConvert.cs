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
namespace Probel.NDoctor.Plugins.DbConvert
{
    using System;
    using System.ComponentModel.Composition;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.Mvvm;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Plugins.DbConvert.Properties;
    using Probel.NDoctor.Plugins.DbConvert.View;
    using Probel.NDoctor.Plugins.DbConvert.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    [Export(typeof(IPlugin))]
    public class DbConvert : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.DbConvert;component/Images\{0}.png";

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public DbConvert([Import("version")] Version version)
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

            this.ConfigureViewService();
            this.BuildButtons();
            this.BuildContextMenu();
        }

        /// <summary>
        /// Builds the buttons for the main menu.
        /// </summary>
        private void BuildButtons()
        {
            var navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());

            var navigateButton = new RibbonButtonData(Messages.Title_DbConvertManager
                    , imgUri.FormatWith("Source")
                    , navigateCommand) { Order = 4 };

            PluginContext.Host.AddToApplicationMenu(navigateButton);
        }

        /// <summary>
        /// Builds the context menu of this plugin.
        /// </summary>
        private void BuildContextMenu()
        {
            /*Nothing to do*/
        }

        private bool CanNavigate()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Administer);
        }

        private void ConfigureAutoMapper()
        {
            /*Nothing to do*/
        }

        private void ConfigureViewService()
        {
            LazyLoader.Set<WorkbenchView>(() => new WorkbenchView());
            ViewService.Configure(e =>
            {
                e.Bind<WorkbenchView, WorkbenchViewModel>();
            });
        }

        private void Navigate()
        {
            ViewService.Manager.ShowDialog<WorkbenchViewModel>();
        }

        #endregion Methods
    }
}