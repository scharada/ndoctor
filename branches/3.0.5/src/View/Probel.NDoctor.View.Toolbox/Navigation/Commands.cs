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
namespace Probel.NDoctor.View.Toolbox.Navigation
{
    using System.Windows;
    using System.Windows.Input;

    using log4net;

    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.View.Toolbox.Properties;

    /// <summary>
    /// Set of generic command reusable thoughout the application.
    /// </summary>
    public static class Commands
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(Commands));

        private static RelayArgCommand closeCommand;
        private static RelayCommand shutdown = new RelayCommand(() => Application.Current.MainWindow.Close());
        private static RelayCommand shutdowWithConfirmation;

        #endregion Fields

        #region Properties

        public static ICommand CloseView
        {
            get
            {
                if (closeCommand == null) { closeCommand = new RelayArgCommand(e => Close(e)); }
                return closeCommand;
            }
        }

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

        private static void Close(object e)
        {
            if (e == null)
            {
                Logger.Warn("Null argument passed in the CloseView command");
            }
            else if (e is IRequestCloseViewModel)
            {
                (e as IRequestCloseViewModel).Close();
            }
        }

        private static void ShutDownWithConfirmationCommand()
        {
            var result = ViewService.MessageBox.Question(Messages.Msg_AskForShutdown);

            if (result) Application.Current.Shutdown();
        }

        #endregion Methods
    }
}