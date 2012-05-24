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
namespace Probel.NDoctor.Plugins.PrescriptionManager.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Probel.Helpers.Conversions;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PrescriptionManager.Helpers;
    using Probel.NDoctor.Plugins.PrescriptionManager.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class SearchPrescriptionViewModel : BaseViewModel
    {
        #region Fields

        private IPrescriptionComponent component = PluginContext.ComponentFactory.GetInstance<IPrescriptionComponent>();
        private DateTime endCriteria = DateTime.Today;
        private DateTime startCriteria = DateTime.Today.AddDays(-30);

        #endregion Fields

        #region Constructors

        public SearchPrescriptionViewModel()
        {
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IPrescriptionComponent>();
            this.SearchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());
        }

        #endregion Constructors

        #region Properties

        public DateTime EndCriteria
        {
            get { return this.endCriteria; }
            set
            {
                this.endCriteria = value;
                this.OnPropertyChanged(() => EndCriteria);
            }
        }

        public ICommand SearchCommand
        {
            get;
            private set;
        }

        public DateTime StartCriteria
        {
            get { return this.startCriteria; }
            set
            {
                this.startCriteria = value;
                this.OnPropertyChanged(() => StartCriteria);
            }
        }

        #endregion Properties

        #region Methods

        private bool CanSearch()
        {
            return this.StartCriteria < this.EndCriteria;
        }

        private void Search()
        {
            try
            {
                using (this.component.UnitOfWork)
                {
                    var found = this.component.FindPrescriptionsByDates(PluginContext.Host.SelectedPatient
                        , this.StartCriteria, this.EndCriteria);
                    Notifyer.OnPrescriptionFound(this, new PrescriptionResultDto(found, this.StartCriteria, this.EndCriteria));
                    InnerWindow.Close();

                    if (found.Count > 0) PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_FoundPrescription);
                    else PluginContext.Host.WriteStatus(StatusType.Warning, Messages.Msg_NothingFound);
                }
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_ErrorSearchingPrescriptions);
            }
        }

        #endregion Methods
    }
}