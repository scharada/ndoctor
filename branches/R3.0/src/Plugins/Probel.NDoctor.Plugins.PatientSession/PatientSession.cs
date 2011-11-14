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

namespace Probel.NDoctor.Plugins.PatientSession
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;

    using AutoMapper;

    using Microsoft.Windows.Controls;

    using Probel.Helpers.Strings;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientSession.Properties;
    using Probel.NDoctor.Plugins.PatientSession.View;
    using Probel.NDoctor.Plugins.PatientSession.ViewModel;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    using StructureMap;

    [Export(typeof(IPlugin))]
    public class PatientSession : Plugin
    {
        #region Fields

        private const string uriIco = @"\Probel.NDoctor.Plugins.PatientSession;component/Images\{0}.ico";
        private const string uriPng = @"\Probel.NDoctor.Plugins.PatientSession;component/Images\{0}.png";

        private ICommand addCommand;
        private ICommand searchCommand;
        private ICommand showTopTenCommand;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public PatientSession([Import("version")] Version version, [Import("host")] IPluginHost host)
            : base(version, host)
        {
            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);
            this.ConfigureAutoMapper();
            this.ConfigureStructureMap();
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
        }

        private void BuildButtons()
        {
            #region Add
            var splitterExist = true;
            var splitter = this.Host.FindInHome("add", Groups.Tools);
            if (splitter == null || splitter.GetType() != typeof(RibbonSplitButtonData))
            {
                splitterExist = false;
                splitter = new RibbonSplitButtonData(Messages.Btn_Add, uriPng.StringFormat("Add"), null)
                {
                    Order = 1,
                    Name = "add",
                };
            }

            this.addCommand = new RelayCommand(() => this.NavigateAddPatient());
            var addButton = new RibbonButtonData(Messages.Title_ButtonAddPatient, uriPng.StringFormat("Add"), this.addCommand)
            {
                Order = 2,
            };

            (splitter as RibbonSplitButtonData).Command = addCommand;
            (splitter as RibbonSplitButtonData).ControlDataCollection.Add(addButton);
            if (!splitterExist) this.Host.AddInHome((splitter as RibbonSplitButtonData), Groups.Tools);
            #endregion

            #region Search
            this.searchCommand = new RelayCommand(() => this.NavigateSearchPatient());
            var searchButton = new RibbonButtonData(Messages.Title_SearchPatient, this.searchCommand)
            {
                SmallImage = new Uri(uriPng.StringFormat("SearchSmall"), UriKind.Relative),
                Order = 0,
            };

            this.showTopTenCommand = new RelayCommand(() => this.NavigateTopTen());
            var topTenButton = new RibbonButtonData(Messages.Title_MostUsed, this.showTopTenCommand)
            {
                SmallImage = new Uri(uriPng.StringFormat("SearchSmall"), UriKind.Relative),
                Order = 0,
            };

            var searchSplitButton = new RibbonSplitButtonData(Messages.Title_ButtonSearch, uriIco.StringFormat("Search"), this.searchCommand)
            {
                Order = 0,
            };
            searchSplitButton.ControlDataCollection.Add(searchButton);
            searchSplitButton.ControlDataCollection.Add(topTenButton);

            this.Host.AddInHome(searchSplitButton, Groups.Tools);
            #endregion
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<LightPatientDto, LightPatientViewModel>();
            Mapper.CreateMap<LightPatientViewModel, LightPatientDto>();
        }

        private void ConfigureStructureMap()
        {
            ObjectFactory.Configure(x =>
            {
                x.For<IPatientSessionComponent>().Add<PatientSessionComponent>();
                x.SelectConstructor<PatientSessionComponent>(() => new PatientSessionComponent());
            });
        }

        private void NavigateAddPatient()
        {
            ChildWindowContext.Content = new AddPatientControl();
            ChildWindowContext.WindowState = WindowState.Open;
            ChildWindowContext.IsModal = false;
            ChildWindowContext.Caption = Messages.Title_AddPatient;
        }

        private void NavigateSearchPatient()
        {
            ChildWindowContext.Content = new SearchPatientControl();
            ChildWindowContext.WindowState = WindowState.Open;
            ChildWindowContext.IsModal = false;
            ChildWindowContext.Caption = Messages.Title_SearchPatient;
        }

        private void NavigateTopTen()
        {
            ChildWindowContext.Content = new TopTenControl();
            ChildWindowContext.WindowState = WindowState.Open;
            ChildWindowContext.IsModal = false;
            ChildWindowContext.Caption = Messages.Title_MostUsed;
        }

        #endregion Methods
    }
}