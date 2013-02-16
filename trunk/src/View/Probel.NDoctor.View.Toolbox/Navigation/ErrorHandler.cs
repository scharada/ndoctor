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
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using log4net;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.View.Toolbox;
    using Probel.NDoctor.View.Toolbox.Properties;
    using Probel.NDoctor.View.Toolbox.ViewModel;

    /// <summary>
    /// This class provides error handling methods
    /// </summary>
    public class ErrorHandler : IErrorHandler
    {
        #region Fields

        private readonly IStatusWriter StatusWriter;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandler"/> class.
        /// </summary>
        /// <param name="caller">The caller.</param>
        /// <param name="statusWriter">The status writer.</param>
        /// <param name="afterLogHandler">The handler executed after loggin.</param>
        internal ErrorHandler(object caller, IStatusWriter statusWriter, Action afterLogHandler)
        {
            this.Logger = LogManager.GetLogger(caller.GetType());
            this.StatusWriter = statusWriter;
            this.AfterLogHandler = afterLogHandler;
        }
        private readonly Action AfterLogHandler;
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
        public void Error(Exception ex)
        {
            if (ex is TranslateableException)
            {
                this.Error(ex, (ex as TranslateableException).TranslatedMessage);
            }
            else
            {
                this.Error(ex, ex.Message);
            }
        }

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void Error(Exception ex, string format, params object[] args)
        {
            this.HandleError(false, ex, format, args);
        }

        /// <summary>
        /// Handles a list of errors, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        public void Error(AggregateException ex)
        {
            var msg = ex.Message + Environment.NewLine + "===========" + Environment.NewLine;
            foreach (var error in ex.InnerExceptions)
            {

                if (error is TranslateableException)
                {
                    msg += (error as TranslateableException).TranslatedMessage + Environment.NewLine;
                }
                this.HandleError(true, error, msg);
            }
            ViewService.MessageBox.Error(msg);
        }

        /// <summary>
        /// Handles the error, log it and DOESN'T show a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void ErrorSilently(Exception ex, string format, params object[] args)
        {
            this.HandleError(false, ex, format, args);
        }

        /// <summary>
        /// Handles the fatal error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        public void Fatal(Exception ex)
        {
            if (ex is TranslateableException)
            {
                this.Fatal(ex, (ex as TranslateableException).TranslatedMessage);
            }
            else
            {
                this.Fatal(ex, ex.Message);
            }
        }

        /// <summary>
        /// Handles the fatal error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void Fatal(Exception ex, string format, params object[] args)
        {
            this.HandleFatal(ex, format, args);
        }

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// This error is showed as a warning
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void Warning(Exception ex, string format, params object[] args)
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
        public void WarningSilently(Exception ex, string format, params object[] args)
        {
            this.HandleWarning(true, ex, format, args);
        }

        private void HandleError(bool silent, Exception ex, string format, params object[] args)
        {
            var message = (format == null)
                ? ex.Message
                : format.FormatWith(args);

            //Logs only error, not authorisation errors...
            if (!(ex is AuthorisationException)) { this.Logger.Error(format.FormatWith(args), ex); }

            if (ex is AssertionException) //Any AssertionException highlight a important issue, it's FATAL
            {
                ViewService.Manager.ShowDialog<ExceptionViewModel>(vm => vm.Exception = ex);
            }
            else if (!silent) //Check if the error should be shown in the foreground.
            {
                if (ex.GetType() == typeof(AuthorisationException))
                {
                    ViewService.MessageBox.Error(Messages.Msg_ErrorAuthorisation);
                }
                else
                {
                    ViewService.MessageBox.Error(format.FormatWith(args));
                    this.WriteErrorInStatus(message);
                }
            }
            if (this.AfterLogHandler != null) { this.AfterLogHandler(); }
        }

        private void HandleFatal(Exception ex, string format, params object[] args)
        {
            this.Logger.Fatal(format.FormatWith(args), ex);
            ViewService.Manager.ShowDialog<ExceptionViewModel>(vm => vm.Exception = ex);
            if (this.AfterLogHandler != null) { this.AfterLogHandler(); }
        }

        private void HandleWarning(bool silent, Exception ex, string format, params object[] args)
        {
            var message = format.FormatWith(args);
            this.Logger.Warn(ex);
            if (!silent)
            {
                if (ex.GetType() == typeof(AuthorisationException))
                {
                    ViewService.MessageBox.Error(Messages.Msg_ErrorAuthorisation);
                }
                else
                {
                    ViewService.MessageBox.Warning(message);
                }
            }
            this.WriteWarningInStatus(message);
            if (this.AfterLogHandler != null) { this.AfterLogHandler(); }
        }

        private void WriteErrorInStatus(string message = null)
        {
            if (this.StatusWriter != null)
            {
                message = (message == null)
                    ? Messages.Msg_ErrorOccured
                    : message;

                StatusWriter.WriteStatus(StatusType.Error, message);
            }
            else { this.Logger.Warn("No StatusWriter is configured"); }
        }

        private void WriteWarningInStatus(string message)
        {
            if (StatusWriter != null)
            {
                StatusWriter.WriteStatus(StatusType.Warning, message);
            }
            else { this.Logger.Warn("No StatusWriter is configured"); }
        }

        #endregion Methods
    }
}