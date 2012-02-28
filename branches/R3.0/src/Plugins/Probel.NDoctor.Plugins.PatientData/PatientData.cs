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
            set
            {
                if (this.workbench == null) this.workbench = new Workbench();
                this.workbench.DataContext = value; ;
            }
        }

        #endregion Properties

        #region Methods

        public override void Initialise()
        {
            this.component = ObjectFactory.GetInstance<IPatientDataComponent>();
            PluginContext.Host.Invoke(() =>
            {
                this.ViewModel = new WorkbenchViewModel();
                this.ViewModel.Refresh();
                this.workbench = new Workbench();
            });
            this.BuildButtons();
            this.BuildContextMenu();
        }

        private void BuildButtons()
        {
            this.navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());
            this.saveCommand = new RelayCommand(() => this.ViewModel.Save());
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
            var addDoctorButton = new RibbonButtonData(Messages.Title_AddDoctor, imgUri.FormatWith("Save"), this.addDoctorCommand);
            var addSpecialisationButton = new RibbonButtonData(Messages.Title_AddSpecialisation, imgUri.FormatWith("Save"), this.addSpecialisationCommand);
            var addInsuranceButton = new RibbonButtonData(Messages.Title_AddInsurance, imgUri.FormatWith("Save"), this.addInsuranceCommand);
            var addReputationButton = new RibbonButtonData(Messages.Title_AddReputation, imgUri.FormatWith("Save"), this.addReputationCommand);
            var addPracticeButton = new RibbonButtonData(Messages.Title_AddPractice, imgUri.FormatWith("Save"), this.addPracticeCommand);
            var addProfessionButton = new RibbonButtonData(Messages.Title_AddProfession, imgUri.FormatWith("Save"), this.addProfessionCommand);

            var cgroup = new RibbonGroupData(Messages.Menu_Actions);
            var cgroupAdd = new RibbonGroupData(Messages.Menu_Add);

            cgroup.ButtonDataCollection.Add(saveButton);

            cgroupAdd.ButtonDataCollection.Add(addDoctorButton);
            cgroupAdd.ButtonDataCollection.Add(addSpecialisationButton);
            cgroupAdd.ButtonDataCollection.Add(addInsuranceButton);
            cgroupAdd.ButtonDataCollection.Add(addReputationButton);
            cgroupAdd.ButtonDataCollection.Add(addProfessionButton);

            var tab = new RibbonTabData(Messages.Menu_File) { ContextualTabGroupHeader = Messages.Title_ContextMenu };
            tab.GroupDataCollection.Add(cgroup);
            tab.GroupDataCollection.Add(cgroupAdd);
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
            PluginContext.Host.Navigate(this.workbench);
            var viewModel = new WorkbenchViewModel();
            viewModel.Refresh();
            this.workbench.DataContext = viewModel;

            this.contextualMenu.IsVisible = true;
            this.contextualMenu.TabDataCollection[0].IsSelected = true;
        }

        #endregion Methods
    }
}