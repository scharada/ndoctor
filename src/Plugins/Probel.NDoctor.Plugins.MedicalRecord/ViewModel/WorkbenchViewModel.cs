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
namespace Probel.NDoctor.Plugins.MedicalRecord.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System.Windows.Media;

    using Probel.Helpers.Assertion;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Dto;
    using Probel.NDoctor.Plugins.MedicalRecord.Editor;
    using Probel.NDoctor.Plugins.MedicalRecord.Helpers;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.Plugins.MedicalRecord.View;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private readonly IMedicalRecordComponent Component = PluginContext.ComponentFactory.GetInstance<IMedicalRecordComponent>();
        private readonly ViewService ViewService = new ViewService();

        private TitledMedicalRecordCabinetDto cabinet;
        private FontFamily defaultFontFamily;
        private int defaultFontSize;
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
            PluginContext.Host.NewPatientConnected += (sender, e) =>
            {
                this.SelectedRecord = null;
            };

            this.MacroMenu = new ObservableCollection<MacroMenuItem>();

            this.RefreshCommand = new RelayCommand(() => this.Refresh(), () => this.CanRefresh());
            this.SaveCommand = new RelayCommand(() => Save(), () => this.CanSave());
            this.ShowRevisionsCommand = new RelayCommand(() => this.ShowRevisions(), () => this.CanShowRevisions());
            this.RefreshDefaultFontsCommand = new RelayCommand(() => this.RefreshDefaultFonts());

            Notifyer.Refreshed += (sender, e) => this.Refresh();
            Notifyer.MacroUpdated += (sender, e) => this.RefreshMacroMenu();
            Notifyer.MacroUpdated += (sender, e) => this.Refresh();

            InnerWindow.Closed += (sender, e) => this.Refresh();
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

        public FontFamily DefaultFontFamily
        {
            get { return this.defaultFontFamily; }
            set
            {
                this.defaultFontFamily = value;
                this.OnPropertyChanged(() => DefaultFontFamily);
            }
        }

        public int DefaultFontSize
        {
            get { return this.defaultFontSize; }
            set
            {
                this.defaultFontSize = value;
                this.OnPropertyChanged(() => DefaultFontSize);
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

        public ObservableCollection<MacroMenuItem> MacroMenu
        {
            get;
            private set;
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public ICommand RefreshDefaultFontsCommand
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

        public ICommand ShowRevisionsCommand
        {
            get;
            private set;
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

        private PluginSettings PluginSettings
        {
            get { return new PluginSettings(); }
        }

        #endregion Properties

        #region Methods

        private ICommand BuildMenuItemCommand(MacroDto macro)
        {
            return new RelayCommand(() =>
            {
                var text = this.Component.Resolve(macro, PluginContext.Host.SelectedPatient);
                TextEditor.Control.CaretPosition.InsertTextInRun(text);
            });
        }

        private bool CanRefresh()
        {
            return PluginContext.Host != null
                && PluginContext.Host.SelectedPatient != null;
        }

        private bool CanSave()
        {
            this.IsGranted = PluginContext.DoorKeeper.IsUserGranted(To.Write);

            return this.SelectedRecord != null && this.IsGranted;
        }

        private bool CanShowRevisions()
        {
            return this.SelectedRecord != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private void Refresh()
        {
            try
            {

                Assert.IsNotNull(PluginContext.Host);
                Assert.IsNotNull(PluginContext.Host.SelectedPatient);

                var result = this.Component.FindMedicalRecordCabinet(PluginContext.Host.SelectedPatient);
                this.Cabinet = TitledMedicalRecordCabinetDto.CreateFrom(result);
                this.Tags = this.Component.FindTags(TagCategory.MedicalRecord);

                if (this.SelectedRecord != null)
                {
                    var record = this.Component.FindMedicalRecordById(this.SelectedRecord.Id);
                    this.SelectedRecord = (record != null)
                        ? TitledMedicalRecordDto.CreateFrom(record)
                        : null;
                }

                this.RefreshMacroMenu();

                PluginContext.Host.WriteStatus(StatusType.Info, BaseText.Refreshed);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private void RefreshDefaultFonts()
        {
            this.DefaultFontFamily = this.PluginSettings.FontFamily;
            this.DefaultFontSize = this.PluginSettings.FontSize;
        }

        private void RefreshMacroMenu()
        {
            var macros = new List<MacroMenuItem>();
            foreach (var macro in this.Component.GetAllMacros())
            {
                macros.Add(new MacroMenuItem(macro.Title, this.BuildMenuItemCommand(macro)));
            }
            this.MacroMenu.Refill(macros);
        }

        private void Save()
        {
            try
            {
                Assert.IsNotNull(PluginContext.Host);
                Assert.IsNotNull(PluginContext.Host.SelectedPatient);

                TextEditor.UpdateBinding();

                this.Cabinet.ForEachRecord(x =>
                {
                    if (x.Id == this.selectedRecord.Id
                        && x.Rtf != this.selectedRecord.Rtf)
                    {
                        x.Rtf = this.SelectedRecord.Rtf;
                        x.LastUpdate = DateTime.Now;
                    }
                }
                    , s => s.Id == this.SelectedRecord.Id);

                this.Component.Update(this.Cabinet);

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_RecordsSaved);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private void ShowRevisions()
        {
            var history = this.Component.GetHistory(this.SelectedRecord);
            var view = new RecordHistoryView();

            this.ViewService.GetViewModel(view).History.Refill(history);
            this.ViewService.GetViewModel(view).Record = this.SelectedRecord;

            InnerWindow.Show(Messages.Title_Rollback, view);
        }

        #endregion Methods
    }
}