using System;

namespace dasz.LinqCube
{
    /// <summary>
    /// A decimal measure result node
    /// </summary>
    public class TimeSpanMeasureResult<TFact> : MeasureResult<TFact, TimeSpan>
    {
        /// <summary>
        /// Not supported by this measure result
        /// </summary>
        public override double Average => throw new NotSupportedException();

        /// <summary>
        /// Result value as interger in milliseconds
        /// </summary>
        public override int IntValue => (int)Value.TotalMilliseconds;

        /// <summary>
        /// Result value as double in milliseconds
        /// </summary>
        public override double DoubleValue => Value.TotalMilliseconds;

        /// <summary>
        /// Result value as decimal in milliseconds
        /// </summary>
        public override decimal DecimalValue => (decimal)Value.TotalMilliseconds;

        /// <summary>
        /// Result value as DateTime
        /// </summary>
        public override DateTime DateTimeValue => DateTime.MinValue.Add(Value);

        /// <summary>
        /// Result value as TimeSpan
        /// </summary>
        public override TimeSpan TimeSpanValue => Value;

        /// <summary>
        /// Creates a new MeasureResult
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="init"></param>
        public TimeSpanMeasureResult(IMeasure<TFact> measure, TimeSpan init)
            : base(measure, init)
        {
        }
    }
}