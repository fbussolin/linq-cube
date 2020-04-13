using System;

namespace dasz.LinqCube
{
    /// <summary>
    /// Measure to min a TimeSpan value.
    /// </summary>
    /// <typeparam name="TFact"></typeparam>
    public class TimeSpanMinMeasure<TFact> : Measure<TFact, TimeSpan>
    {
        /// <summary>
        /// Constructs a new Measure
        /// </summary>
        /// <param name="name"></param>
        /// <param name="selector"></param>
        public TimeSpanMinMeasure(string name, Func<TFact, TimeSpan> selector)
            : this(name, (fact, entry) => selector(fact))
        {
        }

        /// <summary>
        /// Constructs a new Measure
        /// </summary>
        /// <param name="name"></param>
        /// <param name="selector"></param>
        public TimeSpanMinMeasure(string name, Func<TFact, IDimensionResult<TFact>, TimeSpan> selector)
            : base(name, selector)
        {
        }

        /// <summary>
        /// Returns the result of the measure.
        /// </summary>
        /// <returns></returns>
        public override IMeasureResult<TFact> CreateResult() =>
            new TimeSpanMeasureResult<TFact>(this, TimeSpan.MaxValue);

        /// <summary>
        /// Applies a entry to a measure result.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="entry"></param>
        /// <param name="item"></param>
        public override void Apply(IMeasureResult<TFact> result, IDimensionResult<TFact> entry, TFact item)
        {
            var myResult = (TimeSpanMeasureResult<TFact>)result ??
                throw new ArgumentNullException(nameof(result));

            var v = Selector(item, entry);
            if (v < myResult.TimeSpanValue)
                myResult.Set(v);
        }
    }
}
