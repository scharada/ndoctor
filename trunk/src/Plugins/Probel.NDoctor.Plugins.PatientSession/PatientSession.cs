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
namespace Probel.NDoctor.Plugins.PatientSession
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientSession.Properties;
    using Probel.NDoctor.Plugins.PatientSession.View;
    using Probel.NDoctor.Plugins.PatientSession.ViewModel;
    using Probel.NDoctor.View;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.MenuData;

    [Export(typeof(IPlugin))]
    [PartMetadata(Keys.Constraint, ">3.0.0.0")]
    [PartMetadata(Keys.PluginId, "{FD1502BB-8A85-4705-A7B1-49B5CCE1E7FD}")]
    public class PatientSession : Plugin
    {
        #region Fields

        private const string uriIco = @"\Probel.NDoctor.Plugins.PatientSession;component/Images\{0}.ico";
        private const string uriPng = @"\Probel.NDoctor.Plugins.PatientSession;component/Images\{0}.png";

        private readonly ICommand AddCommand;
        private readonly ICommand ExtendedSearchCommand;
        private readonly ICommand SearchCommand;
        private readonly ICommand ShowTopTenCommand;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public PatientSession()
            : base()
        {
            this.SearchCommand = new RelayCommand(() => this.NavigateSearchPatient(), () => this.CanSearchPatient());
            this.AddCommand = new RelayCommand(() => this.NavigateAddPatient(), () => this.CanNavigateAddPatient());
            this.ShowTopTenCommand = new RelayCommand(() => this.NavigateTopTen(), () => this.CanSearchPatient());
            this.ExtendedSearchCommand = new RelayCommand(() => this.NavigateExtendedSearch(), () => this.CanSearchPatient());
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
            this.BuildButtons();
            this.ConfigureViewService();
        }

        private void BuildButtons()
        {
            #region Add
            var splitterExist = true;
            var splitter = PluginContext.Host.GetMenuInHome("add", Groups.Tools);
            if (splitter == null || splitter.GetType() != typeof(RibbonMenuButtonData))
            {
                splitterExist = false;
                splitter = new RibbonMenuButtonData(Messages.Btn_Add, uriPng.FormatWith("Add"), null)
                {
                    Order = 1,
                    Name = "add",
                };
            }

            var addButton = new RibbonMenuItemData(Messages.Title_ButtonAddPatient, uriPng.FormatWith("Add"), this.AddCommand)
            {
                Order = 2,
            };

            (splitter as RibbonMenuButtonData).Command = AddCommand;
            (splitter as RibbonMenuButtonData).ControlDataCollection.Add(addButton);
            if (!splitterExist) PluginContext.Host.AddInHome((splitter as RibbonMenuButtonData), Groups.Tools);
            #endregion

            #region Search
            var searchButton = new RibbonButtonData(Messages.Title_SearchPatient, this.SearchCommand)
            {
                SmallImage = new Uri(uriPng.FormatWith("SearchSmall"), UriKind.Relative),
                Order = 0,
            };

            var extendedSerchButton = new RibbonButtonData(Messages.Title_ExtendedSearchPatient, this.ExtendedSearchCommand)
            {
                SmallImage = new Uri(uriPng.FormatWith("SearchSmall"), UriKind.Relative),
                Order = 0,
            };

            var topTenButton = new RibbonButtonData(Messages.Title_MostUsed, this.ShowTopTenCommand)
            {
                SmallImage = new Uri(uriPng.FormatWith("SearchSmall"), UriKind.Relative),
                Order = 0,
            };

            var searchSplitButton = new RibbonSplitButtonData(Messages.Title_ButtonSearch, uriIco.FormatWith("Search"), this.SearchCommand)
            {
                Order = 0,
            };
            searchSplitButton.ControlDataCollection.Add(searchButton);
            searchSplitButton.ControlDataCollection.Add(extendedSerchButton);
            searchSplitButton.ControlDataCollection.Add(topTenButton);

            PluginContext.Host.AddInHome(searchSplitButton, Groups.Tools);
            #endregion
        }

        private bool CanNavigateAddPatient()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private bool CanSearchPatient()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Read);
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<LightPatientDto, LightPatientViewModel>();
            Mapper.CreateMap<LightPatientViewModel, LightPatientDto>();
        }

        private void ConfigureViewService()
        {
            ViewService.Configure(e =>
            {
                e.Bind<SearchPatientExtendedControl, SearchPatientExtendedViewModel>()
                    .OnShow(vm => vm.RefreshCommand.TryExecute());
                e.Bind<SearchPatientControl, SearchPatientViewModel>();
                e.Bind<TopTenControl, TopTenViewModel>();
                e.Bind<AddPatientControl, AddPatientViewModel>();
            });
        }

        private void NavigateAddPatient()
        {
            ViewService.Manager.ShowDialog<AddPatientViewModel>();
        }

        private void NavigateExtendedSearch()
        {
            ViewService.Manager.ShowDialog<SearchPatientExtendedViewModel>();
        }

        private void NavigateSearchPatient()
        {
            ViewService.Manager.ShowDialog<SearchPatientViewModel>();
        }

        private void NavigateTopTen()
        {
            ViewService.Manager.ShowDialog<TopTenViewModel>();
        }

        #endregion Methods
    }
}