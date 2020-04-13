using System;

namespace dasz.LinqCube
{
    /// <summary>
    /// A integer measure result node
    /// </summary>
    public class IntMeasureResult<TFact> : MeasureResult<TFact, int>
    {
        /// <summary>
        /// The aggregated value devided by the count of records.
        /// </summary>
        public override double Average => Count == 0 ? 0 : Value / Count;

        /// <summary>
        /// Result value as int
        /// </summary>
        public override int IntValue => Value;

        /// <summary>
        /// Result value as double
        /// </summary>
        public override double DoubleValue => Value;

        /// <summary>
        /// Result value as decimal
        /// </summary>
        public override decimal DecimalValue => Value;

        /// <summary>
        /// Not suppored by this measure result
        /// </summary>
        public override DateTime DateTimeValue => throw new NotSupportedException();

        /// <summary>
        /// Not suppored by this measure result
        /// </summary>
        public override TimeSpan TimeSpanValue => throw new NotSupportedException();

        /// <summary>
        /// Creates a new MeasureResult
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="init"></param>
        public IntMeasureResult(IMeasure<TFact> measure, int init)
            : base(measure, init)
        {
        }
    }
}