using System;

namespace dasz.LinqCube
{
    /// <summary>
    /// Abstract base class for measure result.
    /// </summary>
    /// <typeparam name="TFact"></typeparam>
    /// <typeparam name="TIntermediate"></typeparam>
    public abstract class MeasureResult<TFact, TIntermediate> : IMeasureResult<TFact>
    {
        /// <summary>
        /// Measure this result belongs to
        /// </summary>
        public IMeasure<TFact> Measure { get; private set; }

        /// <summary>
        /// The number of records that were aggregated to create this result.
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// The aggregated value devided by the count of records.
        /// </summary>
        public abstract double Average { get; }

        protected TIntermediate Value { get; set; }

        /// <summary>
        /// Result value as interger
        /// </summary>
        public abstract int IntValue { get; }

        /// <summary>
        /// Result value as double
        /// </summary>
        public abstract double DoubleValue { get; }

        /// <summary>
        /// Result value as decimal
        /// </summary>
        public abstract decimal DecimalValue { get; }

        /// <summary>
        /// Result value as DateTime
        /// </summary>
        public abstract DateTime DateTimeValue { get; }

        /// <summary>
        /// Result value as TimeSpan
        /// </summary>
        public abstract TimeSpan TimeSpanValue { get; }

        /// <summary>
        /// Creates a new MeasureResult
        /// </summary>
        /// <param name="Measure"></param>
        public MeasureResult(IMeasure<TFact> measure, TIntermediate init)
        {
            Measure = measure;
            Value = init;

            Count = 0;
        }

        internal void Set(TIntermediate item)
        {
            Value = item;
            Count++;
        }

        /// <summary>
        /// Returns a string represenation
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"MeasureResult<{typeof(TIntermediate).Name}>: {Measure?.Name} = {Value}";
    }
}