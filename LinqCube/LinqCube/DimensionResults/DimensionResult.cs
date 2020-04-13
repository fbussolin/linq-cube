using System;
using System.Collections.Generic;
using System.Linq;

namespace dasz.LinqCube
{
    /// <summary>
    /// Implementation of a dimension result
    /// Implementation of a dimension entry result
    /// </summary>
    /// <typeparam name="TFact"></typeparam>
    public class DimensionResult<TDimension, TFact> : IDimensionResult<TFact>
        where TDimension : IComparable
    {
        /// <summary>
        /// Returns the associated dimension entry
        /// </summary>
        public IDimensionEntry DimensionEntry { get; }

        /// <summary>
        /// Returns all children dimension entry results
        /// </summary>
        public IDictionary<IDimensionEntry, IDimensionResult<TFact>> Children { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private IDictionary<IDimensionResult<TFact>, IDimensionResult<TFact>> OtherDimensions;

        /// <summary>
        /// All associated measures
        /// </summary>
        private IEnumerable<IMeasure<TFact>> Measures;

        /// <summary>
        /// Returns all measure results
        /// </summary>
        private IDictionary<IMeasure<TFact>, IMeasureResult<TFact>> MeasureResults;

        /// <summary>
        /// 
        /// </summary>
        private DimensionResult<TDimension, TFact> ParentCoordinate;

        /// <summary>
        /// Creates a new dimension result
        /// </summary>
        /// <param name="e"></param>
        /// <param name="measures"></param>
        public DimensionResult(IDimensionEntry dimensionEntry, IEnumerable<IMeasure<TFact>> measures)
        {
            DimensionEntry = dimensionEntry;
            Children = new Dictionary<IDimensionEntry, IDimensionResult<TFact>>();
            OtherDimensions = new Dictionary<IDimensionResult<TFact>, IDimensionResult<TFact>>();
            Measures = measures;
            MeasureResults = new Dictionary<IMeasure<TFact>, IMeasureResult<TFact>>();
        }

        public IDimensionResult<TFact> AddToOtherDimension(IDimensionResult<TFact> entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            var newResult = new DimensionResult<TDimension, TFact>(entry.DimensionEntry, Measures);
            OtherDimensions[entry] = newResult;
            return newResult;
        }

        public IDimensionResult<TFact> AddChild(IDimensionEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            var newResult = new DimensionResult<TDimension, TFact>(entry, Measures);
            Children[entry] = newResult;
            return newResult;
        }

        #region Initialize
        /// <summary>
        /// Initialize the entry result
        /// </summary>
        /// <param name="parentCoordinate"></param>
        public IDimensionResult<TFact> Initialize(IDimensionResult<TFact> parentCoordinate)
        {
            foreach (var measure in Measures)
                MeasureResults[measure] = measure.CreateResult();

            ParentCoordinate = (DimensionResult<TDimension, TFact>)parentCoordinate;

            return this;
        }
        #endregion

        #region Apply
        public void Apply(TFact item) =>
            Apply(item, DimensionEntry, this);

        public void Apply(TFact item, IDimensionResult<TFact> dimResult)
        {
            if (dimResult == null)
                throw new ArgumentNullException(nameof(dimResult));

            Apply(item, DimensionEntry, dimResult);
        }

        private void Apply(TFact item, IDimensionEntry entry, IDimensionResult<TFact> result)
        {
            var root = ((Dimension<TDimension, TFact>)DimensionEntry.Root);
            if (!root.CheckFilter(item, entry))
                return;

            // Do something
            result.ProvideItemToMeasures(item);

            // All other
            result.ProvideItemToOtherDimensions(item);

            // Provide item to Children
            foreach (var child in entry.Children)
                Apply(item, child, result.Children[child]);
        }
        /// <summary>

        /// </summary>
        /// <param name="item"></param>
        public void ProvideItemToMeasures(TFact item)
        {
            foreach (var measure in MeasureResults)
                measure.Key.Apply(measure.Value, this, item);
        }

        /// <summary>
        /// Provide item to other dimensions
        /// </summary>
        /// <param name="item"></param>
        public void ProvideItemToOtherDimensions(TFact item)
        {
            foreach (var otherDim in OtherDimensions)
                otherDim.Key.Apply(item, otherDim.Value);
        }
        #endregion

        #region Indexer keys
        /// <summary>
        /// Return a dimension entry result by the given dimension entry
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IDimensionResult<TFact> this[string key] =>
            Children[Children.Keys.Single(i => i.Name == key)];

        /// <summary>
        /// Return a dimension entry result by the given dimension entry
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IDimensionResult<TFact> this[IDimensionEntry key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                if (DimensionEntry == key)
                    return this;

                if (Children.TryGetValue(key, out var result))
                    return result;

                if (key.Parent != null)
                    return this[key.Parent][key];

                foreach (var dim in OtherDimensions)
                {
                    if (dim.Key.DimensionEntry == key.Root)
                        return dim.Value[key];
                }

                throw new ArgumentOutOfRangeException(nameof(key), $"{nameof(key)} does not match dimension");
            }
        }

        /// <summary>
        /// Return a dimension entry result by the given measure entry
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IMeasureResult<TFact> this[IMeasure<TFact> key] =>
            MeasureResults[key];
        #endregion

        #region Previoulsy from Extensions (check later)
        //TODO: Check Extensions for DimensionResult
        private IEnumerable<DimensionResult<TDimension, TFact>> CubeCoordinates
        {
            get
            {
                var self = this;
                while (self != null)
                {
                    yield return self;
                    self = self.ParentCoordinate;
                }
            }
        }

        public bool Count<TDimensionEntry>(IDimension dimension, Func<DimensionEntry<TDimensionEntry>, bool> selector)
            where TDimensionEntry : IComparable
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var dimEntryResult = CubeCoordinates.FirstOrDefault(c => c.DimensionEntry.Root == dimension);
            if (dimEntryResult == null)
                return false;

            var dimEntry = (DimensionEntry<TDimensionEntry>)dimEntryResult.DimensionEntry;
            if (dimEntry == null)
                return false;

            return selector(dimEntry);
        }

        /// <summary>
        /// Retrieves the inner-most parent date coordinate of this entry.
        /// </summary>
        /// <param name="self"></param>
        /// <returns>the inner-most parent date coordinate of this entry or null</returns>
        public DimensionEntry<DateTime> GetDateTimeEntry()
        {
            var self = this;
            while (self != null)
            {
                var entry = self.DimensionEntry as DimensionEntry<DateTime>;
                if (entry != null)
                    return entry;

                self = self.ParentCoordinate;
            }
            return null;
        }
        #endregion

        /// <summary>
        /// Returns a string represenation
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"DimensionResult: {DimensionEntry.Root.Name}";
    }
}