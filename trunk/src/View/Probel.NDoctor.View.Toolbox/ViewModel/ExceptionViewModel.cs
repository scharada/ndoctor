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

namespace Probel.NDoctor.View.Toolbox.ViewModel
{
    using System.Diagnostics;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;

    using log4net;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.View.Toolbox.Navigation;
    using System;

    public class ExceptionViewModel : ObservableObject
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ExceptionViewModel));

        private readonly ICommand closeCommand;
        private readonly ICommand loadedCommand;
        private readonly ICommand recordIssueCommand;
        private readonly ICommand reportIssueCommand;

        private bool isClipboardChecked = true;
        private string logStack;

        #endregion Fields

        #region Constructors

        public ExceptionViewModel()
        {
            this.closeCommand = new RelayCommand(() => this.Close(), () => this.CanClose());
            this.reportIssueCommand = new RelayCommand(() => this.ReportIssue(), () => this.CanReportIssue());
            this.loadedCommand = new RelayCommand(() => this.Loaded(), () => this.CanLoaded());
            this.recordIssueCommand = new RelayCommand(() => this.RecordIssue(), () => this.canRecordIssue);
        }

        #endregion Constructors

        #region Properties

        public ICommand CloseCommand
        {
            get { return this.closeCommand; }
        }

        public bool IsClipboardChecked
        {
            get { return this.isClipboardChecked; }
            set
            {
                this.isClipboardChecked = value;
                this.OnPropertyChanged(() => IsClipboardChecked);
            }
        }

        public ICommand LoadedCommand
        {
            get { return this.loadedCommand; }
        }

        public string LogStack
        {
            get { return this.logStack; }
            set
            {
                this.logStack = value;
                this.OnPropertyChanged(() => LogStack);
            }
        }

        public ICommand RecordIssueCommand
        {
            get { return this.recordIssueCommand; }
        }

        public ICommand ReportIssueCommand
        {
            get { return this.reportIssueCommand; }
        }

        #endregion Properties

        #region Methods

        private bool CanClose()
        {
            return true;
        }

        private bool CanLoaded()
        {
            return true;
        }

        private bool canRecordIssue = true;

        private bool CanReportIssue()
        {
            return true;
        }

        private void Close()
        {
            Commands.Shutdown.TryExecute();
        }

        private void Loaded()
        {
            var sb = new StringBuilder();
            LogMessageRecorder.AppendRecentLogMessages(sb, Logger);
            this.LogStack = sb.ToString();
        }

        private void RecordIssue()
        {
            try
            {
                Process.Start("psr.exe");
            }
            catch (Exception) { this.canRecordIssue = false; }
        }

        private void ReportIssue()
        {
            if (this.IsClipboardChecked) { Clipboard.SetText(this.LogStack); }

            if (Thread.CurrentThread.CurrentUICulture.Name.ToLower().Contains("fr"))
            {
                Process.Start("https://groups.google.com/forum/?fromgroups#!forum/ndoctor-fr");
            }
            else
            {
                Process.Start("https://groups.google.com/forum/?fromgroups#!forum/ndoctor-en");
            }
        }

        #endregion Methods

    }
}