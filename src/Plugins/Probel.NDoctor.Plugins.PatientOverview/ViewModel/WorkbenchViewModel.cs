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
namespace Probel.NDoctor.Plugins.PatientOverview.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Probel.Helpers;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.Mvvm.Gui.FileServices;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientOverview.Actions;
    using Probel.NDoctor.Plugins.PatientOverview.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private readonly ICommand changeImageCommand;
        private readonly ICommand saveCommand;
        private readonly ICommand sendPrivateMailCommand;
        private readonly ICommand sendProMailCommand;

        private IPatientDataComponent component;
        private bool isBusy;
        private bool isEditModeActivated;
        private DoctorDto selectedDoctor;
        private InsuranceDto selectedInsurance;
        private PatientDto selectedPatient;
        private PracticeDto selectedPractice;
        private byte[] thumbnail;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
        {
            this.component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();

            PluginContext.Host.UserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();

            this.Reputations = new ObservableCollection<ReputationDto>();
            this.Professions = new ObservableCollection<ProfessionDto>();
            this.Insurances = new ObservableCollection<LightInsuranceDto>();
            this.Practices = new ObservableCollection<LightPracticeDto>();
            this.Doctors = new ObservableCollection<DoctorDto>();

            this.Genders = new ObservableCollection<Tuple<string, Gender>>();
            Genders.Add(new Tuple<string, Gender>(Messages.Male, Gender.Male));
            Genders.Add(new Tuple<string, Gender>(Messages.Female, Gender.Female));

            this.sendPrivateMailCommand = new RelayCommand(() => this.SendMail(this.SelectedPatient.PrivateMail), () => this.CanSendMail());
            this.sendProMailCommand = new RelayCommand(() => SendMail(this.SelectedPatient.ProMail), () => this.CanSendMail());
            this.saveCommand = new RelayCommand(() => this.Save(), () => this.CanSave());
            this.changeImageCommand = new RelayCommand(() => this.ChangeImage(), () => this.CanChangeImage());
        }

        #endregion Constructors

        #region Properties

        public ICommand ChangeImageCommand
        {
            get { return this.changeImageCommand; }
        }

        public ObservableCollection<DoctorDto> Doctors
        {
            get;
            private set;
        }

        public ObservableCollection<Tuple<string, Gender>> Genders
        {
            get;
            private set;
        }

        public ObservableCollection<LightInsuranceDto> Insurances
        {
            get;
            private set;
        }

        public bool IsBusy
        {
            get { return this.isBusy; }
            set
            {
                this.isBusy = value;
                this.OnPropertyChanged(() => IsBusy);
            }
        }

        public bool IsEditModeActivated
        {
            get { return this.isEditModeActivated; }
            set
            {
                this.isEditModeActivated = value;
                this.OnPropertyChanged(() => IsEditModeActivated);
            }
        }

        public ObservableCollection<LightPracticeDto> Practices
        {
            get;
            private set;
        }

        public ObservableCollection<ProfessionDto> Professions
        {
            get;
            private set;
        }

        public ObservableCollection<ReputationDto> Reputations
        {
            get;
            private set;
        }

        public ICommand SaveCommand
        {
            get { return this.saveCommand; }
        }

        public DoctorDto SelectedDoctor
        {
            get { return this.selectedDoctor; }
            set
            {
                this.selectedDoctor = value;
                this.OnPropertyChanged(() => SelectedDoctor);
            }
        }

        public Tuple<string, Gender> SelectedGender
        {
            get
            {
                if (this.SelectedPatient != null)
                {
                    return (from g in this.Genders
                            where g.Item2 == this.SelectedPatient.Gender
                            select g).Single();
                }
                else { return this.Genders[0]; }
            }
            set
            {
                if (this.SelectedPatient != null)
                {
                    this.SelectedPatient.Gender = value.Item2;
                    this.OnPropertyChanged(() => SelectedGender);
                }
            }
        }

        public InsuranceDto SelectedInsurance
        {
            get { return this.selectedInsurance; }
            set
            {
                this.selectedInsurance = value;
                this.OnPropertyChanged(() => SelectedInsurance);
            }
        }

        public PatientDto SelectedPatient
        {
            get { return this.selectedPatient; }
            set
            {
                this.selectedPatient = value;
                this.OnPropertyChanged(() => SelectedPatient);
            }
        }

        public PracticeDto SelectedPractice
        {
            get { return this.selectedPractice; }
            set
            {
                this.selectedPractice = value;
                this.OnPropertyChanged(() => SelectedPractice);
            }
        }

        public ICommand SendPrivateMailCommand
        {
            get { return this.sendPrivateMailCommand; }
        }

        public ICommand SendProMailCommand
        {
            get { return this.sendProMailCommand; }
        }

        public byte[] Thumbnail
        {
            get { return this.thumbnail; }
            set
            {
                this.thumbnail = value;
                this.OnPropertyChanged(() => Thumbnail);
            }
        }

        #endregion Properties

        #region Methods

        public bool AskToLeave()
        {
            var cancelSaving = ViewService.MessageBox.Question(Messages.Question_LeaveWithoutSaving);

            if (cancelSaving)
            {
                this.IsEditModeActivated = false;
            }

            return cancelSaving;
        }

        public bool CanLeave()
        {
            return this.IsEditModeActivated == false;
        }

        /// <summary>
        /// Refreshes the whole data of this instance.
        /// </summary>
        public void Refresh()
        {
            try
            {
                this.IsBusy = true;
                var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
                var token = new CancellationTokenSource().Token;
                Task.Factory
                    .StartNew<ThreadContext>(state =>
                     {
                         var selectedPatient = state as LightPatientDto;
                         var ctx = new ThreadContext();

                         ctx.Reputations = this.component.GetAllReputations();
                         ctx.Insurances = this.component.GetAllInsurancesLight();
                         ctx.Practices = this.component.GetAllPracticesLight();
                         ctx.Professions = this.component.GetAllProfessions();
                         ctx.SelectedPatient = this.component.GetPatient(selectedPatient);
                         ctx.SelectedDoctor = this.component.GetFullDoctorOf(selectedPatient);
                         ctx.Doctors = this.component.GetFullDoctorOf(selectedPatient);
                         ctx.SelectedInsurance = this.component.GetInsuranceById(ctx.SelectedPatient.Insurance.Id);
                         ctx.SelectedPractice = this.component.GetPracticeById(ctx.SelectedPatient.Practice.Id);
                         ctx.Thumbnail = this.component.GetThumbnail(ctx.SelectedPatient);

                         return ctx;
                     }, PluginContext.Host.SelectedPatient, token)
                    .ContinueWith(t =>
                    {
                        var ctx = t.Result as ThreadContext;
                        this.Reputations.Refill(ctx.Reputations);
                        this.Insurances.Refill(ctx.Insurances);
                        this.Practices.Refill(ctx.Practices);
                        this.Professions.Refill(ctx.Professions);
                        this.Doctors.Refill(ctx.Doctors);
                        this.SelectedPatient = ctx.SelectedPatient;
                        this.Thumbnail = ctx.Thumbnail;

                        this.SelectedGender = (from gender in this.Genders
                                               where gender.Item2 == this.selectedPatient.Gender
                                               select gender).Single();
                        this.SelectedPatient.Insurance = ctx.SelectedInsurance;
                        this.SelectedPatient.Practice = ctx.SelectedPractice;
                        this.RefreshComboBoxes();

                        this.IsBusy = false;
                    }, token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler)
                    .ContinueWith(t => this.Handle.Error(t.Exception.InnerException), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private bool CanChangeImage()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private bool CanSave()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private bool CanSendMail()
        {
            return this.SelectedPatient != null;
        }

        private void ChangeImage()
        {
            var file = string.Empty;
            var option = new Options()
            {
                Multiselect = false,
                Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF"
            };
            var dr = FileGuiFactory.Win32.SelectFile(e => file = e, option);

            if (dr == true && File.Exists(file))
            {
                var img = Image.FromFile(file);
                this.Thumbnail = img.GetThumbnail();
                PluginDataContext.Instance.Invoker.AddThumbnail(this.component, this.SelectedPatient, this.Thumbnail);
            }
        }

        private void RefreshComboBoxes()
        {
            if (this.SelectedPatient != null)
            {
                if (this.SelectedPatient.Reputation != null)
                {
                    this.SelectedPatient.Reputation = (from r in this.Reputations
                                                       where r.Id == this.SelectedPatient.Reputation.Id
                                                       select r).Single();
                }
                if (this.SelectedPatient.Insurance != null)
                {
                    this.SelectedPatient.Insurance = (from i in this.Insurances
                                                      where i.Id == this.SelectedPatient.Insurance.Id
                                                      select i).Single();
                }
                if (this.SelectedPatient.Practice != null)
                {
                    this.SelectedPatient.Practice = (from p in this.Practices
                                                     where p.Id == this.SelectedPatient.Practice.Id
                                                     select p).Single();
                }
                if (this.SelectedPatient.Profession != null)
                {
                    this.SelectedPatient.Profession = (from p in this.Professions
                                                       where this.SelectedPatient.Profession.Id == p.Id
                                                       select p).Single();
                }
            }
            else
            {
                this.Logger.Warn("Trying to refresh the comboboxes while no patient is selected!");
                return;
            }
        }

        private void Save()
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var token = new CancellationTokenSource().Token;

            var task = Task.Factory.StartNew(e => this.SaveAsync(e), new { Patient = this.SelectedPatient, Invoker = PluginDataContext.Instance.Invoker });
            task.ContinueWith(e => this.Refresh(), token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
            task.ContinueWith(e => this.Handle.Error(e.Exception.InnerException), token, TaskContinuationOptions.OnlyOnFaulted, scheduler);
        }

        private void SaveAsync(dynamic context)
        {
            this.component.Update(context.Patient);
            context.Invoker.Execute();
        }

        private void SendMail(string mail)
        {
            try
            {
                mail = (mail == null)
            ? string.Empty
            : mail;

                Process.Start(string.Format("mailto:{0}", mail));
            }
            catch (Exception ex) { this.Handle.Error(ex, Messages.Err_CantSendMail); }
        }

        #endregion Methods

        #region Nested Types

        private class ThreadContext
        {
            #region Properties

            public IEnumerable<DoctorDto> Doctors
            {
                get; set;
            }

            public IEnumerable<LightInsuranceDto> Insurances
            {
                get; set;
            }

            public IEnumerable<LightPracticeDto> Practices
            {
                get; set;
            }

            public IEnumerable<ProfessionDto> Professions
            {
                get; set;
            }

            public IEnumerable<ReputationDto> Reputations
            {
                get; set;
            }

            public IList<DoctorDto> SelectedDoctor
            {
                get; set;
            }

            public InsuranceDto SelectedInsurance
            {
                get; set;
            }

            public PatientDto SelectedPatient
            {
                get; set;
            }

            public PracticeDto SelectedPractice
            {
                get; set;
            }

            public byte[] Thumbnail
            {
                get; set;
            }

            #endregion Properties
        }

        #endregion Nested Types
    }
}