using System.Collections.Generic;

namespace dasz.LinqCube
{
    /// <summary>
    /// The result of a cube query
    /// </summary>
    public class QueryResult<TFact> : Dictionary<IDimensionEntry, IDimensionResult<TFact>>
    {
        /// <summary>
        /// Creates a new query result
        /// </summary>
        public QueryResult()
        {
        }

        /// <summary>
        /// Indexer for accessing a specific dimension
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new IDimensionResult<TFact> this[IDimensionEntry key] =>
            base[key?.Root][key];

        /// <summary>
        /// Returns a string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"QueryResult: Count={Count}";
    }
}