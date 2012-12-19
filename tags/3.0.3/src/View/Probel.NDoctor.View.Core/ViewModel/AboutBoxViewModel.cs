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

namespace Probel.NDoctor.View.Core.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Windows.Input;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.View.Core.Properties;

    internal class AboutBoxViewModel : BaseViewModel
    {
        #region Fields

        private string application;
        private string author;
        private string copyright;
        private string license;

        #endregion Fields

        #region Constructors

        public AboutBoxViewModel()
        {
            this.Plugins = new ObservableCollection<string>();
            this.RefreshCommand = new RelayCommand(() => this.Refresh());
            this.OpenLogCommand = new RelayCommand(() => this.OpenLog());
        }

        #endregion Constructors

        #region Properties

        public string Author
        {
            get { return this.author; }
            set
            {
                this.author = value;
                this.OnPropertyChanged(() => Author);
            }
        }

        public string Copyright
        {
            get { return this.copyright; }
            set
            {
                this.copyright = value;
                this.OnPropertyChanged(() => Copyright);
            }
        }

        public string License
        {
            get { return this.license; }
            set
            {
                this.license = value;
                this.OnPropertyChanged(() => License);
            }
        }

        public ICommand OpenLogCommand
        {
            get;
            private set;
        }

        public ObservableCollection<string> Plugins
        {
            get;
            private set;
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public string Title
        {
            get { return this.application; }
            set
            {
                this.application = value;
                this.OnPropertyChanged(() => Title);
            }
        }

        #endregion Properties

        #region Methods

        private string GetLicense()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Probel.NDoctor.View.Core.License.txt");
            if (stream == null) throw new NullReferenceException("The license is not foud in the resource of the executing assembly.");

            using (var reader = new StreamReader(stream, Encoding.UTF8)) { return reader.ReadToEnd(); }
        }

        private IEnumerable<string> GetPlugins()
        {
            var list = new List<string>();
            var directories = Directory.GetDirectories(@".\Plugins");

            foreach (var directory in directories)
            {
                list.Add(directory);
            }

            return list;
        }

        private void OpenLog()
        {
            try
            {
                var appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                Process.Start(Path.Combine(appdata, @"Probel\nDoctor"));
            }
            catch (Exception ex) { this.Handle.Error(ex); }
        }

        private void Refresh()
        {
            var asm = Assembly.GetAssembly(this.GetType());
            this.Title = "nDoctor {0}".FormatWith(asm.GetName().Version);
            this.Author = Messages.Title_WrittenBy.FormatWith("Jean-Baptiste Wautier");
            this.Copyright = "Copyright Probel 2006-{0}".FormatWith(DateTime.Today.Year);
            this.License = this.GetLicense().FormatWith(DateTime.Today.Year);
            this.Plugins.Refill(this.GetPlugins());
        }

        #endregion Methods
    }
}