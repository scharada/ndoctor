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

namespace Probel.NDoctor.Plugins.PatientOverview.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Timers;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    internal class BindDoctorViewModel : BaseViewModel
    {
        #region Fields

        public static readonly Timer Countdown = new Timer(250) { AutoReset = true };

        private readonly IPatientDataComponent Component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();
        private readonly ICommand searchCommand;
        private readonly ICommand selectDoctorCommand;

        private string criteria;
        private LightDoctorDto selectedDoctor;

        #endregion Fields

        #region Constructors

        public BindDoctorViewModel()
        {
            this.searchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());
            this.selectDoctorCommand = new RelayCommand(() => this.SelectDoctor(), () => this.CanSelectDoctor());

            this.FoundDoctors = new ObservableCollection<LightDoctorDto>();

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
                this.criteria = value;
                Countdown.Start();
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
            get { return this.searchCommand; }
        }

        public ICommand SelectDoctorCommand
        {
            get { return this.selectDoctorCommand; }
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
            return PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private bool CanSelectDoctor()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private void Search()
        {
            try
            {
                var result = this.Component.GetNotLinkedDoctorsFor(PluginContext.Host.SelectedPatient, this.Criteria, PluginContext.Configuration.SearchType);
                this.FoundDoctors.Refill(result);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void SelectDoctor()
        {
            try
            {
                PluginDataContext.Instance.Invoker.Bind(PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>()
                    , PluginContext.Host.SelectedPatient
                    , this.SelectedDoctor);
                this.Close();
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        #endregion Methods
    }
}