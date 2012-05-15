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
    /// <summary>
    /// Represents a point to be displayed on a chart
    /// </summary>
    /// <typeparam name="Tx">The type of the x.</typeparam>
    /// <typeparam name="Ty">The type of the y.</typeparam>
    public class ChartPoint<Tx, Ty>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartPoint&lt;Tx, Ty&gt;"/> class.
        /// </summary>
        public ChartPoint()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartPoint&lt;Tx, Ty&gt;"/> class.
        /// </summary>
        /// <param name="x">The x point.</param>
        /// <param name="y">The y point.</param>
        public ChartPoint(Tx x, Ty y)
        {
            this.X = x;
            this.Y = y;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>
        /// The X.
        /// </value>
        public Tx X
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>
        /// The Y.
        /// </value>
        public Ty Y
        {
            get;
            set;
        }

        #endregion Properties
    }
}