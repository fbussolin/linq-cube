using System;
using System.Collections.Generic;
using System.Linq;

namespace dasz.LinqCube
{
    public class Query<TFact>
    {
        public string Name { get; private set; }

        public QueryResult<TFact> Result { get; private set; }


        /// <summary>
        /// The list of top-level query dimensions. This is initialised together with the QueryResult and is used to decouple the executor from the chained/crossing distinction.
        /// </summary>
        private IList<IDimensionResult<TFact>> TopLevelDimensions;

        /// <summary>
        /// The list of chained dimensions. These dimensions can only be accessed in the order of their definition.
        /// </summary>
        /// <remarks>
        /// The runtime of a query is in the order of the product of all entry-counts.
        /// </remarks>
        private IList<IDimensionResult<TFact>> ChainedDimensions;

        /// <summary>
        /// The list of crossing dimensions. These dimensions can only be accessed in any order after all chained dimensions were walked.
        /// </summary>
        /// <remarks>
        /// The runtime of a query is O(n^d), where d is the number of crossing query dimensions.
        /// </remarks>
        private IList<IDimensionResult<TFact>> CrossingDimensions;

        /// <summary>
        /// 
        /// </summary>
        private IList<IMeasure<TFact>> Measures;

        public Query(string name)
        {
            Name = name;
            ChainedDimensions = new List<IDimensionResult<TFact>>();
            CrossingDimensions = new List<IDimensionResult<TFact>>();
            TopLevelDimensions = new List<IDimensionResult<TFact>>();
            Measures = new List<IMeasure<TFact>>();
        }

        #region Extensions
        public Query<TFact> WithChainedDimension<TDimension>(Dimension<TDimension, TFact> dimension)
            where TDimension : IComparable
        {
            if (dimension == null)
                throw new ArgumentNullException(nameof(dimension));

            if (CrossingDimensions.Count > 0)
                throw new InvalidOperationException("Already added crossing dimensions.");

            if (ChainedDimensions.Any(i => i.DimensionEntry == dimension))
                throw new InvalidOperationException("Dimension already added.");

            var dimResult = new DimensionResult<TDimension, TFact>(dimension, Measures);

            if (ChainedDimensions.Count == 0)
                TopLevelDimensions.Add(dimResult);

            ChainedDimensions.Add(dimResult);

            return this;
        }

        public Query<TFact> WithCrossingDimension<TDimension>(Dimension<TDimension, TFact> dimension)
            where TDimension : IComparable
        {
            if (dimension == null)
                throw new ArgumentNullException(nameof(dimension));

            if (CrossingDimensions.Any(i => i.DimensionEntry == dimension))
                throw new InvalidOperationException("Dimension already added");

            var dimResult = new DimensionResult<TDimension, TFact>(dimension, Measures);
            TopLevelDimensions.Add(dimResult);
            CrossingDimensions.Add(dimResult);

            return this;
        }

        public Query<TFact> WithMeasure<TIntermediate>(Measure<TFact, TIntermediate> measure)
        {
            if (measure == null)
                throw new ArgumentNullException(nameof(measure));

            if (Measures.Contains(measure))
                throw new InvalidOperationException("Measure already added.");

            Measures.Add(measure);

            return this;
        }
        #endregion

        #region Initialize
        internal void Initialize()
        {
            Result = new QueryResult<TFact>();
            foreach (var dimResult in TopLevelDimensions)
                Result.Add(dimResult.DimensionEntry, dimResult);

            InitializeResults(null, ChainedDimensions, CrossingDimensions, null);
        }

        private void InitializeResults(IDimensionResult<TFact> dimResult, IEnumerable<IDimensionResult<TFact>> chainedDimensions, IEnumerable<IDimensionResult<TFact>> crossingDimensions, IDimensionResult<TFact> parentCoordinate)
        {
            var nextDim = chainedDimensions?.FirstOrDefault();

            // We have a "next" chained dimension.
            // Create result and recurse initialization
            if (nextDim != null)
                InitializeResults(dimResult == null ? nextDim : dimResult.AddToOtherDimension(nextDim),
                    chainedDimensions.Skip(1), crossingDimensions, dimResult);

            // No chained dimensions set/left
            // Generate all crossing permutations
            else
                foreach (var other in crossingDimensions)
                    InitializeResults(dimResult == null ? other : dimResult.AddToOtherDimension(other),
                        null, crossingDimensions.Where(i => i != other), dimResult);

            if (dimResult == null)
                return;

            dimResult.Initialize(parentCoordinate);
            foreach (var child in dimResult.DimensionEntry.Children)
                InitializeResults(dimResult.AddChild(child), chainedDimensions, crossingDimensions, parentCoordinate);
        }
        #endregion

        #region Apply
        internal void Apply(TFact item)
        {
            if (Measures.Count == 0)
                throw new InvalidOperationException("No measures added.");

            if (Result == null)
                throw new InvalidOperationException("Not initialized yet: no result created.");

            foreach (var dimResult in TopLevelDimensions)
                dimResult.Apply(item);
        }
        #endregion

        /// <summary>
        /// Implicit operator to be used as indexer for arrays
        /// </summary>
        /// <param name="query"></param>
        //public static implicit operator string(Query<TFact> query) => query?.Name;

        /// <summary>
        /// Returns a string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"Query: {Name}";
    }
}