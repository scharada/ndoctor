﻿/*
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
namespace Probel.NDoctor.Plugins.RescueTools.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.MemorySearches;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.RescueTools.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        public static readonly System.Timers.Timer Countdown = new System.Timers.Timer(250) { AutoReset = true };

        private readonly IRescueToolsComponent Component = PluginContext.ComponentFactory.GetInstance<IRescueToolsComponent>();
        private readonly ICommand deactivateAllCommand;
        private readonly ICommand refreshCommand;
        private readonly ICommand replaceCommand;
        private readonly ICommand replaceWithFirstCommand;
        private readonly ICommand searchCommand;
        private readonly ICommand searchOnAgeCommand;
        private readonly ICommand updateOldPatientsCommand;

        private int ageCriteria = 100;
        private string doctorDoubloonsCount;
        private LightDoctorDto replacementDoctor;
        private string searchCriteria = string.Empty;
        private DoubloonDoctorDto selectedDoctorDoubloon;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
        {
            this.DoubloonDoctorSearcher = new DoubloonDoctorSearcher();

            this.OldPatients = new ObservableCollection<LightPatientDto>();
            this.DoctorDoubloons = new ObservableCollection<DoubloonDoctorDto>();
            this.DoubloonsOfSelectedDoctor = new ObservableCollection<LightDoctorDto>();

            this.refreshCommand = new RelayCommand(() => this.Refresh(), () => this.CanRefresh());
            this.replaceCommand = new RelayCommand(() => this.Replace(), () => this.CanReplace());
            this.replaceWithFirstCommand = new RelayCommand(() => this.ReplaceWithFirst(), () => this.CanReplaceWithFirst());
            this.searchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());
            this.searchOnAgeCommand = new RelayCommand(() => this.SearchOnAge(), () => this.CanSearchOnAge());
            this.deactivateAllCommand = new RelayCommand(() => this.DeactivateAll(), () => this.CanDeactivateAll());
            this.updateOldPatientsCommand = new RelayCommand(() => this.UpdateOldPatients(), () => this.CanUpdateOldPatients());

            Countdown.Elapsed += (sender, e) => PluginContext.Host.Invoke(() =>
            {
                this.SearchCommand.TryExecute();
                Countdown.Stop();
            });
        }

        #endregion Constructors

        #region Properties

        public int AgeCriteria
        {
            get { return this.ageCriteria; }
            set
            {
                this.ageCriteria = value;
                this.OnPropertyChanged(() => AgeCriteria);
            }
        }

        public ICommand DeactivateAllCommand
        {
            get { return this.deactivateAllCommand; }
        }

        public ObservableCollection<DoubloonDoctorDto> DoctorDoubloons
        {
            get;
            private set;
        }

        public string DoctorDoubloonsCount
        {
            get { return this.doctorDoubloonsCount; }
            set
            {
                this.doctorDoubloonsCount = value;
                this.OnPropertyChanged(() => DoctorDoubloonsCount);
            }
        }

        public ObservableCollection<LightDoctorDto> DoubloonsOfSelectedDoctor
        {
            get;
            private set;
        }

        public ObservableCollection<LightPatientDto> OldPatients
        {
            get;
            private set;
        }

        public ICommand RefreshCommand
        {
            get { return this.refreshCommand; }
        }

        public ICommand ReplaceCommand
        {
            get { return this.replaceCommand; }
        }

        public LightDoctorDto ReplacementDoctor
        {
            get { return this.replacementDoctor; }
            set
            {
                this.replacementDoctor = value;
                this.OnPropertyChanged(() => ReplacementDoctor);
            }
        }

        public ICommand ReplaceWithFirstCommand
        {
            get { return this.replaceWithFirstCommand; }
        }

        public ICommand SearchCommand
        {
            get { return this.searchCommand; }
        }

        public string SearchCriteria
        {
            get { return this.searchCriteria; }
            set
            {
                Countdown.Start();
                this.searchCriteria = value;
                this.OnPropertyChanged(() => SearchCriteria);
            }
        }

        public ICommand SearchOnAgeCommand
        {
            get { return this.searchOnAgeCommand; }
        }

        public DoubloonDoctorDto SelectedDoctorDoubloon
        {
            get { return this.selectedDoctorDoubloon; }
            set
            {
                this.selectedDoctorDoubloon = value;
                this.OnPropertyChanged(() => SelectedDoctorDoubloon);
                this.RefreshSelectedDoctorDoubloons();
            }
        }

        public ICommand UpdateOldPatientsCommand
        {
            get { return this.updateOldPatientsCommand; }
        }

        private DoubloonDoctorSearcher DoubloonDoctorSearcher
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        private bool CanDeactivateAll()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Administer); ;
        }

        private bool CanRefresh()
        {
            return true;
        }

        private bool CanReplace()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Administer)
                && this.ReplacementDoctor != null;
        }

        private bool CanReplaceWithFirst()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Administer);
        }

        private bool CanSearch()
        {
            return this.DoctorDoubloons != null;
        }

        private bool CanSearchOnAge()
        {
            return true;
        }

        private bool CanUpdateOldPatients()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Administer);
        }

        private void DeactivateAll()
        {
            foreach (var patient in this.OldPatients)
            {
                patient.IsDeactivated = true;
            }
        }

        private void Refresh(bool silently = false)
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;

            Task.Factory
                .StartNew<IEnumerable<DoubloonDoctorDto>>(() => this.Component.GetDoctorDoubloons())
                .ContinueWith(t => this.RefreshCallback(silently, t), token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler)
                .ContinueWith(t => this.Handle.Error(t.Exception.InnerExceptions), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
        }

        private void RefreshCallback(bool silently, Task<IEnumerable<DoubloonDoctorDto>> t)
        {
            this.DoctorDoubloons.Clear();
            this.DoubloonsOfSelectedDoctor.Clear();
            this.SearchCriteria = string.Empty;

            this.DoctorDoubloons.Refill(t.Result);
            this.DoubloonDoctorSearcher = new DoubloonDoctorSearcher(t.Result);
            if (!InDebugMode && !silently) { ViewService.MessageBox.Information(Messages.Msg_DoubloonsFound.FormatWith(this.DoctorDoubloons.Count)); }
            this.DoctorDoubloonsCount = string.Format(Messages.Msg_DoubloonsCount, this.DoctorDoubloons.Count);
        }

        private void RefreshSelectedDoctorDoubloons()
        {
            if (this.SelectedDoctorDoubloon != null)
            {
                var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
                var token = new CancellationTokenSource().Token;
                Task.Factory
                    .StartNew<IEnumerable<LightDoctorDto>>(ctx =>
                    {
                        var doc = ctx as LightDoctorDto;
                        return this.Component.GetDoubloonsOf(doc.FirstName, doc.LastName, doc.Specialisation);
                    }, this.SelectedDoctorDoubloon)
                    .ContinueWith(t => this.DoubloonsOfSelectedDoctor.Refill(t.Result), token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler)
                    .ContinueWith(t => this.Handle.Error(t.Exception.InnerExceptions), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
            }
        }

        private void Replace()
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;

            var yes = (!InDebugMode)
                ? ViewService.MessageBox.Question(Messages.Msg_ConfirmReplacement)
                : true;

            if (yes)
            {
                Task.Factory
                    .StartNew<IEnumerable<DoubloonDoctorDto>>(() =>
                    {
                        this.Component.Replace(this.DoubloonsOfSelectedDoctor, this.ReplacementDoctor);
                        return this.Component.GetDoctorDoubloons();
                    })
                    .ContinueWith(t =>
                    {
                        this.RefreshCallback(true, t);
                        if (!InDebugMode) { ViewService.MessageBox.Information(Messages.Msg_DoubloonsDeleted); }
                        PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_DoubloonsDeleted);
                    }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler)
                    .ContinueWith(t => this.Handle.Error(t.Exception.InnerExceptions), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
            }
        }

        private void ReplaceWithFirst()
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;

            var yes = (!InDebugMode)
                ? ViewService.MessageBox.Question(Messages.Msg_ConfirmReplaceWithFirst)
                : true;

            if (yes)
            {
                Task.Factory
                    .StartNew<IEnumerable<DoubloonDoctorDto>>(() =>
                    {
                        this.Component.ReplaceWithFirstDoubloon(this.DoctorDoubloons);
                        return this.Component.GetDoctorDoubloons();
                    })
                    .ContinueWith(t =>
                    {
                        this.RefreshCallback(true, t);
                        if (!InDebugMode) { ViewService.MessageBox.Information(Messages.Msg_DoubloonsDeleted); }
                        PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_DoubloonsDeleted);
                    }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler)
                    .ContinueWith(t => this.Handle.Error(t.Exception.InnerExceptions), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
            }
        }

        private void Search()
        {
            try
            {
                var result = this.DoubloonDoctorSearcher
                    .FindByFirstOrLastName(this.SearchCriteria);
                this.DoctorDoubloons.Refill(result);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void SearchOnAge()
        {
            try
            {
                this.OldPatients.Refill(this.Component.GetOlderThan(this.AgeCriteria));
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void UpdateOldPatients()
        {
            try
            {
                this.Component.UpdateDeactivation(this.OldPatients);
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_PatientUpdated);
                this.OldPatients.Refill(this.Component.GetOlderThan(this.AgeCriteria));
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        #endregion Methods
    }
}