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
namespace Probel.NDoctor.Plugins.RescueTools.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Memory;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.RescueTools.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox;
    using Probel.NDoctor.View.Toolbox.Threads;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        public static readonly System.Timers.Timer Countdown = new System.Timers.Timer(250) { AutoReset = true };

        private readonly IRescueToolsComponent Component = PluginContext.ComponentFactory.GetInstance<IRescueToolsComponent>();
        private readonly ICommand deactivateAllCommand;
        private readonly ICommand findDeactivatedPatientsCommand;
        private readonly ICommand findUntaggedPatientsCommand;
        private readonly ICommand reactivateAllCommand;
        private readonly ICommand refreshCommand;
        private readonly ICommand replaceCommand;
        private readonly ICommand replaceWithFirstCommand;
        private readonly ICommand searchCommand;
        private readonly ICommand searchOnAgeCommand;
        private readonly ICommand setDefaultTagCommand;
        private readonly ICommand updateDeactivatedPatientsCommand;
        private readonly ICommand updateOldPatientsCommand;

        private int ageCriteria = 100;
        private string doctorDoubloonsCount;
        private DoctorDto fullReplacementDoctor;
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
            this.OldPatients = new ObservableCollection<LightPatientDto>();
            this.DoctorDoubloons = new ObservableCollection<DoubloonDoctorDto>();
            this.DoubloonsOfSelectedDoctor = new ObservableCollection<LightDoctorDto>();
            this.DeactivatedPatients = new ObservableCollection<LightPatientDto>();
            this.UntaggedPatients = new ObservableCollection<LightPatientDto>();

            this.refreshCommand = new RelayCommand(() => this.Refresh(), () => this.CanRefresh());
            this.replaceCommand = new RelayCommand(() => this.Replace(), () => this.CanReplace());
            this.replaceWithFirstCommand = new RelayCommand(() => this.ReplaceWithFirst(), () => this.CanReplaceWithFirst());
            this.searchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());
            this.searchOnAgeCommand = new RelayCommand(() => this.SearchOnAge(), () => this.CanSearchOnAge());
            this.deactivateAllCommand = new RelayCommand(() => this.DeactivateAll(), () => this.CanDeactivateAll());
            this.updateOldPatientsCommand = new RelayCommand(() => this.UpdateOldPatients(), () => this.CanUpdateOldPatients());
            this.findDeactivatedPatientsCommand = new RelayCommand(() => this.FindDeactivatedPatients(), () => this.CanFindDeactivatedPatients());
            this.updateDeactivatedPatientsCommand = new RelayCommand(() => this.UpdateDeactivatedPatients(), () => this.CanUpdateDeactivatedPatients());
            this.reactivateAllCommand = new RelayCommand(() => this.ReactivateAll(), () => this.CanReactivateAll());
            this.findUntaggedPatientsCommand = new RelayCommand(() => this.FindUntaggedPatients(), () => this.CanFindUntaggedPatients());
            this.setDefaultTagCommand = new RelayCommand(() => this.SetDefaultTag(), () => this.CanSetDefaultTag());

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

        public ObservableCollection<LightPatientDto> DeactivatedPatients
        {
            get;
            private set;
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

        public ICommand FindDeactivatedPatientsCommand
        {
            get { return this.findDeactivatedPatientsCommand; }
        }

        public ICommand FindUntaggedPatientsCommand
        {
            get { return this.findUntaggedPatientsCommand; }
        }

        public DoctorDto FullReplacementDoctor
        {
            get { return this.fullReplacementDoctor; }
            set
            {
                this.fullReplacementDoctor = value;
                this.OnPropertyChanged(() => FullReplacementDoctor);
            }
        }

        public ObservableCollection<LightPatientDto> OldPatients
        {
            get;
            private set;
        }

        public ICommand ReactivateAllCommand
        {
            get { return this.reactivateAllCommand; }
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
                this.ShowFullDoctor();
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

        public ICommand SetDefaultTagCommand
        {
            get { return this.setDefaultTagCommand; }
        }

        public ObservableCollection<LightPatientDto> UntaggedPatients
        {
            get;
            private set;
        }

        public ICommand UpdateDeactivatedPatientsCommand
        {
            get { return this.updateDeactivatedPatientsCommand; }
        }

        public ICommand UpdateOldPatientsCommand
        {
            get { return this.updateOldPatientsCommand; }
        }

        private DoubloonDoctorRefiner DoubloonDoctorSearcher
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

        private bool CanFindDeactivatedPatients()
        {
            return true;
        }

        private bool CanFindUntaggedPatients()
        {
            return true;
        }

        private bool CanReactivateAll()
        {
            return true;
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

        private bool CanSetDefaultTag()
        {
            return true;
        }

        private bool CanUpdateDeactivatedPatients()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Administer);
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

        private void FindDeactivatedPatients()
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;
            PluginContext.Host.SetWaitCursor();

            var task = Task.Factory
                   .StartNew<IEnumerable<LightPatientDto>>(() => this.Component.GetDeactivated());
            task.ContinueWith(t =>
            {
                this.DeactivatedPatients.Refill(t.Result);
                var msg = Messages.Msg_FoundDeactivatedPatients.FormatWith(t.Result.Count());
                PluginContext.Host.WriteStatus(StatusType.Info, msg);
                PluginContext.Host.SetArrowCursor();
            }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
        }

        private void FindUntaggedPatients()
        {
            new AsyncAction(this.Handle).ExecuteAsync<IEnumerable<LightPatientDto>>(
                () => this.FindUntaggedPatientsAsync(),
                t => this.FindUntaggedPatientsCallback(t));
        }

        private IEnumerable<LightPatientDto> FindUntaggedPatientsAsync()
        {
            return this.Component.GetUntaggedPatients();
        }

        private void FindUntaggedPatientsCallback(IEnumerable<LightPatientDto> patients)
        {
            this.UntaggedPatients.Refill(patients);
            ViewService.MessageBox.Information(Messages.Msg_FoundPatients.FormatWith(patients.Count()));
        }

        private void ReactivateAll()
        {
            foreach (var item in this.DeactivatedPatients)
            {
                item.IsDeactivated = false;
            }
        }

        private void Refresh(bool silently = false)
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;

            var task = Task.Factory
                  .StartNew<IEnumerable<DoubloonDoctorDto>>(() => this.Component.GetDoctorDoubloons());
            task.ContinueWith(t => this.RefreshCallback(silently, t), token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
        }

        private void RefreshCallback(bool silently, Task<IEnumerable<DoubloonDoctorDto>> t)
        {
            this.DoctorDoubloons.Clear();
            this.DoubloonsOfSelectedDoctor.Clear();
            this.SearchCriteria = string.Empty;

            this.DoctorDoubloons.Refill(t.Result);
            this.DoubloonDoctorSearcher = new DoubloonDoctorRefiner(t.Result);
            if (!silently) { ViewService.MessageBox.Information(Messages.Msg_DoubloonsFound.FormatWith(this.DoctorDoubloons.Count)); }
            this.DoctorDoubloonsCount = string.Format(Messages.Msg_DoubloonsCount, this.DoctorDoubloons.Count);
        }

        private void RefreshSelectedDoctorDoubloons()
        {
            if (this.SelectedDoctorDoubloon != null)
            {
                var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
                var token = new CancellationTokenSource().Token;
                var task = Task.Factory
                      .StartNew<IEnumerable<LightDoctorDto>>(ctx =>
                      {
                          var doc = ctx as LightDoctorDto;
                          return this.Component.GetDoubloonsOf(doc.FirstName, doc.LastName, doc.Specialisation);
                      }, this.SelectedDoctorDoubloon);
                task.ContinueWith(t => this.DoubloonsOfSelectedDoctor.Refill(t.Result), token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
                task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
            }
        }

        private void Replace()
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;
            PluginContext.Host.SetWaitCursor();

            var yes = ViewService.MessageBox.Question(Messages.Msg_ConfirmReplacement);
            if (yes)
            {
                var task = Task.Factory
                       .StartNew<IEnumerable<DoubloonDoctorDto>>(() =>
                       {
                           this.Component.Replace(this.DoubloonsOfSelectedDoctor, this.ReplacementDoctor);
                           return this.Component.GetDoctorDoubloons();
                       });
                task.ContinueWith(t =>
                {
                    this.RefreshCallback(true, t);
                    ViewService.MessageBox.Information(Messages.Msg_DoubloonsDeleted);
                    PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_DoubloonsDeleted);
                    PluginContext.Host.SetArrowCursor();
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
                task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
            }
        }

        private void ReplaceWithFirst()
        {
            PluginContext.Host.SetWaitCursor();
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;


            var yes = ViewService.MessageBox.Question(Messages.Msg_ConfirmReplaceWithFirst);
            if (yes)
            {
                var task = Task.Factory
                      .StartNew<IEnumerable<DoubloonDoctorDto>>(() =>
                      {
                          this.Component.ReplaceWithFirstDoubloon(this.DoctorDoubloons);
                          return this.Component.GetDoctorDoubloons();
                      });
                task.ContinueWith(t =>
                {
                    this.RefreshCallback(true, t);
                    ViewService.MessageBox.Information(Messages.Msg_DoubloonsDeleted);
                    PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_DoubloonsDeleted);
                    PluginContext.Host.SetArrowCursor();
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
                task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
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
                PluginContext.Host.SetWaitCursor();
                var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
                var token = new CancellationTokenSource().Token;

                var task = Task.Factory
                      .StartNew<IEnumerable<LightPatientDto>>(() => this.Component.GetOlderThan(this.AgeCriteria));
                task.ContinueWith(t =>
                {
                    this.OldPatients.Refill(t.Result);
                    PluginContext.Host.SetArrowCursor();
                    PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_FoundPatients.FormatWith(t.Result.Count()));
                }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
                task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void SetDefaultTag()
        {
            PluginContext.Host.SetWaitCursor();
            new AsyncAction(this.Handle).ExecuteAsync(
                () => this.SetDefaultTagAsync(),
                t => this.SetDefaultTagCallback(t));
        }

        private IEnumerable<LightPatientDto> SetDefaultTagAsync()
        {
            var defaultName = "Patient";
            var defaultTag = this.Component.GetTag(defaultName);

            if (defaultTag == null) { defaultTag = new TagDto(TagCategory.Patient) { Name = defaultName }; }

            this.Component.SetTag(defaultTag, this.UntaggedPatients);
            return this.Component.GetUntaggedPatients();
        }

        private void SetDefaultTagCallback(IEnumerable<LightPatientDto> patients)
        {
            this.UntaggedPatients.Refill(patients);
            PluginContext.Host.SetArrowCursor();
            ViewService.MessageBox.Information(Messages.Msg_PatientUpdated);
        }

        private void ShowFullDoctor()
        {
            var token = new CancellationTokenSource().Token;
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            var task = Task.Factory.StartNew<DoctorDto>(() =>
            {
                if (this.ReplacementDoctor != null) { return this.Component.GetFullDoctor(this.ReplacementDoctor); }
                else { return null; }
            });
            task.ContinueWith(t =>
            {
                if (t.Result != null) { this.FullReplacementDoctor = t.Result; }
            }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
        }

        private void UpdateDeactivatedPatients()
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;

            var task = Task.Factory
                .StartNew<IEnumerable<LightPatientDto>>(() =>
                {
                    this.Component.UpdateDeactivation(this.DeactivatedPatients);
                    return this.Component.GetDeactivated();
                });
            task.ContinueWith(t =>
            {
                this.DeactivatedPatients.Refill(t.Result);
                ViewService.MessageBox.Information(Messages.Msg_PatientUpdated);
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_PatientUpdated);
            }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
            task.ContinueWith(t => this.Handle.Error(t.Exception), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
        }

        private void UpdateOldPatients()
        {
            try
            {
                this.Component.UpdateDeactivation(this.OldPatients);
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_PatientUpdated);
                ViewService.MessageBox.Information(Messages.Msg_PatientUpdated);
                this.OldPatients.Refill(this.Component.GetOlderThan(this.AgeCriteria));
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        #endregion Methods
    }
}