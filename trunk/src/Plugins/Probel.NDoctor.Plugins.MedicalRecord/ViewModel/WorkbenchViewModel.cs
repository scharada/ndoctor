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
namespace Probel.NDoctor.Plugins.MedicalRecord.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    using AutoMapper;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Dto;
    using Probel.NDoctor.Plugins.MedicalRecord.Helpers;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private TitledMedicalRecordCabinetDto cabinet;
        private IMedicalRecordComponent component = new ComponentFactory().GetInstance<IMedicalRecordComponent>();
        private TitledMedicalRecordDto selectedRecord;
        private IList<TagDto> tags = new List<TagDto>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
            : base()
        {
            Notifyer.Refreshed += (sender, e) => this.Refresh();
        }

        #endregion Constructors

        #region Properties

        public TitledMedicalRecordCabinetDto Cabinet
        {
            get { return this.cabinet; }
            set
            {
                this.cabinet = value;
                this.OnPropertyChanged(() => Cabinet);
            }
        }

        public bool IsRecordSelected
        {
            get { return this.SelectedRecord != null; }
        }

        public TitledMedicalRecordDto SelectedRecord
        {
            get { return this.selectedRecord; }
            set
            {
                this.selectedRecord = value;
                this.OnPropertyChanged(() => this.SelectedRecord);
                this.OnPropertyChanged(() => this.IsRecordSelected);
            }
        }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public IList<TagDto> Tags
        {
            get { return this.tags; }
            set
            {
                this.tags = value;
                this.OnPropertyChanged(() => Tags);
            }
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            Assert.IsNotNull(PluginContext.Host);
            Assert.IsNotNull(PluginContext.Host.SelectedPatient);

            using (this.component.UnitOfWork)
            {
                var result = this.component.GetMedicalRecordCabinet(PluginContext.Host.SelectedPatient);
                this.Cabinet = TitledMedicalRecordCabinetDto.CreateFrom(result);
                this.Tags = this.component.FindTags(TagCategory.MedicalRecord);

                if (this.SelectedRecord != null)
                {
                    var record = this.component.FindMedicalRecordById(this.SelectedRecord.Id);
                    this.SelectedRecord = (record != null)
                        ? TitledMedicalRecordDto.CreateFrom(record)
                        : null;
                }
            }
        }

        public void Save()
        {
            Assert.IsNotNull(PluginContext.Host);
            Assert.IsNotNull(PluginContext.Host.SelectedPatient);

            this.Cabinet.ForEachRecord(x =>
            {
                if (x.Rtf != this.selectedRecord.Rtf)
                {
                    x.Rtf = this.SelectedRecord.Rtf;
                    //x.State = State.Updated;
                    x.LastUpdate = DateTime.Now;
                }
            }
                , s => s.Id == this.SelectedRecord.Id);

            using (this.component.UnitOfWork)
            {
                this.component.UpdateCabinet(PluginContext.Host.SelectedPatient, this.Cabinet);
            }
        }

        /// <summary>
        /// Saves the record if the user interaction demand saving.
        /// </summary>
        internal void SaveOnUserAction()
        {
            if (this.SelectedRecord != null && this.SelectedRecord.State == State.Updated)
            {
                var dr = MessageBox.Show(Messages.Msg_SaveMedicalRecord, Messages.Question, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dr == MessageBoxResult.Yes) { this.Save(); }
            }
        }

        #endregion Methods
    }
}