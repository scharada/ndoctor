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
namespace Probel.NDoctor.View.Core.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DAL.Cfg;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.View.Core.Properties;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;
    using Probel.NDoctor.View.Toolbox.Navigation;

    using StructureMap;

    internal class SpashScreenViewModel : BaseViewModel
    {
        #region Fields

        private CultureInfo cultureInfo = Thread.CurrentThread.CurrentUICulture;
        private uint progress;
        private string status;
        private string title;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpashScreenViewModel"/> class.
        /// </summary>
        public SpashScreenViewModel()
            : base()
        {
        }

        #endregion Constructors

        #region Events

        public event EventHandler Failed;

        public event EventHandler Loaded;

        #endregion Events

        #region Properties

        public uint Progress
        {
            get
            {
                return (this.progress > 100)
                    ? 100
                    : this.progress;
            }
            set
            {
                if (value > 100) return;
                this.progress = value;
                this.OnPropertyChanged(() => Progress);
            }
        }

        public string Status
        {
            get { return this.status; }
            set
            {
                this.status = value;
                this.OnPropertyChanged(() => Status);
            }
        }

        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                this.OnPropertyChanged(() => Title);
            }
        }

        private bool CreateDatabase
        {
            get
            {
                var result = ConfigurationManager.AppSettings["CreateDatabase"];
                var createDatabase = false;
                if (bool.TryParse(result, out createDatabase)) return createDatabase;
                else return false;
            }
        }

        #endregion Properties

        #region Methods

        public void Start()
        {
            this.Logger.InfoFormat("Current culture: '{0}'", this.cultureInfo.ToString());
            var thread = new BackgroundWorker();

            thread.DoWork += (sender, e) =>
            {
                try
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    Thread.CurrentThread.CurrentUICulture = cultureInfo;

                    this.BuildHomeMenu();

                    var version = Assembly.GetExecutingAssembly().GetName().Version;
                    this.Title = string.Format("nDoctor BETA {0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);

                    this.Status = Messages.Msg_ConfiguringNHibernate;
                    this.ConfigureNHibernate();
                    this.Progress = 20;

                    this.Status = Messages.Msg_ConfiguringStatusWriter;
                    this.ConfigureStatusWriter();
                    this.Progress = 40;

                    this.Status = Messages.Msg_ConfiguringStructureMap;
                    this.ConfigureStructureMap();
                    this.Progress = 60;

                    this.Status = Messages.Msg_ConfiguringPlugins;
                    this.ConfigurePlugins();
                    this.Progress = 80;

                    this.Status = Messages.Msg_ConfiguringThumbnails;
                    this.CongigureThumbnails();
                    this.Progress = 100;
                    this.Logger.Info("Configuration done.");

                    stopwatch.Stop();
                    this.Logger.InfoFormat("Loading time {0},{1} sec", stopwatch.Elapsed.Seconds, stopwatch.Elapsed.Milliseconds);

                }
                catch (Exception ex)
                {
                    this.Logger.Error("An error occured in the spashscreen", ex);

                    MessageBox.Show(Messages.Msg_FatalErrorOccured.FormatWith(ex.Message)
                        , Messages.Title_Error
                        , MessageBoxButton.OK
                        , MessageBoxImage.Error);
                    this.OnFailed();
                }
            };
            thread.RunWorkerCompleted += (sender, e) => this.OnLoaded();
            thread.RunWorkerAsync();
        }

        private void BuildHomeMenu()
        {
            var cmd = new RelayCommand(() => MessageBox.Show("Hello World!"));

            var groups = new ObservableCollection<RibbonGroupData>();
            groups.Add(new RibbonGroupData(Messages.Title_Tools));
            groups.Add(new RibbonGroupData(Messages.Title_Managers));
            groups.Add(new RibbonGroupData(Messages.Title_GlobalTools));

            PluginContext.Host.AddTab(new RibbonTabData(Messages.Title_Home, groups));
        }

        private void ConfigureNHibernate()
        {
            this.Logger.Info("Configuring nHibernate...");
            var path = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["Database"]);

            if (!File.Exists(path) && !this.CreateDatabase) { this.Logger.Warn("It seems to be the first start of the application. An empty database is created."); }

            this.Logger.DebugFormat("Database path: {0}", path);
            this.LogDatabaseCreation();
            new DalConfigurator()
                .ConfigureUsingFile(path, this.CreateDatabase)
                .InjectDefaultData();
        }

        private void ConfigurePlugins()
        {
            this.Logger.Info("Configuring plugins...");
            var loader = ObjectFactory.GetInstance<IPluginLoader>();
            var container = new PluginContainer(PluginContext.Host, loader);
            container.LoadPlugins();
        }

        private void ConfigureStatusWriter()
        {
            ErrorHandlerFactory.ConfigureStatusWriter(PluginContext.Host);
        }

        private void ConfigureStructureMap()
        {
            string repository = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Plugins\");

            this.Logger.Info("Configuring StructureMap...");
            ObjectFactory.Configure(x =>
             {
                 x.For<IPluginLoader>().Add<MefPluginLoader>()
                     .Ctor<string>("repository").Is(repository);
             });
        }

        private void CongigureThumbnails()
        {
            PluginContext.ComponentFactory
                .GetInstance<IPictureComponent>()
                .CreateAllThumbnails();
        }

        private void LogDatabaseCreation()
        {
            if (this.CreateDatabase) { this.Logger.Warn("Creation of a new database. Old data is deleted"); }
        }

        private void OnFailed()
        {
            if (this.Failed != null)
                this.Failed(this, EventArgs.Empty);
        }

        private void OnLoaded()
        {
            if (this.Loaded != null)
                this.Loaded(this, EventArgs.Empty);
        }

        #endregion Methods
    }
}