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
namespace Probel.NDoctor.Plugins.PatientData.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Timers;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Conversions;
    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class BindDoctorViewModel : BaseViewModel
    {
        #region Fields

        public static readonly Timer Countdown = new Timer(250) { AutoReset = true };

        private IPatientDataComponent component;
        private string criteria;
        private LightDoctorViewModel selectedDoctor;

        #endregion Fields

        #region Constructors

        public BindDoctorViewModel()
        {
            if (!Designer.IsDesignMode)
            {
                this.component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();
                PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();
            }

            this.FoundDoctors = new ObservableCollection<LightDoctorViewModel>();
            this.SearchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());

            Countdown.Elapsed += (sender, e) => PluginContext.Host.Invoke(() =>
            {
                this.SearchCommand.TryExecute();
                Countdown.Stop();
            });
        }

        #endregion Constructors

        #region Properties

        public string Criteria
        {
            get { return this.criteria; }
            set
            {
                Countdown.Start();
                this.criteria = value;
                this.OnPropertyChanged(() => Criteria);
            }
        }

        public ObservableCollection<LightDoctorViewModel> FoundDoctors
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
                this.OnPropertyChanged(() => SelectedDoctor);
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
            var result = this.component.FindNotLinkedDoctorsFor(PluginContext.Host.SelectedPatient, this.Criteria, SearchOn.FirstAndLastName);
            var mapped = Mapper.Map<IList<LightDoctorDto>, IList<LightDoctorViewModel>>(result);
            this.FoundDoctors.Refill(mapped);
        }

        #endregion Methods
    }
}