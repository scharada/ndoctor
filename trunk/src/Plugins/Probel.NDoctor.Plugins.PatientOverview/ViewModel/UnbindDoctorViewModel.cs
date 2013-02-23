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
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.Plugins.PatientOverview.Actions;

    internal class UnbindDoctorViewModel : BaseViewModel
    {
        #region Fields

        private readonly IPatientDataComponent Component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();
        private readonly ICommand selectDoctorCommand;

        private LightDoctorDto selectedDoctor;

        #endregion Fields

        #region Constructors

        public UnbindDoctorViewModel()
        {
            this.FoundDoctors = new ObservableCollection<LightDoctorDto>();
            this.selectDoctorCommand = new RelayCommand(() => this.SelectDoctor(), () => this.CanSelectDoctor());
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<LightDoctorDto> FoundDoctors
        {
            get;
            private set;
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

        public void Refresh()
        {
            try
            {
                var result = this.Component.GetDoctorOf(PluginContext.Host.SelectedPatient);
                this.FoundDoctors.Refill(result);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private bool CanSelectDoctor()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private void SelectDoctor()
        {
            PluginDataContext.Instance.Actions.Add(new UnbindDoctorAction(this.Component, PluginContext.Host.SelectedPatient, this.SelectedDoctor));
            PluginDataContext.Instance.OnDoctorUnbinded(this.SelectedDoctor);
            this.Close();
        }

        #endregion Methods
    }
}