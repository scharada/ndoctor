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
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Windows.Media;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Plugins.PatientOverview.Properties;
    using Probel.NDoctor.Plugins.PatientOverview.View;
    using Probel.NDoctor.Plugins.PatientOverview.ViewModel;
    using Probel.NDoctor.View;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.MenuData;
    using Probel.NDoctor.View.Toolbox.Translations;

    [Export(typeof(IPlugin))]
    [PartMetadata(Keys.Constraint, ">3.0.0.0")]
    [PartMetadata(Keys.PluginId, "{7D16F7FE-87D8-4435-AF23-7593379E4986}")]
    public class PluginManager : StaticViewPlugin<WorkbenchView>
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.PatientOverview;component/Images\{0}.png";

        private readonly ICommand deactivateCommand;
        private readonly ICommand editCommand;
        private readonly ICommand revertCommand;
        private readonly ICommand saveCommand;

        private ICommand navigateCommand;
        private WorkbenchView workbench;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public PluginManager()
            : base()
        {
            this.saveCommand = new RelayCommand(() => this.Save(), () => this.CanSave());
            this.editCommand = new RelayCommand(() => this.Edit(), () => this.CanEdit());
            this.revertCommand = new RelayCommand(() => this.Revert(), () => this.CanRevert());
            this.deactivateCommand = new RelayCommand(() => this.Deactivate(), () => this.CanDeactivate());

            this.ConfigureAutoMapper();
        }

        #endregion Constructors

        #region Properties

        public ICommand DeactivateCommand
        {
            get { return this.deactivateCommand; }
        }

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
            this.BuildViewService();
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
        /// Builds the context menu for this plugin's ribbon.
        /// </summary>
        private void BuildContextMenu()
        {
            var cgroup1 = new RibbonGroupData(BaseText.Group_Action);
            cgroup1.ButtonDataCollection.Add(new RibbonButtonData(Messages.Btn_Edit, imgUri.FormatWith("Edit"), this.EditCommand));
            cgroup1.ButtonDataCollection.Add(new RibbonButtonData(Messages.Btn_Save, imgUri.FormatWith("Save"), this.SaveCommand));
            cgroup1.ButtonDataCollection.Add(new RibbonButtonData(Messages.Btn_Revert, imgUri.FormatWith("Revert"), this.RevertCommand));
            cgroup1.ButtonDataCollection.Add(new RibbonButtonData(Messages.Btn_Deactivate, imgUri.FormatWith("Deactivate"), this.DeactivateCommand));

            var cgroup2 = new RibbonGroupData(Messages.Group_Add);
            cgroup2.ButtonDataCollection.Add(new RibbonButtonData(Messages.Btn_Reputation, imgUri.FormatWith("Reputation")
                , new RelayCommand(() => ViewService.Manager.Show<AddReputationViewModel>())));
            cgroup2.ButtonDataCollection.Add(new RibbonButtonData(Messages.Btn_Insurance, imgUri.FormatWith("Insurance")
                , new RelayCommand(() => ViewService.Manager.Show<AddInsuranceViewModel>())));
            cgroup2.ButtonDataCollection.Add(new RibbonButtonData(Messages.Btn_Job, imgUri.FormatWith("Job")
                , new RelayCommand(() => ViewService.Manager.Show<AddProfessionViewModel>())));
            cgroup2.ButtonDataCollection.Add(new RibbonButtonData(Messages.Btn_Practice, imgUri.FormatWith("Practice")
                , new RelayCommand(() => ViewService.Manager.Show<AddPracticeViewModel>())));
            cgroup2.ButtonDataCollection.Add(new RibbonButtonData(Messages.Btn_Specialisation, imgUri.FormatWith("Specialisation")
                , new RelayCommand(() => ViewService.Manager.Show<AddSpecialisationViewModel>())));
            cgroup2.ButtonDataCollection.Add(new RibbonButtonData(Messages.Btn_Doctor, imgUri.FormatWith("Doctor")
                , new RelayCommand(() => ViewService.Manager.Show<AddDoctorViewModel>())));

            var cgroup3 = new RibbonGroupData(Messages.Group_LinkedDoctor);
            cgroup3.ButtonDataCollection.Add(new RibbonButtonData(BaseText.Add, imgUri.FormatWith("DoctorAdd")
                , new RelayCommand(() => ViewService.Manager.Show<BindDoctorViewModel>(), () => this.CanManageDoctor())));
            cgroup3.ButtonDataCollection.Add(new RibbonButtonData(BaseText.Remove, imgUri.FormatWith("DoctorRemove")
                , new RelayCommand(() => ViewService.Manager.Show<UnbindDoctorViewModel>(), () => this.CanManageDoctor())));

            var cgroup4 = new RibbonGroupData(Messages.Group_Tag);
            cgroup4.ButtonDataCollection.Add(new RibbonButtonData(Messages.Btn_BindTag, imgUri.FormatWith("TagAdd")
                , new RelayCommand(() => ViewService.Manager.Show<BindTagViewModel>(), () => this.CanManageTag())));
            cgroup4.ButtonDataCollection.Add(new RibbonButtonData(Messages.Btn_UnbindTag, imgUri.FormatWith("DoctorRemove")
                , new RelayCommand(() => ViewService.Manager.Show<RemoveTagViewModel>(), () => this.CanManageTag())));
            cgroup4.ButtonDataCollection.Add(new RibbonButtonData(BaseText.Add, imgUri.FormatWith("DoctorAdd")
                , new RelayCommand(() => ViewService.Manager.Show<AddTagViewModel>(), () => this.CanManageTag())));

            var tab = new RibbonTabData(BaseText.Menu_File) { ContextualTabGroupHeader = Messages.Title_PluginName };
            tab.GroupDataCollection.Add(cgroup1);
            tab.GroupDataCollection.Add(cgroup2);
            tab.GroupDataCollection.Add(cgroup3);
            tab.GroupDataCollection.Add(cgroup4);
            PluginContext.Host.AddTab(tab);

            this.ContextualMenu = new RibbonContextualTabGroupData(Messages.Title_PluginName, tab) { Background = Brushes.OrangeRed, IsVisible = false };
            PluginContext.Host.AddContextualMenu(this.ContextualMenu);
        }

        private void BuildViewService()
        {
            ViewService.Configure(e =>
            {
                e.Bind<AddReputationView, AddReputationViewModel>()
                    .OnClosing(vm => this.Refresh(vm));
                e.Bind<AddInsuranceView, AddInsuranceViewModel>()
                    .OnClosing(vm => this.Refresh(vm));
                e.Bind<AddPracticeView, AddPracticeViewModel>()
                    .OnClosing(vm => this.Refresh(vm));
                e.Bind<AddProfessionView, AddProfessionViewModel>()
                    .OnClosing(vm => this.Refresh(vm));
                e.Bind<AddSpecialisationView, AddSpecialisationViewModel>()
                    .OnClosing(vm => this.Refresh(vm));
                e.Bind<AddDoctorView, AddDoctorViewModel>();
                e.Bind<BindDoctorView, BindDoctorViewModel>();
                e.Bind<UnbindDoctorView, UnbindDoctorViewModel>()
                    .OnShow(vm => vm.Refresh());
                e.Bind<AddTagView, AddTagViewModel>()
                    .OnClosing(vm => RefreshTags(vm));
                e.Bind<RemoveTagView, RemoveTagViewModel>()
                    .OnShow(vm => vm.RefreshCommand.TryExecute())
                    .OnClosing(vm => RefreshTags(vm));
                e.Bind<BindTagView, BindTagViewModel>()
                    .OnShow(vm => vm.RefreshCommand.TryExecute())
                    .OnClosing(vm => RefreshTags(vm));
            });
        }

        private bool CanDeactivate()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write)
                && this.IsEditionActivated;
        }

        private bool CanEdit()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write)
                && this.IsEditionActivated == false;
        }

        private bool CanManageDoctor()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write)
                && this.IsEditionActivated == true;
        }

        private bool CanManageTag()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private bool CanNavigate()
        {
            return PluginContext.Host.SelectedPatient != null;
        }

        private bool CanRevert()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write)
                && this.IsEditionActivated == true;
        }

        private bool CanSave()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write)
                && this.IsEditionActivated == true;
        }

        /// <summary>
        /// Configures the mapping for AutoMapper.
        /// </summary>
        private void ConfigureAutoMapper()
        {
            //Add the mapping here...
        }

        private void Deactivate()
        {
            this.View.As<WorkbenchViewModel>().DeactivateCommand.TryExecute();
        }

        private void Edit()
        {
            this.View.As<WorkbenchViewModel>().IsEditModeActivated
                = this.IsEditionActivated
                = true;
        }

        private void Navigate()
        {
            if (PluginContext.Host.Navigate(this.View))
            {
                var vm = this.View.As<WorkbenchViewModel>();
                vm.Refresh();
                this.IsEditionActivated = vm.IsEditModeActivated;

                this.ShowContextMenu();
            }
        }

        private void Refresh(InsertionViewModel vm)
        {
            if (vm.HasInsertedItem)
            {
                this.View.As<WorkbenchViewModel>().Refresh();
            }
        }

        private void RefreshTags(TagViewModel vm)
        {
            if (vm.IsModified)
            {
                this.View.As<WorkbenchViewModel>().RefreshTagsCommand.TryExecute();
            }
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