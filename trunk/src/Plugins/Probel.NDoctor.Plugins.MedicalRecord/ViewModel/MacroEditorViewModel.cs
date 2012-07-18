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
    using System.Collections.ObjectModel;
    using System.Timers;
    using System.Windows.Input;

    using ICSharpCode.AvalonEdit.Document;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class MacroEditorViewModel : BaseViewModel
    {
        #region Fields

        private readonly IMedicalRecordComponent Component = PluginContext.ComponentFactory.GetInstance<IMedicalRecordComponent>();
        private readonly object locker = new object();
        private readonly Timer timer;

        private MacroDto selectedMacro;
        private string resolvedMacro;
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
        }

        #endregion Constructors

        #region Properties

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

        public MacroDto SelectedMacro
        {
            get { return this.selectedMacro; }
            set
            {
                this.selectedMacro = value;
                this.TextDocument = new TextDocument(value.Expression);
                this.OnPropertyChanged(() => SelectedMacro);
            }
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

        public void Update()
        {
            if (this.SelectedMacro != null)
            {
                this.timer.Start();
            }
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

        private void TestMacro()
        {
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

        #endregion Methods
    }
}