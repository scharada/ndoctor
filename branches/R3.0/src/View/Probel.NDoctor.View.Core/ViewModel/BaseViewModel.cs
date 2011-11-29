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

    using log4net;

    using Probel.Helpers.Events;
    using Probel.Helpers.Strings;
    using Probel.Helpers.WPF;
    using Probel.NDoctor.View.Core.Properties;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Exceptions;
    using Probel.NDoctor.View.Plugins.Helpers;

    public abstract class BaseViewModel : ObservableObject, IErrorHandler
    {
        #region Fields

        private ErrorHandler errorHandler;

        #endregion Fields

        #region Constructors

        protected BaseViewModel()
        {
            this.errorHandler = new ErrorHandler(this);
            if (!Designer.IsDesignMode)
            {
                if (PluginContext.Host == null) throw new NDoctorConfigurationException(Messages.Ex_NDoctorConfigurationException_HostNull);
                PluginContext.Host = PluginContext.Host;
            }
        }

        #endregion Constructors

        #region Properties

        public ILog Logger
        {
            get { return this.errorHandler.Logger; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        public void HandleError(Exception ex)
        {
            this.errorHandler.HandleError(ex, ex.Message);
        }

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void HandleError(Exception ex, string format, params object[] args)
        {
            this.errorHandler.HandleError(ex, format, args);
        }

        /// <summary>
        /// Handles the error, log it and DOESN'T show a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void HandleErrorSilently(Exception ex, string format, params object[] args)
        {
            this.errorHandler.HandleErrorSilently(ex, format, args);
            this.IndicateError(string.Format(format, args));
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
            this.errorHandler.HandleWarning(ex, format, args);
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
            this.errorHandler.HandleWarning(ex, format, args);
            this.IndicateWarning(string.Format(format, args));
        }

        /// <summary>
        /// Sets the status of the host to ready.
        /// </summary>
        public void SetStatusToReady()
        {
            if (PluginContext.Host != null) PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_Ready);
        }

        private void IndicateError()
        {
            this.IndicateError(string.Empty);
        }

        private void IndicateError(string msg)
        {
            if (PluginContext.Host == null) return;

            PluginContext.Host.WriteStatus(StatusType.Error, Messages.Msg_ErrorOccured.StringFormat(msg));
        }

        private void IndicateWarning()
        {
            this.IndicateError(string.Empty);
        }

        private void IndicateWarning(string msg)
        {
            if (PluginContext.Host == null) return;

            PluginContext.Host.WriteStatus(StatusType.Error, Messages.Msg_WarningOccured.StringFormat(msg));
        }

        #endregion Methods
    }
}