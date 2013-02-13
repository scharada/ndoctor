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

namespace Probel.NDoctor.Domain.Test.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;

    [TestFixture]
    public class DataStatisticsComponentTest : BaseComponentTest<DataStatisticsComponent>
    {
        #region Methods

        [Test]
        public void GetChart_OfAgeRepartition_ValuesAreReturned()
        {
            var chart = this.ComponentUnderTest.GetAgeRepartion();
            Assert.AreEqual(chart.Points.Count(), 1);
        }

        [Test]
        public void GetChart_OfGenderRepartition_ValuesAreReturned()
        {
            var chart = this.ComponentUnderTest.GetGenderRepartition();

            var males = (from p in chart.Points
                         where p.X == Gender.Male.Translate()
                         select p.Y).Single();

            var females = (from p in chart.Points
                           where p.X == Gender.Female.Translate()
                           select p.Y).Single();

            Assert.AreEqual(7, females);
            Assert.AreEqual(3, males);
        }

        [Test]
        public void GetChart_OfPatientGrowth_ValuesAreReturned()
        {
            var chart = this.ComponentUnderTest.GetPatientGrowth();

            var lastResult = 0d;
            foreach (var item in chart.Points)
            {
                Assert.GreaterOrEqual(item.Y, lastResult);
                lastResult = item.Y;
            }
        }

        protected override void _Setup()
        {
            this.BuildComponent(session => new DataStatisticsComponent(session));
        }

        #endregion Methods
    }
}