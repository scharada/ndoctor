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
    using System.Threading;

    using NSubstitute;

    using NUnit.Framework;

    using Probel.Helpers.Data;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.Components.Statistics;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.Test.TestHelpers;

    using StructureMap;

    [TestFixture]
    public class ApplicationStatisticsComponentTest : BaseComponentTest<ApplicationStatisticsComponent>
    {
        #region Fields

        private readonly ComponentFactory Factory = ComponentFactory.TestInstance(true);
        private readonly string[] Names = new string[] { "Method1", "Method2", "Method3" };

        #endregion Fields

        #region Methods

        [Test]
        public void CheckHelpers_WithDoublons_ReturnsTrue()
        {
            var chart = new Chart<string, double>();
            chart.AddPoint(this.Names[0], 0);
            chart.AddPoint(this.Names[0], 0);
            chart.AddPoint(this.Names[0], 0);

            Assert.IsTrue(this.HasDoublons(chart));
        }

        [Test]
        public void CheckHelpers_WithoutDoublons_ReturnsFalse()
        {
            var chart = new Chart<string, double>();
            chart.AddPoint(GetRandom.String, 0);
            chart.AddPoint(GetRandom.String, 0);
            chart.AddPoint(GetRandom.String, 0);

            Assert.IsFalse(this.HasDoublons(chart));
        }

        [Test]
        public void RetrieveAvgExecutionTime_NoValue_Returns0()
        {
            var avg = this.ComponentUnderTest.GetAvgExecutionTime();
            Assert.AreEqual(0, avg);
        }

        [Test]
        public void RetrieveAvgExecution_CountEntries_GreaterThan0()
        {
            var insCount = 50;
            this.FillStatistics(insCount);
            var count = this.ComponentUnderTest.CountEntries();
            Assert.AreEqual(insCount, count);
        }

        [Test]
        public void RetrieveAvgExecution_ValuesInTable_ReturnsGreaterThan0()
        {
            this.FillStatistics(50);

            var avg = this.ComponentUnderTest.GetAvgExecutionTime();
            Assert.Greater(avg, 0);
        }

        [Test]
        public void RetrieveBottlenecks_ByMethods_NoDoublons()
        {
            this.FillStatisticsWithRandomBottlenecks(50);

            var chart = this.ComponentUnderTest.GetBottlenecks();

            Assert.Greater(chart.Points.Count(), 0);
            Assert.IsFalse(HasDoublons(chart), "Chart has doublons");
        }

        [Test]
        public void RetrieveBottlenecks_ByMethods_ReturnsCharts()
        {
            this.FillStatisticsWithRandomBottlenecks(50);

            var chart = this.ComponentUnderTest.GetBottlenecks();

            Assert.Greater(chart.Points.Count(), 0);
        }

        [Test]
        public void RetrievedExecutionTime_GridList_HasCount()
        {
            this.FillStatisticsWithRandomBottlenecks(50);

            var list = this.ComponentUnderTest.GetBottlenecksArray();

            Assert.Greater(list.Count(), 0);
            Assert.IsFalse(this.HasDoublons(list), "Has doublons");
            Assert.AreEqual(1, this.Count(list, this.Names[0], this.Names[0]));
        }

        [Test]
        public void RetrievedExecutionTime_GridList_NoDoublons()
        {
            this.FillStatisticsWithRandomBottlenecks(50);

            var list = this.ComponentUnderTest.GetBottlenecksArray();

            Assert.Greater(list.Count(), 0);
            Assert.IsFalse(this.HasDoublons(list), "Chart has doublons");
        }

        [Test]
        public void RetrievedExecutionTime_TimeGraph_HasCount()
        {
            this.FillStatistics(50);

            var chart = this.ComponentUnderTest.GetAvgExecutionTimeGraph();

            Assert.AreEqual(1, chart.Points.Count());
            Assert.IsFalse(this.HasDoublons(chart), "Chart has doublons");
            Assert.AreEqual(1, this.Count(chart, "method"));
        }

        [Test]
        public void RetrievedExecutionTime_TimeGraph_NoDoublons()
        {
            this.FillStatisticsWithRandomBottlenecks(50);

            var chart = this.ComponentUnderTest.GetAvgExecutionTimeGraph();

            Assert.Greater(chart.Points.Count(), 0);
            Assert.IsFalse(this.HasDoublons(chart), "Chart has doublons");
        }

        [Test]
        public void RetrievedUsage_OfMethods_HasCount()
        {
            this.FillStatistics(50);

            var chart = this.ComponentUnderTest.GetUsageByMethods();

            Assert.AreEqual(1, chart.Points.Count());
            Assert.IsFalse(this.HasDoublons(chart), "Chart has doublons");
            Assert.AreEqual(1, this.Count(chart, "method"));
        }

        [Test]
        public void RetrievedUsage_OfMethods_NoDoublons()
        {
            this.FillStatisticsWithRandomBottlenecks(50);

            var chart = this.ComponentUnderTest.GetUsageByMethods();

            Assert.Greater(chart.Points.Count(), 0);
            Assert.IsFalse(this.HasDoublons(chart), "Chart has doublons");
        }

        [Test]
        public void RetrievedUsage_OfTargets_HasCount()
        {
            this.FillStatistics(50);

            var chart = this.ComponentUnderTest.GetUsageByTargets();

            Assert.AreEqual(1, chart.Points.Count());
            Assert.IsFalse(this.HasDoublons(chart), "Chart has doublons");
            Assert.AreEqual(1, this.Count(chart, "target"));
        }

        [Test]
        public void RetrievedUsage_OfTargets_NoDoublons()
        {
            this.FillStatisticsWithRandomBottlenecks(50);

            var chart = this.ComponentUnderTest.GetUsageByTargets();

            Assert.Greater(chart.Points.Count(), 0);
            Assert.IsFalse(this.HasDoublons(chart), "Chart has doublons");
        }

        [Test]
        public void RetrieveExecutionTime_ByDay_HasCount()
        {
            this.FillStatistics(50);

            var chart = this.ComponentUnderTest.ExecutionTimeGraph();

            Assert.AreEqual(1, chart.Points.Count());
        }

        [Test]
        public void RetrieveExecutionTime_ByDay_NoDoublons()
        {
            this.FillStatisticsWithRandomBottlenecks(50);

            var chart = this.ComponentUnderTest.ExecutionTimeGraph();

            Assert.Greater(chart.Points.Count(), 0);

            Assert.IsFalse(this.HasDoublons(chart), "Chart has doublons");
        }

        protected override void _Setup()
        {
            this.BuildComponent(session => new ApplicationStatisticsComponent(session));
        }

        private double Count(Chart<string, double> chart, string with)
        {
            return (from c in chart.Points
                    where c.X == with
                    select c).Count();
        }

        private double Count(IEnumerable<BottleneckDto> list, string target, string method)
        {
            return (from item in list
                    where item.MethodName == method
                       && item.TargetTypeName == target
                    select item).Count();
        }

        private void FillStatistics(int counter)
        {
            var stat = new NDoctorStatisticsTester(this.Session);

            for (int i = 0; i < counter; i++)
            {
                stat.Add(new ApplicationStatistics()
                {
                    ExecutionTime = 10,
                    IsPossibleBottleneck = false,
                    Message = string.Empty,
                    MethodName = "method",
                    TargetTypeName = "target",
                    Threshold = 1000,
                    TimeStamp = DateTime.Now,
                });
            }
            stat.Flush();
        }

        private void FillStatisticsOnTime()
        {
            var counter = 365 * 3;//Statistics on three years
            var stat = new NDoctorStatisticsTester(this.Session);
            var random = new Random(150);

            var time = DateTime.Now.AddYears(-3);

            for (int i = 0; i < counter; i++)
            {
                stat.Add(new ApplicationStatistics()
                {
                    ExecutionTime = random.Next(5, 10),
                    IsPossibleBottleneck = (random.Next(0, 1) == 0),
                    Message = string.Empty,
                    MethodName = Names[random.Next(0, 2)],
                    TargetTypeName = Names[random.Next(0, 2)],
                    Threshold = 10,
                    TimeStamp = time.AddDays(1),
                });
            }
            stat.Flush();
        }

        private void FillStatisticsWithRandomBottlenecks(int counter)
        {
            var stat = new NDoctorStatisticsTester(this.Session);
            var random = new Random(150);

            for (int i = 0; i < counter; i++)
            {
                stat.Add(new ApplicationStatistics()
                {
                    ExecutionTime = random.Next(5, 10),
                    IsPossibleBottleneck = (random.Next(0, 1) == 0),
                    Message = string.Empty,
                    MethodName = Names[random.Next(0, 2)],
                    TargetTypeName = Names[random.Next(0, 2)],
                    Threshold = 10,
                    TimeStamp = DateTime.Now,
                });
            }
            stat.Flush();
        }

        private bool HasDoublons<Tx, Ty>(Chart<Tx, Ty> chart)
        {
            return (from x in chart.XCollection
                    group x by x into g
                    where g.Count() > 1
                    select g.Key).Count() > 0;
        }

        private bool HasDoublons(IEnumerable<BottleneckDto> list)
        {
            return (from x in list
                    group x by new { x.TargetTypeName, x.MethodName } into g
                    where g.Count() > 1
                    select g.Key).Count() > 0;
        }

        #endregion Methods
    }
}