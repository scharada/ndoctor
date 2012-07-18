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

    using Probel.Helpers.Assertion;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Dto;
    using Probel.NDoctor.Plugins.MedicalRecord.Helpers;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;
    using System.Collections.ObjectModel;
    using Probel.NDoctor.Plugins.MedicalRecord.Editor;

    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private TitledMedicalRecordCabinetDto cabinet;
        private IMedicalRecordComponent component = PluginContext.ComponentFactory.GetInstance<IMedicalRecordComponent>();
        private bool isGranted = true;
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
            PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IMedicalRecordComponent>();

            this.MacroMenu = new ObservableCollection<MacroMenuItem>();

            this.RefreshCommand = new RelayCommand(() => this.Refresh());
            this.SaveCommand = new RelayCommand(() => Save(), () => this.CanSave());

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

        public bool IsGranted
        {
            get { return this.isGranted; }
            set
            {
                if (this.isGranted != value)
                {
                    this.isGranted = value;
                    this.OnPropertyChanged(() => IsGranted);
                }
            }
        }

        public bool IsRecordSelected
        {
            get { return this.SelectedRecord != null; }
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public ICommand SaveCommand
        {
            get;
            private set;
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

        /// <summary>
        /// Saves the record if the user interaction demand saving.
        /// </summary>
        internal void SaveOnUserAction()
        {
            try
            {
                if (this.SelectedRecord != null && this.SelectedRecord.State == State.Updated)
                {
                    var dr = MessageBox.Show(Messages.Msg_SaveMedicalRecord, Messages.Question, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (dr == MessageBoxResult.Yes) { this.Save(); }
                }
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private bool CanSave()
        {
            this.IsGranted = PluginContext.DoorKeeper.IsUserGranted(To.Write);

            return this.SelectedRecord != null && this.IsGranted;
        }

        private void Refresh()
        {
            try
            {
                Assert.IsNotNull(PluginContext.Host);
                Assert.IsNotNull(PluginContext.Host.SelectedPatient);

                using (var unitOfWork = this.component.UnitOfWork)
                {
                    var result = this.component.FindMedicalRecordCabinet(PluginContext.Host.SelectedPatient);
                    this.Cabinet = TitledMedicalRecordCabinetDto.CreateFrom(result);
                    this.Tags = this.component.FindTags(TagCategory.MedicalRecord);

                    if (this.SelectedRecord != null)
                    {
                        var record = this.component.FindMedicalRecordById(this.SelectedRecord.Id);
                        this.SelectedRecord = (record != null)
                            ? TitledMedicalRecordDto.CreateFrom(record)
                            : null;
                    }
                    this.RefreshMacroMenu();
                }

                PluginContext.Host.WriteStatus(StatusType.Info, BaseText.Refreshed);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private void RefreshMacroMenu()
        {
            var macros = new List<MacroMenuItem>();
            foreach (var macro in this.component.GetAllMacros())
            {
                macros.Add(new MacroMenuItem(macro.Title, this.BuildMenuItemCommand(macro)));
            }
            this.MacroMenu.Refill(macros);
        }

        private ICommand BuildMenuItemCommand(MacroDto macro)
        {
            return new RelayCommand(() =>
            {
                string text;
                using (this.component.UnitOfWork)
                {
                    text = this.component.Resolve(macro, PluginContext.Host.SelectedPatient);
                }
                Context.RichTextBox.CaretPosition.InsertTextInRun(text);
            });
        }

        private void Save()
        {
            try
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

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_RecordsSaved);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        #endregion Methods


        public ObservableCollection<MacroMenuItem> MacroMenu
        {
            get;
            private set;
        }
    }
}