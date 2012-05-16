﻿/*
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
    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PrescriptionManager.Helpers;
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
            Notifyer.PrescriptionFound += (sender, e) =>
            {
                this.FoundPrescriptions.Refill(e.Data.Prescriptions);
                this.StartCriteria = e.Data.From;
                this.EndCriteria = e.Data.To;

                this.Logger.Debug("Load prescriptions");
            };
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
                this.OnPropertyChanged(() => PrescriptionHeader);
            }
        }

        public ObservableCollection<PrescriptionDocumentDto> FoundPrescriptions
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the prescription header. The binding is done when the <see cref="EndCriteria"/> is updated
        /// </summary>
        public string PrescriptionHeader
        {
            get
            {
                return Messages.Title_PrescriptionHeader.FormatWith(
                    this.StartCriteria.ToShortDateString()
                    , this.EndCriteria.ToShortDateString());
            }
        }

        public PrescriptionDocumentDto SelectedPrescriptionDocument
        {
            get { return this.selectPrescriptionDocument; }
            set
            {
                this.selectPrescriptionDocument = value;
                this.OnPropertyChanged(() => SelectedPrescriptionDocument);
            }
        }

        public DateTime StartCriteria
        {
            get { return this.startCriteria; }
            set
            {
                this.startCriteria = value;
                // I update the  header 'FromTo' from the EndCriteria
                this.OnPropertyChanged(() => StartCriteria);
            }
        }

        #endregion Properties
    }
}