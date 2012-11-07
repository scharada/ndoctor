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

namespace Probel.NDoctor.Plugins.PathologyManager.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Timers;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PathologyManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Toolbox;

    internal class AddPeriodViewModel : BaseViewModel
    {
        #region Fields

        private readonly ICommand addPeriodCommand;
        private readonly IPathologyComponent Component = PluginContext.ComponentFactory.GetInstance<IPathologyComponent>();
        private readonly Timer Countdown = new Timer(250) { AutoReset = true };
        private readonly ICommand searchCommand;

        private string criteria;
        private DateTime endDate;
        private PathologyDto selectedPathology;
        private DateTime startDate;

        #endregion Fields

        #region Constructors

        public AddPeriodViewModel()
        {
            this.StartDate = DateTime.Today.AddDays(-5);
            this.EndDate = DateTime.Today;
            this.FoundPathologies = new ObservableCollection<PathologyDto>();

            this.searchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());
            this.addPeriodCommand = new RelayCommand(() => this.AddPeriod(), () => this.CanAddPeriod());

            Countdown.Elapsed += (sender, e) => PluginContext.Host.Invoke(() =>
            {
                this.SearchCommand.TryExecute();
                Countdown.Stop();
            });
        }

        #endregion Constructors

        #region Properties

        public ICommand AddPeriodCommand
        {
            get { return this.addPeriodCommand; }
        }

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

        public DateTime EndDate
        {
            get { return this.endDate; }
            set
            {
                this.endDate = value;
                this.OnPropertyChanged(() => EndDate);
            }
        }

        public ObservableCollection<PathologyDto> FoundPathologies
        {
            get;
            private set;
        }

        public ICommand SearchCommand
        {
            get { return this.searchCommand; }
        }

        public PathologyDto SelectedPathology
        {
            get { return this.selectedPathology; }
            set
            {
                this.selectedPathology = value;
                this.OnPropertyChanged(() => SelectedPathology);
            }
        }

        public DateTime StartDate
        {
            get { return this.startDate; }
            set
            {
                this.startDate = value;
                this.OnPropertyChanged(() => StartDate);
            }
        }

        #endregion Properties

        #region Methods

        private void AddPeriod()
        {
            try
            {
                this.Component.Create(new IllnessPeriodDto()
                {
                    Start = this.StartDate,
                    End = this.EndDate,
                    Pathology = this.SelectedPathology,
                }, PluginContext.Host.SelectedPatient);
                this.Close();
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_PeriodAdded);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private bool CanAddPeriod()
        {
            return this.SelectedPathology != null;
        }

        private bool CanSearch()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Read);
        }

        private void Search()
        {
            try
            {
                var temp = this.Component.GetPathologiesByName(this.Criteria);
                this.FoundPathologies.Refill(temp);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        #endregion Methods
    }
}