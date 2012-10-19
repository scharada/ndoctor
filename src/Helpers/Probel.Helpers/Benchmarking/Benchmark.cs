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
        {
            this.OnDispose = afterExecution;
            this.Stopwatch.Start();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Stopwatch.Stop();
            this.OnDispose(this.Stopwatch.Elapsed);
        }

        #endregion Methods
    }
}