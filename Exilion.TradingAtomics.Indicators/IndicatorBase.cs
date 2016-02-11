using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Exilion.TradingAtomics.Core;

namespace Exilion.TradingAtomics.Indicators
{
    public abstract class IndicatorBase :IIndicator, IDisposable
    {
       // protected readonly List<DataPoint<decimal>> TimeSeries = new List<DataPoint<decimal>>();
        protected TimeSeries<decimal> TimeSeries = new TimeSeries<decimal>();
        private readonly Subject<DataPoint<decimal>> _newValueSubject = new Subject<DataPoint<decimal>>();
        protected abstract void Add(DataPoint<decimal> dataPoint);

        protected abstract void OnNewDataPoint(DataPoint<decimal> dataPoint);
        protected abstract DataPoint<decimal> ProduceNewValue();
        protected void ProcessNewValue()
        {
            var value = ProduceNewValue();
            _newValueSubject.OnNext(value);
        }

        public void Reset()
        {
            TimeSeries = TimeSeries.Clear();
        }
        
        public IDisposable Subscribe(IObserver<DataPoint<decimal>> observer)
        {
            return _newValueSubject.Subscribe(observer);
        }

        public void OnNext(DataPoint<decimal> value)
        {
            Add(value);
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
        #region IDisposable
        ~IndicatorBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _newValueSubject.Dispose();
            }
        }
        #endregion IDisposable
    }
}
