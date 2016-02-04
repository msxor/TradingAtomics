using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Exilion.TradingAtomics
{
    public class TimeSeries<T>:IReadOnlyList<TimeValue<T>>
    {
        
        public TimeSeries()
        {
            _values = new List<TimeValue<T>>();
        }
        
        private readonly List<TimeValue<T>> _values;

        public TimeSeries(IEnumerable<Tuple<DateTime,T>> values )
        {
            _values = values.Select(value => new TimeValue<T>(value.Item1, value.Item2)).ToList();
        }

        public TimeSeries(Dictionary<DateTime, T> values)
        {
            _values = values.Select(kvp => new TimeValue<T>(kvp.Key, kvp.Value)).ToList();
        }

        /// <summary>
        /// Creates a copy
        /// </summary>
        /// <param name="orig"></param>
        public TimeSeries(TimeSeries<T> orig )
        {
            _values = orig._values;
        }
        //public IReadOnlyList<TimeValue<T>> Data { get { return _values;} }

        public TimeSeries<T> Add(DateTime time, T value )
        {
            var clone = new TimeSeries<T>(this);
            clone._values.Add(new TimeValue<T>(time, value));
            return clone;
        }

        public IEnumerator<TimeValue<T>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get { return _values.Count; } }

        public TimeValue<T> this[int index]
        {
            get { return _values[index]; }
        }

    }

    public class TimeValue<T>
    {
        public TimeValue(DateTime time, T value)
        {
            Time = time;
            Value = value;
        }
        public DateTime Time { get; private set; }
        public T Value { get; private set; }
        public override string ToString()
        {
            return string.Format("{0} = {1}",Time, Value);
        }

        internal TimeValue<T> Clone()
        {
           return new TimeValue<T>(Time,Value);
        }
    }
}
