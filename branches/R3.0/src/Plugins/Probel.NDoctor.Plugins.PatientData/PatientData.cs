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
namespace Probel.NDoctor.Plugins.PatientData
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using System.Windows.Media;

    using AutoMapper;

    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientData.Properties;
    using Probel.NDoctor.Plugins.PatientData.View;
    using Probel.NDoctor.Plugins.PatientData.ViewModel;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    using StructureMap;
    using Probel.Helpers.Assertion;

    [Export(typeof(IPlugin))]
    public class PatientData : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.PatientData;component/Images\{0}.png";

        private ICommand addDoctorCommand;
        private ICommand addInsuranceCommand;
        private ICommand addPracticeCommand;
        private ICommand addProfessionCommand;
        private ICommand addReputationCommand;
        private ICommand addSpecialisationCommand;
        private IPatientDataComponent component;
        private RibbonContextualTabGroupData contextualMenu;
        private ICommand navigateCommand;
        private ICommand rollbackCommand;
        private ICommand saveCommand;
        private Workbench workbench;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public PatientData([Import("version")] Version version)
            : base(version)
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
                if (this.workbench == null) this.workbench = new Workbench();

                return this.workbench.DataContext as WorkbenchViewModel;
            }
        }

        #endregion Properties

        #region Methods

        public override void Initialise()
        {
            this.component = ObjectFactory.GetInstance<IPatientDataComponent>();
            this.BuildButtons();
            this.BuildContextMenu();
        }

        private void BuildButtons()
        {
            this.navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());
            this.saveCommand = new RelayCommand(() => this.ViewModel.Save());
            this.rollbackCommand = new RelayCommand(() => this.ViewModel.Rollback(), () => this.ViewModel.CanRollback);

            this.addDoctorCommand = new RelayCommand(() => InnerWindow.Show(Messages.Title_AddDoctor, new DoctorView()));
            this.addSpecialisationCommand = new RelayCommand(() => InnerWindow.Show(Messages.Title_AddSpecialisation, new AddSpecialisationView()));
            this.addInsuranceCommand = new RelayCommand(() => InnerWindow.Show(Messages.Title_AddInsurance, new AddInsuranceView()));
            this.addReputationCommand = new RelayCommand(() => InnerWindow.Show(Messages.Title_AddReputation, new AddReputationView()));
            this.addPracticeCommand = new RelayCommand(() => InnerWindow.Show(Messages.Title_AddPractice, new AddPracticeView()));
            this.addProfessionCommand = new RelayCommand(() => InnerWindow.Show(Messages.Title_AddProfession, new AddProfessionView()));

            var navigateButton = new RibbonButtonData(Messages.Title_PatientDataManager
                    , imgUri.FormatWith("Properties")
                    , navigateCommand) { Order = 1 };

            PluginContext.Host.AddInHome(navigateButton, Groups.Managers);
        }

        private void BuildContextMenu()
        {
            var saveButton = new RibbonButtonData(Messages.Title_Save, imgUri.FormatWith("Save"), this.saveCommand);

            var rollbackButton = new RibbonButtonData(Messages.Title_Rollback, imgUri.FormatWith("Save"), this.rollbackCommand);

            var splitter = new RibbonMenuItemData(Messages.Btn_Add, imgUri.FormatWith("Add"), null);
            splitter.ControlDataCollection.Add(new RibbonMenuItemData(Messages.Title_AddDoctor, imgUri.FormatWith("Add"), this.addDoctorCommand));
            splitter.ControlDataCollection.Add(new RibbonMenuItemData(Messages.Title_AddSpecialisation, imgUri.FormatWith("Add"), this.addSpecialisationCommand));
            splitter.ControlDataCollection.Add(new RibbonMenuItemData(Messages.Title_AddInsurance, imgUri.FormatWith("Add"), this.addInsuranceCommand));
            splitter.ControlDataCollection.Add(new RibbonMenuItemData(Messages.Title_AddReputation, imgUri.FormatWith("Add"), this.addReputationCommand));
            splitter.ControlDataCollection.Add(new RibbonMenuItemData(Messages.Title_AddPractice, imgUri.FormatWith("Add"), this.addPracticeCommand));
            splitter.ControlDataCollection.Add(new RibbonMenuItemData(Messages.Title_AddProfession, imgUri.FormatWith("Add"), this.addProfessionCommand));

            var cgroup = new RibbonGroupData(Messages.Menu_Actions);
            cgroup.ButtonDataCollection.Add(saveButton);
            cgroup.ButtonDataCollection.Add(rollbackButton);
            cgroup.ButtonDataCollection.Add(splitter);

            var tab = new RibbonTabData(Messages.Menu_File) { ContextualTabGroupHeader = Messages.Title_ContextMenu };
            tab.GroupDataCollection.Add(cgroup);
            PluginContext.Host.AddTab(tab);

            this.contextualMenu = new RibbonContextualTabGroupData(Messages.Title_ContextMenu, tab) { Background = Brushes.OrangeRed, IsVisible = false };
            PluginContext.Host.AddContextualMenu(this.contextualMenu);
        }

        private bool CanNavigate()
        {
            return PluginContext.Host.SelectedPatient != null;
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<LightDoctorDto, LightDoctorViewModel>();
            Mapper.CreateMap<LightDoctorViewModel, LightDoctorDto>();
        }

        private void ConfigureStructureMap()
        {
            ObjectFactory.Configure(x =>
            {
                x.For<IPatientDataComponent>().Add<PatientDataComponent>();
                x.SelectConstructor<PatientDataComponent>(() => new PatientDataComponent());
            });
        }

        private void Navigate()
        {
            Assert.IsNotNull(this.ViewModel, "ViewModel");

            PluginContext.Host.Navigate(this.workbench);
            this.ViewModel.Refresh();
            this.workbench.DataContext = this.ViewModel;

            this.contextualMenu.IsVisible = true;
            this.contextualMenu.TabDataCollection[0].IsSelected = true;
        }

        #endregion Methods
    }
}