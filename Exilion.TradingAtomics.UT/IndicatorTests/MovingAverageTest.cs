using System;
using System.Collections.Generic;
using System.Linq;
using Exilion.TradingAtomics.Indicators;
using NUnit.Framework;

namespace Exilion.TradingAtomics.UT.IndicatorTests
{
    [TestFixture]
    public class MovingAverageTest
    {
        [Test]
        
        public void ProducesValues()
        {
            var sut = new MovingAverageIndicator(3);
            
            var results = new List<DataPoint<decimal>>();
            using (sut.Subscribe(results.Add))
            {
                sut.OnNext(new DataPoint<decimal>(DateTime.Now, 1));
                Assert.That(results.Count, Is.EqualTo(0));

                sut.OnNext(new DataPoint<decimal>(DateTime.Now, 2));
                Assert.That(results.Count, Is.EqualTo(0));

                sut.OnNext(new DataPoint<decimal>(DateTime.Now, 3));
                Assert.That(results.Count, Is.EqualTo(1));
                Assert.That(results.Last().Value, Is.EqualTo(2));

                sut.OnNext(new DataPoint<decimal>(DateTime.Now, 4));
                Assert.That(results.Count, Is.EqualTo(2));
                Assert.That(results.Last().Value, Is.EqualTo(3));

                sut.OnNext(new DataPoint<decimal>(DateTime.Now, 2));
                Assert.That(results.Count, Is.EqualTo(3));
                Assert.That(results.Last().Value, Is.EqualTo(3));

                sut.Dispose();
            }
        }
        [Test]
        public void Indicator_Chaining_MAofMA()
        {
            var results1 = new List<DataPoint<decimal>>();
            var results2 = new List<DataPoint<decimal>>();
            var ma1 = new MovingAverageIndicator(2);
            var ma2 = new MovingAverageIndicator(2);
            ma1.Subscribe(ma2); // ma2 of ma1
            ma1.Subscribe(results1.Add);
            ma2.Subscribe(results2.Add);

            ma1.OnNext(new DataPoint<decimal>(DateTime.Now, 1));
            ma1.OnNext(new DataPoint<decimal>(DateTime.Now, 2));
            Assert.That(results1.Last().Value, Is.EqualTo(1.5));
            Assert.That(results2.Any(), Is.EqualTo(false));

            ma1.OnNext(new DataPoint<decimal>(DateTime.Now, 3));
            Assert.That(results1.Last().Value, Is.EqualTo(2.5));
            Assert.That(results2.Last().Value, Is.EqualTo(2));

            ma1.OnNext(new DataPoint<decimal>(DateTime.Now, 4));
            Assert.That(results1.Last().Value, Is.EqualTo(3.5));
            Assert.That(results2.Last().Value, Is.EqualTo(3));

            ma1.OnNext(new DataPoint<decimal>(DateTime.Now, 5));
            Assert.That(results1.Last().Value, Is.EqualTo(4.5));
            Assert.That(results2.Last().Value, Is.EqualTo(4));

            ma1.Dispose();
            ma2.Dispose();
        }
    }
}
