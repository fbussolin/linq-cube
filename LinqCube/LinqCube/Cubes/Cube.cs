using System;
using System.Linq;

namespace dasz.LinqCube
{
    /// <summary>
    /// Static class for executing a cube
    /// </summary>
    public static class Cube
    {
        /// <summary>
        /// Executes a cube and build all results
        /// </summary>
        /// <typeparam name="TFact">Type of the underlying fact.</typeparam>
        /// <param name="source">Source</param>
        /// <param name="queries">list of cube queries</param>
        /// <returns>a cube result</returns>
        public static CubeResult<TFact> Execute<TFact>(IQueryable<TFact> source, params Query<TFact>[] queries)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var result = new CubeResult<TFact>();

            foreach (var query in queries)
                query.Initialize();

            foreach (var item in source)
                foreach (var query in queries)
                    query.Apply(item);

            foreach (var query in queries)
                result[query] = query.Result;

            return result;
        }
    }
}