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

        private const string searchUri = @"\Probel.NDoctor.Plugins.PatientSession;component/Images\{0}.png";

        private ICommand addCommand;

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
            this.Host.Invoke(() =>
            {
                this.Host.AddDockablePane(Messages.Title_MostUsed, new TopTenControl());
                this.Host.AddDockablePane(Messages.Title_SearchPatient, new SearchPatientControl());
            });

            this.BuildButtons();
        }

        private void BuildButtons()
        {
            this.addCommand = new RelayCommand(() => this.NavigateAddPatient());
            var addButton = new RibbonButtonData(Messages.Title_ButtonAddPatient, this.addCommand)
            {
                SmallImage = new Uri(searchUri.StringFormat("Add"), UriKind.Relative),
                Order = 2,
            };
            this.Host.AddInHome(addButton, Groups.Tools);
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

        #endregion Methods
    }
}