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

namespace Probel.NDoctor.Plugins.MedicalRecord.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Timers;
    using System.Windows;
    using System.Windows.Input;

    using ICSharpCode.AvalonEdit.Document;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Helpers;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class MacroEditorViewModel : BaseViewModel
    {
        #region Fields

        private readonly IMedicalRecordComponent Component = PluginContext.ComponentFactory.GetInstance<IMedicalRecordComponent>();
        private readonly object locker = new object();
        private readonly Timer timer;

        private string resolvedMacro;
        private MacroDto selectedMacro;
        private TextDocument textDocument;

        #endregion Fields

        #region Constructors

        public MacroEditorViewModel()
        {
            this.timer = new Timer(500) { AutoReset = true };
            this.timer.Elapsed += (sender, e) => this.TestMacro();
            this.timer.Start();

            this.Macros = new ObservableCollection<MacroDto>();

            this.RefreshCommand = new RelayCommand(() => this.Refresh());
            this.UpdateCommand = new RelayCommand(() => this.Update(), () => this.CanUpdate());
            this.CreateCommand = new RelayCommand(() => this.Create(), () => this.CanCreate());
            this.RemoveCommand = new RelayCommand(() => this.Remove(), () => this.CanRemove());

            InnerWindow.Closed += (sender, e) => this.UpdateCommand.TryExecute();
        }

        #endregion Constructors

        #region Properties

        public ICommand CreateCommand
        {
            get;
            private set;
        }

        public ObservableCollection<MacroDto> Macros
        {
            get;
            private set;
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public ICommand RemoveCommand
        {
            get;
            private set;
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

        public MacroDto SelectedMacro
        {
            get { return this.selectedMacro; }
            set
            {

                this.selectedMacro = value;

                var text = (value != null)
                    ? value.Expression ?? string.Empty
                    : string.Empty;

                this.TextDocument = new TextDocument(text);
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

        public ICommand UpdateCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public void StartTimer()
        {
            if (this.SelectedMacro != null)
            {
                this.timer.Start();
            }
        }

        private bool CanCreate()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Administer);
        }

        private bool CanRemove()
        {
            return this.SelectedMacro != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Administer);
        }

        private bool CanUpdate()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Administer)
                && this.SelectedMacro != null
                && this.SelectedMacro.IsValid();
        }

        private void Create()
        {
            // Save previous macro.
            if (this.SelectedMacro != null) { this.Update(); }

            this.SelectedMacro = new MacroDto()
            {
                Title = Messages.Title_DefaultMacroTitle,
            };

            try
            {
                using (this.Component.UnitOfWork)
                {
                    this.Component.Create(this.SelectedMacro);
                }
            }
            catch (Exception ex) { this.HandleError(ex); }

            Notifyer.OnMacroUpdated();
            this.Refresh();
        }

        private void Refresh()
        {
            MacroDto[] macros;
            using (this.Component.UnitOfWork)
            {
                macros = this.Component.GetAllMacros();
            }
            if (macros != null) { this.Macros.Refill(macros); }
        }

        private void Remove()
        {
            var dr = MessageBox.Show(Messages.Question_RemoveMacro.FormatWith(this.SelectedMacro.Title), BaseText.Question, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dr != MessageBoxResult.Yes) return;

            try
            {
                using (this.Component.UnitOfWork)
                {
                    this.Component.Remove(this.SelectedMacro);
                }
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_MacroRemoved.FormatWith(this.SelectedMacro.Title));

                this.SelectedMacro = null;

                Notifyer.OnMacroUpdated();
                this.Refresh();
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private void TestMacro()
        {
            //Todo: this code has a problem with multithread. If this thread is running, all component method call will throw an SessionException
            timer.Stop();
            lock (this.locker)
            {
                PluginContext.Host.Invoke(() =>
                {
                    if (this.SelectedMacro != null) { this.SelectedMacro.Expression = this.TextDocument.Text; }

                    using (this.Component.UnitOfWork)
                    {
                        if (this.Component.IsValid(this.SelectedMacro))
                        {
                            this.ResolvedMacro = this.Component.Resolve(this.SelectedMacro, PluginContext.Host.SelectedPatient);
                        }
                        else { this.ResolvedMacro = Messages.Err_InvalidMacro; }
                    }
                });
            }
        }

        private void Update()
        {
            try
            {
                using (this.Component.UnitOfWork)
                {
                    this.Component.Update(this.SelectedMacro);
                }
                var macroName = (this.SelectedMacro != null)
                    ? this.SelectedMacro.Title ?? Messages.NoName
                    : Messages.NoName;

                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_MacroUpdated.FormatWith(macroName));
                Notifyer.OnMacroUpdated();
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        #endregion Methods
    }
}