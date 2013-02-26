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

namespace Probel.NDoctor.Domain.DAL.Entities
{
    using System;

    public class ApplicationStatistics : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the execution time in milliseconds.
        /// </summary>
        /// <value>
        /// The execution time.
        /// </value>
        public virtual double ExecutionTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this statistics entry is exported.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this statistic entry is exported; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsExported
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the execution time is a possible bottleneck.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is possible bottleneck; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsPossibleBottleneck
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the message related to this statistic.
        /// A message  and a new threshold can be set thank to <see cref="BenchmarkThresholdAttribute"/>
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public virtual string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>
        /// The name of the method.
        /// </value>
        public virtual string MethodName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the target type.
        /// </summary>
        /// <value>
        /// The name of the target type.
        /// </value>
        public virtual string TargetTypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the threshold in milliseconds. When
        /// execution time is above threshold, the method is
        /// considered as a bottleneck
        /// </summary>
        /// <value>
        /// The threshold.
        /// </value>
        public virtual double Threshold
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates when the methods was executed
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        public virtual DateTime TimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current version of nDoctor.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public virtual string Version
        {
            get; set;
        }

        #endregion Properties
    }
}