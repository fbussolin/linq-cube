using System;

namespace dasz.LinqCube
{
    /// <summary>
    /// A double measure result node
    /// </summary>
    public class DoubleMeasureResult<TFact> : MeasureResult<TFact, double>
    {
        /// <summary>
        /// The aggregated value devided by the count of records.
        /// </summary>
        public override double Average => Count == 0 ? 0 : Value / Count;

        /// <summary>
        /// Result value as int
        /// </summary>
        public override int IntValue => (int)Value;

        /// <summary>
        /// Result value as double
        /// </summary>
        public override double DoubleValue => Value;

        /// <summary>
        /// Result value as decimal
        /// </summary>
        public override decimal DecimalValue => (decimal)Value;

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
        public DoubleMeasureResult(IMeasure<TFact> measure, double init)
            : base(measure, init)
        {
        }
    }
}