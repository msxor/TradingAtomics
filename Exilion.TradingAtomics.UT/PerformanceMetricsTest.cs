using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TradingAtomics;

namespace Exilion.TradingAtomics.UT
{
    [TestFixture]
    public class PerformanceMetricsTest
    {
        [Test]
        public void Metrics_StraightUp()
        {
            var startTime = DateTime.Parse("1-1-2016");
            TimePriceSeries priceSeries = new TimePriceSeries(new Dictionary<DateTime,decimal>
            {
                {startTime.AddDays(1),5},
                {startTime.AddDays(2),6},
                {startTime.AddDays(3),8},
                {startTime.AddDays(4),9},
                {startTime.AddDays(5),11},
                {startTime.AddDays(6),11},
                {startTime.AddDays(7),13},
                {startTime.AddDays(8),14},
                {startTime.AddDays(9),15},
                {startTime.AddDays(10),16},
            });
            var sut = priceSeries.CalculatePerformanceMetrics();

            Assert.That(sut.Count, Is.EqualTo(10));
            Assert.That(sut.TotalPnL, Is.EqualTo(11));
            Assert.That(sut.LoosingTrades, Is.EqualTo(0));
            Assert.That(sut.WinningTrades, Is.EqualTo(8));
            Assert.That(sut.MaxDrawdown, Is.EqualTo(0));

        }
        [Test]
        public void Metrics_StraightDown()
        {
            var startTime = DateTime.Parse("1-1-2016");
            TimePriceSeries priceSeries = new TimePriceSeries(new Dictionary<DateTime, decimal>
            {
                {startTime.AddDays(1),16},
                {startTime.AddDays(2),15},
                {startTime.AddDays(3),14},
                {startTime.AddDays(4),13},
                {startTime.AddDays(5),11},
                {startTime.AddDays(6),11},
                {startTime.AddDays(7),9},
                {startTime.AddDays(8),8},
                {startTime.AddDays(9),6},
                {startTime.AddDays(10),5},
            });

            var sut = priceSeries.CalculatePerformanceMetrics();

            Assert.That(sut.MaxDrawdown, Is.EqualTo(11));

            Assert.That(sut.Count, Is.EqualTo(10));
            Assert.That(sut.TotalPnL, Is.EqualTo(-11));
            Assert.That(sut.LoosingTrades, Is.EqualTo(8));
            Assert.That(sut.WinningTrades, Is.EqualTo(0));
        }
        [Test]
        public void Metrics_DD1_1Peak()
        {
            var startTime = DateTime.Parse("1-1-2016");
            TimePriceSeries priceSeries = new TimePriceSeries(new Dictionary<DateTime, decimal>
            {
                {startTime.AddDays(1),1},
                {startTime.AddDays(2),2},
                {startTime.AddDays(3),3},
                {startTime.AddDays(4),4},//P
                {startTime.AddDays(5),3},
                {startTime.AddDays(6),2},//T
                {startTime.AddDays(7),3},
                {startTime.AddDays(8),4},
                {startTime.AddDays(9),5},
                {startTime.AddDays(10),6},
            });

            var perfPrice = priceSeries.CalculatePerformanceMetrics();

            Assert.That(perfPrice.MaxDrawdown, Is.EqualTo(2));

        }

        [Test]
        public void Metrics_DD2_1Peak()
        {
            var startTime = DateTime.Parse("1-1-2016");
            TimePriceSeries priceSeries = new TimePriceSeries(new Dictionary<DateTime, decimal>
            {
                {startTime.AddDays(1),10},//P
                {startTime.AddDays(2),8},
                {startTime.AddDays(3),7},
                {startTime.AddDays(4),6},
                {startTime.AddDays(5),5},
                {startTime.AddDays(6),4}, //T
                {startTime.AddDays(7),5},
                {startTime.AddDays(8),6},
                {startTime.AddDays(9),5},
                {startTime.AddDays(10),4},
            });

            var perfPrice = priceSeries.CalculatePerformanceMetrics();

            Assert.That(perfPrice.MaxDrawdown, Is.EqualTo(6));

        }
        [Test]
        public void Metrics_DD3_2Peaks()
        {
            var startTime = DateTime.Parse("1-1-2016");
            TimePriceSeries priceSeries = new TimePriceSeries(new Dictionary<DateTime, decimal>
            {
                {startTime.AddDays(1),1},
                {startTime.AddDays(2),2},
                {startTime.AddDays(3),3},
                {startTime.AddDays(4),2},
                {startTime.AddDays(5),3},
                {startTime.AddDays(6),4},
                {startTime.AddDays(7),6},//P
                {startTime.AddDays(8),3},
                {startTime.AddDays(9),2}, //T
                {startTime.AddDays(10),4},
            });

            var perfPrice = priceSeries.CalculatePerformanceMetrics();

            Assert.That(perfPrice.MaxDrawdown, Is.EqualTo(4));

        }

        [Test]
        public void Metrics_DD4()
        {
            var startTime = DateTime.Parse("1-1-2016");
            TimePriceSeries priceSeries = new TimePriceSeries(new Dictionary<DateTime, decimal>
            {
                {startTime.AddDays(1),5},
                {startTime.AddDays(2),6},
                {startTime.AddDays(3),8},//P
                {startTime.AddDays(4),6},
                {startTime.AddDays(5),5},
                {startTime.AddDays(6),6},
                {startTime.AddDays(7),7},
                {startTime.AddDays(8),6},
                {startTime.AddDays(9),4}, //T
                {startTime.AddDays(10),6},
            });

            var perfPrice = priceSeries.CalculatePerformanceMetrics();

            Assert.That(perfPrice.MaxDrawdown, Is.EqualTo(4));

        }
        [Test]
        public void Metrics_Sharpe()
        {
            var startTime = DateTime.Parse("1-1-2016");
            TimePriceSeries priceSeries = new TimePriceSeries(new Dictionary<DateTime, decimal>
            {
                {startTime.AddDays(1),5},
                {startTime.AddDays(2),6},
                {startTime.AddDays(3),7},
                {startTime.AddDays(4),8},
                {startTime.AddDays(5),9},
                {startTime.AddDays(6),10},
                {startTime.AddDays(7),9},
                {startTime.AddDays(8),12},

            });

            var sut = priceSeries.CalculatePerformanceMetrics();

            Assert.That(sut.SharpeRatio.AlmostEquals(0.830747161m));

        }
        [Test]
        public void Cumulative_Differential()
        {
            var startTime = DateTime.Parse("1-1-2016");
            TimePriceSeries series = new TimePriceSeries(new Dictionary<DateTime, decimal>
            {
                {startTime.AddDays(1),1},
                {startTime.AddDays(2),2},
                {startTime.AddDays(3),-3},
                {startTime.AddDays(4),1},
                {startTime.AddDays(5),-2},
            });

            var cumSeries = TimePriceSeries.FromPnLSeries(series);
            Assert.That(cumSeries[0].Value, Is.EqualTo(1));
            Assert.That(cumSeries[1].Value, Is.EqualTo(3));
            Assert.That(cumSeries[2].Value, Is.EqualTo(0));
            Assert.That(cumSeries[3].Value, Is.EqualTo(1));
            Assert.That(cumSeries[4].Value, Is.EqualTo(-1));

            var diffSeries = cumSeries.ToPnLSeries();
            Assert.That(diffSeries[0].Value, Is.EqualTo(0)); // first is always 0
            for(int i = 1 ; i < series.Count; i++)
                Assert.That(series[i].Value, Is.EqualTo(diffSeries[i].Value));
        }
    }
}
