using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Exilion.TradingAtomics.Indicators;
using NUnit.Framework;

namespace Exilion.TradingAtomics.UT.IndicatorTests
{
    [TestFixture]
    public class CountIndicatorTest
    {
        [Test]
        public void ProducesValues()
        {
            var sut = new CountIndicator(TimeSpan.FromMilliseconds(100));
            
            var results = new List<DataPoint<decimal>>();

            using (sut.Subscribe(results.Add))
            {
                sut.OnNext(new DataPoint<decimal>(DateTime.Now, 1));
                Assert.That(results.Count, Is.EqualTo(0));

                sut.OnNext(new DataPoint<decimal>(DateTime.Now, 2));
                Assert.That(results.Count, Is.EqualTo(0));

                // 110: 1 tick 
                while (results.Count < 1)
                    Thread.Sleep(10);

                sut.OnNext(new DataPoint<decimal>(DateTime.Now, 3)); // this tick is in the next period
                Assert.That(results.Last().Value, Is.EqualTo(2));

                for(int i = 0; i < 10; i++)
                    Thread.Sleep(10);
                
                // 210: 2 ticks
                // actually, wait for the second tick here
                while (results.Count < 2)
                    Thread.Sleep(10);
                
                Assert.That(results.Last().Value, Is.EqualTo(1));

                for (int i = 0; i < 20; i++)
                    Thread.Sleep(10);
                // 410: no new ticks
                Assert.That(results.Last().Value, Is.EqualTo(0));
            }
            sut.Dispose();
        }
        [Test]
        public void CompositeIndicator()
        {
            var ciResults = new List<DataPoint<decimal>>();
            var maResults = new List<DataPoint<decimal>>();

            var ci = new CountIndicator(TimeSpan.FromSeconds(1));
            var ma = new MovingAverageIndicator(2);
            
            // composite indicator
            ci.Subscribe(ma);
            ci.Subscribe(ciResults.Add);
            ma.Subscribe(maResults.Add);

            // 2 second test

            // produce 10 in the second #1
            for (int i = 0; i < 10; i++)
            {
                ci.OnNext(new DataPoint<decimal>(DateTime.Now, 1));
                Thread.Sleep(99);
            }
            Thread.Sleep(50);
            // produce 20 in the second #2
            for (int i = 0; i < 20; i++)
            {
                ci.OnNext(new DataPoint<decimal>(DateTime.Now, 1));
                Thread.Sleep(48);
            }


            Thread.Sleep(50);

            Assert.That(ciResults.ElementAt(0).Value, Is.EqualTo(10));
            Assert.That(ciResults.ElementAt(1).Value, Is.EqualTo(20));
            Assert.That(maResults.ElementAt(0).Value, Is.EqualTo(15));

        }
    }
}
