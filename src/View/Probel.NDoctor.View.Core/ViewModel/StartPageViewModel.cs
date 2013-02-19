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
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Probel.Helpers.Data;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DAL.Remote;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins;
    using System.Linq;

    internal class StartPageViewModel : BaseViewModel
    {
        #region Fields

        private readonly IDbSettingsComponent DbSettings = PluginContext.ComponentFactory.GetInstance<IDbSettingsComponent>();
        private readonly ICommand refreshCommand;

        private static bool hasLoaded = false;

        private Chart<int, int> ageRepartition;
        private Chart<DateTime, double> bmiAverage;
        private IDataStatisticsComponent Component = PluginContext.ComponentFactory.GetInstance<IDataStatisticsComponent>();
        private Chart<string, int> genderRepartition;
        private bool isAgeRepartitionBusy;
        private bool isBmiAverageBusy;
        private bool isGenderRepartitionBusy;
        private bool isPatientGrowthBusy;
        private Chart<DateTime, double> obesity;
        private Chart<DateTime, double> overweight;
        private Chart<DateTime, int> patientGrowth;
        private Chart<DateTime, double> underweight;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StartPageViewModel"/> class.
        /// </summary>
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

        public Chart<DateTime, double> BmiAverage
        {
            get { return this.bmiAverage; }
            set
            {
                this.bmiAverage = value;
                this.OnPropertyChanged(() => BmiAverage);
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

        public bool IsBmiAverageBusy
        {
            get { return this.isBmiAverageBusy; }
            set
            {
                this.isBmiAverageBusy = value;
                this.OnPropertyChanged(() => IsBmiAverageBusy);
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

        public Chart<DateTime, double> Obesity
        {
            get { return this.obesity; }
            set
            {
                this.obesity = value;
                this.OnPropertyChanged(() => Obesity);
            }
        }

        public Chart<DateTime, double> Overweight
        {
            get { return this.overweight; }
            set
            {
                this.overweight = value;
                this.OnPropertyChanged(() => Overweight);
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

        public Chart<DateTime, double> Underweight
        {
            get { return this.underweight; }
            set
            {
                this.underweight = value;
                this.OnPropertyChanged(() => Underweight);
            }
        }

        #endregion Properties

        #region Methods

        private void Refresh()
        {
            if (bool.Parse(this.DbSettings["IsDebug"])) { return; }
            if (!hasLoaded)
            {
                var token = new CancellationTokenSource().Token;
                var scheduler = TaskScheduler.FromCurrentSynchronizationContext();

                this.IsAgeRepartitionBusy
                    = this.IsGenderRepartitionBusy
                    = this.IsPatientGrowthBusy
                    = this.IsBmiAverageBusy
                    = true;

                #region Age Repartition
                var task1 = Task.Factory.StartNew<Chart<int, int>>(() => this.Component.GetAgeRepartion());
                task1.ContinueWith(t =>
                {
                    this.AgeRepartition = t.Result;
                    this.IsAgeRepartitionBusy = false;
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
                task1.ContinueWith(p1 => this.Handle.Error(p1.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
                #endregion

                #region Gender repartition
                var task2 = Task.Factory.StartNew<Chart<string, int>>(() => this.Component.GetGenderRepartition());
                task2.ContinueWith(t =>
                {
                    this.GenderRepartition = t.Result;
                    this.IsGenderRepartitionBusy = false;
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
                task2.ContinueWith(p1 => this.Handle.Error(p1.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
                #endregion

                #region Patient growth
                var task3 = Task.Factory.StartNew<Chart<DateTime, int>>(() => this.Component.GetPatientGrowth());
                task3.ContinueWith(t =>
                {
                    this.PatientGrowth = t.Result;
                    this.IsPatientGrowthBusy = false;
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
                task3.ContinueWith(p1 => this.Handle.Error(p1.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
                #endregion

                #region Bmi repartition
                var task4 = Task.Factory.StartNew<Chart<DateTime, double>>(() => this.Component.GetBmiRepartition());
                task4.ContinueWith(t =>
                {
                    if (t.Result.Points.Count() > 1)
                    {
                        this.BmiAverage = t.Result;
                        this.Obesity = t.Result.GetLinearY(30);
                        this.Overweight = t.Result.GetLinearY(25);
                        this.Underweight = t.Result.GetLinearY(18.5);
                    }
                    this.IsBmiAverageBusy = false;
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
                task4.ContinueWith(p1 => this.Handle.Error(p1.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
                #endregion

                hasLoaded = true;
            }
        }

        #endregion Methods
    }
}