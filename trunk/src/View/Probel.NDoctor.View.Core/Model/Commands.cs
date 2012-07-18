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
namespace Probel.NDoctor.View.Core.Model
{
    using System.Windows;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.View.Core.Properties;

    /// <summary>
    /// Set of generic command reusable thoughout the application.
    /// </summary>
    public static class Commands
    {
        #region Fields

        private static RelayCommand shutdown = new RelayCommand(() => Application.Current.Shutdown());
        private static RelayCommand shutdowWithConfirmation;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the shutdown command.
        /// This command will shutting down the
        /// application without asking confirmation
        /// to the user.
        /// </summary>
        public static ICommand Shutdown
        {
            get
            {
                return shutdown;
            }
        }

        /// <summary>
        /// Gets the shutdown with confirmation command. 
        /// This command will ask confirmation to the user
        /// before closing the application.
        /// </summary>
        public static ICommand ShutdownWithConfirmation
        {
            get
            {
                if (shutdowWithConfirmation == null)
                    shutdowWithConfirmation = new RelayCommand(() => ShutDownWithConfirmationCommand());
                return shutdowWithConfirmation;
            }
        }

        #endregion Properties

        #region Methods

        private static void ShutDownWithConfirmationCommand()
        {
            var result = MessageBox.Show(Messages.Msg_AskForShutdown
                , Messages.Title_Interogation
                , MessageBoxButton.YesNo
                , MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) Application.Current.Shutdown();
        }

        #endregion Methods
    }
}