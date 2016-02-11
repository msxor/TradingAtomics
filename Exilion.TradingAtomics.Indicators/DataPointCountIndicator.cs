using System.Linq;

namespace Exilion.TradingAtomics.Indicators
{
    public abstract class DataPointCountIndicator: IndicatorBase
    {
        private readonly int _period;

        protected DataPointCountIndicator(int period)
        {
            _period = period;
        }

        /// <summary>
        /// Called AFTER new dataPoint was added
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <returns>true if update should be triggered</returns>
        protected override void OnNewDataPoint(DataPoint<decimal> dataPoint)
        {
            //TimeSeriesLock.EnterUpgradeableReadLock();
            try
            {
                if (TimeSeries.Count > _period)
                {
                    //TimeSeriesLock.EnterWriteLock();
                    try
                    {
                        TimeSeries = TimeSeries.Remove(TimeSeries.First());
                    }
                    finally
                    {
                        //TimeSeriesLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                //TimeSeriesLock.ExitUpgradeableReadLock();
            }
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
               // TimeSeriesLock.ExitWriteLock();
            }

            OnNewDataPoint(dataPoint);

            bool haveToProcess = false;
            //TimeSeriesLock.EnterReadLock();
            try
            {
                haveToProcess = TimeSeries.Count >= _period;
            }
            finally
            {
                //TimeSeriesLock.ExitReadLock();
            }
            // not ideal, value can still change, but it is acceptable for the stats
            if (haveToProcess)
                ProcessNewValue();
        }
        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_subscriptions.Dispose();
            }
        }
        #endregion IDisposable
    }
}
