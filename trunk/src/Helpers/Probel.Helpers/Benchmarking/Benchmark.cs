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

namespace Probel.Helpers.Benchmarking
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Calculates the time span of its life time. This object is meant to calculate execution time of processing
    /// </summary>
    [DebuggerStepThrough]
    public class Benchmark : IDisposable
    {
        #region Fields

        private readonly Action<TimeSpan> OnDispose;
        private readonly Stopwatch Stopwatch = new Stopwatch();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Benchmark"/> class.
        /// </summary>
        /// <param name="afterExecution">The action to execute when the benchmarking is done.</param>
        public Benchmark(Action<TimeSpan> afterExecution)
            : this()
        {
            this.OnDispose = afterExecution;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Benchmark"/> class.
        /// </summary>
        public Benchmark()
        {
            this.Stopwatch.Start();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Get a message displaying the elapsed time
        /// </summary>
        /// <param name="timespan">The timespan.</param>
        /// <returns>The formatted message</returns>
        public static string ToMessage(TimeSpan timespan)
        {
            return string.Format("=============> {0,3}.{1:000} sec", timespan.Seconds, timespan.Milliseconds);
        }

        /// <summary>
        /// Execute the specified action that receives the elapsed time
        /// </summary>
        /// <param name="action">The action that manages the elapsed time.</param>
        public void CheckNow(Action<TimeSpan> action)
        {
            this.Stopwatch.Stop();
            action(this.Stopwatch.Elapsed);
            this.Stopwatch.Start();
        }

        /// <summary>
        /// Execute the specified action that receives the elapsed time
        /// </summary>
        /// <param name="action">The action that manages the text describing the elapsed time.</param>
        public void CheckNowText(Action<string> action)
        {
            this.Stopwatch.Stop();
            action(ToMessage(this.Stopwatch.Elapsed));
            this.Stopwatch.Start();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Stopwatch.Stop();
            if (this.OnDispose != null)
            {
                this.OnDispose(this.Stopwatch.Elapsed);
            }
        }

        #endregion Methods
    }
}