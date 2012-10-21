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

namespace Probel.NDoctor.Domain.DAL.AopConfiguration
{
    using System;

    /// <summary>
    /// This attribute is used to override the threshold that indicates when the execution time of a method
    /// could be a bottleneck.
    /// </summary>
    public class BenchmarkThresholdAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BenchmarkThresholdAttribute"/> class.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <param name="explanation">The explanation.</param>
        public BenchmarkThresholdAttribute(uint threshold, string explanation)
        {
            this.Threshold = threshold;
            this.Explanation = explanation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BenchmarkThresholdAttribute"/> class.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        public BenchmarkThresholdAttribute(uint threshold)
        {
            this.Threshold = threshold;
            this.Explanation = string.Format("The threshold is overriden to {0} ms", this.Threshold);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the explanation about why the threshold was ovrriden. By default indicates what is the new threshold
        /// </summary>
        public string Explanation
        {
            get;
            private set;
        }

        /// <summary>
        /// The overriden threshold in milliseconds.
        /// </summary>
        public uint Threshold
        {
            get;
            private set;
        }

        #endregion Properties
    }
}