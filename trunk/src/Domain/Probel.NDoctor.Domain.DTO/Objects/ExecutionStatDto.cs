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

namespace Probel.NDoctor.Domain.DTO.Objects
{
    using System;

    public class ExecutionStatDto : BaseDto
    {
        #region Fields

        private double executionTime;
        private bool isPossibleBottleneck;
        private string methodName;
        private string targetTypeName;
        private double threshold;
        private DateTime timeStamp;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the execution time in milliseconds.
        /// </summary>
        /// <value>
        /// The execution time.
        /// </value>
        public double ExecutionTime
        {
            get { return this.executionTime; }
            set
            {
                this.executionTime = value;
                this.OnPropertyChanged(() => ExecutionTime);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the execution time is a possible bottleneck.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is possible bottleneck; otherwise, <c>false</c>.
        /// </value>
        public bool IsPossibleBottleneck
        {
            get { return this.isPossibleBottleneck; }
            set
            {
                this.isPossibleBottleneck = value;
                this.OnPropertyChanged(() => IsPossibleBottleneck);
            }
        }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>
        /// The name of the method.
        /// </value>
        public string MethodName
        {
            get { return this.methodName; }
            set
            {
                this.methodName = value;
                this.OnPropertyChanged(() => MethodName);
            }
        }

        /// <summary>
        /// Gets or sets the name of the target type.
        /// </summary>
        /// <value>
        /// The name of the target type.
        /// </value>
        public string TargetTypeName
        {
            get { return this.targetTypeName; }
            set
            {
                this.targetTypeName = value;
                this.OnPropertyChanged(() => TargetTypeName);
            }
        }

        /// <summary>
        /// Gets or sets the threshold in milliseconds. When
        /// execution time is above threshold, the method is
        /// considered as a bottleneck
        /// </summary>
        /// <value>
        /// The threshold.
        /// </value>
        public double Threshold
        {
            get { return this.threshold; }
            set
            {
                this.threshold = value;
                this.OnPropertyChanged(() => Threshold);
            }
        }

        /// <summary>
        /// Indicates when the methods was executed
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        public DateTime TimeStamp
        {
            get { return this.timeStamp; }
            set
            {
                this.timeStamp = value;
                this.OnPropertyChanged(() => TimeStamp);
            }
        }

        #endregion Properties
    }
}