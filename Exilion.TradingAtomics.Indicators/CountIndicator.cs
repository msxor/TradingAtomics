using System;

namespace Exilion.TradingAtomics.Indicators
{
    public class CountIndicator:TimeCountIndicator
    {
        public CountIndicator(TimeSpan period) : base(period)
        {
        }

        protected override DataPoint<decimal> ProduceNewValue()
        {
            //TimeSeriesLock.EnterReadLock();
            try
            {
                if (TimeSeries.Count == 0)
                    return new DataPoint<decimal>(DateTime.Now, 0);
                return new DataPoint<decimal>(DateTime.Now, TimeSeries.Count);
            }
            finally
            {
                //TimeSeriesLock.ExitReadLock();
            }
        }
    }
}
