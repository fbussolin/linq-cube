using System;

namespace dasz.LinqCube
{
    /// <summary>
    /// Measure, that wraps another measure and applies a filter operation.
    /// </summary>
    /// <typeparam name="TFact"></typeparam>
    /// <typeparam name="TIntermediate"></typeparam>
    public class FilteredMeasure<TFact, TIntermediate> : Measure<TFact, bool>
    {
        private Measure<TFact, TIntermediate> Measure;

        /// <summary>
        /// Constructs a new Measure
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="measure"></param>
        public FilteredMeasure(Func<TFact, bool> filter, Measure<TFact, TIntermediate> measure)
            : this(measure?.Name ?? throw new ArgumentNullException(nameof(measure)),
                  (fact, entry) => filter(fact), measure)
        {
        }

        /// <summary>
        /// Constructs a new Measure
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="measure"></param>
        public FilteredMeasure(Func<TFact, IDimensionResult<TFact>, bool> filter, Measure<TFact, TIntermediate> measure)
            : this(measure?.Name, filter, measure)
        {
        }

        /// <summary>
        /// Constructs a new Measure
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filter"></param>
        /// <param name="measure"></param>
        public FilteredMeasure(string name, Func<TFact, bool> filter, Measure<TFact, TIntermediate> measure)
            : this(name, (fact, entry) => filter(fact), measure)
        {
        }

        /// <summary>
        /// Constructs a new Measure
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filter"></param>
        /// <param name="measure"></param>
        public FilteredMeasure(string name, Func<TFact, IDimensionResult<TFact>, bool> filter, Measure<TFact, TIntermediate> measure)
            : base(name, filter)
        {
            Measure = measure;
        }

        /// <summary>
        /// Returns the result of the measure.
        /// </summary>
        /// <returns></returns>
        public override IMeasureResult<TFact> CreateResult() =>
            Measure.CreateResult();

        /// <summary>
        /// Applies a entry to a measure result.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="entry"></param>
        /// <param name="item"></param>
        public override void Apply(IMeasureResult<TFact> result, IDimensionResult<TFact> entry, TFact item)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            if (Selector(item, entry))
                Measure.Apply(result, entry, item);
        }
    }
}