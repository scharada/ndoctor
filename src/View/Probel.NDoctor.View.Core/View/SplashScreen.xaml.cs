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
namespace Probel.NDoctor.View.Core.View
{
    using System;
    using System.Windows;

    using Probel.NDoctor.View.Core.ViewModel;

    /// <summary>
    /// Interaction logic for SpashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        #region Constructors

        public SplashScreen()
        {
            InitializeComponent();
            var viewModel = new SpashScreenViewModel();

            viewModel.Loaded += (sender, e) => this.Dispatcher.Invoke((Action)delegate { this.Close(); });
            viewModel.Failed += (sender, e) => this.IsOnError = true;

            this.DataContext = viewModel;
            viewModel.Start();
        }

        #endregion Constructors

        #region Properties

        public bool IsOnError
        {
            get;
            private set;
        }

        #endregion Properties
    }
}