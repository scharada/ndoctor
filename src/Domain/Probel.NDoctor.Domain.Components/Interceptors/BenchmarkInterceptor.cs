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
    using Probel.NDoctor.Domain.Components.Statistics;
    using Probel.NDoctor.Domain.DAL.AopConfiguration;
    using Probel.NDoctor.Domain.DAL.Entities;

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
            var attribute = this.GetAttribute<BenchmarkThresholdAttribute>(invocation);

            var threshold = (attribute != null && attribute.Length > 0)
                ? (double)attribute[0].Threshold
                : (double)this.DefaultThreshold;

            var message = (attribute != null && attribute.Length > 0)
                ? attribute[0].Explanation
                : string.Empty;

            using (new Benchmark(e => this.CheckAndLog(e, threshold, invocation.TargetType.Name, invocation.Method.Name, message)))
            {
                invocation.Proceed();
            }
        }

        private void CheckAndLog(TimeSpan e, double threshold, string targetTypeName, string methodName, string message)
        {
            this.CheckBottleneck(e, threshold, targetTypeName, methodName, message);
            this.SaveStats(e, threshold, targetTypeName, methodName, message);
        }

        private void CheckBottleneck(TimeSpan e, double threshold, string targetTypeName, string methodName, string message)
        {
            message = (string.IsNullOrEmpty(message))
                ? string.Empty
                : "[" + message + "]";

            if (e.TotalMilliseconds > threshold)
            {
                Logger.WarnFormat("Possible bottleneck => [{0,3}.{1:000} sec] ==> {2}.{3}"
                    , e.Seconds
                    , e.Milliseconds
                    , targetTypeName
                    , methodName
                    , message);
            }
            else
            {
                Logger.WarnFormat("                    => [{0,3}.{1:000} sec] ==> {2}.{3}"
                    , e.Seconds
                    , e.Milliseconds
                    , targetTypeName
                    , methodName
                    , message);
            }
        }

        private void SaveStats(TimeSpan e, double threshold, string targetTypeName, string methodName, string message)
        {
            new NDoctorStatistics().Add(new ApplicationStatistics()
            {
                ExecutionTime = e.TotalMilliseconds,
                IsPossibleBottleneck = (e.TotalMilliseconds > threshold),
                MethodName = methodName,
                TargetTypeName = targetTypeName,
                Threshold = threshold,
                TimeStamp = DateTime.Now,
                Message = message,
            });
        }

        #endregion Methods
    }
}