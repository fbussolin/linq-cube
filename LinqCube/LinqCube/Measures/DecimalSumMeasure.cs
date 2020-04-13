using System;

namespace dasz.LinqCube
{
    /// <summary>
    /// Measure to sum a decimal value.
    /// </summary>
    /// <typeparam name="TFact"></typeparam>
    public class DecimalSumMeasure<TFact> : Measure<TFact, decimal>
    {
        /// <summary>
        /// Constructs a new Measure
        /// </summary>
        /// <param name="name"></param>
        /// <param name="selector"></param>
        public DecimalSumMeasure(string name, Func<TFact, decimal> selector)
            : this(name, (fact, entry) => selector(fact))
        {
        }

        /// <summary>
        /// Constructs a new Measure
        /// </summary>
        /// <param name="name"></param>
        /// <param name="selector"></param>
        public DecimalSumMeasure(string name, Func<TFact, IDimensionResult<TFact>, decimal> selector)
            : base(name, selector)
        {
        }

        /// <summary>
        /// Returns the result of the measure.
        /// </summary>
        /// <returns></returns>
        public override IMeasureResult<TFact> CreateResult() =>
            new DecimalMeasureResult<TFact>(this, 0);

        /// <summary>
        /// Applies a entry to a measure result.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="entry"></param>
        /// <param name="item"></param>
        public override void Apply(IMeasureResult<TFact> result, IDimensionResult<TFact> entry, TFact item)
        {
            var myResult = (DecimalMeasureResult<TFact>)result ??
                throw new ArgumentNullException(nameof(result));

            myResult.Set(myResult.DecimalValue + Selector(item, entry));
        }
    }
}