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

namespace Probel.Helpers.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    using Probel.Helpers.Data;

    [TestFixture]
    class TestData
    {
        #region Methods

        [Test]
        public void Create_LinearChart_WithXAxis()
        {
            var chart = new Chart<DateTime, double>();
            var linearX = chart.GetLinearX(DateTime.Today);
            Assert.NotNull(linearX);
        }

        [Test]
        public void Create_LinearChart_WithYAxis()
        {
            var chart = new Chart<DateTime, double>();

            var linearY = chart.GetLinearY(10d);
            Assert.NotNull(linearY);
        }

        #endregion Methods
    }
}