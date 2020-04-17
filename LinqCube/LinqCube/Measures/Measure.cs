using System;

namespace dasz.LinqCube
{
    /// <summary>
    /// Abstract base class for measures.
    /// </summary>
    /// <typeparam name="TFact"></typeparam>
    /// <typeparam name="TIntermediate"></typeparam>
    public abstract class Measure<TFact, TIntermediate> : IMeasure<TFact>
    {
        /// <summary>
        /// Name of the measure.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Selector to extract the value from an entry
        /// </summary>
        protected Func<TFact, IDimensionResult<TFact>, TIntermediate> Selector { get; private set; }

        /// <summary>
        /// Constructs a new Measure
        /// </summary>
        /// <param name="name"></param>
        /// <param name="selector"></param>
        public Measure(string name, Func<TFact, IDimensionResult<TFact>, TIntermediate> selector)
        {
            Selector = selector;
            Name = name;
        }

        /// <summary>
        /// Derived classes should return the result of the measure.
        /// </summary>
        /// <returns></returns>
        public abstract IMeasureResult<TFact> CreateResult();

        /// <summary>
        /// Derived classes should apply a entry to the measure.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="entry"></param>
        /// <param name="item"></param>
        public abstract void Apply(IMeasureResult<TFact> result, IDimensionResult<TFact> entry, TFact item);

        /// <summary>
        /// Returns "Measure: {Name}"
        /// </summary>
        /// <returns>
        /// Measure: {Name}
        /// </returns>
        public override string ToString() =>
            $"Measure: {Name}";
    }
}