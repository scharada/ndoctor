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
namespace Probel.NDoctor.Plugins.DbConvert.ViewModel
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;

    using Microsoft.Win32;

    using Probel.Helpers.Assertion;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Plugins.DbConvert.Domain;
    using Probel.NDoctor.Plugins.DbConvert.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    internal class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private IImportComponent component;
        private CultureInfo cultureInfo = Thread.CurrentThread.CurrentUICulture;
        private string database;
        private bool importDone = false;
        private string logs;
        private int progress;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        public WorkbenchViewModel()
        {
            this.component = PluginContext.ComponentFactory.GetInstance<IImportComponent>();
            PluginContext.Host.UserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IImportComponent>();

            this.ChooseOldDbCommand = new RelayCommand(() => this.ChooseOldDb());
            this.ImportCommand = new RelayCommand(() => this.Import(), () => this.CanImport());
        }

        #endregion Constructors

        #region Properties

        public ICommand ChooseOldDbCommand
        {
            get;
            private set;
        }

        public string Database
        {
            get { return this.database; }
            set
            {
                this.database = value;
                this.OnPropertyChanged(() => Database);
            }
        }

        public ICommand ImportCommand
        {
            get;
            private set;
        }

        public string Logs
        {
            get { return this.logs; }
            set
            {
                this.logs = value;
                this.OnPropertyChanged(() => Logs);
            }
        }

        public int Progress
        {
            get { return this.progress; }
            set
            {
                this.progress = value;
                this.OnPropertyChanged(() => Progress);
            }
        }

        #endregion Properties

        #region Methods

        private void AppendLog(string line)
        {
            if (string.IsNullOrWhiteSpace(this.Logs)) { this.Logs = line; }
            else { this.Logs += Environment.NewLine + line; }
        }

        private bool CanImport()
        {
            return !string.IsNullOrWhiteSpace(this.Database)
                && File.Exists(this.Database)
                && !this.importDone;
        }

        private void ChooseOldDb()
        {
            var ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.Title = Messages.Title_ChoseOldDb;
            ofd.Multiselect = false;

            var dr = ofd.ShowDialog();

            if (!dr.HasValue && !dr.Value) return;

            this.Database = ofd.FileName;
            this.AppendLog(Messages.Log_DbChosen);
        }

        private void ClearLog()
        {
            this.Logs = string.Empty;
        }

        private void Import()
        {
            this.ClearLog();
            var engine = new ImportEngine(this.Database);
            engine.Logged += (sender, e) => this.AppendLog(e.Data);
            engine.ProgressChanged += (sender, e) => this.Progress = e.Data;

            if (engine.Check())
            {
                this.RunInBackground(() =>
                {
                    Thread.CurrentThread.CurrentUICulture = cultureInfo;
                    var result = engine.Import(component);
                    this.importDone = true;
                    return result;
                });
            }
            else { this.AppendLog(Messages.Log_InvalidDb); }
        }

        private void RunInBackground(Func<bool> func)
        {
            Assert.IsNotNull(func, "func");

            var worker = new BackgroundWorker();
            worker.RunWorkerCompleted += (sender, e) =>
            {
                if ((bool)e.Result) { ViewService.MessageBox.Error(Messages.Error_ImportFailed); }
            };
            worker.DoWork += (sender, e) => e.Result = func();
            worker.RunWorkerAsync();
        }

        #endregion Methods
    }
}