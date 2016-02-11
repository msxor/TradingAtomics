using System;
using System.Linq;

namespace Exilion.TradingAtomics.Indicators
{
    public class MovingAverageIndicator:DataPointCountIndicator
    {
        public MovingAverageIndicator(int period) : base(period)
        {
        }

        protected override DataPoint<decimal> ProduceNewValue()
        {
            //TimeSeriesLock.EnterReadLock();
            try
            {
                return new DataPoint<decimal>(DateTime.Now, TimeSeries.Select(dp => dp.Value).Average());
            }
            finally
            {
                //TimeSeriesLock.ExitReadLock();
            }
        }

    }
}
