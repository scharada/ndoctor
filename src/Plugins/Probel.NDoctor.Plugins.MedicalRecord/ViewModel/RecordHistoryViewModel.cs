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

namespace Probel.NDoctor.Plugins.MedicalRecord.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    internal class RecordHistoryViewModel : BaseViewModel
    {
        #region Fields

        private readonly IMedicalRecordComponent Component = PluginContext.ComponentFactory.GetInstance<IMedicalRecordComponent>();

        private MedicalRecordDto record;
        private MedicalRecordStateDto selectedState;

        #endregion Fields

        #region Constructors

        public RecordHistoryViewModel()
        {
            this.UpdateRecordCommand = new RelayCommand(() => this.UpdateRecord(), () => this.CanUpdateRecord());
            this.History = new ObservableCollection<MedicalRecordStateDto>();
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<MedicalRecordStateDto> History
        {
            get;
            private set;
        }

        public MedicalRecordDto Record
        {
            get { return this.record; }
            set
            {
                this.record = value;
                this.OnPropertyChanged(() => Record);
            }
        }

        public MedicalRecordStateDto SelectedState
        {
            get { return this.selectedState; }
            set
            {
                this.selectedState = value;
                this.OnPropertyChanged(() => SelectedState);
            }
        }

        public ICommand UpdateRecordCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private bool CanUpdateRecord()
        {
            return this.Record != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private void UpdateRecord()
        {
            this.Component.Revert(this.Record, this.SelectedState);
            this.Close();
        }

        #endregion Methods
    }
}