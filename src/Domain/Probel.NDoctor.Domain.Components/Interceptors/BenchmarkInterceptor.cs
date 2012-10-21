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
    using Probel.NDoctor.Domain.DAL.AopConfiguration;

    /// <summary>
    /// Benchmarks the call of a method and logs if time if higher that a threshold
    /// </summary>
    internal class BenchmarkInterceptor : BaseInterceptor
    {
        #region Fields

        private readonly uint DefaultThreshold;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BenchmarkInterceptor"/> class.
        /// </summary>
        /// <param name="threshold">The execution time threshold.</param>
        public BenchmarkInterceptor(uint threshold)
            : base()
        {
            this.DefaultThreshold = threshold;
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
            var thresholdInfo = this.GetThreshold(invocation);
            using (new Benchmark(e => LogTime(e, methodName, thresholdInfo.Item1, thresholdInfo.Item2)))
            {
                invocation.Proceed();
            }
        }

        private Tuple<double, string> GetThreshold(IInvocation invocation)
        {
            var attribute = this.GetAttribute<BenchmarkThresholdAttribute>(invocation);
            return (attribute != null && attribute.Length > 0)
                ? new Tuple<double, string>((double)attribute[0].Threshold, "[" + attribute[0].Explanation + "]")
                : new Tuple<double, string>((double)this.DefaultThreshold, string.Empty);
        }

        private void LogTime(TimeSpan e, string methodName, double threshold, string explanation)
        {
            if (e.TotalMilliseconds > threshold)
            {
                Logger.WarnFormat("Possible bottleneck | [{1,3}.{2:000} sec] ==> {0} {3}"
                    , methodName
                    , e.Seconds
                    , e.Milliseconds
                    , explanation);
            }
            else
            {
                Logger.DebugFormat("                    | [{1,3}.{2:000} sec] ==> {0} {3}"
                    , methodName
                    , e.Seconds
                    , e.Milliseconds
                    , explanation);
            }
        }

        #endregion Methods
    }
}