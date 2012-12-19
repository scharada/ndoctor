namespace Probel.Helpers.Test
{
    using System.Threading;

    using NUnit.Framework;

    using Probel.Helpers.Benchmarking;

    [TestFixture]
    public class TestBenchmark
    {
        #region Methods

        [Test]
        public void GlobalBenchmarking_Measure100Ms_SpecifyValueBetween95And110()
        {
            using (new Benchmark(e => Assert.AreEqual(20, e.TotalMilliseconds, 5)))
            {
                Thread.Sleep(20);
            }
        }

        [Test]
        public void GlobalBenchmarking_MeasureInBetweenValue_InBetweenAreCorrect()
        {
            using (var bench = new Benchmark(e => Assert.AreEqual(40, e.TotalMilliseconds, 5)))
            {
                bench.CheckNow(e => Assert.AreEqual(0, e.TotalMilliseconds, 5));
                Thread.Sleep(20);
                bench.CheckNow(e => Assert.AreEqual(20, e.TotalMilliseconds, 5));
                Thread.Sleep(20);
            }
        }

        [Test]
        public void GlobalBenchmarking_UseDefaultCtor_AllIsWorking()
        {
            using (new Benchmark())
            {
                //Do nothing.
            }
            //No exception is expected
        }

        #endregion Methods
    }
}