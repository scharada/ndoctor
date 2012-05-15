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

namespace Probel.NDoctor.Plugins.BmiRecord.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Conversions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.BmiRecord.Helpers;
    using Probel.NDoctor.Plugins.BmiRecord.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private BmiDto bmiToAdd;
        private IBmiComponent component;
        private PatientBmiDto patient;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
            : base()
        {
            this.CurrentBmi = new BmiDto();
            this.component = ComponentFactory.BmiComponent;
            this.BmiHistory = new ObservableCollection<BmiViewModel>();

            this.AddBmiCommand = new RelayCommand(() => this.AddBmi(), () => this.CanAddBmi());

            Notifyer.ItemChanged += (sender, e) => this.Refresh();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets command to add a new BMI entry.
        /// </summary>
        public ICommand AddBmiCommand
        {
            get;
            private set;
        }

        public ObservableCollection<BmiViewModel> BmiHistory
        {
            get;
            set;
        }

        public BmiDto CurrentBmi
        {
            get { return this.bmiToAdd; }
            set
            {
                this.bmiToAdd = value;
                this.OnPropertyChanged("CurrentBmi");
            }
        }

        /// <summary>
        /// Gets the command to delete the selected BMI entry.
        /// </summary>
        public ICommand DelBmiCommand
        {
            get;
            private set;
        }

        public DateTime EndDate
        {
            get
            {
                if (this.Patient != null && this.Patient.BmiHistory != null && this.Patient.BmiHistory.Count > 0)
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
                this.CurrentBmi.Height = value.Height;
                this.OnPropertyChanged("Patient", "StartDate", "EndDate", "CurrentBmi");
            }
        }

        public DateTime StartDate
        {
            get
            {
                if (this.Patient != null && this.Patient.BmiHistory != null && this.Patient.BmiHistory.Count > 0)
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
            Assert.IsNotNull(this.Host);
            Assert.IsNotNull(this.Host.SelectedPatient);

            var thread = new BackgroundWorker();
            PatientBmiDto patient = null;

            thread.DoWork += (sender, e) =>
            {
                this.Host.Invoke(() =>
                {
                    using (this.component.UnitOfWork)
                    {
                        patient = this.component.GetPatientWithBmiHistory(this.Host.SelectedPatient);
                    }

                    if (patient.BmiHistory.Count > 0)
                    {
                        var list = Mapper.Map<IList<BmiDto>, IList<BmiViewModel>>(patient.BmiHistory);
                        this.BmiHistory.Refill(list);
                    }
                });
            };
            thread.RunWorkerCompleted += (sender, e) =>
            {
                if (patient == null) return;

                this.Host.Invoke(() =>
                {
                    this.Patient = patient;
                    this.Host.WriteStatus(StatusType.Info, Messages.Msg_BmiHistoryLoaded);
                });
                this.Logger.DebugFormat("Loaded Bmi history ({0} item(s))", patient.BmiHistory.Count);
            };

            thread.RunWorkerAsync();
        }

        private void AddBmi()
        {
            Assert.IsNotNull(this.Host, "The host shouldn't be null");
            Assert.IsNotNull(this.Host.SelectedPatient, "A patient should be selected if you want to manage data of a patient");
            Assert.IsNotNull(this.bmiToAdd, "The BMI to add shouldn't be null in order to add the item to the BMI history");

            try
            {
                using (this.component.UnitOfWork)
                {
                    this.component.AddBmi(this.bmiToAdd, this.Host.SelectedPatient);

                    this.Host.SelectedPatient.Height = this.CurrentBmi.Height;
                    this.component.Update(this.Host.SelectedPatient);
                }
                this.Refresh();
                this.Host.WriteStatus(StatusType.Info, Messages.Msg_BmiAdded);
                this.bmiToAdd = new BmiDto();
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_ErrAddBmi);
            }
        }

        private bool CanAddBmi()
        {
            return this.bmiToAdd.Date <= DateTime.Now
                && this.bmiToAdd.Height > 0
                && this.bmiToAdd.Weight > 0;
        }

        #endregion Methods
    }
}