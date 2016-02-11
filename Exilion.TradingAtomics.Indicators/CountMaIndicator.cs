using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;


namespace Exilion.TradingAtomics.Indicators
{
    public class CountMaIndicator : IObservable<Tuple<string, DataPoint<decimal>>>, IDisposable
    {
        private readonly CountIndicator _workerCallCountIndicator;
        private readonly MovingAverageIndicator _workerCallMaIndicator;
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();
        private readonly Subject<Tuple<string, DataPoint<decimal>>> _newItemSubject = new Subject<Tuple<string, DataPoint<decimal>>>();
        public string Name { get; private set; }
        public CountMaIndicator(string name, TimeSpan countPeriod, int maPeriod)
        {
            _workerCallCountIndicator = new CountIndicator(countPeriod);
            _workerCallMaIndicator = new MovingAverageIndicator(maPeriod);
            _subscriptions.Add(_workerCallCountIndicator.Subscribe(_workerCallMaIndicator));
            _subscriptions.Add(_workerCallMaIndicator.Do(d => LastReading = d).Subscribe(dp =>
            {
                var newReading = new Tuple<string, DataPoint<decimal>>(Name, dp);
                _newItemSubject.OnNext(newReading);
            }));

            Name = name;
            LastReading = new DataPoint<decimal>(DateTime.Now, 0);
        }

        public void Tick()
        {
            _workerCallCountIndicator.OnNext(new DataPoint<decimal>(DateTime.Now, 0));
        }

        public IDisposable Subscribe(IObserver<Tuple<string, DataPoint<decimal>>> observer)
        {
            return _newItemSubject.Subscribe(observer);
        }
        public DataPoint<decimal> LastReading { get; private set; }
        #region IDisposable
        ~CountMaIndicator()
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
                _subscriptions.Dispose();
                _workerCallCountIndicator.Dispose();
                _workerCallMaIndicator.Dispose();
                _newItemSubject.Dispose();
            }
        }
        #endregion IDisposable
    }
}
