using System;
using System.Reactive.Linq;

namespace Exilion.TradingAtomics.Indicators
{
    public abstract class TimeCountIndicator:IndicatorBase
    {
        private readonly IDisposable _subscription;
        protected TimeCountIndicator(TimeSpan period)
        {
            _subscription = Observable
                .Timer(period,period)
                .Subscribe(OnTick);
        }

        private void OnTick(long obj)
        {
            ProcessNewValue();
            //TimeSeriesLock.EnterWriteLock();
            try
            {
                TimeSeries = TimeSeries.Clear();
            }
            finally
            {
                //TimeSeriesLock.ExitWriteLock();
            }
        }
        protected override void OnNewDataPoint(DataPoint<decimal> dataPoint)
        {

        }

        protected override void Add(DataPoint<decimal> dataPoint)
        {
            //TimeSeriesLock.EnterWriteLock();
            try
            {
                TimeSeries.Add(dataPoint);
            }
            finally
            {
                //TimeSeriesLock.ExitWriteLock();
            }
            OnNewDataPoint(dataPoint);
        }

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _subscription.Dispose();
            }
        }
        #endregion IDisposable
    }
}
