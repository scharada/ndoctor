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

namespace Probel.NDoctor.Domain.Components.Interceptors
{
    using System;

    using Castle.DynamicProxy;
    using Probel.Helpers.Benchmarking;

    /// <summary>
    /// Benchmarks the call of a method and logs if time if higher that a threshold
    /// </summary>
    internal class BenchmarkInterceptor : BaseInterceptor
    {
        #region Fields

        private readonly uint Threshold;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BenchmarkInterceptor"/> class.
        /// </summary>
        /// <param name="threshold">The execution time threshold.</param>
        public BenchmarkInterceptor(uint threshold)
            : base()
        {
            this.Threshold = threshold;
        }
        #endregion Constructors

        #region Methods

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public override void Intercept(IInvocation invocation)
        {
            var methodName = string.Format("{0}.{1}", invocation.TargetType.Name, invocation.Method.Name);
            using (new Benchmark(e => LogTime(e, methodName)))
            {
                invocation.Proceed();
            }
        }

        private void LogTime(TimeSpan e, string methodName)
        {
            if (e.TotalMilliseconds > this.Threshold)
            {
                Logger.WarnFormat("Possible bottleneck | [{1,3}.{2:000} sec] ==> {0}."
                    , methodName
                    , e.Seconds
                    , e.Milliseconds);
            }
            else
            {
                Logger.DebugFormat("                    | [{1,3}.{2:000} sec] ==> {0}."
                    , methodName
                    , e.Seconds
                    , e.Milliseconds);
            }
        }

        #endregion Methods
    }
}