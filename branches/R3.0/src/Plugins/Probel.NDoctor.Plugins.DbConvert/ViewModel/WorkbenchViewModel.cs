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
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Input;

    using Microsoft.Win32;

    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.DbConvert.Domain;
    using Probel.NDoctor.Plugins.DbConvert.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        IImportComponent component = ObjectFactory.GetInstance<IImportComponent>();
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
                this.OnPropertyChanged("Database");
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
                this.OnPropertyChanged("Logs");
            }
        }

        public int Progress
        {
            get { return this.progress; }
            set
            {
                this.progress = value;
                this.OnPropertyChanged("Progress");
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
            var db = new ImportEngine(this.Database);
            db.Logged += (sender, e) => this.AppendLog(e.Data);
            db.ProgressChanged += (sender, e) => this.Progress = e.Data;

            if (db.Check())
            {
                db.Import(component);
                this.importDone = true;
            }
            else { this.AppendLog(Messages.Log_InvalidDb); }
        }

        #endregion Methods
    }
}