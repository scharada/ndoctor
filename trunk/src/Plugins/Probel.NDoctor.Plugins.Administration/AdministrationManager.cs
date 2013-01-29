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
namespace Probel.NDoctor.Plugins.Administration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using System.Windows.Media;

    using AutoMapper;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.Mvvm;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Administration.Properties;
    using Probel.NDoctor.Plugins.Administration.View;
    using Probel.NDoctor.Plugins.Administration.ViewModel;
    using Probel.NDoctor.View;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.MenuData;
    using Probel.NDoctor.View.Toolbox.Translations;

    [Export(typeof(IPlugin))]
    [PartMetadata(Keys.Constraint, ">3.0.0.0")]
    [PartMetadata(Keys.PluginId, "{C4706773-CF41-49E9-8F47-6FCEA7A86456}")]
    public class AdministrationManager : Plugin
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.Administration;component/Images\{0}.png";

        private ICommand navigateCommand;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public AdministrationManager()
            : base()
        {
            LazyLoader.Set<WorkbenchView>(() => new WorkbenchView());

            this.ConfigureAutoMapper();
        }

        #endregion Constructors

        #region Properties

        public WorkbenchView WorkbenchView
        {
            get
            {
                return LazyLoader.Get<WorkbenchView>();
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
            this.navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());

            var navigateButton = new RibbonButtonData(Messages.Title_AdministratorManager
                    , imgUri.FormatWith("Administration")
                    , navigateCommand) { Order = 4 };

            PluginContext.Host.AddToApplicationMenu(navigateButton);
        }

        /// <summary>
        /// Builds the context menu of this plugin.
        /// </summary>
        private void BuildContextMenu()
        {
            var tab = new RibbonTabData() { Header = BaseText.Menu_File, ContextualTabGroupHeader = Messages.Title_AdministratorManager };
            this.ContextualMenu = new RibbonContextualTabGroupData(Messages.Title_AdministratorManager, tab) { Background = Brushes.OrangeRed, IsVisible = false, };
            var cgroup = new RibbonGroupData(Messages.Group_Add, 1);

            tab.GroupDataCollection.Add(cgroup);
            PluginContext.Host.AddContextualMenu(this.ContextualMenu);
            PluginContext.Host.AddTab(tab);

            int i = 0;
            var buttons = new List<RibbonButtonData>();

            buttons.Add(new RibbonButtonData(Messages.Title_AddInsurance, imgUri.FormatWith("Insurance")
                , new RelayCommand(() => ViewService.Manager.ShowDialog<AddInsuranceViewModel>(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddPractice, imgUri.FormatWith("Practice")
                , new RelayCommand(() => ViewService.Manager.ShowDialog<AddPracticeViewModel>(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddDrugType, imgUri.FormatWith("DrugType")
                , new RelayCommand(() => ViewService.Manager.ShowDialog<AddDrugTypeViewModel>(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddDrug, imgUri.FormatWith("Drug")
                , new RelayCommand(() => ViewService.Manager.ShowDialog<AddDrugViewModel>(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddPathologyType, imgUri.FormatWith("PathologyType")
                , new RelayCommand(() => ViewService.Manager.ShowDialog<AddPathologyTypeViewModel>(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddPathology, imgUri.FormatWith("Pathology")
                , new RelayCommand(() => ViewService.Manager.ShowDialog<AddPathologyViewModel>(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddProfession, imgUri.FormatWith("Job")
                , new RelayCommand(() => ViewService.Manager.ShowDialog<AddProfessionViewModel>(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddReputation, imgUri.FormatWith("Reputation")
                , new RelayCommand(() => ViewService.Manager.ShowDialog<AddReputationViewModel>(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddPictureType, imgUri.FormatWith("PictureType")
                , new RelayCommand(() => ViewService.Manager.ShowDialog<AddPictureTypeViewModel>(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddSpecialisation, imgUri.FormatWith("Specialisation")
                , new RelayCommand(() => ViewService.Manager.ShowDialog<AddSpecialisationViewModel>(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            buttons.Add(new RibbonButtonData(Messages.Title_AddDoctor, imgUri.FormatWith("Doctor")
                , new RelayCommand(() => ViewService.Manager.ShowDialog<AddDoctorViewModel>(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write))) { Order = i++ });

            foreach (var button in buttons) { cgroup.ButtonDataCollection.Add(button); }
        }

        private bool CanNavigate()
        {
            return true && PluginContext.DoorKeeper.IsUserGranted(To.Read);
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<TagViewModel, TagDto>();
            Mapper.CreateMap<TagDto, TagViewModel>();
        }

        private void ConfigureViewService()
        {
            ViewService.Configure(e =>
            {
                e.Bind<AddInsuranceView, AddInsuranceViewModel>()
                    .OnClosing(() => this.Refresh());
                e.Bind<AddPracticeView, AddPracticeViewModel>()
                    .OnClosing(() => this.Refresh());
                e.Bind<AddDrugView, AddDrugViewModel>()
                    .OnShow(vm => vm.Refresh())
                    .OnClosing(() => this.Refresh());
                e.Bind<AddDrugTypeView, AddDrugTypeViewModel>()
                    .OnClosing(() => this.Refresh());
                e.Bind<AddPathologyTypeView, AddPathologyTypeViewModel>()
                    .OnClosing(() => this.Refresh());
                e.Bind<AddPathologyView, AddPathologyViewModel>()
                    .OnShow(vm => vm.Refresh())
                    .OnClosing(() => this.Refresh());
                e.Bind<AddProfessionView, AddProfessionViewModel>()
                    .OnClosing(() => this.Refresh());
                e.Bind<AddReputationView, AddReputationViewModel>()
                    .OnClosing(() => this.Refresh());
                e.Bind<AddPictureTypeView, AddPictureTypeViewModel>()
                    .OnClosing(() => this.Refresh());
                e.Bind<AddSpecialisationView, AddSpecialisationViewModel>()
                    .OnClosing(() => this.Refresh());
                e.Bind<AddDoctorView, AddDoctorViewModel>()
                    .OnShow(vm => vm.Refresh())
                    .OnClosing(() => this.Refresh());
                e.Bind<EditTagView, EditTagViewModel>();
            });
        }

        private void Navigate()
        {
            if (PluginContext.Host.Navigate(this.WorkbenchView))
            {
                this.WorkbenchView.As<WorkbenchViewModel>().Refresh();
                this.ContextualMenu.IsVisible = true;
                this.ContextualMenu.TabDataCollection[0].IsSelected = PluginContext.Configuration.AutomaticContextMenu;
            }
        }

        private void Refresh()
        {
            this.WorkbenchView.As<WorkbenchViewModel>().Refresh();
        }

        #endregion Methods
    }
}