using System;
using System.Collections.Generic;

namespace dasz.LinqCube
{
    /// <summary>
    /// Represents the result of a cube
    /// </summary>
    public class CubeResult<TFact> : Dictionary<Query<TFact>, QueryResult<TFact>>
    {
        /// <summary>
        /// Constructs a new cube result
        /// </summary>
        public CubeResult()
        {
        }

        public CubeResult<TFact> Add(CubeResult<TFact> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            foreach (var kv in other)
                Add(kv.Key, kv.Value);

            return this;
        }

        /// <summary>
        /// Returns a string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"CubeResult: Count={Count}";
    }
}