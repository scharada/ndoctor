namespace Probel.Helpers.Test
{
    using System;

    using NUnit.Framework;

    using Probel.Helpers.Data;

    public class TestData : TestBase
    {
        #region Methods

        [Test]
        public void CanDetectEquality()
        {
            var baseRange = new DateRange(DateTime.Today, DateTime.Today.AddHours(4));
            var range1 = new DateRange(DateTime.Today, DateTime.Today.AddHours(4));

            Assert.IsTrue(baseRange.AreEqualTimeSpan(range1), "This ranges should be equal");
        }

        [Test]
        public void CanDetectNonOverlapping()
        {
            var baseRange = new DateRange(DateTime.Today, DateTime.Today.AddHours(4));
            var range1 = new DateRange(DateTime.Today.AddHours(-5), DateTime.Today.AddHours(-1));
            var range2 = new DateRange(DateTime.Today.AddHours(4), DateTime.Today.AddHours(6));
            var range3 = new DateRange(DateTime.Today.AddHours(9), DateTime.Today.AddHours(10));

            Assert.IsFalse(baseRange.Overlaps(range1), "Range 1 shouldn't overlap");
            Assert.IsFalse(baseRange.Overlaps(range2), "Range 2 shouldn't overlap");
            Assert.IsFalse(baseRange.Overlaps(range3), "Range 3 shouldn't overlap");
        }

        [Test]
        public void CanDetectOverlapping()
        {
            var baseRange = new DateRange(DateTime.Today, DateTime.Today.AddHours(4));
            var range1 = new DateRange(DateTime.Today.AddHours(-5), DateTime.Today.AddHours(2));
            var range2 = new DateRange(DateTime.Today.AddHours(3), DateTime.Today.AddHours(6));
            var range3 = new DateRange(DateTime.Today.AddHours(-6), DateTime.Today.AddHours(10));

            Assert.IsTrue(baseRange.Overlaps(range1), "Range 1 should overlap");
            Assert.IsTrue(baseRange.Overlaps(range2), "Range 2 should overlap");
            Assert.IsTrue(baseRange.Overlaps(range3), "Range 3 should overlap");
        }

        #endregion Methods
    }
}