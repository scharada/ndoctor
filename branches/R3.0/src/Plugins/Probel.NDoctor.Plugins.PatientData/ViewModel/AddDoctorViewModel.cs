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

namespace Probel.NDoctor.Plugins.PatientData.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Conversions;
    using Probel.Helpers.WPF;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    public class AddDoctorViewModel : BaseViewModel
    {
        #region Fields

        private IPatientDataComponent component;
        private string criteria;
        private bool isPopupOpened;
        private LightDoctorViewModel selectedDoctor;

        #endregion Fields

        #region Constructors

        public AddDoctorViewModel()
        {
            if (!Designer.IsDesignMode) this.component = ObjectFactory.GetInstance<IPatientDataComponent>();

            this.FoundDoctors = new ObservableCollection<LightDoctorViewModel>();
            this.SearchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());
            this.OpenPopupCommand = new RelayCommand(() => this.IsPopupOpened = true);
        }

        #endregion Constructors

        #region Properties

        public string Criteria
        {
            get { return this.criteria; }
            set
            {
                this.criteria = value;
                this.OnPropertyChanged("Criteria");
            }
        }

        public ObservableCollection<LightDoctorViewModel> FoundDoctors
        {
            get;
            private set;
        }

        public bool IsPopupOpened
        {
            get { return this.isPopupOpened; }
            set
            {
                if (value) this.FoundDoctors.Clear();

                this.isPopupOpened = value;
                this.OnPropertyChanged("IsPopupOpened");
            }
        }

        public ICommand OpenPopupCommand
        {
            get;
            private set;
        }

        public ICommand SearchCommand
        {
            get;
            private set;
        }

        public LightDoctorViewModel SelectedDoctor
        {
            get { return this.selectedDoctor; }
            set
            {
                this.selectedDoctor = value;
                this.OnPropertyChanged("SelectedDoctor");
            }
        }

        #endregion Properties

        #region Methods

        private bool CanSearch()
        {
            return !string.IsNullOrWhiteSpace(this.Criteria);
        }

        private void Search()
        {
            IList<LightDoctorDto> result;
            using (this.component.UnitOfWork)
            {
                result = this.component.FindDoctorsFor(PluginContext.Host.SelectedPatient, this.Criteria, SearchOn.FirstAndLastName);
            }
            var mapped = Mapper.Map<IList<LightDoctorDto>, IList<LightDoctorViewModel>>(result);
            this.FoundDoctors.Refill(mapped);
        }

        #endregion Methods
    }
}