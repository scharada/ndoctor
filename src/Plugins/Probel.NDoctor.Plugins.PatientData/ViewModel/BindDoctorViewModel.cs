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
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Timers;
    using System.Windows.Input;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientData.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Toolbox;

    internal class BindDoctorViewModel : BaseViewModel
    {
        #region Fields

        public static readonly Timer Countdown = new Timer(250) { AutoReset = true };

        private IPatientDataComponent component;
        private string criteria;
        private LightDoctorDto selectedDoctor;

        #endregion Fields

        #region Constructors

        public BindDoctorViewModel()
        {
            if (!Designer.IsDesignMode)
            {
                this.component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();
                PluginContext.Host.UserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();
            }

            this.FoundDoctors = new ObservableCollection<LightDoctorDto>();
            this.SearchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());
            this.SelectDoctorCommand = new RelayCommand(() => this.SelectDoctor(), () => this.CanSelectDoctor());

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

        public ObservableCollection<LightDoctorDto> FoundDoctors
        {
            get;
            private set;
        }

        public ICommand SearchCommand
        {
            get;
            private set;
        }

        public ICommand SelectDoctorCommand
        {
            get;
            private set;
        }

        public LightDoctorDto SelectedDoctor
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

        private bool CanSelectDoctor()
        {
            var result = this.SelectedDoctor != null
                && PluginContext.Host.SelectedPatient != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);

            if (!result)
            {
                if (!PluginContext.DoorKeeper.IsUserGranted(To.Write))
                {
                    PluginContext.Host.WriteStatus(StatusType.Warning, Messages.Msg_CantSelectDoctor_NotGranted);
                }
                else { PluginContext.Host.WriteStatus(StatusType.Warning, Messages.Msg_CantSelectDoctor); }
            }

            return result;
        }

        private void Search()
        {
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory
                .StartNew<IList<LightDoctorDto>>(state => this.SearchAsync(state as LightPatientDto), PluginContext.Host.SelectedPatient)
                .ContinueWith(e => this.FoundDoctors.Refill(e.Result), context);
        }

        private IList<LightDoctorDto> SearchAsync(LightPatientDto selectedPatient)
        {
            Assert.IsNotNull(selectedPatient, "selectedPatient");
            PluginContext.Host.SetWaitCursor();
            var result = this.component.GetNotLinkedDoctorsFor(selectedPatient, this.Criteria, PluginContext.Configuration.SearchType);
            PluginContext.Host.SetArrowCursor();
            return result;
        }

        private void SelectDoctor()
        {
            try
            {
                this.component.AddDoctorTo(PluginContext.Host.SelectedPatient, this.SelectedDoctor);
                this.Close();
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        #endregion Methods
    }
}