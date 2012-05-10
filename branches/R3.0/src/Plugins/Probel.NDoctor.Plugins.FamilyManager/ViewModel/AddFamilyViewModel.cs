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
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Conversions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.FamilyManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    public class AddFamilyViewModel : BaseViewModel
    {
        #region Fields

        private IFamilyComponent component = ObjectFactory.GetInstance<IFamilyComponent>();
        private string criteria;
        private LightPatientViewModel selectedPatient;

        #endregion Fields

        #region Constructors

        public AddFamilyViewModel()
            : base()
        {
            this.FoundPatients = new ObservableCollection<LightPatientViewModel>();
            this.SearchCommand = new RelayCommand(() => Search(), () => CanSearch());
        }

        #endregion Constructors

        #region Properties

        public string BtnSearch
        {
            get { return Messages.Btn_Search; }
        }

        public string Criteria
        {
            get { return this.criteria; }
            set
            {
                this.criteria = value;
                this.OnPropertyChanged("Criteria");
            }
        }

        public ObservableCollection<LightPatientViewModel> FoundPatients
        {
            get;
            private set;
        }

        public ICommand SearchCommand
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
                this.OnPropertyChanged("SelectedPatient");
            }
        }

        #endregion Properties

        #region Methods

        private bool CanSearch()
        {
            return !string.IsNullOrEmpty(this.Criteria);
        }

        private void Search()
        {
            IList<LightPatientDto> result = new List<LightPatientDto>();
            using (this.component.UnitOfWork)
            {
                result = this.component.FindPatientNotFamilyMembers(PluginContext.Host.SelectedPatient, this.Criteria, SearchOn.FirstAndLastName);
            }

            var mappedResult = Mapper.Map<IList<LightPatientDto>, IList<LightPatientViewModel>>(result);

            for (int i = 0; i < mappedResult.Count; i++)
            {
                mappedResult[i].SessionPatient = PluginContext.Host.SelectedPatient;
                mappedResult[i].Refreshed += (sender, e) =>
                {
                    PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_RelationAdded);
                    this.Search();
                };
            }

            this.FoundPatients.Refill(mappedResult);
            PluginContext.Host.WriteStatusReady();
        }

        #endregion Methods
    }
}