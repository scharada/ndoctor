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
namespace Probel.NDoctor.PluginHost.Host.View
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows;

    /// <summary>
    /// Interaction logic for SpashScreen.xaml
    /// </summary>
    public partial class SplashScreenView : Window, Probel.NDoctor.PluginHost.Host.View.ISplashScreen
    {
        #region Fields

        private const string appName = "nDoctor BETA {0}";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SplashScreenView"/> class.
        /// </summary>
        public SplashScreenView()
        {
            InitializeComponent();

            this.title.Text = string.Format(appName, Assembly.GetExecutingAssembly().GetName().Version);
        }

        #endregion Constructors

        #region Properties

        public Action<ISplashScreen> SplashWork
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public void Execute()
        {
            var worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;

            worker.DoWork += (sender, e) =>
            {
                if (this.SplashWork != null)
                    this.SplashWork(this);
            };
            worker.RunWorkerCompleted += (sender1, e1) => this.Close();

            worker.RunWorkerAsync();
            this.ShowDialog();
        }

        public void SetStatus(int value, string format, params object[] args)
        {
            this.Dispatcher.BeginInvoke((Action<int, string>)delegate(int pct, string msg)
            {
                this.progressBar.Value = (pct > 100)
                    ? 100
                    : pct;
                this.status.Text = msg;
            }, value, string.Format(format, args));

        }

        #endregion Methods
    }
}