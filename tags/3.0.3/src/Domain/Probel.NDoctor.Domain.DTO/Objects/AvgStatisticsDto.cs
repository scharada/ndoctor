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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a possible bottle neck in the application
    /// </summary>
    public class BottleneckDto : BaseDto
    {
        #region Fields

        private double avgExecutionTime;
        private double avgThreshold;
        private int count;
        private string methodName;
        private string targetTypeName;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the average execution time.
        /// </summary>
        /// <value>
        /// The avg execution time.
        /// </value>
        public double AvgExecutionTime
        {
            get { return this.avgExecutionTime; }
            set
            {
                this.avgExecutionTime = value;
                this.OnPropertyChanged(() => AvgExecutionTime);
            }
        }

        /// <summary>
        /// Gets or sets the aerage threshold for this bottleneck.
        /// </summary>
        /// <value>
        /// The avg threshold.
        /// </value>
        public double AvgThreshold
        {
            get { return this.avgThreshold; }
            set
            {
                this.avgThreshold = value;
                this.OnPropertyChanged(() => AvgThreshold);
            }
        }

        /// <summary>
        /// Gets or sets the count of bottleneck of this type.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get { return this.count; }
            set
            {
                this.count = value;
                this.OnPropertyChanged(() => Count);
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

        public string TargetTypeName
        {
            get { return this.targetTypeName; }
            set
            {
                this.targetTypeName = value;
                this.OnPropertyChanged(() => TargetTypeName);
            }
        }

        #endregion Properties
    }
}