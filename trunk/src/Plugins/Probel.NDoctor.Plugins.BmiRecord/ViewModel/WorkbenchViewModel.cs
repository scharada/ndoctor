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
namespace Probel.NDoctor.Plugins.BmiRecord.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.BmiRecord.Helpers;
    using Probel.NDoctor.Plugins.BmiRecord.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private IBmiComponent component;
        private PatientBmiDto patient;
        private BmiDto selectedBmi;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
            : base()
        {
            if (!Designer.IsDesignMode)
            {
                this.component = PluginContext.ComponentFactory.GetInstance<IBmiComponent>();

                PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IBmiComponent>();
                PluginContext.Host.NewPatientConnected += (sender, e) => this.Refresh();
            }
            this.SelectedBmi = new BmiDto();
            this.component = PluginContext.ComponentFactory.GetInstance<IBmiComponent>();
            this.BmiHistory = new ObservableCollection<BmiDto>();

            this.RemoveBmiCommand = new RelayCommand(() => this.RemoveBmi(), () => this.CanRemoveBmi());

            Notifyer.ItemChanged += (sender, e) => this.Refresh();
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<BmiDto> BmiHistory
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get
            {
                if (this.Patient != null && this.Patient.BmiHistory != null && this.Patient.BmiHistory.Length > 0)
                {
                    var result = this.Patient.BmiHistory.Max(e => e.Date);
                    return result.Date;
                }
                else return DateTime.Now;
            }
        }

        public PatientBmiDto Patient
        {
            get { return this.patient; }
            set
            {
                this.patient = value;
                if (this.SelectedBmi != null) { this.SelectedBmi.Height = value.Height; }

                this.OnPropertyChanged(() => this.Patient);
                this.OnPropertyChanged(() => this.StartDate);
                this.OnPropertyChanged(() => this.EndDate);
                this.OnPropertyChanged(() => this.SelectedBmi);
            }
        }

        /// <summary>
        /// Gets the command to delete the selected BMI entry.
        /// </summary>
        public ICommand RemoveBmiCommand
        {
            get;
            private set;
        }

        public BmiDto SelectedBmi
        {
            get { return this.selectedBmi; }
            set
            {
                this.selectedBmi = value;
                this.OnPropertyChanged(() => SelectedBmi);
            }
        }

        public DateTime StartDate
        {
            get
            {
                if (this.Patient != null && this.Patient.BmiHistory != null && this.Patient.BmiHistory.Length > 0)
                {
                    var result = this.Patient.BmiHistory.Min(e => e.Date);
                    return result.Date;
                }
                else return DateTime.Now;
            }
        }

        public string TitleAddBmi
        {
            get { return Messages.Title_BtnAdd; }
        }

        public string TitleAddExpanderHeader
        {
            get { return Messages.Title_ExpanderHeaderAdd; }
        }

        public string TitleBmi
        {
            get { return Messages.Title_Bmi; }
        }

        public string TitleBmiChart
        {
            get { return Messages.Header_BmiChart; }
        }

        public string TitleBmiOverweight
        {
            get { return Messages.Title_BmiOverweight; }
        }

        public string TitleBmiUnderweight
        {
            get { return Messages.Title_BmiUnderweight; }
        }

        public string TitleBmiXAxes
        {
            get { return Messages.Title_BmiXAxes; }
        }

        public string TitleBmiYAxes
        {
            get { return Messages.Title_BmiYAxes; }
        }

        public string TitleDate
        {
            get { return Messages.Title_BmiDate; }
        }

        public string TitleDeleteBmi
        {
            get { return Messages.Title_BtnDelete; }
        }

        public string TitleDeleteExpanderHeader
        {
            get { return Messages.Title_ExpanderHeaderDelete; }
        }

        public string TitleHeight
        {
            get { return Messages.Title_BmiHeight; }
        }

        public string TitleHeightChart
        {
            get { return Messages.Header_HeightChart; }
        }

        public string TitleObesity
        {
            get { return Messages.Title_BmiObesity; }
        }

        public string TitleRollbackBmi
        {
            get { return Messages.Title_RollbackBmi; }
        }

        public string TitleWeight
        {
            get { return Messages.Title_BmiWeight; }
        }

        public string TitleWeightChart
        {
            get { return Messages.Header_WeightChart; }
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            Assert.IsNotNull(PluginContext.Host);
            Assert.IsNotNull(PluginContext.Host.SelectedPatient);

            try
            {
                var patient = this.component.GetPatientWithBmiHistory(PluginContext.Host.SelectedPatient);

                this.Patient = patient;
                this.BmiHistory.Refill(this.Patient.BmiHistory);
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_BmiHistoryLoaded);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private bool CanRemoveBmi()
        {
            return this.SelectedBmi != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private void RemoveBmi()
        {
            try
            {
                var dr = MessageBox.Show(Messages.Msg_AskDeleteBmi, BaseText.Question, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dr != MessageBoxResult.Yes) return;

                this.component.Remove(this.SelectedBmi, this.Patient);

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_BmiDeleted);
                Notifyer.OnItemChanged(this);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        #endregion Methods
    }
}