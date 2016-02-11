using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Exilion.TradingAtomics.Core
{
    /// <summary>
    /// Immutable thread safe TimeSeries base class
    /// Note: Values are not guatenteed to be thread safe
    /// </summary>
    /// <typeparam name="T">value data type</typeparam>
    public class TimeSeries<T>:IReadOnlyList<DataPoint<T>>
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly List<DataPoint<T>> _values;
        #region constructors
        public TimeSeries()
        {
            _values = new List<DataPoint<T>>();
        }
        
        public TimeSeries(IEnumerable<Tuple<DateTime,T>> values )
        {
            _values = values.Select(value => new DataPoint<T>(value.Item1, value.Item2)).ToList();
        }

        public TimeSeries(Dictionary<DateTime, T> values)
        {
            _values = values.Select(kvp => new DataPoint<T>(kvp.Key, kvp.Value)).ToList();
        }

        /// <summary>
        /// Creates a copy
        /// </summary>
        /// <param name="orig"></param>
        private TimeSeries(TimeSeries<T> orig )
        {
            _values = orig._values;
        }
        #endregion constructors

        public TimeSeries<T> Add(DateTime time, T value )
        {
            _lock.EnterWriteLock();
            try
            {
                var clone = new TimeSeries<T>(this);
                clone._values.Add(new DataPoint<T>(time, value));
                return clone;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        public TimeSeries<T> Add(DataPoint<T> dataPoint)
        {
            _lock.EnterWriteLock();
            try
            {
                var clone = new TimeSeries<T>(this);
                clone._values.Add(dataPoint);
                return clone;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        #region IEnumerable
        public IEnumerator<DataPoint<T>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _values.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        public DataPoint<T> this[int index]
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                return _values[index];
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }
        #endregion IEnumerable

        public TimeSeries<T> Clear()
        {
            return new TimeSeries<T>();
        }

        public TimeSeries<T> Remove(DataPoint<T> dataPoint)
        {
            _lock.EnterWriteLock();
            try
            {
                var clone = new TimeSeries<T>(this);
                clone._values.Remove(dataPoint);
                return clone;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }

}
