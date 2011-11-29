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
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Probel.Helpers.Conversions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PrescriptionManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private IPrescriptionComponent component = ObjectFactory.GetInstance<IPrescriptionComponent>();
        private DateTime endCriteria;
        private PrescriptionDocumentDto selectPrescriptionDocument;
        private DateTime startCriteria;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
            : base()
        {
            this.StartCriteria = DateTime.Today.AddMonths(-1);
            this.EndCriteria = DateTime.Today.AddMonths(1);

            this.FoundPrescriptions = new ObservableCollection<PrescriptionDocumentDto>();

            this.SearchCommand = new RelayCommand(() => this.Search(), () => this.CanSearch());
        }

        #endregion Constructors

        #region Properties

        public string BtnSearch
        {
            get { return Messages.Btn_Search; }
        }

        public DateTime EndCriteria
        {
            get { return this.endCriteria; }
            set
            {
                this.endCriteria = value;
                this.OnPropertyChanged("EndCriteria");
            }
        }

        public ObservableCollection<PrescriptionDocumentDto> FoundPrescriptions
        {
            get;
            private set;
        }

        public ICommand SearchCommand
        {
            get;
            private set;
        }

        public PrescriptionDocumentDto SelectedPrescriptionDocument
        {
            get { return this.selectPrescriptionDocument; }
            set
            {
                this.selectPrescriptionDocument = value;
                this.OnPropertyChanged("SelectedPrescriptionDocument");
            }
        }

        public DateTime StartCriteria
        {
            get { return this.startCriteria; }
            set
            {
                this.startCriteria = value;
                this.OnPropertyChanged("StartCriteria");
            }
        }

        public string TitleFrom
        {
            get { return Messages.Title_From; }
        }

        public string TitleTo
        {
            get { return Messages.Title_To; }
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
                    this.FoundPrescriptions.Refill(found);
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