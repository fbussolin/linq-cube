using System;
using System.Collections.Generic;

namespace dasz.LinqCube
{
    /// <summary>
    /// Represents the result of a dimension with all sub dimensions and measures.
    /// Represents the result of a dimension entry with all sub dimensions and measures.
    /// </summary>
    public interface IDimensionResult<TFact>
    {
        IDimensionEntry DimensionEntry { get; }

        IDimensionResult<TFact> Initialize(IDimensionResult<TFact> parentCoordinate);
        IDimensionResult<TFact> AddToOtherDimension(IDimensionResult<TFact> entry);
        IDimensionResult<TFact> AddChild(IDimensionEntry entry);

        void Apply(TFact item);

        void Apply(TFact item, IDimensionResult<TFact> dimResult);

        /// <summary>
        /// Provide item to measures
        /// </summary>
        /// <param name="item"></param>
        void ProvideItemToMeasures(TFact item);

        /// <summary>
        /// Provide item to other dimensions
        /// </summary>
        /// <param name="item"></param>
        void ProvideItemToOtherDimensions(TFact item);

        /// <summary>
        /// Returns all children dimension entry results
        /// </summary>
        IDictionary<IDimensionEntry, IDimensionResult<TFact>> Children { get; }

        /// <summary>
        /// Return a dimension entry result by the given dimension entry
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IDimensionResult<TFact> this[string key] { get; }

        /// <summary>
        /// Return a dimension entry result by the given dimension entry
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IDimensionResult<TFact> this[IDimensionEntry key] { get; }

        /// <summary>
        /// Access a measure result by measure
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IMeasureResult<TFact> this[IMeasure<TFact> key] { get; }

        bool Count<TDimension>(IDimension dimension, Func<DimensionEntry<TDimension>, bool> selector)
            where TDimension : IComparable;
    }
}