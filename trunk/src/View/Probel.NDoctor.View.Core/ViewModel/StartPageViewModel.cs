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
namespace Probel.NDoctor.View.Core.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Probel.Helpers.Data;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.View.Plugins;

    internal class StartPageViewModel : BaseViewModel
    {
        #region Fields

        private readonly ICommand refreshCommand;

        private IDataStatisticsComponent Component = PluginContext.ComponentFactory.GetInstance<IDataStatisticsComponent>();
        private bool isAgeRepartitionBusy;

        #endregion Fields

        #region Constructors

        private Chart<string, int> genderRepartition;
        public Chart<string, int> GenderRepartition
        {
            get { return this.genderRepartition; }
            set
            {
                this.genderRepartition = value;
                this.OnPropertyChanged(() => GenderRepartition);
            }
        }
        private Chart<DateTime, int> patientGrowth;
        public Chart<DateTime, int> PatientGrowth
        {
            get { return this.patientGrowth; }
            set
            {
                this.patientGrowth = value;
                this.OnPropertyChanged(() => PatientGrowth);
            }
        }

        private Chart<int, int> ageRepartition;
        public Chart<int, int> AgeRepartition
        {
            get { return this.ageRepartition; }
            set
            {
                this.ageRepartition = value;
                this.OnPropertyChanged(() => AgeRepartition);
            }
        }
        public StartPageViewModel()
        {

            this.refreshCommand = new RelayCommand(() => this.Refresh());
        }

        #endregion Constructors

        #region Properties

        public bool IsAgeRepartitionBusy
        {
            get { return this.isAgeRepartitionBusy; }
            set
            {
                this.isAgeRepartitionBusy = value;
                this.OnPropertyChanged(() => IsAgeRepartitionBusy);
            }
        }

        private bool isPatientGrowthBusy;
        public bool IsPatientGrowthBusy
        {
            get { return this.isPatientGrowthBusy; }
            set
            {
                this.isPatientGrowthBusy = value;
                this.OnPropertyChanged(() => IsPatientGrowthBusy);
            }
        }
        private bool isGenderRepartitionBusy;
        public bool IsGenderRepartitionBusy
        {
            get { return this.isGenderRepartitionBusy; }
            set
            {
                this.isGenderRepartitionBusy = value;
                this.OnPropertyChanged(() => IsGenderRepartitionBusy);
            }
        }

        public ICommand RefreshCommand
        {
            get { return this.refreshCommand; }
        }

        #endregion Properties

        #region Methods
        private void Refresh()
        {
            this.IsAgeRepartitionBusy
                = this.IsGenderRepartitionBusy
                = this.IsPatientGrowthBusy
                = true;

            var task1 = Task.Factory.StartNew<Chart<int, int>>(() => this.Component.GetAgeRepartion());
            task1.ContinueWith(p1 =>
            {
                PluginContext.Host.Invoke(() => this.AgeRepartition = p1.Result);
                this.IsAgeRepartitionBusy = false;
            });

            var task2 = Task.Factory.StartNew<Chart<string, int>>(() => this.Component.GetGenderRepartition());
            task2.ContinueWith(p2 =>
                {
                    PluginContext.Host.Invoke(() => this.GenderRepartition = p2.Result);
                    this.IsGenderRepartitionBusy = false;
                });

            var task3 = Task.Factory.StartNew<Chart<DateTime, int>>(() => this.Component.GetPatientGrowth());
            task3.ContinueWith(p3 =>
            {
                PluginContext.Host.Invoke(() => this.PatientGrowth = p3.Result);
                this.IsPatientGrowthBusy = false;
            });



        }
        #endregion Methods
    }
}