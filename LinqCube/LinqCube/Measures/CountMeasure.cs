using System;

namespace dasz.LinqCube
{
    /// <summary>
    /// Measure to count items.
    /// </summary>
    /// <typeparam name="TFact"></typeparam>
    public class CountMeasure<TFact> : Measure<TFact, bool>
    {
        /// <summary>
        /// Constructs a new Measure
        /// </summary>
        /// <param name="name"></param>
        /// <param name="selector"></param>
        public CountMeasure(string name, Func<TFact, bool> selector)
            : this(name, (fact, entry) => selector(fact))
        {
        }

        /// <summary>
        /// Constructs a new Measure
        /// </summary>
        /// <param name="name"></param>
        /// <param name="selector"></param>
        public CountMeasure(string name, Func<TFact, IDimensionResult<TFact>, bool> selector)
            : base(name, selector)
        {
        }

        /// <summary>
        /// Returns the result of the measure.
        /// </summary>
        /// <returns></returns>
        public override IMeasureResult<TFact> CreateResult() =>
            new IntMeasureResult<TFact>(this, 0);

        /// <summary>
        /// Applies a entry to a measure result.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="entry"></param>
        /// <param name="item"></param>
        public override void Apply(IMeasureResult<TFact> result, IDimensionResult<TFact> entry, TFact item)
        {
            var myResult = (IntMeasureResult<TFact>)result ??
                throw new ArgumentNullException(nameof(result));

            if (Selector(item, entry))
                myResult.Set(myResult.IntValue + 1);
        }
    }
}