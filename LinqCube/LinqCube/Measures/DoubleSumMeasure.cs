using System;

namespace dasz.LinqCube
{
    /// <summary>
    /// Measure to sum a double value.
    /// </summary>
    /// <typeparam name="TFact"></typeparam>
    public class DoubleSumMeasure<TFact> : Measure<TFact, double>
    {
        /// <summary>
        /// Constructs a new Measure
        /// </summary>
        /// <param name="name"></param>
        /// <param name="selector"></param>
        public DoubleSumMeasure(string name, Func<TFact, double> selector)
            : this(name, (fact, entry) => selector(fact))
        {
        }

        /// <summary>
        /// Constructs a new Measure
        /// </summary>
        /// <param name="name"></param>
        /// <param name="selector"></param>
        public DoubleSumMeasure(string name, Func<TFact, IDimensionResult<TFact>, double> selector)
            : base(name, selector)
        {
        }

        /// <summary>
        /// Returns the result of the measure.
        /// </summary>
        /// <returns></returns>
        public override IMeasureResult<TFact> CreateResult() =>
            new DoubleMeasureResult<TFact>(this, 0);

        /// <summary>
        /// Applies a entry to a measure result.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="entry"></param>
        /// <param name="item"></param>
        public override void Apply(IMeasureResult<TFact> result, IDimensionResult<TFact> entry, TFact item)
        {
            var myResult = (DoubleMeasureResult<TFact>)result ??
                throw new ArgumentNullException(nameof(result));

            myResult.Set(myResult.DoubleValue + Selector(item, entry));
        }
    }
}