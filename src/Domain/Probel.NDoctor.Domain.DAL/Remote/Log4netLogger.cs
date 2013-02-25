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

namespace Probel.NDoctor.Domain.DAL.Remote
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Probel.NDoctor.Statistics.Loggers;

    internal class Log4netLogger : Probel.NDoctor.Statistics.Loggers.ILog
    {
        #region Fields

        private readonly log4net.ILog Logger;

        #endregion Fields

        #region Constructors

        public Log4netLogger(log4net.ILog logger)
        {
            this.Logger = logger;
        }

        #endregion Constructors

        #region Methods

        public void Debug(object message, Exception exception)
        {
            this.Logger.Debug(message, exception);
        }

        public void Debug(object message)
        {
            this.Logger.Debug(message);
        }

        public void DebugFormat(string format, params object[] args)
        {
            this.Logger.DebugFormat(format, args);
        }

        public void Error(object message, Exception exception)
        {
            this.Logger.Error(message, exception);
        }

        public void Error(object message)
        {
            this.Logger.Error(message);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            this.Logger.ErrorFormat(format, args);
        }

        public void Fatal(object message, Exception exception)
        {
            this.Logger.Fatal(message, exception);
        }

        public void Fatal(object message)
        {
            this.Logger.Fatal(message);
        }

        public void FatalFormat(string format, params object[] args)
        {
            this.Logger.FatalFormat(format, args);
        }

        public void Info(object message, Exception exception)
        {
            this.Logger.Info(message, exception);
        }

        public void Info(object message)
        {
            this.Logger.Info(message);
        }

        public void InfoFormat(string format, params object[] args)
        {
            this.Logger.InfoFormat(format, args);
        }

        public void Warn(object message, Exception exception)
        {
            this.Logger.Warn(message, exception);
        }

        public void Warn(object message)
        {
            this.Logger.Warn(message);
        }

        public void WarnFormat(string format, params object[] args)
        {
            this.Logger.WarnFormat(format, args);
        }

        #endregion Methods
    }
}