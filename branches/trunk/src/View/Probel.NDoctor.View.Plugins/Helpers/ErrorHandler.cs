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
namespace Probel.NDoctor.View.Plugins.Helpers
{
    using System;
    using System.Windows;

    using log4net;

    using Probel.Helpers.Strings;
    using Probel.NDoctor.View.Plugins.Properties;

    /// <summary>
    /// This class provides error handling methods
    /// </summary>
    public class ErrorHandler : IErrorHandler
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandler"/> class.
        /// </summary>
        /// <param name="callerType">Type of the caller.</param>
        public ErrorHandler(object caller)
        {
            this.Logger = LogManager.GetLogger(caller.GetType());
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public ILog Logger
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        public void HandleError(Exception ex)
        {
            this.HandleError(ex, ex.Message);
        }

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void HandleError(Exception ex, string format, params object[] args)
        {
            this.HandleError(false, ex, format, args);
        }

        /// <summary>
        /// Handles the error, log it and DOESN'T show a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void HandleErrorSilently(Exception ex, string format, params object[] args)
        {
            this.HandleError(false, ex, format, args);
        }

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// This error is showed as a warning
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void HandleWarning(Exception ex, string format, params object[] args)
        {
            this.HandleWarning(false, ex, format, args);
        }

        /// <summary>
        /// Handles the error, log it and DOESN'T show a message box with the error.
        /// This error is showed as a warning
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void HandleWarningSilently(Exception ex, string format, params object[] args)
        {
            this.HandleWarning(true, ex, format, args);
        }

        private void HandleError(bool silent, Exception ex, string format, params object[] args)
        {
            this.Logger.Error(format.StringFormat(args), ex);
            if (!silent)
            {
                MessageBox.Show(format.StringFormat(args), Messages.Title_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.WriteErrorInStatus();
        }

        private void HandleWarning(bool silent, Exception ex, string format, params object[] args)
        {
            this.Logger.Warn(format.StringFormat(args), ex);
            if (!silent)
            {
                MessageBox.Show(ex.Message, Messages.Title_Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.WriteWarningInStatus(string.Format(format, args));
        }

        private void WriteErrorInStatus()
        {
            if (PluginContext.Host != null)
            {
                PluginContext.Host.WriteStatus(StatusType.Error, Messages.Msg_ErrorOccured);
            }
            else
            {
                this.Logger.Warn("The host is not configured. Impossible to write in the status bar");
            }
        }

        private void WriteWarningInStatus(string message)
        {
            if (PluginContext.Host != null)
            {
                PluginContext.Host.WriteStatus(StatusType.Warning, message);
            }
            else
            {
                this.Logger.Warn("The host is not configured. Impossible to write in the status bar");
            }
        }

        #endregion Methods
    }
}