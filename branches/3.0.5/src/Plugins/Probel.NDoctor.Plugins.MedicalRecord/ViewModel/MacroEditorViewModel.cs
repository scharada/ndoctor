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
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using ICSharpCode.AvalonEdit.Document;

    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Toolbox;

    internal class MacroEditorViewModel : BaseViewModel
    {
        #region Fields

        private readonly IMedicalRecordComponent Component = PluginContext.ComponentFactory.GetInstance<IMedicalRecordComponent>();
        private readonly ICommand createCommand;
        private readonly ICommand refreshCommand;
        private readonly ICommand removeCommand;
        private readonly ICommand saveCommand;

        private bool hasWarnedOnInvalidMacro = false;
        private string resolvedMacro;
        private MacroDto selectedMacro;
        private TextDocument textDocument;

        #endregion Fields

        #region Constructors

        public MacroEditorViewModel()
        {
            this.Macros = new ObservableCollection<MacroDto>();

            this.refreshCommand = new RelayCommand(() => this.Refresh());
            this.createCommand = new RelayCommand(() => this.Create(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.removeCommand = new RelayCommand(() => this.Remove(), () => PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.saveCommand = new RelayCommand(() => this.Save(), () => this.CanSave());
        }

        #endregion Constructors

        #region Properties

        public ICommand CreateCommand
        {
            get { return this.createCommand; }
        }

        public ObservableCollection<MacroDto> Macros
        {
            get;
            private set;
        }

        public ICommand RefreshCommand
        {
            get { return this.refreshCommand; }
        }

        public ICommand RemoveCommand
        {
            get { return this.removeCommand; }
        }

        public string ResolvedMacro
        {
            get { return this.resolvedMacro; }
            set
            {
                this.resolvedMacro = value;
                this.OnPropertyChanged(() => ResolvedMacro);
            }
        }

        public ICommand SaveCommand
        {
            get { return this.saveCommand; }
        }

        public MacroDto SelectedMacro
        {
            get { return this.selectedMacro; }
            set
            {
                this.selectedMacro = value;

                var text = (value != null)
                    ? value.Expression ?? string.Empty
                    : string.Empty;

                this.TextDocument = null;
                this.TextDocument = new TextDocument(text);
                this.TextDocument.TextChanged += (sender, e) =>
                {
                    if (this.SelectedMacro != null) { this.SelectedMacro.Expression = this.TextDocument.Text; }
                };

                this.OnPropertyChanged(() => SelectedMacro);
            }
        }

        public TextDocument TextDocument
        {
            get { return this.textDocument; }
            set
            {
                this.textDocument = value;
                this.OnPropertyChanged(() => TextDocument);
            }
        }

        #endregion Properties

        #region Methods

        internal void ResolveMacro()
        {
            try
            {
                this.ResolvedMacro = this.Component.Resolve(this.SelectedMacro, PluginContext.Host.SelectedPatient);
            }
            catch (Exception)
            {
                this.ResolvedMacro = string.Format(Messages.Err_InvalidMacro);
            }
        }

        private bool CanSave()
        {
            var areValidMacros = this.Component.IsValid(this.Macros);

            if (!areValidMacros && !this.hasWarnedOnInvalidMacro)
            {
                this.hasWarnedOnInvalidMacro = true;
                ViewService.MessageBox.Warning(Messages.Err_InvalidMacros);
            }

            return PluginContext.DoorKeeper.IsUserGranted(To.Write)
                && areValidMacros;
        }

        private void Create()
        {
            try
            {
                var macro = new MacroDto() { Title = Messages.Macro_New };
                this.Component.Create(macro);
                this.Macros.Add(macro);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void Refresh()
        {
            try
            {
                var macros = this.Component.GetAllMacros();
                if (macros != null) { this.Macros.Refill(macros); }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void Remove()
        {
            try
            {
                if (this.SelectedMacro != null)
                {
                    this.Component.Remove(this.SelectedMacro);
                    this.Macros.Remove(this.SelectedMacro);
                    this.refreshCommand.TryExecute();
                }
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void Save()
        {
            try
            {
                this.Component.Update(this.Macros);

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_MacrosUpdated);
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        #endregion Methods
    }
}