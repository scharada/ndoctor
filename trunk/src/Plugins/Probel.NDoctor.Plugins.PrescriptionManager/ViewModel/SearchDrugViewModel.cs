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

namespace Probel.NDoctor.Plugins.PrescriptionManager.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Timers;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PrescriptionManager.Helpers;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    internal class SearchDrugViewModel : BaseViewModel
    {
        #region Fields

        private static readonly Timer Countdown = new Timer(250) { AutoReset = true };

        private IPrescriptionComponent component = PluginContext.ComponentFactory.GetInstance<IPrescriptionComponent>();
        private string criteriaName;
        private TagDto criteriaTag;
        private DrugDto selectedDrug;

        #endregion Fields

        #region Constructors

        public SearchDrugViewModel()
        {
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPrescriptionComponent>();

            this.FoundDrugs = new ObservableCollection<DrugDto>();
            this.Tags = new ObservableCollection<TagDto>();

            this.SelectCommand = new RelayCommand(() => this.Select(), () => this.CanSelect());
            this.SearchOnNameCommand = new RelayCommand(() => this.SearchOnName(), () => this.CanSearchOnName());
            this.SearchOnTagCommand = new RelayCommand(() => this.SearchOnTag(), () => this.CanSearchOnTag());

            Countdown.Elapsed += (sender, e) => PluginContext.Host.Invoke(() =>
            {
                this.SearchOnNameCommand.TryExecute();
                Countdown.Stop();
            });
        }

        #endregion Constructors

        #region Properties

        public string CriteriaName
        {
            get { return this.criteriaName; }
            set
            {
                Countdown.Start();
                this.criteriaName = value;
                this.OnPropertyChanged(() => CriteriaName);
            }
        }

        public TagDto CriteriaTag
        {
            get { return this.criteriaTag; }
            set
            {
                this.criteriaTag = value;
                this.OnPropertyChanged(() => CriteriaTag);
            }
        }

        public ObservableCollection<DrugDto> FoundDrugs
        {
            get;
            private set;
        }

        public ICommand SearchOnNameCommand
        {
            get;
            private set;
        }

        public ICommand SearchOnTagCommand
        {
            get;
            private set;
        }

        public ICommand SelectCommand
        {
            get;
            private set;
        }

        public DrugDto SelectedDrug
        {
            get { return this.selectedDrug; }
            set
            {
                this.selectedDrug = value;
                this.OnPropertyChanged(() => SelectedDrug);
            }
        }

        public ObservableCollection<TagDto> Tags
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            try
            {
                IList<TagDto> results;
                using (this.component.UnitOfWork)
                {
                    results = this.component.FindTags(TagCategory.Drug);
                }
                this.Tags.Refill(results);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private bool CanSearchOnName()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Read)
                && !string.IsNullOrWhiteSpace(this.CriteriaName);
        }

        private bool CanSearchOnTag()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Read)
                && this.CriteriaTag != null
                && this.CriteriaTag.Category == TagCategory.Drug;
        }

        private bool CanSelect()
        {
            return this.SelectedDrug != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private void SearchOnName()
        {
            try
            {
                IList<DrugDto> results;
                using (this.component.UnitOfWork)
                {
                    results = this.component.FindDrugsByName(this.CriteriaName);
                }
                this.FoundDrugs.Refill(results);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private void SearchOnTag()
        {
            try
            {
                IList<DrugDto> results;
                using (this.component.UnitOfWork)
                {
                    results = this.component.FindDrugsByTags(this.CriteriaTag.Name);
                }
                this.FoundDrugs.Refill(results);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private void Select()
        {
            Notifyer.OnDrugSelected(this, this.SelectedDrug);
            InnerWindow.Close();
        }

        #endregion Methods
    }
}