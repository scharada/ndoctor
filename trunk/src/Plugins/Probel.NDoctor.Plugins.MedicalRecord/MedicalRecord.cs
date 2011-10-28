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

namespace Probel.NDoctor.Plugins.MedicalRecord
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using System.Windows.Media;

    using AutoMapper;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Dto;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.Plugins.MedicalRecord.View;
    using Probel.NDoctor.Plugins.MedicalRecord.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    using StructureMap;

    [Export(typeof(IPlugin))]
    public class MedicalRecord : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.MedicalRecord;component/Images\{0}.png";

        private MedicalRecordComponent component = new MedicalRecordComponent();
        private RibbonContextualTabGroupData contextualMenu;
        private ICommand navigateCommand;
        private ICommand saveCommand;
        private Workbench workbench;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public MedicalRecord([Import("version")] Version version, [Import("host")] IPluginHost host)
            : base(version, host)
        {
            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);

            this.ConfigureStructureMap();
            this.ConfigureAutoMapper();
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
            Assert.IsNotNull(this.Host, "To initialise the plugin, IPluginHost should be set.");

            this.Host.Invoke(() => workbench = new Workbench());
            this.BuildButtons();
            this.BuildContextMenu();
        }

        private void BuildButtons()
        {
            this.navigateCommand = new RelayCommand(() => this.NavigateAdd(), () => this.CanNavigateAdd());
            this.saveCommand = new RelayCommand(() => this.Save());

            var navigateButton = new RibbonButtonData(Messages.Title_MedicalRecord
                , imgUri.StringFormat("MedicalRecord")
                , navigateCommand) { Order = 2 };

            this.Host.AddInHome(navigateButton, Groups.Managers);
        }

        private void BuildContextMenu()
        {
            var saveButton = new RibbonButtonData(Messages.Title_Save, imgUri.StringFormat("Save"), saveCommand);
            var cgroup = new RibbonGroupData(Messages.Menu_Actions);

            cgroup.ButtonDataCollection.Add(saveButton);

            var tab = new RibbonTabData(Messages.Menu_File, cgroup) { ContextualTabGroupHeader = Messages.Title_MedicalRecord };
            this.Host.Add(tab);

            this.contextualMenu = new RibbonContextualTabGroupData(Messages.Title_MedicalRecord, tab) { Background = Brushes.OrangeRed, IsVisible = false };
            this.Host.Add(this.contextualMenu);
        }

        private bool CanNavigateAdd()
        {
            return this.Host.SelectedPatient != null;
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<MedicalRecordCabinetDto, TitledMedicalRecordCabinetDto>();
            Mapper.CreateMap<MedicalRecordDto, TitledMedicalRecordDto>();
            Mapper.CreateMap<MedicalRecordFolderDto, TitledMedicalRecordFolderDto>();
        }

        private void ConfigureStructureMap()
        {
            ObjectFactory.Configure(x =>
            {
                x.For<IMedicalRecordComponent>().Add<MedicalRecordComponent>();
                x.SelectConstructor<MedicalRecordComponent>(() => new MedicalRecordComponent());
            });
        }

        private void NavigateAdd()
        {
            try
            {
                this.ViewModel.Refresh();
                this.Host.Navigate(this.workbench);

                this.contextualMenu.IsVisible = true;
                this.contextualMenu.TabDataCollection[0].IsSelected = true;
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_FailToLoadMedicalRecords.StringFormat(ex.Message));
            }
        }

        private void Save()
        {
            using (this.component.UnitOfWork)
            {
                this.ViewModel.Cabinet.ForEachRecord(x => x.Html = this.ViewModel.SelectedRecord.Html
                    , s => s.Id == this.ViewModel.SelectedRecord.Id);

                this.ViewModel.Save();
            }
            this.Host.WriteStatus(StatusType.Info, Messages.Msg_RecordsSaved);
        }

        #endregion Methods
    }
}