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
namespace Probel.Helpers.Data
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a chart with X and Y axes
    /// </summary>
    /// <typeparam name="Tx">The type of the x.</typeparam>
    /// <typeparam name="Ty">The type of the y.</typeparam>
    public class Chart<Tx, Ty>
    {
        #region Fields

        private IList<ChartPoint<Tx, Ty>> points = new List<ChartPoint<Tx, Ty>>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Chart&lt;Tx, Ty&gt;"/> class.
        /// </summary>
        public Chart()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Chart&lt;Tx, Ty&gt;"/> class.
        /// </summary>
        /// <param name="xAxes">The x axes.</param>
        /// <param name="yAxes">The y axes.</param>
        public Chart(IEnumerable<ChartPoint<Tx, Ty>> points)
        {
            this.points = points.ToList();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the list of point the chart contains.
        /// </summary>
        /// <value>
        /// The X axes.
        /// </value>
        public IEnumerable<ChartPoint<Tx, Ty>> Points
        {
            get { return this.points; }
        }

        /// <summary>
        /// Gets the collection of the X.
        /// </summary>
        public IEnumerable<Tx> XCollection
        {
            get
            {
                return (from p in this.points
                        select p.X).ToList();
            }
        }

        /// <summary>
        /// Gets the collection of the Y.
        /// </summary>
        public IEnumerable<Ty> YCollection
        {
            get
            {
                return (from p in this.points
                        select p.Y).ToList();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds a point into the chart.
        /// </summary>
        /// <param name="point">The point.</param>
        public void AddPoint(ChartPoint<Tx, Ty> point)
        {
            this.points.Add(point);
        }

        /// <summary>
        /// Adds a point into the chart.
        /// </summary>
        /// <param name="point">The point.</param>
        public void AddPoint(Tx x, Ty y)
        {
            this.AddPoint(new ChartPoint<Tx, Ty>(x, y));
        }

        #endregion Methods
    }
}