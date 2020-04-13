namespace dasz.LinqCube
{
    /// <summary>
    /// A measure represents a result. e.g. the sum of all hours.
    /// </summary>
    public interface IMeasure<TFact>
    {
        /// <summary>
        /// Name of the measure
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Applies a entry to a measure result.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="entry"></param>
        /// <param name="item"></param>
        void Apply(IMeasureResult<TFact> result, IDimensionResult<TFact> entry, TFact item);

        /// <summary>
        /// Returns the result of the measure.
        /// </summary>
        /// <returns></returns>
        IMeasureResult<TFact> CreateResult();
    }
}