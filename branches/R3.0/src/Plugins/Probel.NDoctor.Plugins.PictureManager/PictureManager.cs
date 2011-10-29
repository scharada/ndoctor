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

namespace Probel.NDoctor.Plugins.PictureManager
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using System.Windows.Media;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Plugins.PictureManager.Properties;
    using Probel.NDoctor.Plugins.PictureManager.View;
    using Probel.NDoctor.Plugins.PictureManager.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    using StructureMap;

    [Export(typeof(IPlugin))]
    public class PictureManager : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.PictureManager;component/Images\{0}.png";

        private RibbonContextualTabGroupData contextualMenu;
        private ICommand navigateCommand;
        private Workbench workbench;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureManager"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="host">The host.</param>
        [ImportingConstructor]
        public PictureManager([Import("version")] Version version, [Import("host")] IPluginHost host)
            : base(version, host)
        {
            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);
            this.ConfigureStructureMap();
        }

        #endregion Constructors

        #region Properties

        private WorkbenchViewModel ViewModel
        {
            get
            {
                Assert.IsNotNull(this.Host, "The IPluginHost is not set. It is impossible to setup the data context of the workbench of the plugin medical record");
                if (this.workbench.DataContext == null) this.workbench.DataContext = new WorkbenchViewModel();
                return this.workbench.DataContext as WorkbenchViewModel;
            }
            set
            {
                Assert.IsNotNull(this.workbench.DataContext);
                this.workbench.DataContext = value;
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
            this.Host.Invoke(() =>
            {
                this.workbench = new Workbench();
                this.workbench.DataContext = this.ViewModel;
            });
            this.BuildButtons();
            this.BuildContextMenu();
        }

        private void BuildButtons()
        {
            this.navigateCommand = new RelayCommand(() => Navigate(), () => this.CanNavigate());

            var navigateButton = new RibbonButtonData(Messages.Title_PictureManager
                    , imgUri.StringFormat("Picture")
                    , navigateCommand) { Order = 5 };

            this.Host.AddInHome(navigateButton, Groups.Managers);
        }

        private void BuildContextMenu()
        {
            var addPicButton = new RibbonButtonData(Messages.Title_BtnAddPic, imgUri.StringFormat("Add"), this.GetAddPicCommand());
            var saveButton = new RibbonButtonData(Messages.Title_Save, imgUri.StringFormat("Save"), this.GetSaveCommand());
            var cgroup = new RibbonGroupData(Messages.Menu_Actions);

            cgroup.ButtonDataCollection.Add(saveButton);
            cgroup.ButtonDataCollection.Add(addPicButton);

            var tab = new RibbonTabData(Messages.Menu_File, cgroup) { ContextualTabGroupHeader = Messages.Title_Pictures };
            this.Host.Add(tab);

            this.contextualMenu = new RibbonContextualTabGroupData(Messages.Title_Pictures, tab) { Background = Brushes.OrangeRed, IsVisible = false };
            this.Host.Add(this.contextualMenu);
        }

        private bool CanNavigate()
        {
            return this.Host.SelectedPatient != null;
        }

        private void ConfigureStructureMap()
        {
            ObjectFactory.Configure(x =>
            {
                x.For<IPictureComponent>().Add<PictureComponent>();
                x.SelectConstructor<PictureComponent>(() => new PictureComponent());
            });
        }

        private ICommand GetAddPicCommand()
        {
            ICommand cmd = null;
            this.Host.Invoke(() => cmd = this.ViewModel.AddPictureCommand);
            return cmd;
        }

        private ICommand GetSaveCommand()
        {
            ICommand cmd = null;
            this.Host.Invoke(() => cmd = this.ViewModel.SaveCommand);
            return cmd;
        }

        private void Navigate()
        {
            try
            {
                this.Host.Navigate(this.workbench);
                this.workbench.DataContext = this.ViewModel;

                this.ViewModel.Refresh();

                this.contextualMenu.IsVisible = true;
                this.contextualMenu.TabDataCollection[0].IsSelected = true;
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_FailToLoadMedicalPictures.StringFormat(ex.Message));
            }
        }

        #endregion Methods
    }
}