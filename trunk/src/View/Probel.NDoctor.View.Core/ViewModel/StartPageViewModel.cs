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

        private Chart<int, int> ageRepartition;
        private IDataStatisticsComponent Component = PluginContext.ComponentFactory.GetInstance<IDataStatisticsComponent>();
        private Chart<string, int> genderRepartition;
        private bool isAgeRepartitionBusy;
        private bool isGenderRepartitionBusy;
        private bool isPatientGrowthBusy;
        private Chart<DateTime, int> patientGrowth;

        #endregion Fields

        #region Constructors

        public StartPageViewModel()
        {
            this.refreshCommand = new RelayCommand(() => this.Refresh());
        }

        #endregion Constructors

        #region Properties

        public Chart<int, int> AgeRepartition
        {
            get { return this.ageRepartition; }
            set
            {
                this.ageRepartition = value;
                this.OnPropertyChanged(() => AgeRepartition);
            }
        }

        public Chart<string, int> GenderRepartition
        {
            get { return this.genderRepartition; }
            set
            {
                this.genderRepartition = value;
                this.OnPropertyChanged(() => GenderRepartition);
            }
        }

        public bool IsAgeRepartitionBusy
        {
            get { return this.isAgeRepartitionBusy; }
            set
            {
                this.isAgeRepartitionBusy = value;
                this.OnPropertyChanged(() => IsAgeRepartitionBusy);
            }
        }

        public bool IsGenderRepartitionBusy
        {
            get { return this.isGenderRepartitionBusy; }
            set
            {
                this.isGenderRepartitionBusy = value;
                this.OnPropertyChanged(() => IsGenderRepartitionBusy);
            }
        }

        public bool IsPatientGrowthBusy
        {
            get { return this.isPatientGrowthBusy; }
            set
            {
                this.isPatientGrowthBusy = value;
                this.OnPropertyChanged(() => IsPatientGrowthBusy);
            }
        }

        public Chart<DateTime, int> PatientGrowth
        {
            get { return this.patientGrowth; }
            set
            {
                this.patientGrowth = value;
                this.OnPropertyChanged(() => PatientGrowth);
            }
        }

        public ICommand RefreshCommand
        {
            get { return this.refreshCommand; }
        }

        #endregion Properties

        #region Methods

        private static bool hasLoaded = false;
        private void Refresh()
        {
            if (!hasLoaded)
            {
                this.IsAgeRepartitionBusy
            = this.IsGenderRepartitionBusy
            = this.IsPatientGrowthBusy
            = true;

                var task1 = Task.Factory.StartNew<Chart<int, int>>(() => this.Component.GetAgeRepartion());
                task1.ContinueWith(p1 =>
                {
                    this.AgeRepartition = p1.Result;
                    this.IsAgeRepartitionBusy = false;
                });

                var task2 = Task.Factory.StartNew<Chart<string, int>>(() => this.Component.GetGenderRepartition());
                task2.ContinueWith(p2 =>
                {
                    this.GenderRepartition = p2.Result;
                    this.IsGenderRepartitionBusy = false;
                });

                var task3 = Task.Factory.StartNew<Chart<DateTime, int>>(() => this.Component.GetPatientGrowth());
                task3.ContinueWith(p3 =>
                {
                    this.PatientGrowth = p3.Result;
                    this.IsPatientGrowthBusy = false;
                });

                hasLoaded = true;
            }
        }

        #endregion Methods
    }
}