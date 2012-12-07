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

namespace Probel.NDoctor.View.Toolbox.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using log4net;
    using log4net.Appender;
    using log4net.Core;

    using Probel.Helpers.Events;
    using Probel.Mvvm.DataBinding;

    /// <summary>
    /// This appender insert all log events into a WPF UI item
    /// </summary>
    public sealed class WpfAppender : AppenderSkeleton
    {
        #region Fields

        /// <summary>
        /// Gets a read-only snapshot of the recorded events
        /// currently stored in the buffer.
        /// The returned collection contains the events
        /// in the same order as they have been appended.
        /// </summary>
        private readonly List<LogEvent> RecordedEvents = new List<LogEvent>();

        #endregion Fields

        #region Methods

        /// <summary>
        /// Registers the specified displayer.
        /// </summary>
        /// <param name="displayer">The displayer.</param>
        /// <param name="log">The log.</param>
        public static IEnumerable<LogEvent> GetLogs(ILog log)
        {
            WpfAppender recorder = log.Logger.Repository.GetAppenders().OfType<WpfAppender>().Single();
            return recorder.RecordedEvents;
        }

        /// <summary>
        /// Append the logging event into the configured Log Displayer.
        /// If no log displayer is configured, it does nothing
        /// </summary>
        /// <param name="loggingEvent">The event to append.</param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            var @event = new LogEvent()
            {
                LoggerName = loggingEvent.LoggerName,
                TimeStamp = loggingEvent.TimeStamp,
                ThreadName = loggingEvent.ThreadName,
                LevelName = loggingEvent.Level.Name,
                Message = loggingEvent.RenderedMessage,
            };
            this.RecordedEvents.Add(@event);
        }

        #endregion Methods
    }
}