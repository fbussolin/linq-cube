using System;

namespace dasz.LinqCube
{
    /// <summary>
    /// A result node in the cube of a measure.
    /// </summary>
    public interface IMeasureResult<TFact>
    {
        /// <summary>
        /// Measure this result belongs to
        /// </summary>
        public IMeasure<TFact> Measure { get; }

        /// <summary>
        /// The number of records that were aggregated to create this result.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The aggregated value devided by the count of records.
        /// </summary>
        double Average { get; }

        /// <summary>
        /// result value as interger
        /// </summary>
        int IntValue { get; }

        /// <summary>
        /// result value as double
        /// </summary>
        double DoubleValue { get; }

        /// <summary>
        /// result value as decimal
        /// </summary>
        decimal DecimalValue { get; }

        /// <summary>
        /// result value as DateTme
        /// </summary>
        DateTime DateTimeValue { get; }

        /// <summary>
        /// result value as TimeSpan
        /// </summary>
        TimeSpan TimeSpanValue { get; }
    }
}