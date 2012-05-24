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
namespace Probel.NDoctor.Plugins.FamilyManager.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using AutoMapper;

    using Probel.Helpers.Conversions;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.FamilyManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class RemoveFamilyViewModel : BaseViewModel
    {
        #region Fields

        private IFamilyComponent component;
        private LightPatientViewModel selectedPatient;

        #endregion Fields

        #region Constructors

        public RemoveFamilyViewModel()
            : base()
        {
            this.component = PluginContext.ComponentFactory.GetInstance<IFamilyComponent>();
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IFamilyComponent>();

            this.FamilyMembers = new ObservableCollection<LightPatientViewModel>();
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<LightPatientViewModel> FamilyMembers
        {
            get;
            private set;
        }

        public LightPatientViewModel SelectedPatient
        {
            get { return this.selectedPatient; }
            set
            {
                this.selectedPatient = value;
                this.OnPropertyChanged(() => SelectedPatient);
            }
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            using (this.component.UnitOfWork)
            {
                var result = this.component.GetAllFamilyMembers(PluginContext.Host.SelectedPatient);
                var mapped = Mapper.Map<IList<LightPatientDto>, IList<LightPatientViewModel>>(result);

                for (int i = 0; i < mapped.Count; i++)
                {
                    mapped[i].SessionPatient = PluginContext.Host.SelectedPatient;
                    mapped[i].Refreshed += (sender, e) =>
                    {
                        this.Refresh();
                        if (e.Data == State.Created) { PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_RelationAdded); }
                        else if (e.Data == State.Removed) { PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_RelationRemoved); }
                    };
                }
                this.FamilyMembers.Refill(mapped);
                PluginContext.Host.WriteStatusReady();
            }
        }

        #endregion Methods
    }
}