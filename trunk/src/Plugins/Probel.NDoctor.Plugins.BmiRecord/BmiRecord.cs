#region Header

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

#endregion Header

namespace Probel.NDoctor.Plugins.BmiRecord
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.BmiRecord.Properties;
    using Probel.NDoctor.Plugins.BmiRecord.View;
    using Probel.NDoctor.Plugins.BmiRecord.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    [Export(typeof(IPlugin))]
    public class BmiRecord : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.BmiRecord;component/Images\{0}.png";

        private IBmiComponent component;
        private ICommand navigateCommand;
        private Workbench workbench;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BmiRecord"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="host">The host.</param>
        [ImportingConstructor]
        public BmiRecord([Import("version")] Version version, [Import("host")] IPluginHost host)
            : base(version, host)
        {
            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);

            this.ConfigureAutoMapper();

            this.component = ComponentFactory.BmiComponent;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialises this plugin. Basicaly it should configure the menus into the PluginHost
        /// Every task that could throw exception should be in this method and not in the ctor.
        /// </summary>
        public override void Initialise()
        {
            this.Host.Invoke(() => this.workbench = new Workbench());
            this.BuildButtons();
        }

        private void BuildButtons()
        {
            this.navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());

            var navigateButton = new RibbonButtonData(Messages.Title_BmiRecordManager
                    , imgUri.StringFormat("History")
                    , navigateCommand) { Order = 5 };

            this.Host.AddInHome(navigateButton, Groups.Managers);
        }

        private bool CanNavigate()
        {
            return this.Host.SelectedPatient != null;
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<BmiViewModel, BmiDto>();
            Mapper.CreateMap<BmiDto, BmiViewModel>();
        }

        private void Navigate()
        {
            this.Host.Navigate(this.workbench);
            var viewModel = new WorkbenchViewModel();
            this.workbench.DataContext = viewModel;
            viewModel.Refresh();
        }

        #endregion Methods
    }
}