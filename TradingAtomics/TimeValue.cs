using System;

namespace Exilion.TradingAtomics
{
    /// <summary>
    /// Immutable generic time-value pair
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
            return string.Format("{0} = {1}", Time, Value);
        }

        internal TimeValue<T> Clone()
        {
            return new TimeValue<T>(Time, Value);
        }
    }
}
